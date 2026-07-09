# AI CRM Assistant

AI-powered CRM backend for analyzing customer conversations with support for multiple AI providers.

## Features

- **Company Management** - Multi-tenant CRM with JWT authentication & refresh tokens
- **User Management** - CRUD operations per company with pagination and search
- **Conversation Management** - Create, retrieve, and list conversations
- **Message Tracking** - Add and retrieve messages per conversation
- **AI Conversation Analysis** - Lead scoring, sentiment detection, suggested replies & actions
- **Multi-Provider AI** - Supports OpenAI, DeepSeek, Gemini, OpenRouter
- **Health Checks** - Ready endpoint at `/health`
- **Structured Logging** - Serilog with console and rolling file output

## Tech Stack

- ASP.NET Core 10 (Minimal API)
- PostgreSQL 17 + Entity Framework Core
- FluentValidation
- Serilog
- Scalar OpenAPI (Swagger)
- xUnit + Moq + FluentAssertions (Tests)

## Architecture

**Clean Architecture** with **Vertical Slice** pattern:

```
src/
  Domain.AI-Assistans/        # Entities, Enums, Interfaces
  Application.AI-Assistans/   # DTOs, Service Interfaces
  Infrastructure.AI-Assistans/# Persistence, AI Services, JWT
  AI-Assistans-CRM-Service/   # Minimal API Endpoints, Middleware
  AI-Assistans-CRM-Service.Tests/  # Unit Tests
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker Desktop (for PostgreSQL)

### Setup

1. Clone the repo
2. Start PostgreSQL: `docker compose up -d postgres`
3. Apply migrations: `dotnet ef database update`
4. Run the API: `dotnet run --project AI-Assistans-CRM-Service`
5. Open Swagger UI at `http://localhost:5041/swagger`

### Configuration

Set these environment variables or use User Secrets:

```bash
dotnet user-secrets set "Jwt:Key" "<your-256-bit-secret-key>"
dotnet user-secrets set "AIProviders:DeepSeek:ApiKey" "<your-key>"
```

## API Endpoints

### Auth
- `POST /auth/login` - Company login
- `POST /auth/refresh` - Refresh JWT token

### Users
- `POST /users` - Create user
- `GET /users` - List users (paginated, searchable)

### Conversations
- `POST /conversations` - Create conversation
- `GET /conversations/{id}` - Get conversation with messages & analyses
- `GET /users/{userId}/conversations` - Get user conversations
- `GET /company/conversations` - Get company conversations (paginated)
- `POST /conversations/{id}/analyze` - Run AI analysis
- `GET /conversations/{id}/analysis/latest` - Get latest analysis

### Messages
- `POST /messages` - Add message
- `GET /conversations/{id}/messages` - Get conversation messages

### Health
- `GET /health` - Health check

## Testing

```bash
dotnet test
```

## Docker

```bash
docker compose up --build
```

## License

MIT
