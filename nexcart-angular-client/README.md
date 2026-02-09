# NexCart Angular Client

An Angular frontend application that integrates with the NexCart Microservices APIs (Users, Products, and Orders).

## Available API Controllers

### Users API (`http://localhost:9090/api`)

#### AuthController
- **POST** `/api/auth/register` - Register a new user
- **POST** `/api/auth/login` - Login user

#### UsersController
- **GET** `/api/users/{userID}` - Get user by ID

### Products API (`http://localhost:8080/api`)

#### ProductsController
- **GET** `/api/products` - Get all products
- **GET** `/api/products/search/product-id/{productID}` - Get product by ID
- **GET** `/api/products/search/{searchString}` - Search products by name or category
- **POST** `/api/products` - Add a new product
- **PUT** `/api/products` - Update a product
- **DELETE** `/api/products/{productID}` - Delete a product

### Orders API (`http://localhost:8080/api`)

#### OrdersController
- **GET** `/api/orders` - Get all orders
- **GET** `/api/orders/{id}` - Get order by ID
- **POST** `/api/orders` - Create a new order

## Features

- **Products Management**: Browse, search, add, and delete products
- **Orders Management**: View orders and create new orders
- **User Management**: Search for users by ID
- **Authentication**: User registration and login

## Getting Started

### Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Angular CLI (installed globally or via npx)

### Installation

```bash
npm install
```

### Development Server

```bash
ng serve
```

Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

### Build

```bash
ng build
```

The build artifacts will be stored in the `dist/` directory.

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── products/      # Products component
│   │   ├── orders/        # Orders component
│   │   ├── users/         # Users component
│   │   └── auth/          # Login and Register components
│   ├── models/            # TypeScript interfaces/models
│   │   ├── user.model.ts
│   │   ├── product.model.ts
│   │   └── order.model.ts
│   ├── services/          # API services
│   │   ├── users.service.ts
│   │   ├── products.service.ts
│   │   └── orders.service.ts
│   ├── app.ts            # Root component
│   ├── app.routes.ts     # Routing configuration
│   └── app.config.ts     # App configuration
```

## API Configuration

The API base URLs are configured in the service files:
- Users API: `http://localhost:9090/api`
- Products API: `http://localhost:8080/api`
- Orders API: `http://localhost:8080/api`

Make sure these APIs are running before using the application.

## Usage

1. **Register/Login**: Use the Register or Login pages to authenticate
2. **Browse Products**: Navigate to Products to view and search products
3. **Create Orders**: Go to Orders to create new orders
4. **View Users**: Use the Users page to search for users by ID

## Notes

- The application uses standalone components (Angular 17+)
- CORS must be enabled on the backend APIs for the frontend to communicate with them
- Authentication tokens are stored in localStorage (if provided by the API)
