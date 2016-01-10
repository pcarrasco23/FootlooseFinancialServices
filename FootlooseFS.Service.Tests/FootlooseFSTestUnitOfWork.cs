using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service.Tests
{
    public class FootlooseFSTestUnitOfWork : IFootlooseFSUnitOfWork
    {
        protected TestRepository<Member> _members;
        protected TestRepository<MemberProfile> _memberProfiles;
        protected PersonTestRepository _persons;
        protected TestRepository<Phone> _phones;
        protected TestRepository<Address> _addresses;
        protected TestRepository<PersonAddressAssn> _personAddressAssns;
        protected TestRepository<Account> _accounts;
        protected TestRepository<PersonAccount> _personAccounts;
        protected TestRepository<AccountTransaction> _accountTransactions;
        protected TestRepository<PersonLogin> _personLogins;

        public FootlooseFSTestUnitOfWork()
        {
            List<Person> persons = TestDataStore.GetPersonTestData();

            _persons = new PersonTestRepository();

            foreach (Person person in persons)
                _persons.Add(person);
        }

        public IRepository<Member> Members
        {
            get
            {
                if (_members == null)
                    _members = new TestRepository<Member>();

                return _members;
            }            
        }

        public IRepository<MemberProfile> MemberProfiles
        {
            get
            {
                if (_memberProfiles == null)
                    _memberProfiles = new TestRepository<MemberProfile>();

                return _memberProfiles;
            }
        }

        public IPersonRepository Persons
        {
            get
            {
                return _persons;
            }            
        }

        public IRepository<Phone> Phones
        {
            get
            {
                if (_phones == null)
                    _phones = new TestRepository<Phone>();

                return _phones;
            }
        }

        public IRepository<Address> Addresses
        {
            get
            {
                if (_addresses == null)
                    _addresses = new TestRepository<Address>();

                return _addresses;
            }
        }

        public IRepository<PersonAddressAssn> PersonAddressAssns
        {
            get
            {
                if (_personAddressAssns == null)
                    _personAddressAssns = new TestRepository<PersonAddressAssn>();

                return _personAddressAssns;
            }
        }

        public IRepository<Account> Accounts
        {
            get
            {
                if (_accounts == null)
                    _accounts = new TestRepository<Account>();

                return _accounts;
            }            
        }

        public IRepository<PersonAccount> PersonAccounts
        {
            get
            {
                if (_personAccounts == null)
                    _personAccounts = new TestRepository<PersonAccount>();

                return _personAccounts;
            }            
        }

        public IRepository<AccountTransaction> AccountTransactions
        {
            get
            {
                if (_accountTransactions == null)
                    _accountTransactions = new TestRepository<AccountTransaction>();

                return _accountTransactions;
            }
        }

        public IRepository<PersonLogin> PersonLogins
        {
            get
            {
                if (_personLogins == null)
                    _personLogins = new TestRepository<PersonLogin>();

                return _personLogins;
            }
        }

        public void Commit()
        {            
        }

        public void Dispose()
        {
        }
    }
}
