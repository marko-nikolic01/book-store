using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Book> objBookList = _unitOfWork.BookRepository.GetAll().ToList();
            return View(objBookList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.BookRepository.Add(book);
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

            Book book = _unitOfWork.BookRepository.GetFirstOrDefault(o => o.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.BookRepository.Update(book);
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

            Book book = _unitOfWork.BookRepository.GetFirstOrDefault(o => o.Id == id);
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
            _unitOfWork.BookRepository.Remove(book);
            _unitOfWork.Save();
            TempData["success"] = "Book deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
