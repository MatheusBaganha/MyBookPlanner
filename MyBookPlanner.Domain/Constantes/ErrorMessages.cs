using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Domain.Constantes
{
    public static class ErrorMessages
    {
        // Generic Errors
        public const string GenericError = "Internal Error";
        public const string ErrorOnCreating = "Failed to create the record.";

        // Books
        public const string BookNotFound = "This book was not found.";
        public const string NoMoreBooks = "There is no more books available.";

        // User
        public const string UserNotFound = "User email or password is invalid.";
        public const string UserEmailAlreadyExists = "Someone using that email adress already exists.";

        
    }
}
