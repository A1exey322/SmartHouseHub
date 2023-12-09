using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartHouseHub.Domain.Identity;
using SmartHouseHub.Models.Customer;

namespace SmartHouseHub.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        public CustomerController(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CustomerRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    FistName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToLower().Trim(),
                    UserName = model.Email.ToLower().Trim(),
                    PhoneNumber = model.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(customer, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                await _signInManager.SignInAsync(customer, false);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Login error", "Incorrect login or password");
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
