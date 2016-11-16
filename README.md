# GitGudDevelopment
The GitGud Project...
First check if you have LocalDb. Go to VS->View->SQL Server Object Explorer. You have to have something like: (localdb)\MSSQLLocalDB.
If you have LocalDb, create DB for GitGud project: in VS go to src folder->GitGud and use keyboard combination: Alt + Space.
You will open cmd and enter this commands: 
dotnet ef migrations add InitialDb
dotnet ef database update