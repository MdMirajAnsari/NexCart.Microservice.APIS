# API Controllers Summary

This document lists all available controllers in the NexCart Microservices APIs.

## Users API Controllers

**Base URL:** `http://localhost:9090/api`

### AuthController (`/api/auth`)
- **POST** `/api/auth/register` - Register a new user
  - Request Body: `RegisterRequest` (email, password, personName, gender)
  - Response: `ApiResponse<AuthenticationResponse>`
  
- **POST** `/api/auth/login` - Login user
  - Request Body: `LoginRequest` (email, password)
  - Response: `ApiResponse<AuthenticationResponse>`

### UsersController (`/api/users`)
- **GET** `/api/users/{userID}` - Get user by ID
  - Path Parameter: `userID` (Guid)
  - Response: `ApiResponse<UserDTO>`

## Products API Controllers

**Base URL:** `http://localhost:8080/api`

### ProductsController (`/api/products`)
- **GET** `/api/products` - Get all products
  - Response: `List<ProductResponse>`

- **GET** `/api/products/search/product-id/{productID}` - Get product by ID
  - Path Parameter: `productID` (Guid)
  - Response: `ProductResponse`

- **GET** `/api/products/search/{searchString}` - Search products
  - Path Parameter: `searchString` (string)
  - Response: `List<ProductResponse>`
  - Searches by product name or category

- **POST** `/api/products` - Add a new product
  - Request Body: `ProductAddRequest` (productName, category, unitPrice, quantityInStock)
  - Response: `ProductResponse`

- **PUT** `/api/products` - Update a product
  - Request Body: `ProductUpdateRequest` (productID, productName, category, unitPrice, quantityInStock)
  - Response: `ProductResponse`

- **DELETE** `/api/products/{ProductID}` - Delete a product
  - Path Parameter: `ProductID` (Guid)
  - Response: `boolean`

## Orders API Controllers

**Base URL:** `http://localhost:8080/api`

### OrdersController (`/api/orders`)
- **GET** `/api/orders` - Get all orders
  - Response: `List<OrderResponse>`

- **GET** `/api/orders/{id}` - Get order by ID
  - Path Parameter: `id` (Guid)
  - Response: `OrderResponse`

- **POST** `/api/orders` - Create a new order
  - Request Body: `OrderAddRequest` (userID, orderDate, orderItems)
  - Response: `OrderResponse`

## Data Models

### User Models
- `UserDTO`: userId, email, personName, gender
- `RegisterRequest`: email, password, personName, gender
- `LoginRequest`: email, password
- `AuthenticationResponse`: token, email, personName, userId
- `ApiResponse<T>`: success, message, data

### Product Models
- `ProductResponse`: productID, productName, category, unitPrice, quantityInStock
- `ProductAddRequest`: productName, category, unitPrice, quantityInStock
- `ProductUpdateRequest`: productID, productName, category, unitPrice, quantityInStock
- `CategoryOptions`: Electronics, Clothing, Books, Food, Sports, Other

### Order Models
- `OrderResponse`: orderID, userID, totalBill, orderDate, orderItems, userPersonName, email
- `OrderAddRequest`: userID, orderDate, orderItems
- `OrderItemResponse`: orderItemID, orderID, productID, productName, quantity, unitPrice, totalPrice
- `OrderItemAddRequest`: productID, quantity, unitPrice

## Gateway Routes (Ocelot)

If using the API Gateway (`http://localhost:4000`), routes are prefixed with `/gateway`:

- `/gateway/Users/Auth/register`
- `/gateway/Users/Auth/login`
- `/gateway/Users/{userID}`
- `/gateway/Products`
- `/gateway/Products/{productID}`
- `/gateway/Products/search/product-id/{productID}`
- `/gateway/Products/search/{searchString}`
- `/gateway/Orders`
- `/gateway/Orders/{orderID}`
