using AppServices;
using Data.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUserService _userService;

        public BaseController(IUserService userService)
        {
            _userService = userService;
        }
        public bool IsAuthenticated
        {
            get
            {
                return User.Identity.IsAuthenticated;
            }
        }

        private AppUser _currentUser;
        public AppUser CurrentUser
        {
            get
            {
                return _currentUser ?? 
                    (_currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId()));
            }
        }

        public Guid CurrentPlayerId => CurrentUser?.Players?.Single()?.Id ?? Guid.Empty;

        protected void AddError(string message)
        {
            TempData["ErrorMessage"] = message;
        }

        protected void AddSuccess(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void AddError(IEnumerable<string> messages)
        {
            var message = string.Join("<br/>", messages);
            TempData["ErrorMessage"] = message;
        }

        protected void AddSuccess(IEnumerable<string> messages)
        {
            var message = string.Join("<br/>", messages);
            TempData["SuccessMessage"] = message;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentUser != null && Session["user"] == null)
            {
                Session["user"] = CurrentUser;
                Session["useremail"] = CurrentUser.Email;
                Session["userId"] = CurrentUser.Id;
                Session["playerName"] = _userService.GetPlayerByUserId(CurrentUser.Id)?.Name;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}