
# **Introduction**
-  A online shopping platform designed for store owners to manage their products and for 
consumers to browse, purchase, and track orders. (ASP.NET Core 6 + Bootstrap)
-  Structure:
    -  AdminApp: An administrative application that allows store owners to manage products, categories, user roles, orders, and users.
    -  ApiIntegration: Integration of APIs.
    -  Application: The business logic layer of the website.
    -  BackendApi: The backend API of the website, providing API services for the web application.
    -  Data: The data access layer of the website, containing query logic, data storage, etc.
    -  Utilities: A layer containing utility functions.
    -  eShopCoreWeb: The main web application of the website, containing user interfaces

# **Page**
- Login ![image](https://github.com/user-attachments/assets/740469d4-2533-4634-80c1-c78bf5fae823)
- Register ![image](https://github.com/user-attachments/assets/6899654a-e4db-4c2b-b7bf-6a3ee1bb9b3a)
- Home page ![image](https://github.com/user-attachments/assets/421b198f-f233-4870-904c-547378f66c17)
- All products ![image](https://github.com/user-attachments/assets/0ae2cc89-7771-4d1d-90e5-ec9dcd3460ce)
- Product detail ![image](https://github.com/user-attachments/assets/41f965bf-841a-491d-aab2-512e7e847ffb)
- Shopping cart ![image](https://github.com/user-attachments/assets/92952d57-22fa-43ae-9a40-bddff34333eb)
- Place order (COD) ![image](https://github.com/user-attachments/assets/96ff3cdd-b91b-4023-bad8-7c81559be481)
- User management (only admin) ![image](https://github.com/user-attachments/assets/45de596e-049a-416f-9981-447894e2fa24)
- Order management ![image](https://github.com/user-attachments/assets/66696d50-d72d-4de9-84c8-e5be78c1762f)
- Product management ![image](https://github.com/user-attachments/assets/b64a099c-82fb-44b9-85cb-fe9e838ad266)
...
# **Features**
-   Register, Log in, log out.
-   Login with google
-   Search/Filter product, detail product
-   Add to cart, update item quantity, remove item
-   Update profile
-   Place order, track order status
-   Select languages
-   Admin-page:
    -   CRUD Product, Category
    -   Assign role, category
    -   Order, User management
...
      
# **How to configure and run**

  Clone code from Github: git clone https://github.com/NTL212/eShopCoreWeb

  Open solution eShopSolution.sln in Visual Studio 2022

  Set startup project is eShopSolution.Data

  Change connection string in Appsetting.json in eShopSolution.Data project

  Open Tools --> Nuget Package Manager --> Package Manager Console in Visual Studio

  Run Update-database and Enter.

  After migrate database successful, set Startup Project is eShopSolution.WebApp

  Change database connection in appsettings.json in eShopSolution.WebApp project.

  You need to change 3 projects to self-host profile.

  Set multiple run project: Right click to Solution and choose Properties and set Multiple Project, choose Start for 3 Projects: BackendApi, WebApp and AdminApp.

  Choose profile to run or press F5
