
# **Introduction**
-  A online shopping platform designed for store owners to manage their products and for 
consumers to browse, purchase, and track orders. (ASP.NET Core 6 + Bootstrap)
# **Page**
- Login ![image](https://github.com/user-attachments/assets/740469d4-2533-4634-80c1-c78bf5fae823)
- Register ![image](https://github.com/user-attachments/assets/6899654a-e4db-4c2b-b7bf-6a3ee1bb9b3a)
- Home page ![image](https://github.com/user-attachments/assets/421b198f-f233-4870-904c-547378f66c17)
- Tất cả sản phẩm ![image](https://github.com/user-attachments/assets/0ae2cc89-7771-4d1d-90e5-ec9dcd3460ce)
- Chi tiết sản phẩm (![image](https://github.com/user-attachments/assets/41f965bf-841a-491d-aab2-512e7e847ffb)
- Giỏ hàng ![image](https://github.com/user-attachments/assets/92952d57-22fa-43ae-9a40-bddff34333eb)
- Đặt hàng (COD) ![image](https://github.com/user-attachments/assets/96ff3cdd-b91b-4023-bad8-7c81559be481)
- Quản lý người dùng (only admin) ![image](https://github.com/user-attachments/assets/45de596e-049a-416f-9981-447894e2fa24)
- Quản lý đơn hàng ![image](https://github.com/user-attachments/assets/66696d50-d72d-4de9-84c8-e5be78c1762f)
- Quản lý sản phẩm ![image](https://github.com/user-attachments/assets/b64a099c-82fb-44b9-85cb-fe9e838ad266)
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
