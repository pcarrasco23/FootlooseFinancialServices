using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootlooseFS.Models;

namespace FootlooseFS.DataPersistence
{
    public class PersonAddressAssnTypeConfiguration : EntityTypeConfiguration<PersonAddressAssn>
    {
        public PersonAddressAssnTypeConfiguration()
        {
            this.HasKey(p => new { p.PersonID, p.AddressID, p.AddressTypeID });
        }
    }
}
