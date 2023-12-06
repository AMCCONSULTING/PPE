using Microsoft.EntityFrameworkCore;
using PPE.Models;

namespace PPE.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        /*modelBuilder.Entity<Stock>()
            .Property(s => s.CurrentStock)
            .HasColumnName("CurrentStock");*/
        
        modelBuilder.Entity<Variant>()
            .HasMany(v => v.VariantValues)
            .WithOne(vv => vv.Variant)
            .HasForeignKey(vv => vv.VariantId);
        
        modelBuilder.Entity<Ppe>()
            .HasMany(p => p.Variants)
            .WithOne(v => v.Ppe)
            .HasForeignKey(v => v.PpeId);
        
        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Project)
            .WithMany(p => p.Stocks)
            .HasForeignKey(s => s.ProjectId);
        
        modelBuilder.Entity<Stock>()
            .HasOne(s => s.VariantValue)
            .WithMany(v => v.Stocks)
            .HasForeignKey(s => s.VariantValueId);
        
        modelBuilder.Entity<Function>()
            .HasMany(f => f.Employees)
            .WithOne(e => e.Function)
            .HasForeignKey(e => e.FunctionId);
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Stocks)
            .WithOne(s => s.Project)
            .HasForeignKey(s => s.ProjectId);
        
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.EmployeeStocks)
            .WithOne(es => es.Employee)
            .HasForeignKey(es => es.EmployeeId);
        
        modelBuilder.Entity<Stock>()
            .HasOne(s => s.VariantValue)
            .WithMany(v => v.Stocks)
            .HasForeignKey(s => s.VariantValueId);
        
        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Project)
            .WithMany(p => p.Stocks)
            .HasForeignKey(s => s.ProjectId);
        
        
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ppe> Ppes { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockDetail> StockDetails { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Variant> Variants { get; set; }
    public DbSet<VariantValue> VariantValues { get; set; }
    public DbSet<Function> Functions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Value> Values { get; set; }
    public DbSet<EmployeeStock> EmployeeStocks { get; set; }
    
    
}