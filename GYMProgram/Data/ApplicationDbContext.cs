using System;
using System.Collections.Generic;
using System.Text;
using GYMProgram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GYMProgram.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);
            //modelBuilder.sq(@"create trigger .....");
            modelBuilder.Entity<Customer>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Customer>().Property(x => x.ExpirDate).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<DailyBooking>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<DailyBooking>().Property(x => x.Status).HasDefaultValue(0);
            modelBuilder.Entity<DailyBooking>().Property(x => x.Number).HasDefaultValue(0);
            
            modelBuilder.Entity<GYM>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");
            
            modelBuilder.Entity<Section>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Bookings>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<WeekDaysHead>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<WeekDay>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<WeekDay>().Property(x => x.QTYCustomers).HasDefaultValue(1);
           
            modelBuilder.Entity<UnavailableDate>().Property(x => x.Guid).HasDefaultValueSql("NEWID()");

            // ربط الجداول
            modelBuilder.Entity<DailyBooking>(entity =>
            {
                entity.Property(e => e.CUGuid).HasColumnName("CUGuid");

                entity.Property(e => e.BookingGuid).HasColumnName("BookingGuid");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DailyBookings)
                    .HasForeignKey(d => d.CUGuid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DailyBooking_Customer_CUGuid");

                entity.HasOne(d => d.booking)
                    .WithMany(p => p.dailyBooking)
                    .HasForeignKey(d => d.BookingGuid)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_DailyBooking_Booking_DayGuid");
            });

            modelBuilder.Entity<Section>(entity=> {
                entity.Property(e => e.GYMGuid).HasColumnName("GYMGuid");
                entity.HasOne(d => d.GYM)
                   .WithMany(p => p.Sections)
                   .HasForeignKey(d => d.GYMGuid)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Section_GYM_GYMGuid");
            });

            modelBuilder.Entity<Customer>(entity => {
                entity.Property(e => e.GYMGuid).HasColumnName("GYMGuid");
                entity.HasOne(d => d.GYM)
                   .WithMany(p => p.Customers)
                   .HasForeignKey(d => d.GYMGuid)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Customer_GYM_GYMGuid");
            });

            modelBuilder.Entity<UnavailableDate>(entity => {
                entity.Property(e => e.SectionGuid).HasColumnName("SectionGuid");
                entity.HasOne(d => d.Section)
                   .WithMany(p => p.unavailableDate)
                   .HasForeignKey(d => d.SectionGuid)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_UnavailableDate_Section_SectionGuid");
            });

            modelBuilder.Entity<Bookings>(entity => {
                entity.Property(e => e.SectionGuid).HasColumnName("SectionGuid");
                entity.HasOne(d => d.Section)
                   .WithMany(p => p.bookings)
                   .HasForeignKey(d => d.SectionGuid)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Bookings_Section_SectionGuid");
            });

            modelBuilder.Entity<WeekDay>(entity => {
                entity.Property(e => e.HeadGuid).HasColumnName("HeadGuid");
                entity.HasOne(d => d.WeekDaysHead)
                   .WithMany(p => p.weekDays)
                   .HasForeignKey(d => d.HeadGuid)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_WeekDay_WeekDaysHead_HeadGuid");
            });
            
        }
        public virtual DbSet<GYM> GYMs { get; set; }
        public virtual DbSet<Section> Sections { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<WeekDay> WeekDays { get; set; }
        public virtual DbSet<Bookings> Bookings { get; set; }

        public virtual DbSet<DailyBooking> DailyBookings { get; set; }
        public virtual DbSet<UnavailableDate> UnavailableDates { get; set; }
        public virtual DbSet<WeekDaysHead> WeekDaysHeads { get; set; }
        //public virtual DbSet<triggers> triggers { get; set; }
    }
}
