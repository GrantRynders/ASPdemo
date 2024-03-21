```mermaid
---
title: Final Project Class Diagram
---
classDiagram
    Currency <|-- Category : One-to-Many
    Currency --|> Airdrop : One-to-Many
    Currency <|-- User : Accesses
    User <|-- Admin : Inherits from
    class Currency{
        +int CurrencyId PK
        +int CategoryId FK
        +String CurrencyName
        +String Slug
        +String Symbol
    }
    class Category{
        +int CategoryId PK
        +String CategoryName
    }
    class Airdrop{
        +int CurrencyId FK
        +int AirdropId PK
    }
    class User{
        +int UserId PK
        +String UserName
        -String FirstName
        -String LastName
        +String Email
        -int permissionsLevel
        +List<Currency> followedCurrencies
    }
    class Admin{
        -void ManageUser()
        -void ManageCurrency()
    }
```
