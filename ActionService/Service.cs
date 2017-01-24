using BusinessObjects;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using WebMatrix.WebData;

namespace ActionService {
    // implementation of IService interface. It can handle different data providers.

    // ** Facade pattern.
    // ** Repository pattern (Service could be split up in individual Repositories: Product, Category, etc).

    public class Service : IService
    {
        private static readonly string Provider = ConfigurationManager.AppSettings.Get("DataProvider");
        private static readonly IDaoFactory Factory = DaoFactories.GetFactory(Provider);

        private static readonly ICategoryDao CategoryDao = Factory.CategoryDao;
        private static readonly IProductDao ProductDao = Factory.ProductDao;
        private static readonly IMemberDao MemberDao = Factory.MemberDao;
        private static readonly IOrderDao OrderDao = Factory.OrderDao;
        private static readonly IOrderDetailDao OrderDetailDao = Factory.OrderDetailDao;

        // Category Services

        public List<Category> GetCategories() 
        { 
            return CategoryDao.GetCategories(); 
        }

        public Category GetCategoryByProduct(int productId)
        {
            return CategoryDao.GetCategoryByProduct(productId);
        }

        // Product Services

        public Product GetProduct(int productId)
        {
            var product =  ProductDao.GetProduct(productId);
            if (product.Category == null) 
                product.Category = CategoryDao.GetCategoryByProduct(productId);

            return product;
        }

        public List<Product> GetProductsByCategory(int categoryId, string sortExpression)
        {
            return ProductDao.GetProductsByCategory(categoryId, sortExpression);
        }

        public List<Product> SearchProducts(string productName, double priceFrom, double priceThru, string sortExpression)
        {
            return ProductDao.SearchProducts(productName, priceFrom, priceThru, sortExpression);
        }

        // Member Services

        public Member GetMember(int memberId)
        {
            return MemberDao.GetMember(memberId);
        }

        public Member GetMemberByEmail(string email)
        {
            return MemberDao.GetMemberByEmail(email);
        }

        public List<Member> GetMembers(string sortExpression)
        {
            var members = MemberDao.GetMembers(sortExpression);
            members.RemoveAll(m => m.MemberId == 1);  // exclude admin (for demo purposes)
            return members;
        }

        public Member GetMemberByOrder(int orderId)
        {
            return MemberDao.GetMemberByOrder(orderId);
        }

        public List<Member> GetMembersWithOrderStatistics(string sortExpression)
        {
            return MemberDao.GetMembersWithOrderStatistics(sortExpression);
        }

        public void InsertMember(Member member)
        {
            MemberDao.InsertMember(member);
        }

        public void UpdateMember(Member member)
        {
            MemberDao.UpdateMember(member);
        }

        public void DeleteMember(Member member)
        {
            MemberDao.DeleteMember(member);
        }

        // Order Services

        public Order GetOrder(int orderId)
        {
            return OrderDao.GetOrder(orderId);
        }

        public List<Order> GetOrdersByMember(int memberId)
        {
            return OrderDao.GetOrdersByMember(memberId);
        }

        public List<Order> GetOrdersByDate(DateTime dateFrom, DateTime dateThru)
        {
            return OrderDao.GetOrdersByDate(dateFrom, dateThru);
        }

        // OrderDetail Services

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            return OrderDetailDao.GetOrderDetails(orderId);
        }

        // Authentication and Authorization Services

        public bool Login(string email, string password)
        {
            // websecurity does not accept null or empty

            if (string.IsNullOrEmpty(email)) return false;
            if (string.IsNullOrEmpty(password)) return false;

            return WebSecurity.Login(email, password);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }
    }
}
