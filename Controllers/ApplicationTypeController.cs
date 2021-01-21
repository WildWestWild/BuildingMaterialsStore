using System.Collections.Generic;
using BuildingMaterialsStore.Data;
using BuildingMaterialsStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingMaterialsStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController: Controller
    {
        private readonly ApplicationDBContext _db;
        
        public ApplicationTypeController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> collection = _db.ApplicationTypes;
            return View(collection);
        }

        // GET: Create
        public IActionResult Create()
        {
            
            return View();
        }
        
        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationTypes.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        
        // GET-update
        public IActionResult Edit(int? id)
        {
            if (id != null && id != 0)
            {
                return View();
            }
            return NotFound();
        }
        
        //Post-update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationTypes.Update(applicationType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationType);
        }
        
        //Get-delete
        public IActionResult Delete(int? id)
        {
            if (id != null && id != 0)
            {
                var obj = _db.ApplicationTypes.Find(id);
                return View(obj);
            }
            return NotFound();
        }
        
        //Post-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.ApplicationTypes.Find(id);
            if (obj != null)
            {
                _db.ApplicationTypes.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}