using ActionCommandGame.Ui.MvcApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace ActionCommandGame.Ui.MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityService<AuthenticationResult> _identitySdk;
        private readonly ITokenStore _tokenStore;
        private readonly IItemService _itemSdk;

        public HomeController(
            IIdentityService<AuthenticationResult> identitySdk, 
            ITokenStore tokenStore, 
            IItemService itemSdk)
        {
            _identitySdk = identitySdk;
            _tokenStore = tokenStore;
            _itemSdk = itemSdk;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserSignInRequest request)
        {
            var signInResult = await _identitySdk.SignIn(request);
            if (!signInResult.Success)
            {
                if (signInResult.Errors is not null)
                {
                    foreach (var error in signInResult.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                return View(request);
            }

            var token = signInResult.Token;
            if (token is null || string.IsNullOrWhiteSpace(request.Email))
            {
                ModelState.AddModelError("", "Could not sign in.");
                return View();
            }

            //Save token for later use in the API
            await _tokenStore.SaveTokenAsync(token);
            
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, request.Email));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);

            return RedirectToAction("SignedIn", new { token });
        }

        public IActionResult SignedIn(string token)
        {
            return View("SignedIn", token);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _tokenStore.SaveTokenAsync(string.Empty);
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Shop()
        {
            var result = await _itemSdk.Find();
            if (!result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View(result.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}