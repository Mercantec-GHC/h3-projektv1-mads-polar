namespace API.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    UpdateTimestamps();
        //    return await base.SaveChanges(cancellationToken);
        //}

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Common &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (Common)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (string.IsNullOrEmpty(entity.Id))
                    {
                        entity.Id = Guid.NewGuid().ToString();
                    }
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

}
