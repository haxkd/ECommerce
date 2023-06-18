using Microsoft.EntityFrameworkCore;

namespace EComm.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product>? Products { get; set; }
        public virtual DbSet<Cart>? Carts { get; set; }
        public virtual DbSet<Order>? Orders { get; set; }
        public virtual DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e=>e.uid);
                entity.ToTable("Users");
                entity.Property(e => e.uid).HasColumnName("uid");
                entity.Property(e => e.uname).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.uemail).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.umobile).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.upassword).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.isAdmin).IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e=>e.pid);
                entity.ToTable("Product");
                entity.Property(e => e.pid).HasColumnName("pid");
                entity.Property(e => e.pname).HasMaxLength(15).IsUnicode(false);
                entity.Property(e => e.pDescription).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.pCategory).HasMaxLength(256).IsUnicode(false);
                entity.Property(e => e.pQuantity).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.pImage).IsUnicode(false);
                entity.Property(e => e.pPrice).HasMaxLength(1).IsUnicode(false);
                entity.Property(e => e.pDiscount).HasMaxLength(1).IsUnicode(false);
                entity.Property(e => e.pSpecification).IsUnicode(false);                
            });
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.cid);
                entity.ToTable("Cart");
                entity.Property(e => e.cid).HasColumnName("cid");
                entity.Property(e => e.pid).IsUnicode(false);
                entity.Property(e => e.quantity).IsUnicode(false);
                entity.Property(e => e.uid).IsUnicode(false);
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.ToTable("Order");
                entity.Property(e => e.OrderId).HasColumnName("OrderId");
                entity.Property(e => e.PId).IsUnicode(false);
                entity.Property(e => e.quantity).IsUnicode(false);
                entity.Property(e => e.OrderdId).IsUnicode(false);
                entity.Property(e => e.date).IsUnicode(false);
                entity.Property(e => e.uid).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
