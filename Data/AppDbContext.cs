using Microsoft.EntityFrameworkCore;
using PPE.Models;
using Attribute = PPE.Models.Attribute;

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
        
        /*modelBuilder.Entity<Variant>()
            .HasMany(v => v.VariantValues)
            .WithOne(vv => vv.Variant)
            .HasForeignKey(vv => vv.VariantId);
        
        modelBuilder.Entity<Value>()
            .HasMany(v => v.VariantValues)
            .WithOne(vv => vv.Value)
            .HasForeignKey(vv => vv.ValueId);
        
        modelBuilder.Entity<Ppe>()
            .HasMany(p => p.Variants)
            .WithOne(v => v.Ppe)
            .HasForeignKey(v => v.PpeId);*/
        
        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Project)
            .WithMany(p => p.Stocks)
            .HasForeignKey(s => s.ProjectId);
        
        /*modelBuilder.Entity<Stock>()
            .HasOne(s => s.VariantValue)
            .WithMany(v => v.Stocks)
            .HasForeignKey(s => s.VariantValueId);*/
        
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
            .HasOne(s => s.Project)
            .WithMany(p => p.Stocks)
            .HasForeignKey(s => s.ProjectId);
        
        modelBuilder.Entity<AttributeValue>()
            .HasOne(av => av.Value)
            .WithMany(es => es.AttributeValues)
            .HasForeignKey(es => es.ValueId);
        
        modelBuilder.Entity<AttributeValue>()
            .HasOne(av => av.Attribute)
            .WithMany(es => es.AttributeValues)
            .HasForeignKey(es => es.AttributeId);
        
        
        
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ppe> Ppes { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockDetail> StockDetails { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Function> Functions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Value> Values { get; set; }
    public DbSet<EmployeeStock> EmployeeStocks { get; set; }
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<AttributeValueAttributeCategory> AttrValueAttrCategories { get; set; }
    public DbSet<AttributeCategory> AttributeCategories { get; set; }
    public DbSet<PpeAttributeCategoryAttributeValue> PpeAttributeCategoryAttributeValues { get; set; }
    public DbSet<StockToBePaid> StocksToBePaid { get; set; }
    
    public DbSet<Responsable> Responsables { get; set; }
    public DbSet<Coordinateur> Coordinateurs { get; set; }
    public DbSet<Magazinier> Magaziniers { get; set; }
    public DbSet<Hse> Hses { get; set; }
    public DbSet<Transporteur> Transporteurs { get; set; }
    public DbSet<Mouvement> Mouvements { get; set; }
    public DbSet<MouvementDetail> MouvementDetails { get; set; }
    public DbSet<Stoke> Stokes { get; set; }
    public DbSet<StokeDetail> StokeDetails { get; set; }
    public DbSet<MainStock> MainStocks { get; set; }
    public DbSet<ProjectStock> ProjectStocks { get; set; }
    public DbSet<Dotation> Dotations { get; set; } 
    public DbSet<DotationDetail> DotationDetails { get; set; }
    public DbSet<PayableStock> PayableStocks { get; set; }
    public DbSet<Return> Returns { get; set; }
   // public DbSet<ReturnDetail> ReturnDetails { get; set; }
    public DbSet<StockEmployee> StockEmployees { get; set; }
    
    
}

