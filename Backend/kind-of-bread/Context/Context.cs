using Microsoft.EntityFrameworkCore;
using kind_of_bread.DTOs;

namespace kind_of_bread.Context;

public class BreadContext(DbContextOptions<BreadContext> options) : DbContext(options)
{
    public DbSet<BreadTypeDTO> BreadTypes { get; set; }
    public DbSet<BreadItemDTO> BreadItems { get; set; }
    public DbSet<BreadAddonDTO> BreadAddons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BreadTypeDTO>().ToTable("bread_type");
        modelBuilder.Entity<BreadItemDTO>().ToTable("bread_item").Property(b => b.TypeId).HasColumnName("type_id");
        modelBuilder.Entity<BreadAddonDTO>().ToTable("bread_addon").Property(a => a.BreadId).HasColumnName("bread_id");
    }
}