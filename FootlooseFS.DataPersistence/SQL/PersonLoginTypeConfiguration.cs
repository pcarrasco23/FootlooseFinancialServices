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
    public class PersonLoginTypeConfiguration : EntityTypeConfiguration<PersonLogin>
    {
        public PersonLoginTypeConfiguration()
        {
            this.HasKey(p => new { p.PersonID });

            this.HasRequired(p => p.Person)
                        .WithRequiredDependent(p => p.Login);
        }
    }
}
