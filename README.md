# Simple Payments
----
Simple payments is an online banking type of application.

The structure of the application is modeled using N-Tier which can later be improved to an onion architecture/structure.

Areas of improvement:
- Security: Planning to add [Authorize] in web appl and use tokens when accessing api endpoints.
			this will reduce the use of user id in some endpoints as the application will only use aspnet identity to get the Id.

- Test Coverage: Add more unit test, specifically the application project (AccountLogic and AppUserLogic).

To run application, set Web application as start up project
to transact with application use the following API endpoints:
- api/AppUser
- aoi/Account
- api/Transaction
----
## api/AppUser
Responsible for managing the users of simple payments.
Currently only supports the following endpoints: 
- api/AppUser/Register: for creating new users
- api/AppUser/GetInfo: getting user info using email address.

### api/AppUser/Register - [POST]
Creates a new user for the application. Accepts json parameter with the following body:
- Email: String
- FirstName: String
- LastName: String

### api/AppUser/GetInfo - [GET]
Gets the user info based on the json parameter. Accepts json parameter with the following body:
- EmailAddress: String

----

## api/Account
Responsible for managing the accounts of the users in simple payments.
Currently only supports the following endpoints:
- api/Account/Create
- api/Account/GetList

### api/Account/Create - [POST]
Creates a new account for the user. Accepts json parameter with the following body:
- UserId: Int
- AccountName: String

### api/Account/List - [GET]
Gets list of accounts for user in the json parameter. Accepts json parameter with the following body:
- UserId: Int

----

## api/Transaction
Responsible for managing the transactions of an account.
Currently only supports the following endpoints:
- api/Transaction/Credit
- api/Transaction/Debit
- api/Transaction/PerAccount

## api/Transaction/Credit - [POST]
Add funds to the account. Accepts json parameter with the following body:
- UserId: Int
- AccountId: Int
- Amount: decimal
- TransactionFee: decimal
- Note: string

## api/Transaction/Debit - [POST]
Deduct funds to the account. Accepts json parameter with the following body:
- UserId: Int
- AccountId: Int
- Amount: decimal
- TransactionFee: decimal
- Note: string

### api/Transaction/PerAccount - [GET]
Gets list of transactions specific to the account of the user in the json parameter. 
Accepts json parameter with the following body:
- UserId: Int
- AccountId: Int

----

## Sample workflow:
1. Create user using **api/AppUser/Register**
2. Get user id using **api/AppUser/GetInfo**
3. Create account for user using **api/Account/Create**
4. Get account is using **api/Account/List**
5. Add funds to account using **api/Transaction/Credit**
6. **(optional)** Deduct funds from account using **api/Transaction/Debit**
7. To see all transactions for a specific account, use **api/Transaction/PerAccount**