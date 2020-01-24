using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebShop.Models
{
    public partial class WebDBContext : DbContext
    {
        public WebDBContext()
        {
        }

        public WebDBContext(DbContextOptions<WebDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Basket> Basket { get; set; }
        public virtual DbSet<Code> Code { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Developer> Developer { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductGenre> ProductGenre { get; set; }
        public virtual DbSet<ProductLanguage> ProductLanguage { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserComment> UserComment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=WebDB;Username=postgres;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdProduct })
                    .HasName("basket_pk");

                entity.ToTable("basket");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Basket)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("Product_fk");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Basket)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("User_fk");
            });

            modelBuilder.Entity<Code>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.Property(e => e.Key).IsRequired();

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Code)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Product_fk");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(e => new { e.IdProduct, e.IdUserClientComment, e.IdCommentClientComment })
                    .HasName("Comments_pk");

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.Property(e => e.IdUserClientComment).HasColumnName("ID_User_ClientComment");

                entity.Property(e => e.IdCommentClientComment).HasColumnName("ID_Comment_ClientComment");
            });

            modelBuilder.Entity<Developer>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("Order_pk");

                entity.Property(e => e.Number).ValueGeneratedNever();

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Quantity).HasDefaultValueSql("1");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Product_fk");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_fk");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IconUrl).HasColumnName("Icon_url");

                entity.Property(e => e.IdDeveloper).HasColumnName("ID_Developer");

                entity.Property(e => e.IdPublisher).HasColumnName("ID_Publisher");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.PictureUrl).HasColumnName("Picture_url");

                entity.Property(e => e.SystemRequirements).HasColumnName("System_requirements");

                entity.HasOne(d => d.IdDeveloperNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.IdDeveloper)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Developer_fk");

                entity.HasOne(d => d.IdPublisherNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.IdPublisher)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Publisher_fk");
            });

            modelBuilder.Entity<ProductGenre>(entity =>
            {
                entity.HasKey(e => new { e.IdGenre, e.IdProduct })
                    .HasName("Product_Genre_pk");

                entity.ToTable("Product_Genre");

                entity.Property(e => e.IdGenre).HasColumnName("ID_Genre");

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.ProductGenre)
                    .HasForeignKey(d => d.IdGenre)
                    .HasConstraintName("Genre_fk");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductGenre)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("Product_fk");
            });

            modelBuilder.Entity<ProductLanguage>(entity =>
            {
                entity.HasKey(e => new { e.IdLanguage, e.IdProduct })
                    .HasName("Product_Language_pk");

                entity.ToTable("Product_Language");

                entity.Property(e => e.IdLanguage).HasColumnName("ID_Language");

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.HasOne(d => d.IdLanguageNavigation)
                    .WithMany(p => p.ProductLanguage)
                    .HasForeignKey(d => d.IdLanguage)
                    .HasConstraintName("Language_fk");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductLanguage)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("Product_fk");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EMail)
                    .IsRequired()
                    .HasColumnName("e-mail");

                entity.Property(e => e.IconUrl)
                    .IsRequired()
                    .HasColumnName("icon_url")
                    .HasDefaultValueSql("'none_img'::text");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");
            });

            modelBuilder.Entity<UserComment>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdComment })
                    .HasName("ClientComment_pk");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.IdComment).HasColumnName("ID_Comment");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
