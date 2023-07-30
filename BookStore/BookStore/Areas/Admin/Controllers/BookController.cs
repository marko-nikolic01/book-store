using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Book> objBookList = _unitOfWork.BookRepository.GetAll(includedProperties: "Category").ToList();
            return View(objBookList);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository.GetAll()
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                });

            BookViewModel bookViewModel = new BookViewModel
            {
                Categories = CategoryList,
                Book = new Book()
            };
            return View(bookViewModel);
        }

        [HttpPost]
        public IActionResult Create(BookViewModel bookViewModel, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\book");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    bookViewModel.Book.ImageUrl = @"\images\book\" + fileName;
                }
                _unitOfWork.BookRepository.Add(bookViewModel.Book);
                _unitOfWork.Save();
                TempData["success"] = "Book created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Book book = _unitOfWork.BookRepository.GetFirstOrDefault(o => o.Id == id, includedProperties: "Category");
            if (book == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository.GetAll()
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                });

            BookViewModel bookViewModel = new BookViewModel
            {
                Categories = CategoryList,
                Book = book
            };
            return View(bookViewModel);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel bookViewModel, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (image != null)
                {
                    if (!string.IsNullOrEmpty(bookViewModel.Book.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, bookViewModel.Book.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\book");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    bookViewModel.Book.ImageUrl = @"\images\book\" + fileName;
                }
                _unitOfWork.BookRepository.Update(bookViewModel.Book);
                _unitOfWork.Save();
                TempData["success"] = "Book edited successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Book book = _unitOfWork.BookRepository.GetFirstOrDefault(o => o.Id == id, includedProperties: "Category");
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteFromDb(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Book book = _unitOfWork.BookRepository.GetFirstOrDefault(o => o.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                string oldImagePath = Path.Combine(wwwRootPath, book.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.BookRepository.Remove(book);
            _unitOfWork.Save();
            TempData["success"] = "Book deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
