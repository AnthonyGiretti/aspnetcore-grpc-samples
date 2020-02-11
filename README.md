# Samples of gRPC services in ASP.NET Core 3.1 (work in progress)

## Prerequistes 

Create a database, don't forget to adjust the name of it in your connectionstring and in the sql files

Create table with Sql file: https://github.com/AnthonyGiretti/aspnetcore3-grpc-samples/blob/master/Database/Create%20dbo.Country.sql

Feed the table with Sql file: https://github.com/AnthonyGiretti/aspnetcore3-grpc-samples/blob/master/Database/dbo.Country.data.sql

If you don't setup a database, the sample runs by default by In Memory database

## Samples
Sample of Layered architecture (Ntier)

Sample of Repository pattern with EF Core 3

Sample of gRPC CRUD operation

Sample of gRPC request Validation with [https://github.com/AnthonyGiretti/grpc-aspnetcore-validator]

Sample of mapping with AutoMapper

Sample of gRPC request interceptions

Sample of token validation and get authenticated user from ServerCallContext

Sample of KeyVault configuration and usage

Sample of .NET Core 3.1 client CRUD consumption

Sample of .NET Core 3.1 client using Polly resiliency

Sample of integration tests (work in progess)

Sample of healthcheck (work in progess)
