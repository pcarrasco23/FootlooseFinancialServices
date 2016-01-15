using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.DataPersistence
{
    public class PersonRepository : SqlRepository<Person>, IPersonRepository
    {
        private IFootlooseFSUnitOfWork _unitOfWork;

        public PersonRepository(DbContext dbContext, IFootlooseFSUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public Person Find(int personId, PersonIncludes personIncludes)
        {
            var personQueryable = GetQueryable().Where(p => p.PersonID == personId);

            if (personIncludes.Phones)
            {
                personQueryable = personQueryable.Include("Phones");
                personQueryable = personQueryable.Include("Phones.PhoneType");
            }

            if (personIncludes.Addressses)
            {
                personQueryable = personQueryable.Include("Addresses.Address");
                personQueryable = personQueryable.Include("Addresses.AddressType");
            }

            if (personIncludes.Accounts)
            {
                personQueryable = personQueryable.Include("Accounts.Account")
                                                .Include("Accounts.Account.AccountType");
            }

            if (personIncludes.Login)
                personQueryable = personQueryable.Include("Login");

            if (personIncludes.AccountTransactions)
            {
                personQueryable = personQueryable.Include("Accounts.Account.Transactions");
                personQueryable = personQueryable.Include("Accounts.Account.Transactions.TransactionType");
            }

            if (personQueryable.Any())
                return personQueryable.First();
            else
                return null;
        }

        public override void Delete(Person person)
        {
            // Remove the associated records from the linked tables before deleting the Person entity
            if (person.Phones != null)
            {
                foreach (Phone phone in person.Phones)
                {
                    if (phone.PersonID > 0)
                        _unitOfWork.Phones.Delete(phone);
                }                    

                person.Phones = null;
            }

            if (person.Addresses != null)
            {
                foreach (PersonAddressAssn addressAssn in person.Addresses)
                {
                    if (addressAssn.PersonID > 0)
                        _unitOfWork.PersonAddressAssns.Delete(addressAssn);
                }                                                       

                person.Addresses = null;
            }

            if (person.Accounts != null)
            {
                foreach (PersonAccount account in person.Accounts)
                    _unitOfWork.PersonAccounts.Delete(account);

                person.Addresses = null;
            }

            if (person.Login != null)
                _unitOfWork.PersonLogins.Delete(person.Login);

            base.Delete(person);
        }

        public override Person Update(Person updatedPerson)
        {
            var person = Find(updatedPerson.PersonID, new PersonIncludes { Phones = true, Addressses = true });

            person.FirstName = updatedPerson.FirstName;
            person.LastName = updatedPerson.LastName;
            person.EmailAddress = updatedPerson.EmailAddress;

            updatePhone(person, updatedPerson, (int)PhoneTypes.Home);
            updatePhone(person, updatedPerson, (int)PhoneTypes.Work);
            updatePhone(person, updatedPerson, (int)PhoneTypes.Cell);

            updateAddress(person, updatedPerson, (int)AddressTypes.Home);
            updateAddress(person, updatedPerson, (int)AddressTypes.Work);
            updateAddress(person, updatedPerson, (int)AddressTypes.Alternate);

            base.Update(person);

            return person;
        }

        private void updatePhone(Person person, Person updatedPerson, int phoneType)
        {
            // 
            var updatePhone = updatedPerson.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
            if (updatePhone == null)
            {
                var phone = person.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
                if (phone != null)
                    _unitOfWork.Phones.Delete(phone);
            }
            else
            {
                var phone = person.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
                if (phone == null && !string.IsNullOrEmpty(updatePhone.Number))
                {
                    phone = new Phone();
                    phone.PersonID = updatePhone.PersonID;
                    phone.PhoneTypeID = updatePhone.PhoneTypeID;
                    person.Phones.Add(phone);
                }
                else if (phone != null)
                {
                    phone.Number = updatePhone.Number;
                }
            }
        }

        private void updateAddress(Person person, Person updatedPerson, int addressType)
        {
            var updatedAddressAssn = updatedPerson.Addresses.Where(a => a.AddressTypeID == addressType).FirstOrDefault();
            if (updatedAddressAssn == null)
            {
                var addressAssn = person.Addresses.Where(a => a.AddressTypeID == addressType).FirstOrDefault();
                if (addressAssn != null)
                {
                    _unitOfWork.Addresses.Delete(addressAssn.Address);
                    _unitOfWork.PersonAddressAssns.Delete(addressAssn);
                }
            }
            else
            {
                var addressAssn = person.Addresses.Where(a => a.AddressTypeID == addressType).FirstOrDefault();
                if (addressAssn == null && !string.IsNullOrEmpty(updatedAddressAssn.Address.StreetAddress))
                {
                    addressAssn = new PersonAddressAssn();
                    addressAssn.PersonID = updatedAddressAssn.PersonID;
                    addressAssn.AddressTypeID = updatedAddressAssn.AddressTypeID;

                    addressAssn.Address = new Address();
                    addressAssn.Address.StreetAddress = updatedAddressAssn.Address.StreetAddress;
                    addressAssn.Address.City = updatedAddressAssn.Address.City;
                    addressAssn.Address.State = updatedAddressAssn.Address.State;
                    addressAssn.Address.Zip = updatedAddressAssn.Address.Zip;

                    person.Addresses.Add(addressAssn);
                }
                else if (addressAssn != null)
                {
                    addressAssn.Address.StreetAddress = updatedAddressAssn.Address.StreetAddress;
                    addressAssn.Address.City = updatedAddressAssn.Address.City;
                    addressAssn.Address.State = updatedAddressAssn.Address.State;
                    addressAssn.Address.Zip = updatedAddressAssn.Address.Zip;
                }
            }
        }
    }
}
