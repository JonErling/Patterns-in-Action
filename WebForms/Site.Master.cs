﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebForms.Code;
using WebForms.Controls;

namespace WebForms
{
    public partial class SiteMaster : MasterPage
    {
        // MS autogenerated code

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? string.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? string.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        
        // establishes the composite menu hierarchy which is present on all pages.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // build the composite menu tree
                // this tree implements the Composite Design Pattern

                var root = new MenuCompositeItem("root", null);
                var home = new MenuCompositeItem("home", UrlMaker.ToDefault());
                var shop = new MenuCompositeItem("shopping", UrlMaker.ToShopping());
                var prod = new MenuCompositeItem("products", UrlMaker.ToProducts());
                var srch = new MenuCompositeItem("search", UrlMaker.ToSearch());
                var admn = new MenuCompositeItem("administration", UrlMaker.ToAdmin());
                var cust = new MenuCompositeItem("members", UrlMaker.ToMembers());
                var ordr = new MenuCompositeItem("orders", UrlMaker.ToOrders());

                MenuCompositeItem auth;
                if (Request.IsAuthenticated)
                    auth = new MenuCompositeItem("logout", UrlMaker.ToLogout());
                else
                    auth = new MenuCompositeItem("login", UrlMaker.ToLogin());

                shop.Children.Add(prod);
                shop.Children.Add(srch);
                admn.Children.Add(cust);
                admn.Children.Add(ordr);
                root.Children.Add(home);
                root.Children.Add(shop);
                root.Children.Add(admn);
                root.Children.Add(auth);


                TheMenuComposite.MenuItems = root;
            }
        }

        
        // gets the menu from the master page. This property makes the menu 
        // accessible from contentplaceholders. This allows the individual pages 
        // to set the selected menu item.
        
        public MenuComposite TheMenuInMasterPage
        {
            get { return TheMenuComposite; }
        }
    }
}