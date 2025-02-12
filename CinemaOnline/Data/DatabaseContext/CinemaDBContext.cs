﻿using CinemaOnline.Data.DatabaseContext.Configurations;
using CinemaOnline.Models.CinemaModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinemaOnline.Data.DatabaseContext
{
    public class CinemaDBContext : IdentityDbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public CinemaDBContext(DbContextOptions<CinemaDBContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(CinemaDBContext).Assembly);
        }
    }
}
