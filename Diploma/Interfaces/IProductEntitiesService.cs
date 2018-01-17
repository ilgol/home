using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Interfaces
{
    public interface IProductEntitiesService
    {
        List<ProductModel> GetProducts();
        List<ProductModel> GetShoppingList();
        void UpdateEntities(ProductModel[] toUpdate, ProductModel[] toDelete);
    }
}
