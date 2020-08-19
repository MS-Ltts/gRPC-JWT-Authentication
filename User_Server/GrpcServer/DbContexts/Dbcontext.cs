
using GrpcServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.DbContexts
{
    public class Dbcontext : DbContext
    {
        public DbSet<User>Users{get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
        optionsBuilder.UseSqlServer(@"data source =MYTSP00604; 
initial catalog=UserDB; persist security info=True; user id=sa ; password=Temppass@789;");
        }
    }
}
