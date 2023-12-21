# Secret Gift Exchange

## Project Overview
SecretGiftExchange API is an ASP.NET Core Web API for managing a secret gift exchange event. It provides endpoints for adding participants, assigning gifts, and viewing gift assignments.

## Features
- Participant management (add, update, remove participants).
- Random gift assignment among participants.
- Display of participant assignments.
- Persistent storage of participant data.

## Getting Started

### Prerequisites
- .NET Core SDK (Version 7.0 or higher)
- A text editor or IDE (like Visual Studio, Visual Studio Code)

### Dependencies
The project uses several NuGet packages, which are listed in the .csproj file and will be automatically restored when you build the project. Key dependencies include:

- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Json
- Microsoft.Extensions.Configuration.Binder (if applicable)

### Moq for Unit Testing
Moq: A popular and friendly mocking library for .NET, used in unit testing to mock interfaces and classes. It is used in our test project to simulate the behavior of external dependencies like IConfiguration.
To install Moq in your test project, use the following command:

   ```bash
   dotnet add package Moq
   ```

### Running Tests via Visual Studio
- Open the `SecretGiftExchange.sln` solution file in Visual Studio.
- In the Solution Explorer, right-click on the `SecretGiftExchange.Tests` project.
- Select `Run Tests`. This will execute all the unit tests in the project.
- Alternatively, you can use the Test Explorer in Visual Studio to run individual tests or groups of tests.

### Running Tests via Command Line
- Open a command prompt or terminal.
- Navigate to the root directory of the project where the solution file (`SecretGiftExchange.sln`) is located.
- Run the following command:
  ```
  cd SecretGiftExchange/tests/SecretGiftExchange.Tests
  dotnet test
  ```
- This command will build the solution and run all tests in the test projects. The output will indicate whether the tests passed or failed, along with any errors or failed assertions.
- The tests use Moq to mock external dependencies, ensuring that our tests are focused on the business logic.

### Interpreting Test Results
- The results of the test runs will be displayed in the output.
- For each test, you will see either `Passed`, `Failed`, or `Skipped`.
- Investigate any failed tests to understand and rectify potential issues in the application.

### Publishing the Application
The SecretGiftExchange API can be published as a self-contained deployment. This method packages the application and the .NET runtime into a single executable, allowing it to run on any compatible operating system without requiring a separate .NET installation.

1. Open Command Prompt or Terminal: Navigate to your project's root directory (where the .csproj file is located).
2. Publish the Application: Use the dotnet publish command with the --self-contained option. Replace <RID> with the Runtime Identifier for your target platform (e.g., win-x64 for Windows 64-bit, linux-x64 for Linux 64-bit).
   ```bash
   dotnet publish -c Release -r <RID> --self-contained true -o ./publish
   ```

For example, to publish for Windows 64-bit:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
   ```
3. Locate the Published Files: After the command completes, the published files, including the executable, will be in the ./publish directory.

### Publishing the Application
- Copy the contents of the ./publish directory to the target machine.
- Run the executable file directly (SecretGiftExchange.API.exe). No additional .NET runtime installation is required on the target machine.

### Running the Application:
1. Clone the project repository to your local machine.
   ```bash
   git clone https://github.com/elnashara/SecretGiftExchange.git
   ```
2. Navigate to the Project Directory by Changing to the directory containing the Web API project.
   ```bash
   cd SecretGiftExchange/src/SecretGiftExchange.API
   ```
3. Build and compiles the project and is optional as dotnet run also performs a build.
   ```bash
   dotnet build
   ```
4. Run the Application by starting the Web API using the dotnet run command.
   ```bash
   dotnet run
   ```
   The terminal will display the URLs on which the API is listening, e.g., http://localhost:5196.

5. Accessing the Application: Open a web browser and navigate to the provided URL.


### Using Swagger
This project is configured with Swagger to provide easy-to-use API documentation and testing capabilities.

1. Open Swagger UI: Once the application is running, navigate to http://localhost:5196/swagger in your web browser.
2. Explore the API: Through the Swagger UI, you can see all the available API endpoints, their expected input, and output formats.
3. Test the API: You can also interact with the API directly from the Swagger UI by sending requests to different endpoints and observing the responses.

### Additional Information
- Configuration: Configuration settings like file paths or connection strings can be managed in the appsettings.json file.
- Logging: The application is configured to log various events, which can be customized in the appsettings.json file.

## Usage

## Contributing

## License

## Contact
- Project Maintainer: Ashraf Elnashar - ashraf.elnashar@vanderbilt.edu
- Project Link: https://github.com/elnashara/SecretGiftExchange

