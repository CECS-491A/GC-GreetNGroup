using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetNGroup.DataAccess.Repository.RepoInterfaces;

namespace GreetNGroup.DataAccess.UnitOfWork
{
    interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IClaimRepository Claims { get; }
        IUserClaimRepository UserClaims { get; }
        int Complete();
    }
}
