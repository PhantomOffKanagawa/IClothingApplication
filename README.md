# iCLOTHING Web Application
> CS-4320 Group 16 Project 2 \
> Team Members: Harrison Surma, Samantha Whitaker, Kendra Minch

## Overview

This project was an assignment for a Software Engineering class, the course nor previous courses had covered ASP.NET development and the timeline was short so it was a challenge but meant we got to flex our rapid development skills. Despite the time crunch and high learning curve, it became a strong project that earned extra credit for its functionality & design, and is a strong project. It involved the development of a web application for iCLOTHING company using MVC C# in Visual Studio. SQL Server Management Studio was utilized to implement the database, which was then integrated into Visual Studio to facilitate MVC development.

>[!IMPORTANT]
> Due to the short development and ASP.NET changing since, there are some errors that may occur when running the application especially around Null Reference Exceptions and leaky page access permissions. They have been fixed as noticed but some may remain. The application is still functional and ignoring the errors will not affect the functionality of the application.

>[!WARNING]
> The majority of Null Reference Exceptions have been fixed as they were mainly due to `return View()` and just needed to be changed to `return View(model)`. The leaky page access permissions were either handled in a way that no longer works or an oversight. An `[AdminAuthorize]` filter has been added to the admin pages to prevent non-admin users from accessing them but some pages like editing user billing are necessary for normal users to access. A more robust solution would be another filter that ensures a customer is only accessing their own information. ***This is a good idea for future work on the project.***

## Installation Instructions

> To make the database connection easier there is a provided docker-compose file which when ran, starts a sql server that has the seeded data from `./sql/init.sql`  
> You can either set a environment variable `ICLOTHING_DB_CONN` to the connection string or set it in the `Web.config` file. A section on how to do this is below [here](#environment-variables).    
> The docker-compose file is not required to run the application, but it is a good way to get started quickly with a database that has the seeded data.

1. Ensure you have Visual Studio installed with the ASP.NET development workload.
2. Run the following command to start the SQL Server container:
   ```bash
   docker-compose up -d
   ```

   > You can then connect with the following connection string:

   ```bash
    metadata=res://*/Models.ICLOTHINGModel.csdl|res://*/Models.ICLOTHINGModel.ssdl|res://*/Models.ICLOTHINGModel.msl;provider=System.Data.SqlClient;provider connection string='Data Source=localhost,1433;Initial Catalog=ICLOTHING;User ID=sa;Password=Your_password123;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True'
    ```

   > This will start a SQL Server container with the database initialized with the seeded data.   
   > If not using the docker-compose file, you can create a new SQL Server database and run the `init.sql` script to seed the data. You must include the seed data in the database for the application to work correctly.
3. Clone the repository to your local machine.
4. Open the solution file (`iCLOTHING.sln`) in Visual Studio.
5. Build the solution to restore dependencies and compile the code.
6. Run the application using the built-in web server (IIS Express) or your local IIS server or by debugging it.

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

## Environment Variables

> The application uses the `ICLOTHING_DB_CONN` environment variable to set the database connection string. This can be set in your terminal or in the `Web.config` file.  
> A connection string set in the `Web.config` file overrides the environment variable.

- Option 1: Set System Environment Variable
  - Search for "Environment Variables" in the Windows search bar.
  - Click on "Edit the system environment variables."
  - In the System Properties window, click on the "Environment Variables..." button.
  - Under "User variables," click "New" to create a new variable.
  - Enter `ICLOTHING_DB_CONN` as the variable name and your connection string as the value.
  - Click OK to save the changes.
  - Restart Visual Studio to apply the changes.
- Option 2: Set in Web.config
  - Open the `Web.config` file in your project.
  - Edit the following line near the bottom of the `<configuration>` section:
    ```xml
    <connectionStrings>
      <add name="ICLOTHINGEntities"
          connectionString="REPLACED_WITH_ENV_VAR_ICLOTHING_DB_CONN"
          providerName="System.Data.EntityClient" />
    </connectionStrings>
    ```
  - Replace `REPLACED_WITH_ENV_VAR_ICLOTHING_DB_CONN` with your actual connection string.
  - Save the changes.
  - This will be used as the default connection string for the application and overriden by the environment variable if set.
- Option 3: Set in Terminal
  - Open a terminal window.
  - Set the environment variable and run vstudio from the same terminal session:
    ```bash
    # For Windows Command Prompt
    set ICLOTHING_DB_CONN=your_connection_string_here
    start devenv
    ```
    ```powershell
    # For Windows PowerShell
    $env:ICLOTHING_DB_CONN="your_connection_string_here"; Start-Process devenv
    ```

## Contributors

- Harrison Surma
- Samantha Whitaker
- Kendra Minch
