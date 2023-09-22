### .NET MongoDB Boiler Repo

Purpose is to create a generic examples and PoCs regarding MongoDB and .NET driver.


## MongoDBWebApp

Backend web service that supports CRUD operations in MongoDB

## ChangeStreamsApp

Background Worker service that listens on changes driven by the MongoDBWebApp via MongoDB ChangeStreams

## Usage
- Modify both appsettings.json to reflect your actual deployment connection strings
- Start the MongoDBWebApp and ChangeStreamsApp
- Import and use the MongoDBWebApp/test into Postman to issue CRUD operations