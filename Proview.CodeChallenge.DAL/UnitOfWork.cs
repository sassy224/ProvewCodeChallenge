using Proview.CodeChallenge.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proview.CodeChallenge.DAL
{
    /// <summary>
    /// This class is used to shared database context class between multiple repositories
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        /// <summary>
        /// The single database context to be shared
        /// </summary>
        private ProviewEntities context = new ProviewEntities();
        /// <summary>
        /// The repository that is responsible for manipulating data for table UserInput 
        /// </summary>
        private IRepository<UserInput> userInputRepository;

        public IRepository<UserInput> UserInputRepository
        {
            get
            {

                if (this.userInputRepository == null)
                {
                    this.userInputRepository = new Repository<UserInput>(context);
                }
                return userInputRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
