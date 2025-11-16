# Calculator API

A .NET 10 Web API for calculating probability operations. This API provides endpoints for performing probability calculations with comprehensive logging and error handling.

## ?? Features

- **Probability Calculations**: Supports "CombinedWith" (intersection) and "Either" (union) operations
- **Input Validation**: Validates probability values are between 0.0 and 1.0
- **Comprehensive Logging**: Daily rotating log files with NLog
- **OpenAPI Documentation**: Integrated Swagger/OpenAPI documentation
- **CORS Support**: Configured for cross-origin requests
- **Error Handling**: Robust error handling with meaningful error messages

## ?? Table of Contents

- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Request/Response Examples](#requestresponse-examples)
- [Logging](#logging)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Contributing](#contributing)
- [License](#license)

## ????? Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2022 or Visual Studio Code
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/ajayghosh-8831/CalculatorAPI.git
   cd CalculatorAPI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project CalculatorAPI
   ```

5. **Access the API**
   - API Base URL: `http://localhost:5267`
   - Swagger UI: `http://localhost:5267/swagger`

## ?? Usage

The API calculates probabilities for two events (PA and PB) using different operations:

### Supported Operations

| Operation | Description | Formula |
|-----------|-------------|---------|
| `CombinedWith` | Probability of both events occurring (Intersection) | PA × PB |
| `Either` | Probability of either event occurring (Union) | PA + PB - (PA × PB) |

## ?? API Endpoints

### Calculate Probability

**POST** `/api/Calculator/calculate`

Calculates probability based on the provided operation.

#### Request Body

```json
{
  "PA": 0.5,
  "PB": 0.3,
  "Operation": "CombinedWith"
}
```

#### Response

```json
{
  "pa": 0.5,
  "pb": 0.3,
  "operation": "CombinedWith",
  "result": 0.15
}
```

## ?? Request/Response Examples

### Example 1: Combined Probability (Intersection)

**Request:**
```bash
curl -X POST "http://localhost:5267/api/Calculator/calculate" \
     -H "Content-Type: application/json" \
     -d '{
       "PA": 0.6,
       "PB": 0.4,
       "Operation": "CombinedWith"
     }'
```

**Response:**
```json
{
  "pa": 0.6,
  "pb": 0.4,
  "operation": "CombinedWith",
  "result": 0.24
}
```

### Example 2: Either Probability (Union)

**Request:**
```bash
curl -X POST "http://localhost:5267/api/Calculator/calculate" \
     -H "Content-Type: application/json" \
     -d '{
       "PA": 0.7,
       "PB": 0.5,
       "Operation": "Either"
     }'
```

**Response:**
```json
{
  "pa": 0.7,
  "pb": 0.5,
  "operation": "Either",
  "result": 0.85
}
```

### Error Response

**Invalid Input:**
```json
{
  "error": "PA must be between 0.0 and 1.0"
}
```

## ?? Logging

The application uses **NLog** for comprehensive logging with the following features:

### Log Configuration
- **Daily Rotation**: New log file created each day
- **File Naming**: `YYYY-MM-DD.log` format (e.g., `2025-11-16.log`)
- **Location**: `CalculatorAPI/Logs/` directory
- **Retention**: 30 days of archived logs
- **Levels**: Info, Warning, Error

### Log File Structure
```
CalculatorAPI/Logs/
??? 2025-11-16.log          # Current day's logs
??? 2025-11-15.log          # Previous day's logs
??? internal-nlog.txt       # NLog diagnostic logs
??? archive/                # Archived logs (30+ days old)
    ??? older-log-files.log
```

### What Gets Logged
- API requests and responses
- Calculation operations and results
- Validation warnings
- Error details with stack traces
- Application startup and shutdown events

## ?? Project Structure

```
CalculatorAPI/
??? Controllers/
?   ??? CalculatorController.cs     # API endpoints
??? Contracts/
?   ??? ProbabilityRequest.cs       # Request model
?   ??? ProbabilityResult.cs        # Response model
??? Resources/
?   ??? ICalculatorResource.cs      # Business logic interface
?   ??? CalculatorResource.cs       # Business logic implementation
??? Helper/
?   ??? OperationType.cs            # Operation enumeration
??? Logs/                           # Log files directory
??? Program.cs                      # Application entry point
??? nlog.config                     # Logging configuration
??? CalculatorAPI.csproj            # Project file
??? CalculatorAPI.http              # HTTP test requests

CalculatorAPITestProject/
??? CalculatorResourceTests.cs      # Unit tests
```

## ?? Technologies Used

- **Framework**: .NET 10
- **Language**: C# 14.0
- **Web Framework**: ASP.NET Core
- **Logging**: NLog
- **Documentation**: OpenAPI/Swagger
- **Testing**: xUnit (in test project)
- **Architecture**: Repository Pattern, Dependency Injection

## ?? Configuration

### Application Settings
- **CORS**: Configured to allow all origins for development
- **HTTPS Redirection**: Enabled
- **OpenAPI**: Enabled with Swagger UI

### NLog Configuration
The logging is configured in `nlog.config` with:
- Console and file targets
- Daily file rotation
- Structured logging with timestamps
- Automatic directory creation

## ?? Testing

Run the unit tests:
```bash
dotnet test
```

### Manual Testing with HTTP File
Use the provided `CalculatorAPI.http` file in Visual Studio or VS Code with the REST Client extension.

## ?? Deployment

### Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish -c Release
dotnet run --environment Production
```

## ?? Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ?? Contact

**Author**: Ajay Ghosh  
**GitHub**: [@ajayghosh-8831](https://github.com/ajayghosh-8831)  
**Repository**: [CalculatorAPI](https://github.com/ajayghosh-8831/CalculatorAPI)

## ?? Version History

- **v1.0.0** - Initial release
  - Basic probability calculations
  - Daily rotating logs
  - OpenAPI documentation
  - Input validation

---

## ?? Additional Resources

- [.NET 10 Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [NLog Documentation](https://nlog-project.org/documentation/)
- [OpenAPI/Swagger Documentation](https://swagger.io/docs/)

---

**Happy Calculating! ??**