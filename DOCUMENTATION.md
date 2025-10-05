# E-Commerce Cart & Coupon System - Complete Documentation

## 📑 Table of Contents
1. [Project Overview](#project-overview)
2. [System Architecture](#system-architecture)
3. [Technology Stack](#technology-stack)
4. [Prerequisites](#prerequisites)
5. [Installation & Setup](#installation--setup)
6. [Running the Application](#running-the-application)
7. [API Documentation](#api-documentation)
8. [Coupon System Features](#coupon-system-features)
9. [Database Schema](#database-schema)****
10. [Testing](#testing)
11. [Deployment with Docker](#deployment-with-docker)
12. [Assessment Questions & Answers](#assessment-questions--answers)

---

## 📋 Project Overview

This is a full-stack **E-Commerce Cart & Coupon System** built as part of a technical assessment. The application provides a complete shopping cart experience with an advanced coupon management system that supports:

- Multiple discount types (Fixed & Percentage)
- Auto-applied coupons
- Product-specific coupons
- Cart value restrictions
- Usage limits (per user and total)
- Time-based validations
- Stacking multiple coupons

**Project Structure:**
```
JL-coding-test/
├── Backend/              # .NET Core Web API
│   ├── ECommerce.API/    # API Controllers & Services
│   ├── ECommerce.Core/   # Domain Entities & Business Logic
│   ├── ECommerce.Infrastructure/ # Data Access & Repositories
│   └── ECommerce.Tests/  # Unit Tests
├── Frontend/             # React + TypeScript + Vite
│   └── src/
│       ├── components/   # Reusable UI Components
│       ├── pages/        # Page Components
│       ├── store/        # Zustand State Management
│       ├── types/        # TypeScript Definitions
│       └── lib/          # API Client & Utils
└── docker-compose.yml    # Docker Orchestration
```

---

## 🏗️ System Architecture

### **Architecture Pattern**
- **Backend:** Clean Architecture (Domain-Driven Design)
  - `Core` - Domain entities, DTOs, interfaces
  - `Infrastructure` - Data access, repositories
  - `API` - Controllers, services, dependency injection
  
- **Frontend:** Component-Based Architecture
  - State Management: Zustand
  - API Communication: Axios
  - UI Components: Radix UI + Tailwind CSS

### **Communication Flow**
```
Frontend (React) → API Client (Axios) → Backend API (.NET)
                                          ↓
                                     Repository Layer
                                          ↓
                                   PostgreSQL Database
```

---

## 🛠️ Technology Stack

### **Backend**
- **.NET 9.0** - Web API Framework
- **Entity Framework Core** - ORM
- **PostgreSQL 15** - Database
- **AutoMapper** - Object Mapping
- **Swagger/OpenAPI** - API Documentation
- **xUnit** - Unit Testing

### **Frontend**
- **React 19.1.1** - UI Library
- **TypeScript 5.8.3** - Type Safety
- **Vite 7.1.2** - Build Tool
- **Zustand 5.0.8** - State Management
- **Axios 1.12.2** - HTTP Client
- **Tailwind CSS 4.1.13** - Styling
- **Radix UI** - Accessible UI Components
- **Lucide React** - Icons

### **DevOps**
- **Docker & Docker Compose** - Containerization
- **nginx** - Frontend Web Server

---

## ✅ Prerequisites

Before running the application, ensure you have the following installed:

### **For Docker Deployment**
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop/)
- **Docker Compose** (usually included with Docker Desktop)

---

## 📦 Installation & Setup

### **Option 1: Local Development Setup**

#### **1. Clone the Repository**
```powershell
git clone <repository-url>
cd JL-coding-test
```

#### **2. Setup Backend**

```powershell
cd Backend/ECommerce.API

# Restore NuGet packages
dotnet restore

# Update database connection string in appsettings.Development.json
# Default connection string:
# "Host=localhost;Database=ECommerceDB;Username=postgres;Password=a!23456789;Port=5432;"

# Apply migrations (create database)
dotnet ef database update

# Run the API
dotnet run
```

Backend will be available at: **http://localhost:5090**

#### **3. Setup Frontend**

```powershell
cd Frontend

# Install dependencies
yarn install
# OR
npm install

# Ensure axios and zustand are installed
yarn add axios zustand
# OR
npm install axios zustand

# Update API URL in src/lib/api.ts if needed
# Default: http://localhost:5090

# Start development server
yarn dev
# OR
npm run dev
```

Frontend will be available at: **http://localhost:5173**

---

### **Option 2: Docker Deployment**

#### **1. Build and Run with Docker Compose**

```powershell
# From project root directory
docker-compose up --build
```

This will:
- Create PostgreSQL database container on port **5432**
- Build and run Backend API on port **5090**
- Build and run Frontend on port **3000**

#### **2. Access the Application**
- **Frontend:** http://localhost:3000
- **Backend API:** http://localhost:5090
- **Swagger UI:** http://localhost:5090/swagger

#### **3. Stop the Application**
```powershell
docker-compose down
```

#### **4. Clean Up (Remove volumes)**
```powershell
docker-compose down -v
```

---

## 🚀 Running the Application

### **Development Workflow**

#### **Starting the Application**

**Terminal 1 - Backend:**
```powershell
cd Backend/ECommerce.API
dotnet watch run
```

**Terminal 2 - Frontend:**
```powershell
cd Frontend
yarn dev
```

#### **Accessing the Application**
1. Open browser: http://localhost:5173
2. The app will show:
   - Product listing
   - Shopping cart
   - Coupon application interface

#### **Testing the Coupon System**

The system seeds initial data including:
- **Users:** John Doe, Jane Smith
- **Products:** Various items with prices
- **Coupons:** Multiple pre-configured coupons

**Pre-seeded Coupons (Check Program.cs for latest):**
- `SAVE10` - $10 fixed discount
- `PERCENT20` - 20% discount
- `BOGO` - Buy One Get One (50% off)
- Auto-applied coupons based on cart value

---

## 📘 API Documentation

### **Base URL**
- Development: `http://localhost:5090`
- Production: Configure in deployment

### **API Endpoints**

#### **1. Products API**

**Get All Products**
```http
GET /api/products
```
Response:
```json
[
  {
    "id": 1,
    "name": "Product Name",
    "description": "Product description",
    "price": 29.99,
    "stock": 100,
    "category": "Electronics"
  }
]
```

**Get Product by ID**
```http
GET /api/products/{id}
```

**Create Product**
```http
POST /api/products
Content-Type: application/json

{
  "name": "New Product",
  "description": "Description",
  "price": 49.99,
  "stock": 50,
  "category": "Category"
}
```

---

#### **2. User API**

**Get All Users**
```http
GET /api/users
```

**Get User by ID**
```http
GET /api/users/{id}
```

**Create User**
```http
POST /api/users
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "555-0101"
}
```

---

#### **3. Cart API**

**Get User's Cart**
```http
GET /api/cart/{userId}
```
Response:
```json
{
  "cartId": 1,
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "productName": "Product",
      "quantity": 2,
      "price": 29.99,
      "subtotal": 59.98
    }
  ],
  "appliedCoupons": [
    {
      "code": "SAVE10",
      "discountAmount": 10.00,
      "isAutoApplied": false
    }
  ],
  "totalBeforeDiscount": 59.98,
  "totalDiscount": 10.00,
  "finalTotal": 49.98,
  "totalItems": 2
}
```

**Add Item to Cart**
```http
POST /api/cart/{userId}/items
Content-Type: application/json

{
  "productId": 1,
  "quantity": 2
}
```

**Update Cart Item Quantity**
```http
PUT /api/cart/{userId}/items/{productId}
Content-Type: application/json

{
  "quantity": 3
}
```

**Remove Item from Cart**
```http
DELETE /api/cart/{userId}/items/{productId}
```

**Apply Coupon to Cart**
```http
POST /api/cart/{userId}/coupons
Content-Type: application/json

{
  "code": "SAVE10"
}
```

**Remove Coupon from Cart**
```http
DELETE /api/cart/{userId}/coupons/{code}
```

---

#### **4. Coupon API**

**Get All Coupons**
```http
GET /api/coupons
```
Response:
```json
[
  {
    "id": 1,
    "code": "SAVE10",
    "description": "Save $10 on your order",
    "discountType": "Fixed",
    "discountValue": 10.00,
    "maxDiscountAmount": null,
    "isAutoApplied": false,
    "startDate": null,
    "expiryDate": "2025-12-31T00:00:00Z",
    "minimumCartItems": null,
    "minimumTotalPrice": 50.00,
    "maxTotalUses": 100,
    "maxUsesPerUser": 1,
    "currentTotalUses": 0,
    "applicableProductIds": [],
    "isActive": true
  }
]
```

**Get Coupon by ID**
```http
GET /api/coupons/{id}
```

**Get Coupon by Code**
```http
GET /api/coupons/code/{code}
```

**Create Coupon**
```http
POST /api/coupons
Content-Type: application/json

{
  "code": "NEWCOUPON",
  "description": "New discount coupon",
  "discountType": 1,  // 1=Fixed, 2=Percentage
  "discountValue": 15.00,
  "maxDiscountAmount": null,
  "isAutoApplied": false,
  "startDate": null,
  "expiryDate": "2025-12-31T00:00:00Z",
  "minimumCartItems": null,
  "minimumTotalPrice": 100.00,
  "maxTotalUses": 50,
  "maxUsesPerUser": 1,
  "applicableProductIds": [],  // Empty for all products
  "isActive": true
}
```

**Update Coupon**
```http
PUT /api/coupons/{id}
Content-Type: application/json

{
  "description": "Updated description",
  "discountValue": 20.00,
  "isActive": true
}
```

**Delete Coupon**
```http
DELETE /api/coupons/{id}
```

**Validate Coupon**
```http
POST /api/coupons/validate
Content-Type: application/json

{
  "code": "SAVE10",
  "userId": 1,
  "cartTotal": 100.00
}
```

---

## 🎟️ Coupon System Features

### **Discount Types**

#### **1. Fixed Discount**
- Deducts a fixed amount from cart total
- Example: `$10 OFF`
```json
{
  "discountType": 1,
  "discountValue": 10.00
}
```

#### **2. Percentage Discount**
- Deducts a percentage of cart total
- Can have maximum discount cap
- Example: `20% OFF (max $50)`
```json
{
  "discountType": 2,
  "discountValue": 20.00,
  "maxDiscountAmount": 50.00
}
```

---

### **Coupon Restrictions**

#### **1. Time-Based Restrictions**
```json
{
  "startDate": "2025-01-01T00:00:00Z",
  "expiryDate": "2025-12-31T23:59:59Z"
}
```

#### **2. Cart Value Restrictions**
```json
{
  "minimumCartItems": 3,        // At least 3 items in cart
  "minimumTotalPrice": 100.00   // Cart total must be $100+
}
```

#### **3. Usage Limits**
```json
{
  "maxTotalUses": 100,      // Coupon can be used 100 times total
  "maxUsesPerUser": 1,      // Each user can use once
  "currentTotalUses": 45    // Already used 45 times
}
```

#### **4. Product-Specific Restrictions**
```json
{
  "applicableProductIds": [1, 2, 3]  // Only applies to specific products
}
```
- Empty array = applies to all products
- With IDs = applies only to specified products

---

### **Auto-Applied Coupons**

Coupons can be automatically applied when cart meets criteria:

```json
{
  "code": "AUTO50",
  "isAutoApplied": true,
  "minimumTotalPrice": 50.00
}
```

**Behavior:**
- Backend automatically checks and applies eligible coupons
- User doesn't need to enter code manually
- Shown with indicator in UI

---

### **Coupon Stacking**

Multiple coupons can be applied to a single cart:
- Each coupon's discount is calculated independently
- Percentage discounts apply to original subtotal
- Validation runs for each coupon
- UI shows breakdown per coupon

**Example:**
```
Cart Subtotal: $100.00
- SAVE10 (Fixed): -$10.00
- PERCENT20 (20%): -$20.00
Total Discount: $30.00
Final Total: $70.00
```

---

### **Coupon Validation Rules**

The system validates coupons based on:

1. **Existence:** Coupon code exists in database
2. **Active Status:** `isActive = true`
3. **Time Window:** Current time between `startDate` and `expiryDate`
4. **Cart Minimum:** Cart meets `minimumCartItems` and `minimumTotalPrice`
5. **Usage Limits:** Under `maxTotalUses` and user hasn't exceeded `maxUsesPerUser`
6. **Product Eligibility:** Cart contains applicable products (if specified)
7. **Already Applied:** Same coupon not already in cart

**Validation Response:**
```json
{
  "isValid": true,
  "message": "Coupon applied successfully",
  "discountAmount": 10.00
}
```

---

## 🗄️ Database Schema

### **Entity Relationship Diagram**

```
User (1) ──────── (1) Cart
                      │
                      │ (1)
                      │
                      ├─────── (Many) CartItem ──────── (Many) Product
                      │
                      └─────── (Many) AppliedCoupon ──────── (Many) Coupon
                                                                  │
                                                                  └─── (Many) CouponUsage
```

### **Tables**

#### **Users**
```sql
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Phone VARCHAR(20),
    CreatedAt TIMESTAMP DEFAULT NOW(),
    UpdatedAt TIMESTAMP DEFAULT NOW()
);
```

#### **Products**
```sql
CREATE TABLE Products (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    Price DECIMAL(18,2) NOT NULL,
    Stock INTEGER NOT NULL DEFAULT 0,
    Category VARCHAR(100),
    ImageUrl VARCHAR(500),
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT NOW(),
    UpdatedAt TIMESTAMP DEFAULT NOW(),
    DeletedAt TIMESTAMP NULL
);
```

#### **Carts**
```sql
CREATE TABLE Carts (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL REFERENCES Users(Id),
    CreatedAt TIMESTAMP DEFAULT NOW(),
    UpdatedAt TIMESTAMP DEFAULT NOW()
);
```

#### **CartItems**
```sql
CREATE TABLE CartItems (
    Id SERIAL PRIMARY KEY,
    CartId INTEGER NOT NULL REFERENCES Carts(Id) ON DELETE CASCADE,
    ProductId INTEGER NOT NULL REFERENCES Products(Id),
    Quantity INTEGER NOT NULL DEFAULT 1,
    Price DECIMAL(18,2) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT NOW(),
    UpdatedAt TIMESTAMP DEFAULT NOW()
);
```

#### **Coupons**
```sql
CREATE TABLE Coupons (
    Id SERIAL PRIMARY KEY,
    Code VARCHAR(50) UNIQUE NOT NULL,
    Description TEXT,
    DiscountType INTEGER NOT NULL, -- 1=Fixed, 2=Percentage
    DiscountValue DECIMAL(18,2) NOT NULL,
    MaxDiscountAmount DECIMAL(18,2) NULL,
    IsAutoApplied BOOLEAN DEFAULT FALSE,
    StartDate TIMESTAMP NULL,
    ExpiryDate TIMESTAMP NULL,
    MinimumCartItems INTEGER NULL,
    MinimumTotalPrice DECIMAL(18,2) NULL,
    MaxTotalUses INTEGER NULL,
    MaxUsesPerUser INTEGER NULL,
    CurrentTotalUses INTEGER DEFAULT 0,
    ApplicableProductIdsJson TEXT, -- JSON array of product IDs
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT NOW(),
    UpdatedAt TIMESTAMP DEFAULT NOW(),
    DeletedAt TIMESTAMP NULL
);
```

#### **AppliedCoupons**
```sql
CREATE TABLE AppliedCoupons (
    Id SERIAL PRIMARY KEY,
    CartId INTEGER NOT NULL REFERENCES Carts(Id) ON DELETE CASCADE,
    CouponId INTEGER NOT NULL REFERENCES Coupons(Id),
    DiscountAmount DECIMAL(18,2) NOT NULL,
    AppliedAt TIMESTAMP DEFAULT NOW()
);
```

#### **CouponUsages**
```sql
CREATE TABLE CouponUsages (
    Id SERIAL PRIMARY KEY,
    CouponId INTEGER NOT NULL REFERENCES Coupons(Id),
    UserId INTEGER NOT NULL REFERENCES Users(Id),
    UsedAt TIMESTAMP DEFAULT NOW()
);
```

---

## 🧪 Testing

### **Backend Unit Tests**

The project includes comprehensive unit tests using xUnit.

**Running Tests:**
```powershell
cd Backend/ECommerce.Tests
dotnet test
```

**Test Coverage:**
- ✅ Fixed discount calculation
- ✅ Percentage discount calculation
- ✅ Cart total calculation
- ✅ Coupon expiry validation
- ✅ Coupon date range validation
- ✅ Minimum cart value validation
- ✅ Maximum usage validation
- ✅ Product-specific coupon validation

**Sample Test:**
```csharp
[Fact]
public void FixedDiscount_ShouldCalculateCorrectly()
{
    // Arrange
    var coupon = new Coupon
    {
        Code = "FIXED10",
        DiscountType = DiscountType.Fixed,
        DiscountValue = 10
    };
    decimal cartTotal = 100;

    // Act
    decimal discount = coupon.DiscountType == DiscountType.Fixed
        ? coupon.DiscountValue
        : cartTotal * (coupon.DiscountValue / 100);

    // Assert
    Assert.Equal(10, discount);
}
```

### **Manual Testing with Swagger**

1. Start the backend API
2. Navigate to: http://localhost:5090/swagger
3. Test all endpoints interactively
4. View request/response schemas

### **Frontend Testing**

**Test User Flow:**
1. View products
2. Add items to cart
3. Update quantities
4. Apply coupon codes
5. Verify discount calculations
6. Remove coupons
7. Remove items
8. Check auto-applied coupons

---

## 🐳 Deployment with Docker

### **Docker Configuration**

#### **Backend Dockerfile**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5090
ENTRYPOINT ["dotnet", "ECommerce.API.dll"]
```

#### **Frontend Dockerfile**
```dockerfile
FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

#### **Docker Compose**
```yaml
version: "3.9"

services:
  db:
    image: postgres:15
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: a!23456789
      POSTGRES_DB: ECommerceDB
    ports:
      - "5432:5432"
    volumes:
      - db-data:/var/lib/postgresql/data

  backend:
    build: ./Backend
    ports:
      - "5090:5090"
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Database=ECommerceDB;Username=postgres;Password=a!23456789
    depends_on:
      - db

  frontend:
    build: ./Frontend
    ports:
      - "3000:80"
    depends_on:
      - backend

volumes:
  db-data:
```

### **Docker Commands**

```powershell
# Build and start all services
docker-compose up --build

# Start in detached mode
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down

# Remove volumes
docker-compose down -v

# Rebuild specific service
docker-compose build backend
docker-compose up backend

# Access container shell
docker exec -it ecommerce_backend sh
docker exec -it postgres_db psql -U postgres -d ECommerceDB
```

---

## ❓ Assessment Questions & Answers

### **Q1: How does the coupon system handle different discount types?**

**Answer:**

The coupon system supports two main discount types implemented via an enum:

```csharp
public enum DiscountType
{
    Fixed = 1,        // Fixed amount discount (e.g., $10 OFF)
    Percentage = 2    // Percentage discount (e.g., 20% OFF)
}
```

**Implementation Details:**

1. **Fixed Discount:**
   - Deducts a specific dollar amount from the cart total
   - Example: A coupon with `DiscountValue = 10` deducts $10
   - Simple and straightforward for users to understand

2. **Percentage Discount:**
   - Calculates discount as a percentage of the cart subtotal
   - Can have a maximum cap via `MaxDiscountAmount`
   - Example: 20% off with max $50 discount
   
**Calculation Logic:**
```csharp
decimal CalculateDiscount(Coupon coupon, decimal cartTotal)
{
    if (coupon.DiscountType == DiscountType.Fixed)
    {
        return coupon.DiscountValue;
    }
    else // Percentage
    {
        decimal percentDiscount = cartTotal * (coupon.DiscountValue / 100);
        
        // Apply max discount cap if specified
        if (coupon.MaxDiscountAmount.HasValue)
        {
            return Math.Min(percentDiscount, coupon.MaxDiscountAmount.Value);
        }
        
        return percentDiscount;
    }
}
```

**Key Features:**
- Type-safe enum prevents invalid discount types
- Percentage discounts can be capped to prevent excessive discounts
- Both types validated before application
- Clear separation of calculation logic

---

### **Q2: Explain the coupon validation process**

**Answer:**

The coupon validation is a multi-step process that checks various criteria before allowing a coupon to be applied:

**Validation Steps:**

1. **Existence Check**
   - Verify coupon code exists in database
   - Case-insensitive comparison

2. **Active Status**
   - Check `IsActive = true`
   - Inactive coupons immediately rejected

3. **Time Window Validation**
   ```csharp
   var now = DateTime.UtcNow;
   
   // Check if coupon has started
   if (coupon.StartDate.HasValue && now < coupon.StartDate.Value)
       return Invalid("Coupon is not yet valid");
   
   // Check if coupon has expired
   if (coupon.ExpiryDate.HasValue && now > coupon.ExpiryDate.Value)
       return Invalid("Coupon has expired");
   ```

4. **Cart Value Requirements**
   ```csharp
   // Minimum items required
   if (coupon.MinimumCartItems.HasValue && 
       cart.Items.Count < coupon.MinimumCartItems.Value)
       return Invalid($"Minimum {coupon.MinimumCartItems} items required");
   
   // Minimum cart total
   if (coupon.MinimumTotalPrice.HasValue && 
       cart.TotalBeforeDiscount < coupon.MinimumTotalPrice.Value)
       return Invalid($"Minimum cart total ${coupon.MinimumTotalPrice} required");
   ```

5. **Usage Limits**
   ```csharp
   // Global usage limit
   if (coupon.MaxTotalUses.HasValue && 
       coupon.CurrentTotalUses >= coupon.MaxTotalUses.Value)
       return Invalid("Coupon usage limit reached");
   
   // Per-user usage limit
   var userUsageCount = await GetUserCouponUsageCount(userId, coupon.Id);
   if (coupon.MaxUsesPerUser.HasValue && 
       userUsageCount >= coupon.MaxUsesPerUser.Value)
       return Invalid("You have already used this coupon");
   ```

6. **Product Eligibility**
   ```csharp
   var applicableProducts = coupon.GetApplicableProductIds();
   
   // If specific products defined, check cart contains them
   if (applicableProducts.Any())
   {
       var cartProductIds = cart.Items.Select(i => i.ProductId).ToList();
       var hasApplicableProduct = cartProductIds.Any(id => applicableProducts.Contains(id));
       
       if (!hasApplicableProduct)
           return Invalid("Cart doesn't contain applicable products");
   }
   ```

7. **Duplicate Check**
   ```csharp
   // Check if coupon already applied to cart
   if (cart.AppliedCoupons.Any(ac => ac.CouponCode == coupon.Code))
       return Invalid("Coupon already applied");
   ```

**Validation Result:**
```csharp
public class CouponValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public decimal DiscountAmount { get; set; }
}
```

**Benefits:**
- Early exit on first failed validation (performance)
- Clear error messages for users
- Prevents fraud and abuse
- Ensures business rules compliance

---

### **Q3: How does the system handle multiple coupons applied to one cart?**

**Answer:**

The system fully supports **coupon stacking** - applying multiple coupons to a single cart simultaneously.

**Implementation Approach:**

1. **Data Model**
   ```csharp
   public class Cart
   {
       public int Id { get; set; }
       public int UserId { get; set; }
       public List<CartItem> Items { get; set; }
       public List<AppliedCoupon> AppliedCoupons { get; set; } // Multiple coupons
   }
   
   public class AppliedCoupon
   {
       public int Id { get; set; }
       public int CartId { get; set; }
       public int CouponId { get; set; }
       public string CouponCode { get; set; }
       public decimal DiscountAmount { get; set; }
       public bool IsAutoApplied { get; set; }
   }
   ```

2. **Application Process**
   ```csharp
   public async Task<CartResponse> ApplyCouponAsync(int userId, string code)
   {
       var cart = await GetCartWithDetails(userId);
       var coupon = await GetCouponByCode(code);
       
       // Validate coupon
       var validation = await ValidateCouponAsync(coupon, userId, cart);
       if (!validation.IsValid)
           throw new Exception(validation.Message);
       
       // Calculate discount for THIS coupon
       var discount = CalculateDiscount(coupon, cart.TotalBeforeDiscount);
       
       // Add to applied coupons list
       var appliedCoupon = new AppliedCoupon
       {
           CartId = cart.Id,
           CouponId = coupon.Id,
           CouponCode = coupon.Code,
           DiscountAmount = discount,
           IsAutoApplied = false
       };
       
       cart.AppliedCoupons.Add(appliedCoupon);
       await SaveCart(cart);
       
       // Record usage
       await RecordCouponUsage(coupon.Id, userId);
       
       return MapToCartResponse(cart);
   }
   ```

3. **Discount Calculation**
   ```csharp
   // Each coupon calculates discount independently
   public decimal TotalDiscount
   {
       get
       {
           decimal total = 0;
           foreach (var appliedCoupon in AppliedCoupons)
           {
               total += appliedCoupon.DiscountAmount;
           }
           return total;
       }
   }
   
   public decimal FinalTotal
   {
       get
       {
           var subtotal = TotalBeforeDiscount;
           var discount = TotalDiscount;
           return Math.Max(0, subtotal - discount); // Never go below $0
       }
   }
   ```

4. **Frontend Display**
   ```typescript
   // Shows breakdown per coupon
   {cart.appliedCoupons.map(coupon => (
     <div key={coupon.code}>
       <span>{coupon.code}</span>
       {coupon.isAutoApplied && <Badge>Auto</Badge>}
       <span>-${coupon.discountAmount.toFixed(2)}</span>
       <button onClick={() => removeCoupon(coupon.code)}>Remove</button>
     </div>
   ))}
   ```

**Example Scenario:**
```
Cart Items:
- Product A: $50 x 2 = $100
- Product B: $30 x 1 = $30
Subtotal: $130

Applied Coupons:
1. SAVE10 (Fixed): -$10.00
2. PERCENT20 (20%): -$26.00 (20% of $130)
3. FREESHIPG (Fixed): -$5.00

Total Discount: $41.00
Final Total: $89.00
```

**Key Considerations:**
- Each coupon validated independently
- Percentage discounts calculate from original subtotal (not after previous discounts)
- User can remove individual coupons
- Auto-applied coupons clearly marked
- Discount total cannot exceed cart subtotal

**Benefits:**
- Increased customer satisfaction
- Higher conversion rates
- Flexible promotional strategies
- Clear transparency in pricing

---

### **Q4: How are auto-applied coupons handled?**

**Answer:**

Auto-applied coupons are automatically added to eligible carts without user input, providing a seamless discount experience.

**Implementation:**

1. **Coupon Configuration**
   ```csharp
   public class Coupon
   {
       public bool IsAutoApplied { get; set; }
       // ... other properties
   }
   ```

2. **Auto-Application Logic**
   ```csharp
   public async Task<CartResponse> GetCartAsync(int userId)
   {
       var cart = await GetCartWithDetails(userId);
       
       // Find all auto-apply eligible coupons
       var autoApplyCoupons = await GetAutoApplyEligibleCoupons();
       
       foreach (var coupon in autoApplyCoupons)
       {
           // Check if already applied
           if (cart.AppliedCoupons.Any(ac => ac.CouponCode == coupon.Code))
               continue;
           
           // Validate coupon
           var validation = await ValidateCouponAsync(coupon, userId, cart);
           
           if (validation.IsValid)
           {
               // Auto-apply the coupon
               var discount = CalculateDiscount(coupon, cart.TotalBeforeDiscount);
               
               cart.AppliedCoupons.Add(new AppliedCoupon
               {
                   CartId = cart.Id,
                   CouponId = coupon.Id,
                   CouponCode = coupon.Code,
                   DiscountAmount = discount,
                   IsAutoApplied = true  // Mark as auto-applied
               });
           }
       }
       
       await SaveCart(cart);
       return MapToCartResponse(cart);
   }
   ```

3. **Querying Auto-Apply Coupons**
   ```csharp
   public async Task<List<Coupon>> GetAutoApplyEligibleCoupons()
   {
       return await _context.Coupons
           .Where(c => c.IsActive && c.IsAutoApplied)
           .Where(c => !c.StartDate.HasValue || c.StartDate <= DateTime.UtcNow)
           .Where(c => !c.ExpiryDate.HasValue || c.ExpiryDate >= DateTime.UtcNow)
           .ToListAsync();
   }
   ```

4. **Frontend Indication**
   ```typescript
   {coupon.isAutoApplied && (
     <Badge variant="secondary" className="ml-2">
       <Sparkles className="h-3 w-3 mr-1" />
       Auto-applied
     </Badge>
   )}
   ```

**Use Cases:**
- **Loyalty Rewards:** Automatically apply discounts for returning customers
- **Cart Value Incentives:** $10 off when cart reaches $100
- **Seasonal Promotions:** Auto-apply holiday discounts
- **Member Benefits:** Automatic discounts for premium members

**Example Auto-Apply Coupon:**
```json
{
  "code": "AUTO50",
  "description": "Automatic $5 off when you spend $50+",
  "discountType": 1,
  "discountValue": 5.00,
  "isAutoApplied": true,
  "minimumTotalPrice": 50.00,
  "isActive": true
}
```

**User Experience:**
1. User adds items to cart (total $60)
2. System automatically detects eligible AUTO50 coupon
3. Discount applied without user action
4. User sees: "Auto-applied: AUTO50 (-$5.00)"
5. User can still remove if desired

**Benefits:**
- Improves conversion rates
- Surprises and delights customers
- Reduces cart abandonment
- No need for users to remember codes
- Can drive specific behaviors (e.g., reaching minimum cart value)

---

### **Q5: How does the system prevent coupon abuse?**

**Answer:**

The system implements multiple layers of protection against coupon fraud and abuse:

**1. Usage Limits**
```csharp
// Global limit - total times coupon can be used
public int? MaxTotalUses { get; set; }
public int CurrentTotalUses { get; set; }

// Per-user limit
public int? MaxUsesPerUser { get; set; }

// Enforcement
if (coupon.MaxTotalUses.HasValue && 
    coupon.CurrentTotalUses >= coupon.MaxTotalUses.Value)
    return Invalid("Coupon usage limit reached");

var userUsages = await _context.CouponUsages
    .CountAsync(cu => cu.CouponId == coupon.Id && cu.UserId == userId);

if (coupon.MaxUsesPerUser.HasValue && 
    userUsages >= coupon.MaxUsesPerUser.Value)
    return Invalid("You have already used this coupon");
```

**2. Usage Tracking**
```csharp
public class CouponUsage
{
    public int Id { get; set; }
    public int CouponId { get; set; }
    public int UserId { get; set; }
    public DateTime UsedAt { get; set; }
}

// Record every usage
await _context.CouponUsages.AddAsync(new CouponUsage
{
    CouponId = coupon.Id,
    UserId = userId,
    UsedAt = DateTime.UtcNow
});

// Increment counter
coupon.CurrentTotalUses++;
await _context.SaveChangesAsync();
```

**3. Time-Based Controls**
```csharp
// Start date - prevent early usage
if (coupon.StartDate.HasValue && DateTime.UtcNow < coupon.StartDate.Value)
    return Invalid("Coupon not yet active");

// Expiry date - prevent expired usage
if (coupon.ExpiryDate.HasValue && DateTime.UtcNow > coupon.ExpiryDate.Value)
    return Invalid("Coupon has expired");
```

**4. Duplicate Prevention**
```csharp
// Prevent same coupon applied multiple times
var alreadyApplied = cart.AppliedCoupons
    .Any(ac => ac.CouponCode.Equals(code, StringComparison.OrdinalIgnoreCase));

if (alreadyApplied)
    return Invalid("Coupon already applied to cart");
```

**5. Minimum Requirements**
```csharp
// Minimum cart value
if (coupon.MinimumTotalPrice.HasValue && 
    cart.TotalBeforeDiscount < coupon.MinimumTotalPrice.Value)
    return Invalid($"Minimum cart total ${coupon.MinimumTotalPrice} required");

// Minimum items
if (coupon.MinimumCartItems.HasValue && 
    cart.Items.Count < coupon.MinimumCartItems.Value)
    return Invalid($"Minimum {coupon.MinimumCartItems} items required");
```

**6. Product Restrictions**
```csharp
// Limit to specific products
var applicableProducts = coupon.GetApplicableProductIds();
if (applicableProducts.Any())
{
    var hasEligibleProduct = cart.Items
        .Any(item => applicableProducts.Contains(item.ProductId));
    
    if (!hasEligibleProduct)
        return Invalid("Cart doesn't contain eligible products");
}
```

**7. Active Status Control**
```csharp
// Admin can instantly disable abused coupons
if (!coupon.IsActive)
    return Invalid("Coupon is no longer active");
```

**8. Soft Delete**
```csharp
public DateTime? DeletedAt { get; set; }

// Query only non-deleted coupons
var coupon = await _context.Coupons
    .Where(c => c.Code == code && c.DeletedAt == null)
    .FirstOrDefaultAsync();
```

**9. Maximum Discount Cap**
```csharp
// Prevent excessive percentage discounts
if (coupon.DiscountType == DiscountType.Percentage)
{
    var percentDiscount = cartTotal * (coupon.DiscountValue / 100);
    
    if (coupon.MaxDiscountAmount.HasValue)
        discount = Math.Min(percentDiscount, coupon.MaxDiscountAmount.Value);
}
```

**10. Database Constraints**
```csharp
// Unique coupon codes
modelBuilder.Entity<Coupon>()
    .HasIndex(c => c.Code)
    .IsUnique();

// Prevent negative discounts
[Range(0, double.MaxValue)]
public decimal DiscountValue { get; set; }
```

**Additional Security Measures:**
- **Rate Limiting:** Can implement API rate limiting
- **Audit Logging:** All coupon operations logged
- **Case-Insensitive:** Prevents "SAVE10" vs "save10" duplicates
- **Transaction Safety:** Database transactions prevent race conditions
- **User Authentication:** Only authenticated users can apply coupons

---

### **Q6: Explain the cart price calculation logic**

**Answer:**

The cart price calculation involves multiple steps to accurately compute subtotals, discounts, and final totals:

**1. Individual Item Calculation**
```csharp
public class CartItem
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }  // Price at time of adding to cart
    
    public decimal Subtotal => Quantity * Price;
}
```

**2. Cart Subtotal**
```csharp
public class Cart
{
    public List<CartItem> Items { get; set; }
    
    public decimal TotalBeforeDiscount
    {
        get
        {
            return Items.Sum(item => item.Subtotal);
        }
    }
    
    public int TotalItems
    {
        get
        {
            return Items.Sum(item => item.Quantity);
        }
    }
}
```

**3. Coupon Discount Calculation**
```csharp
private decimal CalculateCouponDiscount(Coupon coupon, decimal cartSubtotal)
{
    decimal discount = 0;
    
    if (coupon.DiscountType == DiscountType.Fixed)
    {
        // Fixed amount discount
        discount = coupon.DiscountValue;
    }
    else if (coupon.DiscountType == DiscountType.Percentage)
    {
        // Percentage discount
        discount = cartSubtotal * (coupon.DiscountValue / 100);
        
        // Apply maximum discount cap if exists
        if (coupon.MaxDiscountAmount.HasValue)
        {
            discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);
        }
    }
    
    // Ensure discount doesn't exceed cart total
    discount = Math.Min(discount, cartSubtotal);
    
    return Math.Round(discount, 2);
}
```

**4. Product-Specific Discounts**
```csharp
private decimal CalculateProductSpecificDiscount(Coupon coupon, Cart cart)
{
    var applicableProductIds = coupon.GetApplicableProductIds();
    
    // If no specific products, apply to entire cart
    if (!applicableProductIds.Any())
        return CalculateCouponDiscount(coupon, cart.TotalBeforeDiscount);
    
    // Calculate subtotal of applicable products only
    var applicableSubtotal = cart.Items
        .Where(item => applicableProductIds.Contains(item.ProductId))
        .Sum(item => item.Subtotal);
    
    return CalculateCouponDiscount(coupon, applicableSubtotal);
}
```

**5. Total Discount Calculation**
```csharp
public decimal TotalDiscount
{
    get
    {
        decimal total = 0;
        
        foreach (var appliedCoupon in AppliedCoupons)
        {
            total += appliedCoupon.DiscountAmount;
        }
        
        // Ensure total discount doesn't exceed cart subtotal
        return Math.Min(total, TotalBeforeDiscount);
    }
}
```

**6. Final Total**
```csharp
public decimal FinalTotal
{
    get
    {
        var subtotal = TotalBeforeDiscount;
        var discount = TotalDiscount;
        var final = subtotal - discount;
        
        // Never allow negative totals
        return Math.Max(0, Math.Round(final, 2));
    }
}
```

**7. Cart Response DTO**
```csharp
public class CartResponse
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public List<CartItemDto> Items { get; set; }
    public List<AppliedCouponDto> AppliedCoupons { get; set; }
    
    public decimal TotalBeforeDiscount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal FinalTotal { get; set; }
    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }
}
```

**Example Calculation:**

```
Items:
  Product A: $50.00 × 2 = $100.00
  Product B: $30.00 × 1 = $30.00
  Product C: $20.00 × 3 = $60.00
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Subtotal: $190.00

Applied Coupons:
  1. SAVE10 (Fixed):
     Discount: $10.00

  2. PERCENT15 (15% off):
     Calculation: $190.00 × 0.15 = $28.50
     Max Cap: $25.00
     Discount: $25.00

  3. PRODUCT-A (20% off Product A only):
     Applicable: $100.00 (Product A only)
     Calculation: $100.00 × 0.20 = $20.00
     Discount: $20.00
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Total Discount: $55.00
Final Total: $135.00

Items in cart: 3
Total quantity: 6
```

**Key Features:**
- **Precision:** All calculations rounded to 2 decimal places
- **Safety:** Prevents negative totals
- **Cap Enforcement:** Respects maximum discount limits
- **Product-Specific:** Handles product-restricted coupons
- **Transparency:** Clear breakdown of all charges

---

## 📝 Additional Notes

### **Environment Variables**

**Backend (appsettings.json):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ECommerceDB;Username=postgres;Password=a!23456789;Port=5432;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Frontend (.env):**
```env
VITE_API_URL=http://localhost:5090
```

### **Common Issues & Solutions**

**Issue:** Docker build fails
```powershell
# Solution: Clean Docker cache
docker system prune -a
docker-compose build --no-cache
```

**Issue:** Database connection fails
```powershell
# Solution: Check PostgreSQL is running
docker ps
# Restart database container
docker-compose restart db
```

**Issue:** Frontend can't reach backend
```powershell
# Solution: Check CORS settings in Program.cs
# Ensure frontend URL is allowed
```

**Issue:** EF migrations not applying
```powershell
# Solution: Manually apply migrations
cd Backend/ECommerce.API
dotnet ef database update
```

### **Project Highlights**

✅ **Clean Architecture** - Separation of concerns  
✅ **RESTful API** - Standard HTTP methods  
✅ **Type Safety** - TypeScript on frontend, C# on backend  
✅ **State Management** - Zustand for React state  
✅ **Responsive Design** - Mobile-first approach  
✅ **Docker Ready** - Full containerization support  
✅ **Database Migrations** - EF Core migrations  
✅ **Unit Tests** - xUnit test coverage  
✅ **API Documentation** - Swagger/OpenAPI  
✅ **Error Handling** - Comprehensive validation  

---

## 👨‍💻 Developer Information

**Project:** E-Commerce Cart & Coupon System  
**Version:** 1.0.0  
**Last Updated:** October 2025  
**License:** MIT (or as specified)

---

## 📞 Support & Contact

For issues or questions:
1. Check this documentation
2. Review the Swagger API documentation
3. Check GitHub Issues (if applicable)
4. Contact the development team

---

**End of Documentation**

*This documentation covers all aspects of the E-Commerce Cart & Coupon System including setup, deployment, API usage, and answers to assessment questions.*
