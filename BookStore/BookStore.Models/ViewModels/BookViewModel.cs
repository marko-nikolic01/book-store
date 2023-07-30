using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IFormFile? Image { set; get; }
    }
}
