# Gringotts Bank Web API
This is a Web API for Gringotts Bank, known as wizards' favorite bank.

##  Brief Details
This project is developed with **.NET 5** and uses **PostgreSQL** as database.

**Repository Pattern** with **Entity Framework** are used in this project.

Endpoints are secured with **JwtToken**. 

It creates example datas(2 customers and 3 accounts) and api user after first run.

Transaction consistency is secured by **UnitOfWork** pattern between Accounts and Transactions entities.

**Unit tests** are written for api controllers.

Validations are made with DataAnnotations in the DTO classes.

It is also deployed on **Heroku**.

## Libraries
There are 5 projects in this solution
#### Core
It contains base entity and entity repository interfaces
#### Entities
It contains database entities, DTOs and filter models. It uses Core library.
#### DAL
It contains database repositories which is used for database access with Entity Framework. It uses database entities in the Entities library.
#### BLL
This is the business layer, all of business logics are in this library. It uses repositories in the DAL library.
#### API
It contains Web API controllers and uses service classes in the BLL library for business logics. 

## Usage
This project can be run with the docker compose command without needing any other process. After the docker compose command finished, project can be accessed at http://localhost:8080/swagger/index.html. This URL has swagger documentation with endpoints and usage.

      git clone https://github.com/bkbalci/GringottsBank.git
      cd GringottsBank
      docker-compose up -d
    
    
