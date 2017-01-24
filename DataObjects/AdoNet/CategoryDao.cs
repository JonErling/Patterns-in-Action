using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.AdoNet
{
    // Data access object for Category
    // ** DAO Pattern

    public class CategoryDao : ICategoryDao
    {
        private static readonly Db Db = new Db();

        public List<Category> GetCategories()
        {
            string sql =
            @"SELECT CategoryId, CategoryName, Description
                FROM [Category]";

            return Db.Read(sql, Make).ToList();
        }

        public Category GetCategoryByProduct(int productId)
        {
            string sql =
            @"SELECT C.CategoryId, CategoryName, Description 
                FROM [Category] C INNER JOIN [Product] P ON P.CategoryId = C.CategoryId 
               WHERE ProductId = @ProductId";

            object[] parms = { "@ProductId", productId };
            return Db.Read(sql, Make, parms).FirstOrDefault();
        }


        private static readonly Func<IDataReader, Category> Make = reader =>
           new Category
           {
               CategoryId = reader["CategoryId"].AsId(),
               CategoryName = reader["CategoryName"].AsString(),
               Description = reader["Description"].AsString()
           };

        private object[] Take(Category category)
        {
            return new object[]  
            {
                "@CategoryId", category.CategoryId,
                "@CategoryName", category.CategoryName,
                "@Description", category.Description
            };
        }
    }
}
