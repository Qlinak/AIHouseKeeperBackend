# AIHouseKeeperBackend

A layered architecture backend server written in C# using [ASP.NET 7 framework](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-7.0?view=aspnetcore-8.0) for the mobile appplication [AIHouseKeeper](https://github.com/Qlinak/AIHouseKeeper)

## Feature
1. Code first migration using [Microsoft Entity Framework](https://learn.microsoft.com/en-us/ef/core/)
2. [JWT](https://auth0.com/docs/secure/tokens/json-web-tokens#:~:text=JSON%20web%20token%20(JWT)%2C,parties%20as%20a%20JSON%20object.) powered authentication and authorisation
3. [Swagger](https://swagger.io/)
4. Auto dependency injection

## How to run
1. git clone
2. Install the .NET7 SDK: You can download it from [the official .NET website](https://dotnet.microsoft.com/)
3. Install [PostgreSQL](https://www.postgresql.org/download/) and run it
4. Do a database migration in the project root directory by following [EF core migration guide](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) to create local DB
5. Go to project directory
6. Restore the project by typing `dotnet restore`
7. Build the project `dotnet build`
8. Run the project `dotnet run` (Or just use an IDE like [Rider](https://www.jetbrains.com/rider/download/#section=mac)~)
