# Booking System Backend

A portfolio-grade .NET 8 booking system backend featuring clean architecture, Redis-based concurrency control, JWT authentication, and comprehensive testing.

## 🚀 Features

- **Clean Architecture** - Separated layers (Domain, Application, Infrastructure, API)
- **Concurrency Control** - Redis atomic operations prevent overbooking
- **JWT Authentication** - Secure token-based auth
- **Credit System** - Purchase packages and book classes with credits
- **Waitlist Management** - FIFO queue with auto-promotion
- **Refund Logic** - Time-based cancellation refunds (≥4 hours)
- **Background Jobs** - Hangfire for scheduled tasks
- **Comprehensive Testing** - Unit, integration, and concurrency tests
- **Docker Support** - Fully containerized development environment
- **API Documentation** - OpenAPI/Swagger with examples

## 🏗️ Architecture

Built using Clean Architecture principles with clear separation of concerns:
├── API Layer (Presentation)
├── Application Layer (Business Logic)
├── Domain Layer (Core Entities & Rules)
└── Infrastructure Layer (Data Access & External Services)

See [Architecture Documentation](docs/architecture.md) for detailed diagrams.

## 🛠️ Technology Stack

- **.NET 8.0** - Latest LTS version
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 8** - ORM with MySQL provider
- **MySQL 8.0** - Relational database
- **Redis** - Caching and concurrency control
- **Hangfire** - Background job processing
- **xUnit** - Testing framework
- **Docker & Docker Compose** - Containerization

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/downloads)
- [Visual Studio Code](https://code.visualstudio.com/) (recommended)

## 🚀 Getting Started

### 1. Clone the Repository