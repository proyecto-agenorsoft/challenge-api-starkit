# Filters API

## Description

Filters API is a RESTful service designed to provide advanced filtering operations on a dataset of individuals. Capable of performing detailed searches based on various criteria such as gender and name, it is built upon a robust architecture that clearly separates concerns through layer separation and dependency injection.

## Project Structure

The project is organized into several layers to promote a clean and maintainable design:

- **Filters.API**: Contains controllers that handle HTTP requests, responsible for client interaction.
- **Filters.Contracts**: Defines interfaces that decouple the service and repository layers.
- **Filters.Domain**: Includes domain entities and any associated business logic.
- **Filters.Infrastructure**: Contains concrete implementations of repositories that interact with the database or data sources.
- **Filters.Services**: Provides service implementations that contain business logic, used by the controllers.
- **Filters.Test**: Houses unit tests to ensure the business logic works as expected.

## Configuration

To set up the project in your local environment, follow these steps:

1. Clone the repository to your local machine.
2. Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed.
3. Navigate to the `Filters.API` folder and execute `dotnet restore` to install dependencies.
4. Start the service by executing `dotnet run`.

## Usage

Once the API is running, you can make requests to the following endpoints:

- GET/api/person
