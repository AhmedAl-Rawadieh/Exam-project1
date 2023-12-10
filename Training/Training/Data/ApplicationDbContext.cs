using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Training.Models;

namespace Training.Data
{
    public class ApplicationDbContext : DbContext
    {
        internal object User;

        public ApplicationDbContext() :
          base("WepApi")
        {

            ////ME 
            base.Configuration.ProxyCreationEnabled = false;
        }

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

        public static ApplicationDbContext Create()
        {

            return new ApplicationDbContext();
        }

        public DbSet <users> users { get; set; }
        public DbSet<exam> Exam { get; set; }
        public DbSet<question> Question { get; set; }
        public DbSet<answer> Answer { get; set; }
        public DbSet<result> Result { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            Database.SetInitializer<ApplicationDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}