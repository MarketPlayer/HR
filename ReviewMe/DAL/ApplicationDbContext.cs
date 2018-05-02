using ReviewMe.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReviewMe.DAL
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Store> Stores { get; set; }

        public ApplicationDbContext(): base("ReviewMe")
        {

        }
    }
}