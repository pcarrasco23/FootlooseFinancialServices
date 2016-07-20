using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootlooseFS.Models;

namespace FootlooseFS.DataPersistence
{
    public class FootlooseFSSqlUnitOfWork : IFootlooseFSUnitOfWork, IDisposable
    {
        protected FootlooseFSSQLLocalDBContext _dbContext;
        protected SqlRepository<Member> _members;
        protected SqlRepository<MemberProfile> _memberProfiles;
        protected PersonRepository _persons;
        protected SqlRepository<Phone> _phones;
        protected SqlRepository<Address> _addresses;
        protected SqlRepository<PersonAddressAssn> _personAddressAssns;
        protected SqlRepository<Account> _accounts;
        protected SqlRepository<PersonAccount> _personAccounts;
        protected SqlRepository<AccountTransaction> _accountTransactions;
        protected SqlRepository<PersonLogin> _personLogins;

        public FootlooseFSSqlUnitOfWork()
        {
            _dbContext = new FootlooseFSSQLLocalDBContext();
        }

        public IRepository<Member> Members
        {
            get
            {
                if (_members == null)
                    _members = new SqlRepository<Member>(_dbContext);

                return _members;
            }            
        }

        public IRepository<MemberProfile> MemberProfiles
        {
            get
            {
                if (_memberProfiles == null)
                    _memberProfiles = new SqlRepository<MemberProfile>(_dbContext);

                return _memberProfiles;
            }
        }

        public IPersonRepository Persons
        {
            get
            {
                if (_persons == null)
                    _persons = new PersonRepository(_dbContext, this);

                return _persons;
            }            
        }

        public IRepository<Phone> Phones
        {
            get
            {
                if (_phones == null)
                    _phones = new SqlRepository<Phone>(_dbContext);

                return _phones;
            }
        }

        public IRepository<Address> Addresses
        {
            get
            {
                if (_addresses == null)
                    _addresses = new SqlRepository<Address>(_dbContext);

                return _addresses;
            }
        }

        public IRepository<PersonAddressAssn> PersonAddressAssns
        {
            get
            {
                if (_personAddressAssns == null)
                    _personAddressAssns = new SqlRepository<PersonAddressAssn>(_dbContext);

                return _personAddressAssns;
            }
        }

        public IRepository<Account> Accounts
        {
            get
            {
                if (_accounts == null)
                    _accounts = new SqlRepository<Account>(_dbContext);

                return _accounts;
            }            
        }

        public IRepository<PersonAccount> PersonAccounts
        {
            get
            {
                if (_personAccounts == null)
                    _personAccounts = new SqlRepository<PersonAccount>(_dbContext);

                return _personAccounts;
            }            
        }

        public IRepository<AccountTransaction> AccountTransactions
        {
            get
            {
                if (_accountTransactions == null)
                    _accountTransactions = new SqlRepository<AccountTransaction>(_dbContext);

                return _accountTransactions;
            }
        }

        public IRepository<PersonLogin> PersonLogins
        {
            get
            {
                if (_personLogins == null)
                    _personLogins = new SqlRepository<PersonLogin>(_dbContext);

                return _personLogins;
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}
