﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.DataAccess
{
    public class DeliveryContext : DbContext
    {
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<TimeInterval> TimeIntervals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>()
            //    .HasRequired(o => o.From)
            //    .WithRequiredPrincipal()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Order>()
            //    .HasRequired(o => o.To)
            //    .WithRequiredPrincipal()
            //    .WillCascadeOnDelete(true);



        }
    }
}