using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CafeLab.Models
{
    public partial class DBCafeContext : DbContext
    {
        public DBCafeContext()
        {
            Database.EnsureCreated();
        }

        public DBCafeContext(DbContextOptions<DBCafeContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Cafe> Cafes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<DishesInOrder> DishesInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<SaleCard> SaleCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new PostgreSqlConnectionStringBuilder("postgres://mrklpjthvjjgfc:5ad58f9ab402a02de0e05c58f8b75e0e3c12e90fe76c25aaaa9a675a155b7627@ec2-52-19-96-181.eu-west-1.compute.amazonaws.com:5432/d2ft41p1mub79v")
                {
                    Pooling = true,
                    TrustServerCertificate = true,
                    SslMode = SslMode.Require
                };
                optionsBuilder.UseNpgsql(builder.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Cafe>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("TEXT");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("TEXT");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("TEXT");

                entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dishes_Categories");
            });

            modelBuilder.Entity<DishesInOrder>(entity =>
            {
                entity.HasKey(e => e.DishInOrderId)
                    .HasName("PK_Dishes_in_Orders");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishesInOrders)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dishes_in_Orders_Dishes");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DishesInOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dishes_in_Orders_Orders");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("TEXT");

                entity.Property(e => e.Datetime).HasColumnType("timestamp");

                entity.HasOne(d => d.Cafe)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CafeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Cafes");

                entity.HasOne(d => d.Salecard)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.SalecardId)
                    .HasConstraintName("FK_Orders_SaleCards");
            });

            modelBuilder.Entity<SaleCard>(entity =>
            {
                entity.Property(e => e.SalecardId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
