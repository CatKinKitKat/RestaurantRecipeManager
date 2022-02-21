using Microsoft.EntityFrameworkCore;
using RestaurantRecipeManager.Models;

namespace RestaurantRecipeManager.Data
{
    public class RestaurantRecipeManagerContext : DbContext
    {
        public RestaurantRecipeManagerContext()
        {
        }

        public RestaurantRecipeManagerContext(DbContextOptions<RestaurantRecipeManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<LoginType> LoginTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<RecIngList> RecIngLists { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TbOrList> TbOrLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(e => e.IId)
                    .HasName("PK__ingredie__98F919BA7613C26B");

                entity.ToTable("ingredients");

                entity.Property(e => e.IId).HasColumnName("i_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.LId)
                    .HasName("PK__login__A7C7B6F85CCC9D26");

                entity.ToTable("login");

                entity.Property(e => e.LId).HasColumnName("l_id");

                entity.Property(e => e.Passhash)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("passhash");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__login__type_id__398D8EEE");
            });

            modelBuilder.Entity<LoginType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__login_ty__2C0005988D8EAF7C");

                entity.ToTable("login_type");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.Chmod).HasColumnName("chmod");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OId)
                    .HasName("PK__orders__904BC20ECE1A14E6");

                entity.ToTable("orders");

                entity.Property(e => e.OId).HasColumnName("o_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.RId).HasColumnName("r_id");

                entity.HasOne(d => d.RIdNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RId)
                    .HasConstraintName("FK__orders__r_id__30F848ED");
            });

            modelBuilder.Entity<RecIngList>(entity =>
            {
                entity.HasKey(e => e.RilId)
                    .HasName("PK__rec_ing___CF18296BA7DADFB5");

                entity.ToTable("rec_ing_list");

                entity.Property(e => e.RilId).HasColumnName("ril_id");

                entity.Property(e => e.IId).HasColumnName("i_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.RId).HasColumnName("r_id");

                entity.HasOne(d => d.IIdNavigation)
                    .WithMany(p => p.RecIngLists)
                    .HasForeignKey(d => d.IId)
                    .HasConstraintName("FK__rec_ing_li__i_id__2E1BDC42");

                entity.HasOne(d => d.RIdNavigation)
                    .WithMany(p => p.RecIngLists)
                    .HasForeignKey(d => d.RId)
                    .HasConstraintName("FK__rec_ing_li__r_id__2D27B809");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.RId)
                    .HasName("PK__recipe__C47623272AA71E66");

                entity.ToTable("recipe");

                entity.Property(e => e.RId).HasColumnName("r_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.ResId)
                    .HasName("PK__reservat__2090B50D68DD8170");

                entity.ToTable("reservation");

                entity.Property(e => e.ResId).HasColumnName("res_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.LId).HasColumnName("l_id");

                entity.Property(e => e.TId).HasColumnName("t_id");

                entity.HasOne(d => d.LIdNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.LId)
                    .HasConstraintName("FK__reservatio__l_id__3D5E1FD2");

                entity.HasOne(d => d.TIdNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.TId)
                    .HasConstraintName("FK__reservatio__t_id__3C69FB99");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.IId)
                    .HasName("PK__stock__98F919BAD5942E7E");

                entity.ToTable("stock");

                entity.Property(e => e.IId).HasColumnName("i_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.IIdNavigation)
                    .WithOne(p => p.Stock)
                    .HasForeignKey<Stock>(d => d.IId)
                    .HasConstraintName("FK__stock__i_id__2A4B4B5E");
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasKey(e => e.TId)
                    .HasName("PK__tables__E579775FD346F07A");

                entity.ToTable("tables");

                entity.Property(e => e.TId).HasColumnName("t_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TbOrList>(entity =>
            {
                entity.HasKey(e => e.TolId)
                    .HasName("PK__tb_or_li__C1A86CA5C5A3C7A8");

                entity.ToTable("tb_or_list");

                entity.Property(e => e.TolId).HasColumnName("tol_id");

                entity.Property(e => e.OId).HasColumnName("o_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.TId).HasColumnName("t_id");

                entity.HasOne(d => d.OIdNavigation)
                    .WithMany(p => p.TbOrLists)
                    .HasForeignKey(d => d.OId)
                    .HasConstraintName("FK__tb_or_list__o_id__33D4B598");

                entity.HasOne(d => d.TIdNavigation)
                    .WithMany(p => p.TbOrLists)
                    .HasForeignKey(d => d.TId)
                    .HasConstraintName("FK__tb_or_list__t_id__34C8D9D1");
            });
        }
    }
}