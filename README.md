# ~ MyBookPlanner API 📖

This is a book catalog API built with **ASP.NET 10**, **Entity Framework Core 10**, and **SQLite**. The API allows users to create accounts, log in, and add books from the catalog to their personal library. Each saved book can have a score and a reading status ("read", "reading", or "wish to read"). The average book rating is also calculated based on user reviews.

The main focus of this project is to **practice layered architecture and clean, maintainable code**, rather than complex business rules. SQLite is used instead of SQL Server to simplify running the project locally, without needing a database server.  

Authentication now also supports **policies** for more granular control. The project is prepared for **unit testing** and includes placeholders for **AI-based book recommendations**.

---

## Technologies Used

- **ASP.NET 10** – Web framework for building APIs and web applications on .NET.  
- **Entity Framework Core 10** – Object-relational mapper (ORM) for database access using C# objects.  
- **SQLite** – Lightweight relational database used for simplicity.  

---

## Features

### User Authentication
- Users can create an account and log in using JWT tokens.  
- Policies are used to enforce more granular access control.  

### Book Management
- Users can add, update, and remove books from their personal library.  
- Each book can have a score and reading status.  
- Average scores for books are updated automatically when marked as "read".  

### Profile Management
- Users can update their profile information, including username, biography, and email address.  

### Library Statistics
- Users can see statistics of their library: books being read, already read, or wish-to-read.  
- Users can see their best-rated book.  

### AI Book Recommendation
- Prepared for AI-based book suggestions.  

---

## Documentation

All API endpoints are fully documented via **Swagger UI**, which is available when running the application.  

---

## Architecture Overview

The project follows a **layered architecture**, organized into the following layers:

1. **Domain** – Contains core entities and configuration objects such as `User`, `Book`, `UserBook`, and settings classes like `JwtSettings`.  
2. **Repository** – Handles database access and mapping. Implements the **Repository Pattern** to encapsulate EF Core queries and provide a clean interface for the service layer.  
3. **Service** – Contains business logic, validations, and higher-level operations. Uses the **Service Layer Pattern** to orchestrate actions between repositories and other services.  
4. **WebAPI** – Contains controllers and endpoint definitions. Responsible for HTTP handling, routing, and input/output models.  
5. **Utils** – Helper classes and utilities used across the project (e.g., Mappers).  
6. **Tests** – Contains unit tests for services, repositories, and other layers. Services and repositories are injected via interfaces to enable easy testing.

### Design Patterns Used

- **Repository Pattern** – Separates data access logic from business logic, making the application easier to maintain and test.  
- **Service Layer Pattern** – Provides a layer that encapsulates business logic and orchestrates operations between repositories.  
- **Result Pattern** – Used in service methods to return a standardized result object, containing success/failure status and messages, improving error handling and testability.  

This architecture allows for **clean separation of concerns**, easier unit testing, and a foundation for scaling the application in the future.

---

## Testing

- The project is prepared for **unit tests** using dependency injection and service interfaces.  
- Services like `UserBookService` and `TokenService` are designed to be easily testable.  

---

## Contribution

Contributions are welcome! Feel free to open issues or submit pull requests to improve this project.  

---

## License

This project is licensed under the MIT License. See the LICENSE file for more details.
