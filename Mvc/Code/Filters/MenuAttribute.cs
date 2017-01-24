using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Code
{
    // sets selected menu item
    
    public class MenuAttribute : ActionFilterAttribute
    {
        private readonly MenuItem _selectedMenu;

        public MenuAttribute(MenuItem selectedMenu)
        {
            _selectedMenu = selectedMenu;
        }

        // sets selected menu in ViewData

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["SelectedMenu"] = _selectedMenu.ToString().ToLower();
        }
    }
}