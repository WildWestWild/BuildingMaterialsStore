using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuildingMaterialsStore.Data;
using BuildingMaterialsStore.Models;
using BuildingMaterialsStore.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BuildingMaterialsStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController: Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static string Upload { get; set; }

        public ProductController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product
                .Include(u => u.Category)
                .Include(u => u.ApplicationType);
            
            // IEnumerable<Product> objList = _db.Product;
            //
            // foreach (var obj in objList)
            // {
            //     // Добавим объект Category в продукт
            //     obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //     // Добавим объект ApplicationType в продукт
            //     obj.ApplicationType = _db.ApplicationTypes.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
            // }
            return View(objList);
        }


        // Get-метод upsert
        public IActionResult Upsert(int? id)
        {
            var productVm = new ProductVM();
            if (id == null)
            {
                productVm.Product = new Product();
            }
            else
            {
                productVm.Product = _db.Product.Find(id);
                if (productVm.Product == null)
                {
                    NotFound();
                }
            }
            CreateDropDownLists(productVm);
            return View(productVm);
        }

        private void CreateDropDownLists(ProductVM productVm)
        {
            productVm.ProductListItem = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            productVm.ApplicationListItem = _db.ApplicationTypes.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        //Post - upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVm)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVm.Product.Id == 0)
                {
                    CreateImageLikeFile(productVm, webRootPath, files);
                    _db.Product.Add(productVm.Product);
                }
                else
                {
                    //Update
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVm.Product.Id);
                    if (objFromDb == null)
                    {
                        return View(productVm);
                    }

                    if (files.Count > 0)
                    {
                        CreateImageLikeFile(productVm, webRootPath, files);
                        // Delete old file

                        DeleteOldFile(objFromDb);
                    }
                    else
                    {
                        productVm.Product.Image = objFromDb.Image;
                    }

                    _db.Product.Update(productVm.Product);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            CreateDropDownLists(productVm);
            return View(productVm);
        }

        private static void CreateImageLikeFile(ProductVM productVm, string webRootPath, IFormFileCollection files)
        {
            Upload = webRootPath + WC.ImagePath;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(Upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            productVm.Product.Image = fileName + extension;
        }

        // GET-метод для удаления
        public IActionResult Delete(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }
            // В продукте нет ссылки на Категорию
            Product product = _db.Product
                .Include(u => u.Category)
                .Include(u => u.ApplicationType)
                .FirstOrDefault(u => u.Id == id); // Include - Eager Loading (жадная загрузка)
            return View(product);
        }
        
        //POST-метод для удаления
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj!=null)
            {
                Upload = _webHostEnvironment.WebRootPath;
                DeleteOldFile(obj);
                
                _db.Product.Remove(obj);
                _db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        private void DeleteOldFile(Product obj)
        {
            var oldFile = Path.Combine(Upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
        }
    }
}