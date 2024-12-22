# Clean Architecture for Restaurants Management API

This project implements a clean architecture pattern for a Restaurants Management API. The architecture is designed for scalability, maintainability, and testability, adhering to domain-driven design principles. The API provides endpoints for managing restaurants, dishes, user favorites, and authentication, with a focus on separation of concerns and modularity.

---

## Table of Contents

- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Endpoints Overview](#endpoints-overview)
  - [Restaurants Endpoints](#restaurants-endpoints)
  - [Dishes Endpoints](#dishes-endpoints)
  - [Favorites Endpoints](#favorites-endpoints)
  - [Identity Endpoints](#identity-endpoints)
- [Authentication and Authorization](#authentication-and-authorization)
- [Pagination, Sorting, and Searching](#pagination-sorting-and-searching)
- [How to Run](#how-to-run)
- [Future Enhancements](#future-enhancements)

---

## Overview

The **Restaurants Management API** is a backend system built with **Clean Architecture** principles. It enables users to:
- Manage restaurants and their dishes.
- Mark restaurants and dishes as favorites.
- Handle user roles and identity management.
- Secure operations with role-based access control (RBAC) and custom authorization policies.

The project follows **CQRS (Command Query Responsibility Segregation)** with **MediatR**, ensuring clear separation between write and read operations.

---

## Technologies Used

- **ASP.NET Core 8**: Framework for building RESTful APIs.
- **Entity Framework Core**: ORM for database interactions.
- **MediatR**: Implements CQRS pattern.
- **FluentValidation**: Provides robust input validation.
- **AutoMapper**: Simplifies object-to-object mapping.
- **Serilog**: Advanced logging system.
- **Swagger/OpenAPI**: API documentation and testing.
- **SQL Server**: Relational database.

---

## Project Structure

The solution is organized into distinct layers:

### **Domain Layer**
- Contains core entities, enums, and interfaces.
- Independent of other layers; represents the business logic and rules.

### **Application Layer**
- Houses application logic, DTOs, commands, queries, and validators.
- Implements business use cases using **CQRS**.

### **Infrastructure Layer**
- Manages database access and third-party service integrations.
- Implements repository patterns.

### **API Layer**
- Contains controllers for handling HTTP requests.
- Serves as the entry point for clients.

---

## Endpoints Overview

### Restaurants Endpoints

#### Base URL: `/api/Restaurant`

1. **GET /api/Restaurant**
   - Retrieves all restaurants.
   - **Response**: `200 OK` (List of restaurants).

2. **GET /api/Restaurant/{restaurantId}**
   - Retrieves a restaurant by ID.
   - **Response**: `200 OK` (Restaurant details) or `404 Not Found`.

3. **POST /api/Restaurant**
   - Creates a new restaurant.
   - **Request**: Restaurant creation details.
   - **Response**: `201 Created` or `400 Bad Request`.

4. **PATCH /api/Restaurant/{restaurantId}**
   - Updates restaurant details.
   - **Response**: `204 No Content` or `404 Not Found`.

5. **DELETE /api/Restaurant/{restaurantId}**
   - Deletes a restaurant by ID.
   - **Response**: `204 No Content` or `404 Not Found`.

---

### Dishes Endpoints

#### Base URL: `/api/Restaurant/{restaurantId}/Dish`

1. **GET /api/Restaurant/{restaurantId}/Dish**
   - Retrieves all dishes for a specific restaurant.
   - **Response**: `200 OK` (List of dishes).

2. **GET /api/Restaurant/{restaurantId}/Dish/{dishId}**
   - Retrieves a specific dish by ID.
   - **Response**: `200 OK` (Dish details) or `404 Not Found`.

3. **POST /api/Restaurant/{restaurantId}/Dish**
   - Creates a new dish.
   - **Request**: Dish creation details.
   - **Response**: `201 Created` or `400 Bad Request`.

4. **PATCH /api/Restaurant/{restaurantId}/Dish/{dishId}**
   - Updates a dish.
   - **Response**: `204 No Content` or `404 Not Found`.

5. **DELETE /api/Restaurant/{restaurantId}/Dish`
   - Deletes all dishes for a restaurant.
   - **Response**: `204 No Content` or `404 Not Found`.

6. **DELETE /api/Restaurant/{restaurantId}/Dish/{dishId}**
   - Deletes a specific dish by ID.
   - **Response**: `204 No Content` or `404 Not Found`.

---

### Favorites Endpoints

#### Base URL: `/api/Favorites`

1. **GET /api/Favorites**
   - Retrieves the authenticated user’s favorite restaurants.
   - **Response**: `200 OK` (List of favorite restaurants).

2. **POST /api/Favorites/Restaurant/{restaurantId}**
   - Adds a restaurant to the user’s favorites.
   - **Response**: `204 No Content` or `404 Not Found`.

3. **DELETE /api/Favorites/Restaurant/{restaurantId}**
   - Removes a restaurant from the user’s favorites.
   - **Response**: `204 No Content` or `404 Not Found`.

4. **POST /api/Favorites/Dish/{dishId}**
   - Adds a dish to the user’s favorites.
   - **Response**: `204 No Content` or `404 Not Found`.

5. **DELETE /api/Favorites/Dish/{dishId}**
   - Removes a dish from the user’s favorites.
   - **Response**: `204 No Content` or `404 Not Found`.

---

### Identity Endpoints (Minimal API)

#### Base URL: `/api/Identity`

1. **PATCH /api/Identity/User**
   - Updates user details.
   - **Response**: `204 No Content` or `400 Bad Request`.

2. **POST /api/Identity/UserRole**
   - Assigns a role to a user.
   - **Response**: `204 No Content` or `400 Bad Request`.

3. **DELETE /api/Identity/UserRole**
   - Removes a role from a user.
   - **Response**: `204 No Content` or `400 Bad Request`.

These endpoints leverage ASP.NET Core's **Minimal API** to simplify authentication and role management.

---

## Authentication and Authorization

### Role-Based Access Control (RBAC)

- **Admin**: Full access to all endpoints.
- **User**: Access to user-specific and public endpoints.
- **Owner**: Access to resources they manage.

### Claim-Based Access Control

1. **Nationality**: Restricts access based on user nationality claims.
2. **Birthdate**: Validates user’s age before granting access.

### Resource-Based Authorization

- Ensures users can only perform actions on resources they own or are explicitly allowed to access.

---

## Pagination, Sorting, and Searching

The API supports pagination, sorting, and searching for optimized data retrieval. This functionality is implemented in endpoints such as `/api/Restaurant`.

### Example Request
```http
GET /api/Restaurant?pageNumber=2&pageSize=5&searchphrase='Kfc'&sortby='Category'&sortDirection='Ascending'
```

### Query Parameters
- **pageNumber**: Specifies the page number (default is 1).
- **pageSize**: Specifies the number of records per page (default is 10).
- **searchphrase**: Filters results based on a search term.
- **sortby**: Specifies the field to sort by (e.g., `Name`, `Category`).
- **sortDirection**: Specifies the sort order (`Ascending` or `Descending`).

### Response
- **200 OK**: Returns the paginated, sorted, and filtered list of restaurants or dishes.

---

## How to Run

1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```

2. Set up the database connection string in `appsettings.json`.

3. Apply migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Access Swagger UI for API documentation:
   - URL: `https://localhost:5001/swagger`

---

## Future Enhancements

1. Add caching for high-traffic endpoints.
2. Implement advanced search and filtering for restaurants and dishes.
3. Extend the Favorites feature with user-defined categories.
4. Introduce comprehensive unit and integration tests.

---

Feel free to contribute by submitting pull requests or issues!

