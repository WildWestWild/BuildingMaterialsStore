using System.Collections.Generic;
using BuildingMaterialsStore.Data;
using BuildingMaterialsStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingMaterialsStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController: Controller
    {
        public readonly ApplicationDBContext _db;

        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Category;
            return View(objList);
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        
        // GET-метод для редактирования
        // Из тэг хелпера
        public IActionResult Edit(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }

            var obj = _db.Category.Find(id);
            return View();
        }
        
        // POST-метод для редактирования
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET-метод для удаления
        public IActionResult Delete(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }
            var obj = _db.Category.Find(id);
            return View(obj);
        }
        
        //POST-метод для удаления
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            if (obj!=null)
            {
                _db.Category.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}