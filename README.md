# BankSystem

This project is to a demo bank system with required microservices support account and transaction operations.

## Basic Design

### General

This bank Web API service has 2 microservices: AccountService for handling account related request, TransactionService for handing transaction related request.
The reason to separate them is due to the transaction service can have much higher usage in real world and the instances requirment maybe different than account service.
Due to only 2 microservices, I chose to only using simple proxy to communicate between each other. 

### If in Real world

In real production world, when microservices are a lot, API gateway is required. 
As well as some event server like RabbitMQ FIFO queue to queue the events between services to assure the requests coming in order to achieve ACID between services.

### Database
It is using InMemory database in EF core and has demo data populated. Schema below:

#### Users table: {Id: int, Username: string, Email: string}
#### Accounts table: {Id: int, UserId: int, Balance: decimal} 
(UserId map to Users.Id)
#### Transactions table: {Id: int, AccountId: int, TransactionType: boolean, TransactionAmount: decimal} 
(Transactions.AccountId map to Accounts.Id)

## How to run

Checkout the code, it only has master branch.
Open command prompt and cd to the folder where `docker-compose.yml` file is existed.
Run `docker-compose up --build`

If want to run through VS without using docker, need to change both appsettings.json file to point the `TransactionServiceURL` and `AccountServiceURL` to the correct address.

## How to use

I kept the Swagger UI enabled for both services. It can be accessed through http://localhost:8100/swagger/index.html (AccountService) and http://localhost:8101/swagger/index.html (TransactionService)
It will show you how to call these APIs.
