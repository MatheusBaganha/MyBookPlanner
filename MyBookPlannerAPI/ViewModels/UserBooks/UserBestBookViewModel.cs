﻿namespace MyBookPlannerAPI.ViewModels.UserBooks
{
    public class UserBestBookViewModel
    {
        public int IdUser { get; set; }

        public int IdBook { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int ReleaseYear { get; set; }

        public string ImageUrl { get; set; }

        public float UserScore { get; set; }

    }
}
