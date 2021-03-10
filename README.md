# ASP.net Chat API

Api that allows user creation, login, logout, message submission and retrieval

## Requirements

 - [VisualStudio](https://visualstudio.microsoft.com/) or [VisualStudioCode](https://visualstudio.microsoft.com/) (VSCode)
 - [.NET SDK](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)
 - [Npgsql](https://www.npgsql.org/)
 - [Postgresql](https://www.postgresql.org/)
   - 'chatapi' database [database.sql](database.sql)
     - 'users' table
       - 'name' TEXT NOTNULL PRIMARYKEY
       - 'joined' TEXT NOTNULL
       - 'token' TEXT UNIQUE
       - 'password' TEXT NOTNUL
     - 'messages' table
       - 'id' BIGINT NOTNULL PRIMARYKEY
       - 'created' TEXT NOTNULL
       - 'modified' TEXT
       - 'text' TEXT NOTNULL
       - 'username' TEXT FOREIGNKEY (users -> name)

### Demo

 - [video](https://youtu.be/z0TYYZw_6FU)

### Run

 1. Restore database from backup file [database.sql](database.sql)
 2. Change 'Host' and 'Password' in the ConnectionString inside [Database.cs](Database.cs)
 3. Open and run code inside VisualStudio
 4. Open the URL https://localhost:5001/swagger
