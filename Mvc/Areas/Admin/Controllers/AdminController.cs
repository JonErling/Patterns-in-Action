using ActionService;
using AutoMapper;
using BusinessObjects;
using Mvc.Areas.Admin.Models;
using Mvc.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Areas.Admin.Controllers
{

    // controller only allows authenticated administrators

    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private IService Service { get; }

        // static constructor. establishes Automapper object maps

        static AdminController()
        {
            Mapper.CreateMap<Member, MemberModel>()
                .ForMember(dest => dest.LastOrderDate, opt => opt.MapFrom(src => src.LastOrderDate.ToShortDateString()));
            Mapper.CreateMap<MemberModel, Member>();
            Mapper.CreateMap<Order, OrderModel>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToShortDateString()))
                .ForMember(dest => dest.RequiredDate, opt => opt.MapFrom(src => src.RequiredDate.ToShortDateString()))
                .ForMember(dest => dest.Freight, opt => opt.MapFrom(src => string.Format("{0:C}", src.Freight)));

            Mapper.CreateMap<OrderDetail, OrderDetailModel>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => string.Format("{0:C}",src.UnitPrice)))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => ((int)(src.Discount * 100)) + "%"));
        }

        // default constructor, establishes default service

        public AdminController() : this(new Service()) { }

        // overloaded 'injectable' constructor
        // ** Constructor Dependency Injection (DI).

        public AdminController(IService service) { this.Service = service; }

        // administration page

        [HttpGet]
        public ActionResult Administration()
        {
            ViewBag.Crumbs = new List<BreadCrumb>();
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "home", Url = "/" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "administration" });

            ViewBag.Menu = "administration";
            return View();
        }

        // members page

        [HttpGet]
        public ActionResult Members(string sort = "memberid", string order = "desc", string message = null)
        {
            var verb = Request.HttpMethod;
            ViewBag.Crumbs = new List<BreadCrumb>();
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "home", Url = "/" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "administration", Url = "/administration" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "members" });

            ViewBag.Menu = "members";

            var model = new MembersModel { Message = message };
            var members = Service.GetMembers(sort + " " + order);
            var memberModels = Mapper.Map<List<Member>, List<MemberModel>>(members);
            model.Members = new SortedList<MemberModel>(memberModels, sort, order);

            return View(model);
        }

        // member details page

        [HttpGet]
        public ActionResult Member(int id)
        {
            MemberModel model;

            // new memnber
            if (id == 0)
                model = new MemberModel();
            else
                model = Mapper.Map<Member, MemberModel>(Service.GetMember(id));

            ViewBag.Crumbs = new List<BreadCrumb>();
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "home", Url = "/" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "administration", Url = "/administration" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "members", Url = "/members" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = (id == 0 ? "new " : model.CompanyName) });

            ViewBag.Menu = "members";

            // set silhouette image if no image is available
            model.PhotoId = (id > 0 && id < 92) ? id : 0; 

            return View(model);
        }

        // delete a member

        [HttpGet]
        public ActionResult MemberDelete(int id)
        {
            var member = Service.GetMember(id);
            var orders = Service.GetOrdersByMember(id);

            string message;
            if (orders != null && orders.Count > 0)
            {
                message = "Cannot delete member because they have existing orders";
            }
            else
            {
                Service.DeleteMember(member);
                message = "Member has been deleted successfully";
            }

            return RedirectToAction("Members", new { message = message });
        }

        // saves member (new or updated)

        [HttpPost]
        public ActionResult Member(MemberModel model)
        {
            if (ModelState.IsValid)
            {
                var member = Mapper.Map<MemberModel, Member>(model);

                string message;
                if (member.MemberId > 0)
                {
                    Service.UpdateMember(member);
                    message = "Member successfully updated";
                }
                else
                {
                    Service.InsertMember(member);
                    message = "Member successfully added";
                }

                return RedirectToAction("Members", new { message = message });
            }

            ViewBag.Crumbs = new List<BreadCrumb>();
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "home", Url = "/" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "administration", Url = "/administration" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "members" });

            ViewBag.Menu = "members";

            // show with errors
            return View(model);
        }

        // orders page

        [HttpGet]
        public ActionResult Orders(string sort = "memberId", string order = "desc")
        {
            ViewBag.Crumbs = new List<BreadCrumb>();
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "home", Url = "/" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "administration", Url = "/administration" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "orders" });

            ViewBag.Menu = "orders";

            var model = new OrdersModel();
            var members = Service.GetMembersWithOrderStatistics(sort + " " + order);
            var memberModels = Mapper.Map<List<Member>, List<MemberModel>>(members);
            model.Members = new SortedList<MemberModel>(memberModels, sort, order);

            return View(model);
        }

        // displays orders by member

        [HttpGet]
        public ActionResult MemberOrders(int id)
        {
            ViewBag.Crumbs = new List<BreadCrumb>();
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "home", Url = "/" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "administration", Url = "/administration" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "orders", Url = "/orders" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "by member" });

            ViewBag.Menu = "orders";

            var model = new MemberOrdersModel();
            model.Member = Mapper.Map<Member, MemberModel>(Service.GetMember(id));
            model.Orders = Mapper.Map<List<Order>, List<OrderModel>>(Service.GetOrdersByMember(id));

            return View(model);
        }

        // display order line items for a given order

        [HttpGet]
        public ActionResult Order(int id, int oid)
        {
            ViewBag.Crumbs = new List<BreadCrumb>();
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "home", Url = "/" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "administration", Url = "/administration" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "orders", Url = "/orders" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "by member" });
            ViewBag.Crumbs.Add(new BreadCrumb { Title = "details" });

            ViewBag.Menu = "orders";

            var model = new OrderDetailsModel();
            var orderDetails = Service.GetOrderDetails(oid);
            model.OrderDetails = Mapper.Map<List<OrderDetail>, List<OrderDetailModel>>(orderDetails);

            foreach (var detail in model.OrderDetails)
            {
                // using a product cache would be better (although # of db hits is fairly small)
                var product = Service.GetProduct(detail.ProductId);
                detail.ProductName = product.ProductName;
            }

            return View(model);
        }
    }
}
