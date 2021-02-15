using Advertisements.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Advertisements.Data
{
    /// <summary>
    /// Контекст данных
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Таблица объявлений
        /// </summary>
        public DbSet<Advertisement> Advertisements { get; set; }
        /// <summary>
        /// Таблица пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.HasSequence<int>("AdvertisementNumbers")
                .StartsAt(1)
                .IncrementsBy(1);

            mb.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);                
                e.HasMany(x => x.Advertisements).WithOne(y => y.User).HasForeignKey(y => y.UserId).OnDelete(DeleteBehavior.Restrict);
            });
            mb.Entity<Advertisement>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Number).HasDefaultValueSql("NEXT VALUE FOR AdvertisementNumbers");
                e.Property(x => x.DateCreate).HasDefaultValueSql("getdate()");
            });
        }
    }
}
