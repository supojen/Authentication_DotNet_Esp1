using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace esp1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]  // This Action Atttirbute will ask a question that are you allow to run this action??
        public IActionResult Secret()
        {
            return View();
        }

        public async Task<IActionResult> Authenticate()
        {
            /*
             * Processing - 
             *      Creating an User (Claim Principal)
             */
            var grandmaClaims = new List<Claim>()
            { 
                new Claim(ClaimTypes.Name,  "Po"),
                new Claim(ClaimTypes.Email, "brian71742@gmail.com"),
                new Claim("Grandma.Says","Very Nice Boy")
            };
            var claimIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var claimPrincipal = new ClaimsPrincipal(new[] { claimIdentity });

            /*
             * Processing -
             *      Sign In with the Claim Principal
             */
            await HttpContext.SignInAsync(claimPrincipal);

            return RedirectToAction("Index");
        }
    }
}
