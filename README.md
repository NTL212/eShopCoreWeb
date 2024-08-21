
# **Introduction**

-  A online shopping platform designed for store owners to manage their products and for 
consumers to browse, purchase, and track orders. (ASP.NET Core 6 + Bootstrap)
# **Page**

###### _(Not logged)_

-   Home.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659182962/pt3no8yqcxheh5gh9pbs.png)
-   Login.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659182977/z6vs0znfqoapmgsjld61.png)
-   Register.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659182982/vt38rfswmk1vd6yqwoeq.png)
-   Forget password.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659183352/jgzolk4l2aejm0rrjlv5.png)
###### _(Logged)_

-   Home.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659182995/uc2gwx26txbqaac3y6rp.png)
-   Messenger.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659183242/pt1idgk5usv2kta9vfho.png)
-   Admin page (Only with admin's accounts).
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659183254/bhyjo17cjtyaznzpjtmi.png)
-   Profile.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659183272/ghzvdp1db5cafsz9x0vm.png)
-   Update profile.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659183289/u4pblu3hehw8ttqryh8e.png)
-   Information a post.
![Dark mode](https://res.cloudinary.com/dcwekkkez/image/upload/v1659183804/wzbdifdw8fk7pmjba2wv.png)

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
