using System;
using GreetNGroup.DataAccess.Repository.RepoInterfaces;
using GreetNGroup.DataAccess.UnitOfWork;

namespace GreetNGroup.DataAccess.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GreetNGroupContext _context;

        public IUserRepository Users { get; private set; }
        public IClaimRepository Claims { get; private set; }
        public IUserClaimRepository UserClaims { get; private set; }

        public UnitOfWork(GreetNGroupContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Claims = new ClaimRepository(_context);
            UserClaims = new UserClaimRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}