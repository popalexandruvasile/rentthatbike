﻿using System.Web.Mvc;
using RentThatBike.Web.Models;
using ServiceStack;
using ServiceStack.Common;
using ServiceStack.Auth;

namespace RentThatBike.Web.Controllers
{
    public class LoginController : Controller
    {
        public AuthenticateService AuthService { get; set; }
        //
        // GET: /login/
        [HttpGet]
        public ActionResult Index(string redirect)
        {
            var loginData = new LoginData{Redirect = redirect};

            //save the default login details for now
            loginData.EmailAddress = "admin@rentthatbike.com";
            loginData.Password = "admin";
            return View(loginData);
        }

        [HttpPost]
        public ActionResult LogIn(LoginData loginData)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", loginData);
            }

            AuthService.Request = System.Web.HttpContext.Current.ToRequest();
            try
            {
                AuthenticateResponse authResponse = AuthService.Authenticate(new Authenticate
                {
                    UserName = loginData.EmailAddress,
                    Password = loginData.Password,
                    RememberMe = loginData.RememberMe,
                    Continue = loginData.Redirect
                });

                return Redirect(loginData.Redirect);
            }
            catch (HttpError ex)
            {
                if (ex.Status == 401)
                {
                    ModelState.AddModelError("", ex.ErrorCode);
                    return View("Index", loginData);
                }
                throw;
            }
        }
    }
}