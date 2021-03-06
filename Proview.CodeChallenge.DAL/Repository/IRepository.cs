﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Proview.CodeChallenge.DAL
{
    /// <summary>
    /// The interface repository that contains common functions that can be used to manipulate data objects
    /// </summary>
    /// <typeparam name="T">An entity class</typeparam>
    public interface IRepository<T> where T : class
    {
        int Count(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        T GetById(object id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        void DeleteMany(IEnumerable<T> entity);

        void InsertMany(IEnumerable<T> entities);

        void UpdateMany(IEnumerable<T> entities);

        void SqlCommand(string command);
    }
}
