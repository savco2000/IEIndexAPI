using System;
using DataLayer.Contexts;

namespace DataLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IContext Context { get; }
        void Save();
    }

    public class UnitOfWork<TContext> : IUnitOfWork where TContext : IContext, new()
    {
        private readonly IContext _context;
        private bool _isAlive = true;
        private bool _isCommitted;

        public IContext Context => _context;

        public UnitOfWork()
        {
            _context = new TContext();
        }

        public UnitOfWork(IContext context)
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
