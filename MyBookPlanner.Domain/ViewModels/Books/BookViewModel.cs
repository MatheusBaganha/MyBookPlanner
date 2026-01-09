using System;
using System.Collections.Generic;
using System.Text;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Domain.ViewModels.Books
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
        public string ImageUrl { get; set; }
        public float Score { get; set; }

    }
}
