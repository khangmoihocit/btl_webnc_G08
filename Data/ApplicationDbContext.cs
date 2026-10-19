using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using btl_webnc_G08.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace btl_webnc_G08.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Community> Communities => Set<Community>();
        public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
        public DbSet<VehicleAvailability> VehicleAvailabilities => Set<VehicleAvailability>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<HandoverRecord> HandoverRecords => Set<HandoverRecord>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ViolationReport> ViolationReports => Set<ViolationReport>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Email người dùng không trùng (Unique Index)
            builder.Entity<User>(b =>
            {
                b.HasIndex(u => u.Email).IsUnique();
                b.HasOne(u => u.Community)
                 .WithMany(c => c.Members)
                 .HasForeignKey(u => u.CommunityId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<Vehicle>(b =>
            {
                b.HasOne(v => v.Owner)
                 .WithMany(u => u.OwnedVehicles)
                 .HasForeignKey(v => v.OwnerId)
                 .OnDelete(DeleteBehavior.Restrict); // Giữ xe khi user bị deactive

                b.HasOne(v => v.Category)
                 .WithMany(c => c.Vehicles)
                 .HasForeignKey(v => v.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(v => v.Community)
                 .WithMany(c => c.Vehicles)
                 .HasForeignKey(v => v.CommunityId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Ràng buộc CHECK: Giá không âm
                b.ToTable(t => t.HasCheckConstraint("CK_Vehicle_Price_NonNegative", "[Price] >= 0"));
            });

            builder.Entity<Booking>(b =>
            {
                b.HasOne(bk => bk.Vehicle)
                 .WithMany(v => v.Bookings)
                 .HasForeignKey(bk => bk.VehicleId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(bk => bk.Borrower)
                 .WithMany(u => u.Bookings)
                 .HasForeignKey(bk => bk.BorrowerId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Ràng buộc CHECK: StartAt < EndAt & Phí >= 0
                b.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Booking_TimeRange", "[StartAt] < [EndAt]");
                    t.HasCheckConstraint("CK_Booking_EstimatedFee_NonNegative", "[EstimatedFee] >= 0");
                });
            });

            //Quan hệ 1 - 1 giữa Booking và HandoverRecord
            builder.Entity<HandoverRecord>(b =>
            {
                b.HasOne(h => h.Booking)
                 .WithOne(bk => bk.HandoverRecord)
                 .HasForeignKey<HandoverRecord>(h => h.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            //Đánh giá Reviews & Ràng buộc rating 1 - 5, duy nhất 1 review/cặp giao dịch
            builder.Entity<Review>(b =>
            {
                b.HasOne(r => r.Booking)
                 .WithMany(bk => bk.Reviews)
                 .HasForeignKey(r => r.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(r => r.Reviewer)
                 .WithMany()
                 .HasForeignKey(r => r.ReviewerId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(r => r.Reviewee)
                 .WithMany()
                 .HasForeignKey(r => r.RevieweeId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Ràng buộc: Mỗi người chỉ được đánh giá 1 lần cho mỗi booking
                b.HasIndex(r => new { r.BookingId, r.ReviewerId }).IsUnique();

                // Ràng buộc CHECK: Rating từ 1 đến 5
                b.ToTable(t => t.HasCheckConstraint("CK_Review_Rating_Range", "[Rating] >= 1 AND [Rating] <= 5"));
            });

            //Availability CHECK: StartAt < EndAt
            builder.Entity<VehicleAvailability>(b =>
            {
                b.ToTable(t => t.HasCheckConstraint("CK_Availability_TimeRange", "[StartAt] < [EndAt]"));
            });
        }
    }
}