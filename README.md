# Simple Payments

Simple payments is an online banking type of application.

The structure of the application is modeled using N-Tier which can later be improved to an onion architecture/structure.

Areas of improvement:
- Security: Planning to add [Authorize] in web appl and use tokens when accessing api endpoints.
			this will reduce the use of user id in some endpoints as the application will only use aspnet identity to get the Id.

- Test Coverage: Add more unit test, specifically the application project (AccountLogic and AppUserLogic).