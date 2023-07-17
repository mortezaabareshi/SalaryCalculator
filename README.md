## SalaryCalculator

Salary Calculator
This is a salary calculator project built with .NET using Domain-Driven Design (DDD) principles and the CQRS architecture pattern. The project utilizes PostgreSQL as the database to store salary information for individuals. Additionally, it allows users to upload salary data in JSON, XML, or a custom file format.

### Getting Started
To run this project locally, follow the steps below:

#### Prerequisites
* Install .NET 6 SDK and runtime from [Microsoft's official website](https://learn.microsoft.com/en-us/dotnet/core/install)
### Installation
Clone this repository to your local machine
Open the project directory in a terminal or command prompt
Run the following command to restore the project's dependencies:
```bash
dotnet restore
```
Configure the PostgreSQL connection string in the
appsettings.json
file with your database information:
```bash
{
  "ConnectionStrings": {
    "Postgres": "YourConnectionStringHere"
  }
}
```
Run the database migrations to create the necessary database schema:
```bash
dotnet ef database update
```
### Usage
Start the project by running the following command:
```bash
dotnet run
```
#### Use the provided APIs to perform different actions:
Add salary from files:
```bash
POST /salaries/{dataType}
```
Update salary data for an individual:
```bash
PUT /salaries
```
Delete salary data for an individual:
```bash
DELETE /salaries
```
Get a salary from an individual:
```bash
GET /salaries/get-salary
```
Get salaries from an individual:
```bash
GET /salaries/get-salary-range
```
