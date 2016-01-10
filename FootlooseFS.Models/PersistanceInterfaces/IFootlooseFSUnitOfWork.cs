using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public interface IFootlooseFSUnitOfWork : IDisposable
    {
        IRepository<Member> Members { get; }
        IRepository<MemberProfile> MemberProfiles { get; }
        IPersonRepository Persons { get; }
        IRepository<Phone> Phones { get; }
        IRepository<Address> Addresses { get; }
        IRepository<PersonAddressAssn> PersonAddressAssns { get; }
        IRepository<Account> Accounts { get; }
        IRepository<PersonAccount> PersonAccounts { get; }
        IRepository<AccountTransaction> AccountTransactions { get; }
        IRepository<PersonLogin> PersonLogins { get; }

        void Commit();
    }
}
