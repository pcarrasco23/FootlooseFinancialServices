using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootlooseFS.Models;

namespace FootlooseFS.DataPersistence
{
    /// <summary>
    /// DB context
    /// The Entoty Framework connection string: FootlooseFSContext must be defined in application configuration
    /// </summary>
    public class FootlooseFSDBContext : DbContext
    {
        public FootlooseFSDBContext()
            : base("FootlooseFSContext")
        {}

        public DbSet<Member> Members { get; set; }
        public DbSet<MemberProfile> MemberProfiles { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonLogin> PersonLogin { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<PersonAccount> PersonAccount { get; set; }
        public DbSet<AccountTransaction> AccountTransaction { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new PersonAccountTypeConfiguration());
            modelBuilder.Configurations.Add(new PersonAddressAssnTypeConfiguration());
            modelBuilder.Configurations.Add(new PhoneTypeConfiguration());
            modelBuilder.Configurations.Add(new PersonLoginTypeConfiguration());
        }
    }
}
