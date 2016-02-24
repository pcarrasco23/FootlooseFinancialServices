using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using FootlooseFS.DataPersistence;
using FootlooseFS.Models;

namespace FootlooseFSDocumentDBETL
{
    class Program
    {
        const int throttleRate = 1000;

        static void Main(string[] args)
        {               
            int startRow = 0;  
              
            // Remove all record from the Person document
            var docUnitOfWork = new FootlooseFSDocUnitOfWork();
            docUnitOfWork.Persons.DeleteAll();

            while (true)
            {
                using (var sqlUnitOfWork = new FootlooseFSSqlUnitOfWork())
                {
                    IQueryable<Person> personQueryable = sqlUnitOfWork.Persons.GetQueryable();

                    // For each person retreived from SQL include the related phones and addresses
                    personQueryable = personQueryable.Include("Addresses.Address").Include("Phones");

                    // Always order by person ID so that we can guarantee the order for the persons retrieved from SQL
                    personQueryable = personQueryable.OrderBy(p => p.PersonID);

                    // Get throttleRate number of persons from SQL
                    List<Person> persons = personQueryable.Skip(startRow)
                                                    .Take(throttleRate)
                                                    .ToList();

                    if (persons.Count > 0)
                    {
                        IEnumerable<PersonDocument> personDocuments = from p in persons
                                                                      select new PersonDocument
                                                                      {
                                                                          PersonID = p.PersonID,
                                                                          FirstName = p.FirstName,
                                                                          LastName = p.LastName,
                                                                          EmailAddress = p.EmailAddress,
                                                                          PhoneNumber = p.Phones != null && p.Phones.Any(h => h.PhoneTypeID == 1) ? p.Phones.First(h => h.PhoneTypeID == 1).Number : "",
                                                                          StreetAddress = p.Addresses != null && p.Addresses.Any(pa => pa.AddressTypeID == 1) ? p.Addresses.First(pa => pa.AddressTypeID == 1).Address.StreetAddress : "",
                                                                          City = p.Addresses != null && p.Addresses.Any(pa => pa.AddressTypeID == 1) ? p.Addresses.First(pa => pa.AddressTypeID == 1).Address.City : "",
                                                                          State = p.Addresses != null && p.Addresses.Any(pa => pa.AddressTypeID == 1) ? p.Addresses.First(pa => pa.AddressTypeID == 1).Address.State : "",
                                                                          Zip = p.Addresses != null && p.Addresses.Any(pa => pa.AddressTypeID == 1) ? p.Addresses.First(pa => pa.AddressTypeID == 1).Address.Zip : ""
                                                                      };

                        docUnitOfWork.Persons.AddBatch(personDocuments);

                        Console.WriteLine(string.Format("Complete {0} - {1}", startRow, startRow + throttleRate));

                        // If the number of persons retrieved is less than throttleRate then we are finished
                        if (personDocuments.Count() < throttleRate)
                            break;
                    }             
                    else
                    {
                        break;
                    }
                               
                }

                // Otherwise move the starting row by throttleRate number of persons
                startRow += throttleRate;
            }
        }
    }
}
