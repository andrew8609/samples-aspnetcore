﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Okta.Idx.Sdk;
using okta_social_login_example.Models;
using okta_social_login_example.Okta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace okta_social_login_example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIdxClient _idxClient;

        public HomeController(IIdxClient idxClient, ILogger<HomeController> logger)
        {
            _logger = logger;
            _idxClient = idxClient;
        }

        public async Task<IActionResult> SocialSignIn()
        {
            SocialLoginResponse socialLoginSettings = await this._idxClient.StartSocialLoginAsync(HttpContext);
            return View(socialLoginSettings);
        }

        public async Task<IActionResult> Profile()
        {
            return View(HttpContext.User.Claims.ToArray());
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
