# Cafe Menu Project

Cafe Menu Project is a web application built with ASP.NET Core that allows cafe owners to manage their menu and customers to view the menu with real-time currency conversion.

## Features

- User authentication and authorization
- Product management (CRUD operations)
- Category management
- Real-time currency conversion
- Admin dashboard with product statistics and exchange rates
- Responsive design for various devices

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- Bootstrap 4
- jQuery

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server

### Installation

1. Clone the repository
   ```
   git clone https://github.com/yourusername/CafeMenuProject.git
   ```

2. Navigate to the project directory
   ```
   cd CafeMenuProject
   ```

3. Restore the NuGet packages
   ```
   dotnet restore
   ```

4. Update the database
   ```
   dotnet ef database update
   ```

5. Run the application
   ```
   dotnet run
   ```

The application should now be running on `https://localhost:5001`.

## Demo User Login Information

### Admin User
- Username: admin@cafemenu.com
- Password: Admin123!

### Regular User
- Username: user@cafemenu.com
- Password: User123!

## Usage

- Visit the home page to view the menu as a customer
- Log in as an admin to access the dashboard and manage products and categories
- Use the currency selector to view prices in different currencies

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.

