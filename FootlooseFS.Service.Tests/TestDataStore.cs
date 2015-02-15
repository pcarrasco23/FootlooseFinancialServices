using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service.Tests
{
    public static class TestDataStore
    {
        static public List<Person> GetPersonTestData()
        {
            List<Person> persons = new List<Person>();

            var person = new Person
            {
                PersonID = 1,
                FirstName = "Pam",
                LastName = "Scicchitano",
                EmailAddress = "pam@scicchitano.com",
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        PersonID = 1,
                        Number = "336-418-5159",
                        PhoneType = new PhoneType { Name = "Home", PhoneTypeID = 1 },
                        PhoneTypeID = 1
                    },                    
                    new Phone
                    {
                        PersonID = 1,
                        Number = "336-418-4000",
                        PhoneType = new PhoneType { Name = "Work", PhoneTypeID = 2 },
                        PhoneTypeID = 2
                    },
                    new Phone
                    {
                        PersonID = 1,
                        Number = "336-418-6000",
                        PhoneType = new PhoneType { Name = "Cell", PhoneTypeID = 3 },
                        PhoneTypeID = 3
                    }
                },
                Addresses = new List<PersonAddressAssn>
                {
                    new PersonAddressAssn
                    {
                        Address = new Address
                        {
                            StreetAddress = "38 S Dunworth St #4185",
                            State = "NC",
                            Zip = "27215",
                            AddressID = 1,
                            City = "Raleigh"
                        },
                        AddressID = 1,
                        AddressType = new AddressType { Name = "Home", AddressTypeID = 1 },
                        AddressTypeID = 1,
                        PersonID = 1
                    }
                },                
            };

            persons.Add(person);

            person = new Person
            {
                PersonID = 2,
                FirstName = "Dominique",
                LastName = "Marantz",
                EmailAddress = "dMarantz@Marantz.com",
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        PersonID = 2,
                        Number = "223-564-5159",
                        PhoneType = new PhoneType { Name = "Home", PhoneTypeID = 1 },
                        PhoneTypeID = 1
                    },                    
                    new Phone
                    {
                        PersonID = 2,
                        Number = "223-564-4000",
                        PhoneType = new PhoneType { Name = "Work", PhoneTypeID = 2 },
                        PhoneTypeID = 1
                    },
                    new Phone
                    {
                        PersonID = 2,
                        Number = "223-564-6000",
                        PhoneType = new PhoneType { Name = "Cell", PhoneTypeID = 3 },
                        PhoneTypeID = 1
                    }
                },
                Addresses = new List<PersonAddressAssn>
                {
                    new PersonAddressAssn
                    {
                        Address = new Address
                        {
                            StreetAddress = "94 Sunland Court",
                            State = "FL",
                            Zip = "33997",
                            AddressID = 2,
                            City = "Ft. Lauderdale"
                        },
                        AddressID = 2,
                        AddressType = new AddressType { Name = "Home", AddressTypeID = 1 },
                        AddressTypeID = 1,
                        PersonID = 2
                    }
                },
            };

            persons.Add(person);

            return persons;
        }
    }
}
