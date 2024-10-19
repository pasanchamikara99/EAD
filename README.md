# E-Commerce Web API with .NET and MongoDB

An e-commerce platform backend built using .NET Core, with MongoDB as the database. This project provides RESTful APIs for managing products, orders, customers, and vendors, which can be used by frontend applications or mobile apps.

## Features

- **Customer Management**: Add, update, delete, and retrieve customer details.
- **Vendor Management**: Manage vendor information.
- **Product Management**: Add, update, delete, and retrieve products.
- **Order Management**: Handle customer orders, including placing and tracking orders.
- **MongoDB Integration**: Uses MongoDB for efficient storage and retrieval of e-commerce data.
- **Error Handling**: Global exception handling for a robust API.
- **Dashboard**: API to get total counts of customers, vendors, orders, and products.

## Tech Stack

- **Backend Framework**: ASP.NET Core
- **Database**: MongoDB
- **API Documentation**: Swagger
- **Dependency Injection**: Built-in .NET Core DI

## Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET 6.0 or higher installed
- MongoDB installed or a MongoDB cloud instance (MongoDB Atlas)
- Visual Studio or Visual Studio Code
- Postman or cURL for API testing
- Docker (optional for containerization)

## Getting Started

Follow these instructions to set up the project locally.

### 1. Clone the Repository

```bash
git clone https://github.com/pasanchamikara99/EAD.git
cd EAD

```
### 2. Configure the MongoDB Connection
Go to the appsettings.json file and set your MongoDB connection string

```
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ECommerceDb"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKeyHere",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceAPIUser"
  }
}

```

### 3. Install Dependencies
Run the following command to restore .NET packages:

```
dotnet restore
```

### 4. Run the Application
To run the API, use the following command:

```
dotnet run
```
The API will be accessible at https://localhost:7022 (HTTPS) or http://localhost:7022 (HTTP).

### 5. API Documentation
Once the API is running, you can access Swagger documentation at:

```
https://localhost:7022/swagger/index.html
```


### API Endpoints
The following endpoints are available for interacting with the e-commerce platform:

#### Customer Endpoints
- GET /api/customers: Get all customers
- GET /api/customers/{id}: Get a specific customer by ID
- POST /api/customers: Add a new customer
- PUT /api/customers/{id}: Update a customer
- DELETE /api/customers/{id}: Delete a customer

#### Vendor Endpoints
 - GET /api/vendors: Get all vendors
 - GET /api/vendors/{id}: Get a specific vendor by ID
 - POST /api/vendors: Add a new vendor
 - PUT /api/vendors/{id}: Update a vendor
 - DELETE /api/vendors/{id}: Delete a vendor
#### Product Endpoints
 - GET /api/products: Get all products
 - GET /api/products/{id}: Get a specific product by ID
 - POST /api/products: Add a new product
 - PUT /api/products/{id}: Update a product
 - DELETE /api/products/{id}: Delete a product
#### Order Endpoints
 - GET /api/orders: Get all orders
 - GET /api/orders/{id}: Get a specific order by ID
 - POST /api/orders: Create a new order
 - PUT /api/orders/{id}: Update an order
 - DELETE /api/orders/{id}: Cancel an order
#### Dashboard Endpoints
 - GET /api/dashboardcounts: Get the total number of customers, orders, vendors, and products.

### Testing the API
You can test the API endpoints using tools like Postman or cURL.

Example POST request to add a new customer using Postman:

```
{
  "customerName": "John Doe",
  "email": "johndoe@example.com",
  "status": "Active",
  "password": "password123"
}

```

### Contact
For any questions or feedback, feel free to contact the me:

 - Name: Pasan Chamikara
 - Email: pasanchamikara989@gmail.com
 - LinkedIn: Pasan Chamikara





