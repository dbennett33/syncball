# Syncball

A C# .NET solution that replicates the API-Football database locally using their REST API. The system continuously updates the local database to maintain near 100% replication while carefully managing API rate limits.

## Key Features

- Local database mirroring the API-Football data structure
- Scheduled tasks for data synchronization based on fixture timing
- Configurable rate limiting to comply with API restrictions
- Priority-based synchronization for optimal data freshness
- Robust error handling and comprehensive logging

## Project Structure

The solution follows a clean architecture pattern with clear separation of concerns:

```
Syncball/
├── src/
│   ├── Syncball.Api/                    # API endpoints (if needed)
│   ├── Syncball.Core/                   # Domain models and interfaces
│   ├── Syncball.Infrastructure/         # Database, API clients
│   ├── Syncball.Service/                # Sync services
│   └── Syncball.Worker/                 # Background processing
├── tests/
│   ├── Syncball.Tests.Unit/
│   └── Syncball.Tests.Integration/
└── tools/
    └── Syncball.DbMigrator/
```

## Getting Started

### Prerequisites

- .NET 8.0
- SQL Server (or compatible database)
- API-Football subscription (Free tier minimum)

### Configuration

Set up your API key and rate limit settings in `appsettings.json`:

```json
{
  "ApiFootball": {
    "BaseUrl": "https://v3.football.api-sports.io",
    "ApiKey": "YOUR_API_KEY_HERE",
    "RateLimit": {
      "Tier": "Free",
      "ReservePercentage": 10,
      "AllowReserveForCritical": true
    }
  }
}
```

### Running the Application

1. Clone the repository
2. Update the configuration
3. Run database migrations: `dotnet run --project tools/Syncball.DbMigrator`
4. Start the worker service: `dotnet run --project src/Syncball.Worker`

## Documentation

Detailed documentation is available in the `/docs` directory:

- [System Architecture](docs/architecture.md)
- [Database Schema](docs/database-schema.md)
- [Rate Limiting System](docs/rate-limiting.md)
- [Synchronization Strategy](docs/sync-strategy.md)
- [Project Structure](docs/project-structure.md)
- [Deployment Guide](docs/deployment-guide.md)

## Contributing

Contributions are welcome! Please check out our [contribution guidelines](docs/contributing.md).

## License

This project is licensed under the MIT License - see the LICENSE file for details.