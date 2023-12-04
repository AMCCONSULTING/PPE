using Microsoft.EntityFrameworkCore;
using PPE.Models;

namespace PPE.Data;

public static class DataSeeder
{
    public static void SeedData(AppDbContext dbContext)
    {
        try
        {
              // Seed Categories
        if (!dbContext.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Title = "Helmet", Description = "Protective headgear" },
                new Category { Title = "Glasses", Description = "Eye protection" },
                new Category { Title = "Glove", Description = "Hand protection" },
                new Category { Title = "Vest", Description = "Body protection" },
                new Category { Title = "Shoes", Description = "Foot protection" }
            };

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();
        }
        
        // Seed PPEs
        if (!dbContext.Ppes.Any())
        {
            var helmet = new Ppe { Title = "Helmet", Description = "Helmet description", CategoryId = 1 };
            var glasses = new Ppe { Title = "Glasses", Description = "Glasses description", CategoryId = 2 };
            var glove = new Ppe { Title = "Glove", Description = "Glove description", CategoryId = 3 };
            var vest = new Ppe { Title = "Vest", Description = "Vest description", CategoryId = 4 };
            var shoes = new Ppe { Title = "Shoes", Description = "Shoes description", CategoryId = 5 };

            dbContext.Ppes.AddRange(helmet, glasses, glove, vest, shoes);
            dbContext.SaveChanges();
        }
        
        // Seed Variants
        if (!dbContext.Variants.Any())
        {
            var helmet = new Variant { Title = "Color", PpeId = 1 };
            var glasses = new Variant { Title = "Size", PpeId = 2 };
            var glove = new Variant { Title = "Shoes Size", PpeId = 3 };
            var vest = new Variant { Title = "Type", PpeId = 4 };

            dbContext.Variants.AddRange(helmet, glasses, glove, vest);
            dbContext.SaveChanges();
        }
        
        // Seed VariantValues
        /*if (!dbContext.VariantValues.Any())
        {
            var helmet = new List<VariantValue>
            {
                new VariantValue { ValueId = 1, VariantId = 1 },
                new VariantValue { ValueId = 2, VariantId = 1 },
                new VariantValue { ValueId = 3, VariantId = 1 },
                new VariantValue { ValueId = 4, VariantId = 1 },
                new VariantValue { ValueId = 5, VariantId = 1 },
                new VariantValue { ValueId = 6, VariantId = 1 },
                new VariantValue { ValueId = 7, VariantId = 1 },
            };
            
            var glasses = new List<VariantValue>
            {
                new VariantValue { ValueId = 8, VariantId = 4 },
                new VariantValue { ValueId = 9, VariantId = 4 },
            };
            
            /*var glove = new List<VariantValue>
            {
                new VariantValue { Value = "Small", VariantId = 3 },
                new VariantValue { Value = "Medium", VariantId = 3 },
                new VariantValue { Value = "Large", VariantId = 3 },
                new VariantValue { Value = "Extra Large", VariantId = 3 },
            };#1#
            
            var vest = new List<VariantValue>
            {
                new VariantValue { ValueId = 10, VariantId = 2 },
                new VariantValue { ValueId = 11, VariantId = 2 },
                new VariantValue { ValueId = 12, VariantId = 2 },
                new VariantValue { ValueId = 13, VariantId = 2 },
                new VariantValue { ValueId = 14, VariantId = 2 },
                new VariantValue { ValueId = 15, VariantId = 2 },
            };
            
            var shoes = new List<VariantValue>
            {
                new VariantValue { ValueId = 16, VariantId = 3 },
                new VariantValue { ValueId = 17, VariantId = 3 },
                new VariantValue { ValueId = 18, VariantId = 3 },
                new VariantValue { ValueId = 19, VariantId = 3 },
                new VariantValue { ValueId = 20, VariantId = 3 },
                new VariantValue { ValueId = 21, VariantId = 3 },
            };
            
            dbContext.VariantValues.AddRange(helmet);
            dbContext.VariantValues.AddRange(glasses);
            //dbContext.VariantValues.AddRange(glove);
            dbContext.VariantValues.AddRange(shoes);
            dbContext.VariantValues.AddRange(vest);
            dbContext.SaveChanges();
        }*/
        
        // Seed Projects
        if (!dbContext.Projects.Any())
        {
            var project1 = new Project { Title = "Project 1", Description = "Project 1 description", Prefix = "P1"};
            var project2 = new Project { Title = "Project 2", Description = "Project 2 description", Prefix = "P2" };
            var project3 = new Project { Title = "Project 3", Description = "Project 3 description", Prefix = "P3" };
            
            dbContext.Projects.AddRange(project1, project2, project3);
            dbContext.SaveChanges();
        }

        // Seed Stocks
        if (!dbContext.Stocks.Any())
        {
            var stock1 = new Stock { ProjectId = 1, Date = new DateTime(), StockIn = 10, StockOut = 0, VariantValueId = 1};
            var stock2 = new Stock { ProjectId = 1, Date = new DateTime(), StockIn = 12, StockOut = 0, VariantValueId = 2};  
            var stock3 = new Stock { ProjectId = 2, Date = new DateTime(), StockIn = 6, StockOut = 0, VariantValueId = 3};
            var stock4 = new Stock { ProjectId = 2, Date = new DateTime(), StockIn = 15, StockOut = 0, VariantValueId = 4};
            var stock5 = new Stock { ProjectId = 3, Date = new DateTime(), StockIn = 20, StockOut = 0, VariantValueId = 5};
            
            dbContext.Stocks.AddRange(stock1, stock2, stock3, stock4,stock5);
            dbContext.SaveChanges();
        }
        
        // Seed StockDetails
        if (!dbContext.StockDetails.Any())
        {
            var stockDetail1 = new StockDetail
                { StockId = 1, Date = new DateTime(), Price = 100, Quantity = 10, Total = 1000,};
            var stockDetail2 = new StockDetail
                { StockId = 1, Date = new DateTime(), Price = 100, Quantity = 12, Total = 1200,};
            var stockDetail3 = new StockDetail
                { StockId = 2, Date = new DateTime(), Price = 200, Quantity = 22, Total = 4400,};
            var stockDetail4 = new StockDetail
                { StockId = 2, Date = new DateTime(), Price = 200, Quantity = 22, Total = 4400,};

            dbContext.StockDetails.AddRange(stockDetail1, stockDetail2, stockDetail3, stockDetail4);
            dbContext.SaveChanges();
        }
        }
        catch (DbUpdateException ex)
        {
            Exception innerException = ex;
            while (innerException != null)
            {
                Console.WriteLine(innerException.Message);
                Console.WriteLine(innerException.StackTrace);
                innerException = innerException.InnerException;
            }
        }
      
    }
}
