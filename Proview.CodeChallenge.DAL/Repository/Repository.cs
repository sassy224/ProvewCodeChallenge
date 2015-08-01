using Proview.CodeChallenge.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Proview.CodeChallenge.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ProviewEntities _dataContext;
        private readonly IDbSet<T> _dbSet;

        public Repository(ProviewEntities dataContext)
        {
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<T>();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            try
            {
                entity = _dbSet.Add(entity);
                /*
                 * Bug: Insert long string to DB
                 */
                _dataContext.Configuration.ValidateOnSaveEnabled = false;
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new Exception("", ex);
            }
        }

        public void InsertMany(IEnumerable<T> items)
        {
            try
            {
                if (!items.Any())
                {
                    return;
                }
                var sqlConnection = _dataContext.Database.Connection as SqlConnection;
                if (sqlConnection != null)
                {
                    using (var bulkInsert = new SqlBulkCopy(sqlConnection))
                    {
                        bulkInsert.BatchSize = 1000;
                        bulkInsert.DestinationTableName = GetTableName();
                        var table = new DataTable();
                        var props = TypeDescriptor.GetProperties(typeof(T))
                                                   .Cast<PropertyDescriptor>()
                                                   .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System") && propertyInfo.IsReadOnly != true)
                                                   .ToArray();
                        foreach (var propertyInfo in props)
                        {
                            bulkInsert.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                            table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                        }

                        var values = new object[props.Length];
                        foreach (var item in items)
                        {
                            for (var i = 0; i < values.Length; i++)
                            {
                                values[i] = props[i].GetValue(item);
                            }

                            table.Rows.Add(values);
                        }

                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            sqlConnection.Open();
                        }

                        bulkInsert.WriteToServer(table);
                    }
                }
                else
                {
                    try
                    {
                        _dataContext.Configuration.AutoDetectChangesEnabled = false;
                        _dataContext.Configuration.ValidateOnSaveEnabled = false;

                        var count = 0;
                        foreach (var item in items)
                        {
                            _dbSet.Add(item);
                            count++;

                            if (count != 100) continue;
                            _dataContext.SaveChanges();
                            count = 0;
                        }

                        if (count > 0)
                        {
                            _dataContext.SaveChanges();
                        }
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        var msg = dbEx.EntityValidationErrors
                            .SelectMany(validationErrors => validationErrors.ValidationErrors)
                            .Aggregate(string.Empty, (current, validationError) => current + (string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + System.Environment.NewLine));

                        throw new Exception(msg, dbEx);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                throw new Exception("", ex);
            }
        }

        public void UpdateMany(IEnumerable<T> entities)
        {
            try
            {
                foreach (var item in entities)
                {
                    _dbSet.Attach(item);
                    _dataContext.Entry(item).State = EntityState.Modified;
                }

                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var msg = ex.EntityValidationErrors
                    .SelectMany(validationErrors => validationErrors.ValidationErrors)
                    .Aggregate(string.Empty, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

                throw new Exception(msg, ex);
            }
        }

        public void SqlCommand(string command)
        {
            _dataContext.Database.ExecuteSqlCommand(command);
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                var isAttached = false;

                if (_dataContext.Entry(entity).State == EntityState.Detached)
                {
                    // Try attach into context
                    var hashCode = entity.GetHashCode();
                    foreach (var obj in _dbSet.Local)
                    {
                        if (obj.GetHashCode() == hashCode)
                        {
                            _dataContext.Entry(obj).CurrentValues.SetValues(entity);
                            isAttached = true;
                            break;
                        }
                    }

                    if (!isAttached)
                    {
                        entity = _dbSet.Attach(entity);
                        _dataContext.Entry(entity).State = EntityState.Modified;
                    }
                }
                else
                {
                    // Set the entity's state to modified
                    _dataContext.Entry(entity).State = EntityState.Modified;
                }

                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors
                    .SelectMany(validationErrors => validationErrors.ValidationErrors)
                    .Aggregate(string.Empty, (current, validationError) => current + (System.Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

                throw new Exception(msg, dbEx);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                _dataContext.Entry(entity).State = EntityState.Deleted;
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new Exception("", ex);
            }
        }

        public void DeleteMany(IEnumerable<T> items)
        {
            if (!items.Any())
            {
                return;
            }
            try
            {
                foreach (var item in items)
                {
                    _dbSet.Remove(item);
                    _dataContext.Entry(item).State = EntityState.Deleted;
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new Exception("", ex);
            }
        }

        private string GetTableName()
        {
            var set = _dataContext.Set<T>();
            var regex = new Regex("FROM (?<table>.*) AS");
            var sql = set.ToString();
            var match = regex.Match(sql);

            return match.Groups["table"].Value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_dataContext == null) return;
            _dataContext.Dispose();
            _dataContext = null;
        }
    }
}
