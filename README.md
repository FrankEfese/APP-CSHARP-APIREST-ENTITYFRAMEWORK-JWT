# <div align="center"> APP-CSHARP-APIREST-ENTITY-FRAMEWORK-JWT</div>

<h4 align="center">REST API in C# using Entity Framework with a MySQL database. Database operations are handled asynchronously to improve performance and scalability. Secured endpoints with JWT (JSON Web Tokens) for protected access. Integrated Swagger for interactive documentation and testing, improving usability for developers and clients.</h4>

<br>

## <div align="center">ENTITY FRAMEWORK</div>

<div align="center">
  <a href="https://postimg.cc/87c8CJZq">
    <img src="https://i.postimg.cc/bwHq31kN/Dbcontext.png" alt="Dbcontext.png" />
  </a>
</div>

<h4 align="center">Using Entity Framework allows me to work with database data quickly and easily, by using classes and objects instead of writing raw SQL queries. It also gives me the ability to make schema changes efficiently and apply them through migrations, which simplifies maintaining and evolving the data model.</h4>

<br>

## <div align="center">JWT</div>

[![Jwt.png](https://i.postimg.cc/pLFmxs4W/Jwt.png)](https://postimg.cc/75qHnnqd)

<h4 align="center">The use of JWT (JSON Web Tokens) allows me to secure the REST API by implementing a safe and efficient authentication system. The tokens ensure that only authorized users can access protected endpoints, maintaining the integrity and confidentiality of the data. Additionally, JWT facilitates session management without the need to store state on the server, improving the application's scalability and performance.</h4>

<br>

## <div align="center">CONTROLLERS</div>

<div align="center">
  <a href="https://postimg.cc/TKGWZbcv">
    <img src="https://i.postimg.cc/cLtMtMgC/Controller.png" alt="Controller.png" />
  </a>
</div>

<h4 align="center">The REST API contains three controllers: one dedicated to authentication and the other two for performing various data operations. All endpoints are secured with authentication to ensure that only authorized users can access and modify information, maintaining the integrity and security of the application.</h4>

<br>

## <div align="center">MYSQL</div>

<div align="center">
  <a href="https://postimg.cc/nXWMnw87">
    <img src="https://i.postimg.cc/VNftB3D7/MySql.png" alt="MySql.png" />
  </a>
</div>

<h4 align="center">I use MySQL as the database to manage the data. The structure consists of two tables with a one-to-many relationship, where one primary entity can be associated with multiple records in the related table. This setup allows efficient data management and ensures referential integrity between the tables.</h4>

<br>

## <div align="center">SWAGGER</div>

[![Swagger.png](https://i.postimg.cc/KYVZvHpq/Swagger.png)](https://postimg.cc/cKRqFXSw)

<h4 align="center">For technical documentation and testing of the REST API, I use Swagger because it provides an interactive interface that makes it easy to visualize, test, and understand the available endpoints. This allows developers and clients to explore the API easily, perform test calls directly from the browser, and identify potential errors or improvements before final integration.</h4>
