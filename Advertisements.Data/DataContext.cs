using Advertisements.Data.Entities;
using Advertisements.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
                e.HasData(new List<User>
                {
                    new User 
                    { 
                        Id = Guid.NewGuid(), 
                        Name = "Admin", 
                        Login = "admin", 
                        Role = UserRoleEnum.Admin, 
                        Password = "94-60-C9-7F-0F-2C-B5-BB-B7-3C-14-2F-12-E2-76-39-84-4C-47-23-E4-45-E5-BC-80-17-DA-F0-4E-DA-02-8E", 
                        PassKey = "aa78ac69-64b3-4142-98b5-f9ca1a5fae5d"
                    }
                });
            });

            mb.Entity<Advertisement>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Number).HasDefaultValueSql("NEXT VALUE FOR AdvertisementNumbers");
                e.Property(x => x.DateCreate).HasDefaultValueSql("getdate()");
            });
        }

        public object FirstOrDefault(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
