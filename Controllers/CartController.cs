using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BuildingMaterialsStore.Data;
using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Models.ViewModel;
using BuildingMaterialsStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingMaterialsStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _db;
        
        [BindProperty]
        public ProductUserVM ProductUserVm { get; set; }

        public CartController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var prodList = GetProdListByShoppingCart();
            return View(prodList);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction("Summary");
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var prodList = GetProdListByShoppingCart();
            ProductUserVm = new ProductUserVM
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u=>u.Id == claim.Value),
                ProductList = prodList
            };
            return View(ProductUserVm);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(ProductUserVM productUserVm)
        {
            return View(productUserVm);
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remove(int id)
        {
            var shoppingCarts = GetListCartBySession();
            shoppingCarts.Remove(shoppingCarts.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction("Index");
        }

        private List<ShoppingCart> GetListCartBySession()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            var contextCart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);
            if (contextCart != null && contextCart.Any())
            {
                shoppingCarts = contextCart.ToList();
            }
            return shoppingCarts;
        }

        private IEnumerable<Product> GetProdListByShoppingCart()
        {
            List<int> prodInCart = GetListCartBySession().Select(i => i.ProductId).ToList();
            return _db.Product.Where(u => prodInCart.Contains(u.Id));
        }
    }
}