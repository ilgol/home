using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Data;
using Diploma.Models;
using Diploma.Interfaces;

namespace Diploma.Services
{
    public class ProductEntitiesService : IProductEntitiesService
    {
        public List<ProductModel> GetProducts()
        {
            using (var db = new homeEntities())
            {
                return db.Products.Select(ConvertToProductModel).ToList();
            }
        }
        public List<ProductModel> GetShoppingList()
        {
            using (var db = new homeEntities())
            {
                return db.Products.Where(i => i.Quantity == 0).Select(ConvertToProductModel).ToList();
            }
        }
        private static ProductModel ConvertToProductModel(Product obj)
        {
            return new ProductModel()
            {
                Name = obj.ProductName,
                Quantity = obj.Quantity,
                DefaultValue = obj.DefaultValue ?? 1
            };
        }

        public void UpdateEntities(ProductModel[] toUpdate, ProductModel[] toDelete)
        {
            using (var db = new homeEntities())
            {
                if (toDelete != null)
                {
                    foreach (var entity in toDelete)
                    {
                        var product = db.Products.Where(i => i.ProductName == entity.Name.Replace("'", "").Replace("\"", "").Trim()).FirstOrDefault();
                        if (product != null)
                        {
                            db.Products.Remove(product);
                        }
                    }

                    db.SaveChanges();
                }
                if (toUpdate != null)
                {
                    foreach (var entity in toUpdate)
                    {
                        var product = db.Products.Where(i => i.ProductName == entity.Name.Replace("'", "").Replace("\"", "").Trim()).FirstOrDefault();
                        if (product != null)
                        {
                            product.Quantity += entity.Quantity;
                        }
                        else
                        {
                            db.Products.Add(new Product()
                            {
                                ProductName = entity.Name.Replace("'", "").Replace("\"", "").Trim(),
                                Quantity = entity.Quantity,
                                DefaultValue = entity.Quantity
                            });
                        }
                    }

                    db.SaveChanges();
                }
            }
        }
    }
}