using System;
using System.Collections.Generic;
using System.Text;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Domain.DTO
{
    public class UserBookDTO
    {
        public int IdUser { get; set; }
        public int IdBook { get; set; }
        public float UserScore { get; set; }
        public string ReadingStatus { get; set; }

    }
}
