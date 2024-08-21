
# **Introduction**

-  A online shopping platform designed for store owners to manage their products and for 
consumers to browse, purchase, and track orders. (ASP.NET Core 6 + Bootstrap)
# **Page**


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
    -   Sales reports
      
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
