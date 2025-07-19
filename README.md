Database Setup and Connection Instructions
1. Create the Database
    First, either:

    Create a new database named BTL_N6_QLKS, or

    Use the backup file btl_QLKS.bak to restore the database in SQL Server.

2. Update the Connection String
    Open the file connectDb.cs and replace the old connection string with your current database connection string.

3. How to Get the Connection String in Visual Studio 2022

    - Go to Tools → Connect to Database

    - Select your SQL Server and the database you want to connect to

    - In the Server Explorer, find your connected database

    - -In the Properties window, double-click on the Connection String field to copy it

Paste the copied connection string into connectDb.cs.