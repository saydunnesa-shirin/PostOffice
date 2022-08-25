# Post Office
It is an API project for managing parcels and letters in the post office.

## Tools and technologies
Visual Studio 2022, .NET 6 API, Entity Framework Core 6, AutoMapper, FluentValidation, NUnit and Moq.

## Project Structure
![Project Structure Image](images/project_structure.PNG)

## Data Store
MsSqllocaldb was used for storing data and entity framework core for CRUD operation. All the entities are added to (PostOffice.Repository/Entities) folder.
A class called "DataContext" was used for db interaction. In PostOffice.API has the appsettings.json file, which contains db connection information. 
DB Migration command:

    >> dotnet ef migrations add InitialCreate --project .\PostOffice\PostOffice.Api.csproj
    >> dotnet ef database update --project .\PostOffice\PostOffice.Api.csproj

As DataContext and appsettings.json are in two different projects, so needed to add project information (--project .\PostOffice\PostOffice.Api.csproj) in the commands. ![Sql Migrations Image](images/sql_migrations.PNG)

## How to run
Open the solution using visual studio 22 and run.

## API description
There are fourteen API endpoints:

1. api/Shipments/get, To get all the shipments(s).

2. api/Shipments/get{id}, To get the shipment by-shipment id.

3. api/Shipments/post, To add a shipment, all validation is checked before adding a new shipment with bags.

4. api/Shipments/put, To updated or finalize a shipment, all validation is checked before updating a shipment with bags.

5. api/Shipments/delete, To dalete a shipment and to free all the bags under this shipment, all validation is checked before deleting.

6. api/Bags/get, To get all the bags(s).

7. api/Bags/get{id}, To get the bag by-bag id.

8. api/Bags/post, To add a bag, all validation is checked before adding a new bag with parcels or letters.

9. api/Bags/put, To updated a bag, all validation is checked before updating a bag with parcels or letters.

10. api/Bags/delete, To dalete a bag and to free all the parcels under this bag, all validation is checked before deleting.

11. api/Parcels/get, To get all the parcels(s).

12. api/Parcels/get{id}, To get the parcel by-parcel id.

13. api/Parcels/post, To add a parcel, all validation is checked before adding a new parcel.

14. api/Parcels/put, To updated a parcel, all validation is checked before updating a parcel.


## Postman
A postman collection is included to call the APIs.

A json script is added in this [a link] (https://github.com/saydunnesa-shirin/PM/blob/main/docs/Postman/PM.postman_collection.json) link.