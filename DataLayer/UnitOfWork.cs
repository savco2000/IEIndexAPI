using System;
using DataLayer.Contexts;

namespace DataLayer
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEIndexContext _context;
        private bool _isAlive = true;
        private bool _isCommitted;

        internal IEIndexContext Context => _context;

        public UnitOfWork()
        {
            _context = new IEIndexContext();
        }

        public UnitOfWork(IEIndexContext context)
        {
            _context = context;
        }

        public void Save()
        {
            if (!_isAlive) return;

            _isCommitted = true;
        }

        public void Dispose()
        {
            if (!_isAlive) return;

            _isAlive = false;

            try
            {
                if (_isCommitted) _context.SaveChanges();
            }
            finally
            {
                _context.Dispose();
            }
        }
    }
}
