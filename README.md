# CS-4320 Group 16 Project 2 README

## Overview

This project involves the development of a web application for iCLOTHING company using MVC C# in Visual Studio. SQL Server Management Studio was utilized to implement the database, which was then integrated into Visual Studio to facilitate MVC development.

## Functionalities Implemented

**Login Page**:
  - Separate login pages for customers and administrators.
  - Default Customer account: (username: customer, password: customer).
  - Default Administrator account: (username: admin, password: admin).

**Directory Viewing**:
  - Non-logged-in users can view directories, categories, and products.

**Customer Features**:
  - Customers can view directories, categories, and products after logging in.
  - Customers can add products to their cart.
  - Registration functionality saves customer information to the database.
  - Customers can submit queries and comments, visible to the admin.
  - Only logged-in customers can submit queries and comments.
  - Non-logged-in users can add products to their cart.

**Checkout**:
  - Customers can submit orders and empty their shopping cart.
  - Non-logged-in customers prompted to login or register at checkout.

**Administrator Features**:
  - Admins have additional functionalities:
    - Editing product catalog.
    - Viewing customer feedback and queries.
    - Managing shopping carts and order status.
    - Sending emails to customers.
    - Managing customer and admin accounts.
    - Adding new categories, departments, and products.
    - Full database management privileges.

**Search and Filter**:
  - Users can search all products.
  - Filtering available based on departments, categories, and brand on product pages.

**Shopping Cart**:
  - Customers can change product quantities in the shopping cart.

**Order Processing**:
  - Order status updates sent via email.
  - Admins can mark orders as delivered.

**Communication**:
  - Customers can receive emails from admins.
  - Only customer queries and comments can be sent back to the admin.

**Billing Information**:
  - Customers must have valid billing information to place orders.
  - Billing information creation and management integrated into the checkout process.

**Account Management**:
  - Existing customers can modify their account information.
  - Account information viewing functionality available.

**Additional Pages**:
  - About Us page providing information about iCLOTHING Inc.

**Logout**:
  - Users can log out at any time.

**Sorting**:
  - Users can sort displayed products by clicking column headings.

## Instructions

1. Unzip the project file.
2. Open the project in Visual Studio.
3. Build and run the application using the backup database file.
4. Access the functionalities through the user interface.

## Contributors

- Harrison Surma
- Samantha Whitaker
- Kendra Minch
- Kody McNamara
