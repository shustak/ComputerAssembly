using StoreServer.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreServer.Data
{
    public class ComputerContext : DbContext
    {
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Processor> Processors { get; set; }
        public DbSet<RAM> RAM { get; set; }
        public DbSet<Motherboard> Motherboards { get; set; }
        public DbSet<HDD_SSD> HDD_SSD { get; set; }
        public DbSet<PowerUnit> PowerUnits { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<OtherComponent> Components { get; set; }

        public ComputerContext(DbContextOptions<ComputerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Computer>()
                .HasOne(c => c.Processor)
                .WithMany()
                .HasForeignKey(c => c.ProcessorId);

            modelBuilder.Entity<Computer>()
                .HasOne(c => c.RAM)
                .WithMany()
                .HasForeignKey(c => c.RAMId);

            modelBuilder.Entity<Computer>()
                .HasOne(c => c.Motherboard)
                .WithMany()
                .HasForeignKey(c => c.MotherboardId);

            modelBuilder.Entity<Computer>()
                .HasOne(c => c.Frame)
                .WithMany()
                .HasForeignKey(c => c.FrameId);

            modelBuilder.Entity<Computer>()
                .HasOne(c => c.PowerUnit)
                .WithMany()
                .HasForeignKey(c => c.PowerUnitId);

            modelBuilder.Entity<Computer>()
                .HasOne(c => c.HDD_SSD)
                .WithMany()
                .HasForeignKey(c => c.StorageId);
        }
    }
}
