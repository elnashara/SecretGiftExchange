# Secret Gift Exchange

## Project Overview
The Secret Gift Exchange is a C# application designed to facilitate the organization of gift exchanges. It allows users to enter participant names, automatically assigns partners for gift giving, and manages the gift exchange process.

## Features
- Participant management (add, update, remove participants).
- Random gift assignment among participants.
- Display of participant assignments.
- Persistent storage of participant data.

## Getting Started

### Prerequisites
- .NET 7.0
- Visual Studio (recommended)

### Installation
1. Clone the repository:
   ```bash
   git clone [repository-url]
   ```

### Running the Application:
1. To run the application, execute the following command in the terminal or use the run feature in Visual Studio:
   ```bash
   dotnet run --project ./src/SecretGiftExchange/SecretGiftExchange.csproj
   ```

## Running Unit Tests

To ensure the reliability and correctness of the "Secret Gift Exchange" application, a suite of unit tests has been developed. These tests can be run easily to verify the functionality of various parts of the application. Here's how you can run them:

### Prerequisites
- Ensure that you have followed the installation steps mentioned above and have the project set up on your local machine.
- The test project uses MSTest, which should be included if you are using Visual Studio.

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
  dotnet test
  ```
- This command will build the solution and run all tests in the test projects. The output will indicate whether the tests passed or failed, along with any errors or failed assertions.

### Interpreting Test Results
- The results of the test runs will be displayed in the output.
- For each test, you will see either `Passed`, `Failed`, or `Skipped`.
- Investigate any failed tests to understand and rectify potential issues in the application.


## Usage

## Contributing

## License

## Contact
- Project Maintainer: Ashraf Elnashar - ashraf.elnashar@vanderbilt.edu
- Project Link: https://github.com/elnashara/SecretGiftExchange

