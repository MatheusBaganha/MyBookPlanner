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
        public const string ErrorOnUpdating = "Failed to update the record.";
        public const string ErrorOnDeleting = "Failed to delete the record.";

        // Books
        public const string BookNotFound = "This book was not found.";
        public const string NoMoreBooks = "There is no more books available.";
        
        // User
        public const string InvalidUser = "User email or password is invalid.";
        public const string UserNotFound = "User not found.";
        public const string UserEmailAlreadyExists = "Someone using that email adress already exists.";
        public const string UserBookAlreadyExists = "User already has that book.";
        public const string UserBookStatusInvalid = "Send a valid reading status: 'LIDO', 'LENDO', 'DESEJO'.";




    }
}
