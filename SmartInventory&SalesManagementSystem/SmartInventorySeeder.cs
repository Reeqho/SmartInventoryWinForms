using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory_SalesManagementSystem
{

    public static class SmartInventorySeeder
    {
        public static void Seed(SmartInventoryDbContext db)
        {
            // ===== ROLES =====
            if (!db.Roles.Any())
            {
                db.Roles.AddRange(
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "Kasir" }
                );
                db.SaveChanges();
            }

            // ===== USERS =====
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User
                    {
                        FullName = "Admin Utama",
                        Username = "admin",
                        PasswordHash = "123456", // untuk demo, nanti ganti hash nyata
                        RoleId = db.Roles.First(r => r.RoleName == "Admin").RoleId,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        FullName = "Kasir 1",
                        Username = "kasir1",
                        PasswordHash = "123456",
                        RoleId = db.Roles.First(r => r.RoleName == "Kasir").RoleId,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                );
                db.SaveChanges();
            }

            // ===== CATEGORIES =====
            if (!db.Categories.Any())
            {
                db.Categories.AddRange(
                    new Category { CategoryName = "Elektronik" },
                    new Category { CategoryName = "Alat Tulis" },
                    new Category { CategoryName = "Furniture" }
                );
                db.SaveChanges();
            }

            // ===== SUPPLIERS =====
            if (!db.Suppliers.Any())
            {
                db.Suppliers.AddRange(
                    new Supplier { SupplierName = "PT Sumber Makmur", Phone = "08123456789", Address = "Surabaya" },
                    new Supplier { SupplierName = "CV Jaya Abadi", Phone = "08234567890", Address = "Jakarta" }
                );
                db.SaveChanges();
            }

            // ===== PRODUCTS =====
            if (!db.Products.Any())
            {
                var elektronikId = db.Categories.First(c => c.CategoryName == "Elektronik").CategoryId;
                var atId = db.Categories.First(c => c.CategoryName == "Alat Tulis").CategoryId;

                var sup1 = db.Suppliers.First(s => s.SupplierName == "PT Sumber Makmur").SupplierId;
                var sup2 = db.Suppliers.First(s => s.SupplierName == "CV Jaya Abadi").SupplierId;

                db.Products.AddRange(
                    new Product
                    {
                        ProductName = "Laptop Lenovo",
                        CategoryId = elektronikId,
                        SupplierId = sup1,
                        Stock = 10,
                        Price = 7500000,
                        CreatedAt = DateTime.Now
                    },
                    new Product
                    {
                        ProductName = "Pulpen Gel",
                        CategoryId = atId,
                        SupplierId = sup2,
                        Stock = 100,
                        Price = 5000,
                        CreatedAt = DateTime.Now
                    }
                );
                db.SaveChanges();
            }

            // ===== SALES =====
            if (!db.Sales.Any())
            {
                var userId = db.Users.First(u => u.Username == "kasir1").UserId;

                var sale = new Sale
                {
                    SaleDate = DateTime.Now,
                    UserId = userId,
                    TotalAmount = 100000
                };
                db.Sales.Add(sale);
                db.SaveChanges();

                // ===== SALE DETAILS =====
                var productId = db.Products.First(p => p.ProductName == "Pulpen Gel").ProductId;

                db.SaleDetails.Add(new SaleDetail
                {
                    SaleId = sale.SaleId,
                    ProductId = productId,
                    Quantity = 10,
                    Price = 5000,
                    SubTotal = 50000
                });

                var productId2 = db.Products.First(p => p.ProductName == "Laptop Lenovo").ProductId;
                db.SaleDetails.Add(new SaleDetail
                {
                    SaleId = sale.SaleId,
                    ProductId = productId2,
                    Quantity = 1,
                    Price = 7500000,
                    SubTotal = 7500000
                });

                sale.TotalAmount = db.SaleDetails.Where(sd => sd.SaleId == sale.SaleId)
                                                 .Sum(sd => sd.SubTotal ?? 0);
                db.SaveChanges();
            }

            Console.WriteLine("Seeder: Data dummy berhasil di-insert!");
        }
    }
}

