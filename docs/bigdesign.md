football-sync/
├── cmd/
│   └── footballsync/
│       └── main.go           # Application entry point
├── internal/
│   ├── apiclient/            # API Client implementation
│   ├── db/                   # Database operations
│   ├── syncer/               # Data Synchronizer implementation
│   ├── config/               # Configuration management
│   ├── models/               # Shared data models
│   ├── migrations/           # Database migrations
│   └── logging/              # Logging utilities
├── pkg/
│   ├── api/                  # Public API (if exposing data)
│   └── utils/                # Shared utilities
├── scripts/                  # Utility scripts
├── configs/                  # Configuration files
├── .gitignore
├── go.mod
├── go.sum
└── README.md
```

## Main Application Flow

The main application serves as the orchestrator for the entire system, initializing components, handling configuration, and managing the application lifecycle.

### Entry Point (main.go)

```go
package main

import (
	"context"
	"flag"
	"fmt"
	"os"
	"os/signal"
	"syscall"
	"time"

	"github.com/your-org/football-sync/internal/apiclient"
	"github.com/your-org/football-sync/internal/config"
	"github.com/your-org/football-sync/internal/db"
	"github.com/your-org/football-sync/internal/syncer"
	"go.uber.org/zap"
)

func main() {
	// Parse command line flags
	configPath := flag.String("config", "configs/config.yaml", "Path to configuration file")
	logLevel := flag.String("log-level", "info", "Log level (debug, info, warn, error)")
	syncOnce := flag.Bool("sync-once", false, "Run sync once and exit")
	syncType := flag.String("sync-type", "", "Type of sync to run (fixtures, leagues, teams, etc.)")
	flag.Parse()

	// Initialize logger
	logger, err := initLogger(*logLevel)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to initialize logger: %v\n", err)
		os.Exit(1)
	}
	defer logger.Sync()

	// Load configuration
	cfg, err := config.LoadConfig(*configPath)
	if err != nil {
		logger.Fatal("Failed to load configuration", zap.Error(err))
	}

	// Connect to database
	database, err := db.Connect(cfg.Database)
	if err != nil {
		logger.Fatal("Failed to connect to database", zap.Error(err))
	}
	defer database.Close()

	// Run database migrations
	if err := db.MigrateUp(database, cfg.Database.MigrationsPath); err != nil {
		logger.Fatal("Failed to run database migrations", zap.Error(err))
	}

	// Create API client
	apiClient := apiclient.NewClient(
		cfg.API.APIKey,
		apiclient.SubscriptionTier(cfg.API.SubscriptionTier),
		logger.Named("api_client"),
	)

	// Create synchronizer
	syncService := syncer.NewSyncService(
		apiClient,
		database,
		logger.Named("syncer"),
		&syncer.Config{
			LiveFixturesSchedule:     cfg.Sync.LiveFixturesSchedule,
			UpcomingFixturesSchedule: cfg.Sync.UpcomingFixturesSchedule,
			LeaguesSchedule:          cfg.Sync.LeaguesSchedule,
			DefaultPastDays:          cfg.Sync.DefaultPastDays,
			DefaultFutureDays:        cfg.Sync.DefaultFutureDays,
			WorkerPoolSize:           cfg.Sync.WorkerPoolSize,
			TaskTimeout:              cfg.Sync.TaskTimeout,
			BatchSize:                cfg.Sync.BatchSize,
			APIRetryAttempts:         cfg.Sync.APIRetryAttempts,
			APIRetryDelay:            cfg.Sync.APIRetryDelay,
		},
	)

	// If sync-once flag is provided, run sync once and exit
	if *syncOnce {
		logger.Info("Running sync once")
		
		ctx, cancel := context.WithTimeout(context.Background(), 30*time.Minute)
		defer cancel()
		
		if *syncType != "" {
			err = runSpecificSync(ctx, syncService, *syncType, logger)
		} else {
			err = syncService.SyncAll(ctx)
		}
		
		if err != nil {
			logger.Fatal("Sync failed", zap.Error(err))
		}
		logger.Info("Sync completed successfully")
		return
	}

	// Start the scheduled sync
	if err := syncService.StartScheduledSync(); err != nil {
		logger.Fatal("Failed to start scheduled sync", zap.Error(err))
	}

	// Handle graceful shutdown
	quit := make(chan os.Signal, 1)
	signal.Notify(quit, syscall.SIGINT, syscall.SIGTERM)
	
	<-quit
	logger.Info("Shutting down...")
	
	// Stop the scheduled sync
	if err := syncService.StopScheduledSync(); err != nil {
		logger.Error("Failed to stop scheduled sync", zap.Error(err))
	}
	
	logger.Info("Application stopped")
}

// initLogger initializes the logger with the given log level
func initLogger(level string) (*zap.Logger, error) {
	var zapLevel zap.AtomicLevel
	
	switch level {
	case "debug":
		zapLevel = zap.NewAtomicLevelAt(zap.DebugLevel)
	case "info":
		zapLevel = zap.NewAtomicLevelAt(zap.InfoLevel)
	case "warn":
		zapLevel = zap.NewAtomicLevelAt(zap.WarnLevel)
	case "error":
		zapLevel = zap.NewAtomicLevelAt(zap.ErrorLevel)
	default:
		zapLevel = zap.NewAtomicLevelAt(zap.InfoLevel)
	}
	
	logConfig := zap.Config{
		Level:             zapLevel,
		Development:       false,
		Encoding:          "json",
		OutputPaths:       []string{"stdout"},
		ErrorOutputPaths:  []string{"stderr"},
		EncoderConfig:     zap.NewProductionEncoderConfig(),
	}
	
	return logConfig.Build()
}

// runSpecificSync runs a specific type of sync
func runSpecificSync(ctx context.Context, syncService syncer.Synchronizer, syncType string, logger *zap.Logger) error {
	switch syncType {
	case "fixtures":
		options := syncer.FixtureSyncOptions{
			DateRange: &syncer.DateRange{
				Start: time.Now().AddDate(0, 0, -7),
				End:   time.Now().AddDate(0, 0, 30),
			},
		}
		return syncService.SyncFixtures(ctx, options)
	case "live_fixtures":
		return syncService.SyncLiveFixtures(ctx)
	case "leagues":
		return syncService.SyncLeagues(ctx)
	case "teams":
		// This requires league ID and season
		logger.Error("Teams sync requires league ID and season. Please use -league-id and -season flags")
		return fmt.Errorf("teams sync requires additional parameters")
	default:
		return fmt.Errorf("unknown sync type: %s", syncType)
	}
}
```

## Configuration Management

The configuration system uses a combination of file-based configuration and environment variables.

### Configuration Structure

```go
package config

import (
	"fmt"
	"os"
	"time"

	"github.com/spf13/viper"
)

// Config represents the application configuration
type Config struct {
	API      APIConfig      `mapstructure:"api"`
	Database DatabaseConfig `mapstructure:"database"`
	Sync     SyncConfig     `mapstructure:"sync"`
	Server   ServerConfig   `mapstructure:"server"`
}

// APIConfig contains configuration for the API client
type APIConfig struct {
	APIKey           string `mapstructure:"api_key"`
	BaseURL          string `mapstructure:"base_url"`
	TimeoutSeconds   int    `mapstructure:"timeout_seconds"`
	RetryAttempts    int    `mapstructure:"retry_attempts"`
	RetryDelay       int    `mapstructure:"retry_delay_seconds"`
	SubscriptionTier string `mapstructure:"subscription_tier"`
}

// DatabaseConfig contains configuration for the database
type DatabaseConfig struct {
	Host          string `mapstructure:"host"`
	Port          int    `mapstructure:"port"`
	Username      string `mapstructure:"username"`
	Password      string `mapstructure:"password"`
	Database      string `mapstructure:"database"`
	SSLMode       string `mapstructure:"ssl_mode"`
	MigrationsPath string `mapstructure:"migrations_path"`
	MaxOpenConns  int    `mapstructure:"max_open_conns"`
	MaxIdleConns  int    `mapstructure:"max_idle_conns"`
	ConnMaxLifetime int  `mapstructure:"conn_max_lifetime_minutes"`
}

// SyncConfig contains configuration for the synchronizer
type SyncConfig struct {
	LiveFixturesSchedule     string        `mapstructure:"live_fixtures_schedule"`
	UpcomingFixturesSchedule string        `mapstructure:"upcoming_fixtures_schedule"`
	LeaguesSchedule          string        `mapstructure:"leagues_schedule"`
	DefaultPastDays          int           `mapstructure:"default_past_days"`
	DefaultFutureDays        int           `mapstructure:"default_future_days"`
	WorkerPoolSize           int           `mapstructure:"worker_pool_size"`
	TaskTimeout              time.Duration `mapstructure:"task_timeout_minutes"`
	BatchSize                int           `mapstructure:"batch_size"`
	APIRetryAttempts         int           `mapstructure:"api_retry_attempts"`
	APIRetryDelay            time.Duration `mapstructure:"api_retry_delay_seconds"`
}

// ServerConfig contains configuration for the API server (if exposing data)
type ServerConfig struct {
	Host            string `mapstructure:"host"`
	Port            int    `mapstructure:"port"`
	ReadTimeoutSec  int    `mapstructure:"read_timeout_seconds"`
	WriteTimeoutSec int    `mapstructure:"write_timeout_seconds"`
}

// LoadConfig loads the configuration from the given file path
func LoadConfig(configPath string) (*Config, error) {
	viper.SetConfigFile(configPath)
	
	// Set default values
	setDefaults()
	
	// Read the config file
	if err := viper.ReadInConfig(); err != nil {
		return nil, fmt.Errorf("failed to read config file: %w", err)
	}
	
	// Override with environment variables
	viper.SetEnvPrefix("FOOTBALL_SYNC")
	viper.AutomaticEnv()
	
	// Parse the config
	var config Config
	if err := viper.Unmarshal(&config); err != nil {
		return nil, fmt.Errorf("failed to unmarshal config: %w", err)
	}
	
	// Validate the config
	if err := validateConfig(&config); err != nil {
		return nil, fmt.Errorf("invalid configuration: %w", err)
	}
	
	// Process environment-specific overrides
	processEnvOverrides(&config)
	
	return &config, nil
}

// setDefaults sets default values for configuration
func setDefaults() {
	// API defaults
	viper.SetDefault("api.base_url", "https://api-football-v1.p.rapidapi.com/v3")
	viper.SetDefault("api.timeout_seconds", 30)
	viper.SetDefault("api.retry_attempts", 3)
	viper.SetDefault("api.retry_delay_seconds", 2)
	viper.SetDefault("api.subscription_tier", "free")
	
	// Database defaults
	viper.SetDefault("database.host", "localhost")
	viper.SetDefault("database.port", 5432)
	viper.SetDefault("database.ssl_mode", "disable")
	viper.SetDefault("database.migrations_path", "internal/migrations")
	viper.SetDefault("database.max_open_conns", 10)
	viper.SetDefault("database.max_idle_conns", 5)
	viper.SetDefault("database.conn_max_lifetime_minutes", 60)
	
	// Sync defaults
	viper.SetDefault("sync.live_fixtures_schedule", "*/5 * * * *")
	viper.SetDefault("sync.upcoming_fixtures_schedule", "0 */12 * * *")
	viper.SetDefault("sync.leagues_schedule", "0 0 * * *")
	viper.SetDefault("sync.default_past_days", 7)
	viper.SetDefault("sync.default_future_days", 30)
	viper.SetDefault("sync.worker_pool_size", 10)
	viper.SetDefault("sync.task_timeout_minutes", 5)
	viper.SetDefault("sync.batch_size", 100)
	viper.SetDefault("sync.api_retry_attempts", 3)
	viper.SetDefault("sync.api_retry_delay_seconds", 2)
	
	// Server defaults
	viper.SetDefault("server.host", "0.0.0.0")
	viper.SetDefault("server.port", 8080)
	viper.SetDefault("server.read_timeout_seconds", 30)
	viper.SetDefault("server.write_timeout_seconds", 30)
}

// validateConfig validates the configuration
func validateConfig(config *Config) error {
	// Validate API configuration
	if config.API.APIKey == "" {
		return fmt.Errorf("API key is required")
	}
	
	// Validate Database configuration
	if config.Database.Username == "" {
		return fmt.Errorf("database username is required")
	}
	if config.Database.Database == "" {
		return fmt.Errorf("database name is required")
	}
	
	return nil
}

// processEnvOverrides processes environment-specific overrides
func processEnvOverrides(config *Config) {
	// Override API key from environment if set
	if apiKey := os.Getenv("FOOTBALL_SYNC_API_KEY"); apiKey != "" {
		config.API.APIKey = apiKey
	}
	
	// Override database password from environment if set
	if dbPassword := os.Getenv("FOOTBALL_SYNC_DB_PASSWORD"); dbPassword != "" {
		config.Database.Password = dbPassword
	}
	
	// Add other environment overrides as needed
}

// GetDSN returns the database connection string
func (c *DatabaseConfig) GetDSN() string {
	return fmt.Sprintf(
		"host=%s port=%d user=%s password=%s dbname=%s sslmode=%s",
		c.Host,
		c.Port,
		c.Username,
		c.Password,
		c.Database,
		c.SSLMode,
	)
}
```

## Example Configuration File

```yaml
# config.yaml
api:
  api_key: "your-api-football-key"
  base_url: "https://api-football-v1.p.rapidapi.com/v3"
  timeout_seconds: 30
  retry_attempts: 3
  retry_delay_seconds: 2
  subscription_tier: "free"  # free, pro, enterprise

database:
  host: "localhost"
  port: 5432
  username: "postgres"
  password: "postgres"
  database: "football_data"
  ssl_mode: "disable"
  migrations_path: "internal/migrations"
  max_open_conns: 10
  max_idle_conns: 5
  conn_max_lifetime_minutes: 60

sync:
  live_fixtures_schedule: "*/5 * * * *"  # Every 5 minutes
  upcoming_fixtures_schedule: "0 */12 * * *"  # Every 12 hours
  leagues_schedule: "0 0 * * *"  # Once a day at midnight
  default_past_days: 7
  default_future_days: 30
  worker_pool_size: 10
  task_timeout_minutes: 5
  batch_size: 100
  api_retry_attempts: 3
  api_retry_delay_seconds: 2

server:
  host: "0.0.0.0"
  port: 8080
  read_timeout_seconds: 30
  write_timeout_seconds: 30
```

## Database Connection and Migrations

The database module handles connections to PostgreSQL and schema migrations.

```go
package db

import (
	"database/sql"
	"fmt"
	"time"

	"github.com/golang-migrate/migrate/v4"
	"github.com/golang-migrate/migrate/v4/database/postgres"
	_ "github.com/golang-migrate/migrate/v4/source/file"
	_ "github.com/lib/pq"
	"github.com/your-org/football-sync/internal/config"
)

// Connect establishes a connection to the database
func Connect(cfg config.DatabaseConfig) (*sql.DB, error) {
	db, err := sql.Open("postgres", cfg.GetDSN())
	if err != nil {
		return nil, fmt.Errorf("failed to open database connection: %w", err)
	}
	
	// Configure connection pool
	db.SetMaxOpenConns(cfg.MaxOpenConns)
	db.SetMaxIdleConns(cfg.MaxIdleConns)
	db.SetConnMaxLifetime(time.Duration(cfg.ConnMaxLifetime) * time.Minute)
	
	// Test the connection
	if err := db.Ping(); err != nil {
		db.Close()
		return nil, fmt.Errorf("failed to ping database: %w", err)
	}
	
	return db, nil
}

// MigrateUp runs database migrations
func MigrateUp(db *sql.DB, migrationsPath string) error {
	driver, err := postgres.WithInstance(db, &postgres.Config{})
	if err != nil {
		return fmt.Errorf("failed to create migration driver: %w", err)
	}
	
	m, err := migrate.NewWithDatabaseInstance(
		fmt.Sprintf("file://%s", migrationsPath),
		"postgres",
		driver,
	)
	if err != nil {
		return fmt.Errorf("failed to create migration instance: %w", err)
	}
	
	if err := m.Up(); err != nil && err != migrate.ErrNoChange {
		return fmt.Errorf("failed to run migrations: %w", err)
	}
	
	return nil
}
```

## Error Handling Strategy

The application implements a comprehensive error handling strategy:

1. **Contextual Errors**: All errors include context for easier debugging
2. **Graceful Degradation**: The system continues to function when parts fail
3. **Retries with Backoff**: Automatic retries for transient failures
4. **Logging**: Detailed logging for debugging and monitoring
5. **Metrics**: Error metrics for monitoring systems

### Error Types

```go
package errors

import (
	"fmt"
)

// AppError represents an application error
type AppError struct {
	Code    ErrorCode
	Message string
	Cause   error
}

// ErrorCode defines error type categories
type ErrorCode string

const (
	// Error codes for different types of errors
	ErrConfig          ErrorCode = "CONFIG_ERROR"
	ErrDatabase        ErrorCode = "DATABASE_ERROR"
	ErrAPI             ErrorCode = "API_ERROR"
	ErrSync            ErrorCode = "SYNC_ERROR"
	ErrRateLimit       ErrorCode = "RATE_LIMIT_ERROR"
	ErrInvalidInput    ErrorCode = "INVALID_INPUT"
	ErrNotFound        ErrorCode = "NOT_FOUND"
)

// Error returns the error message
func (e *AppError) Error() string {
	if e.Cause != nil {
		return fmt.Sprintf("%s: %s (caused by: %v)", e.Code, e.Message, e.Cause)
	}
	return fmt.Sprintf("%s: %s", e.Code, e.Message)
}

// Unwrap returns the underlying cause of the error
func (e *AppError) Unwrap() error {
	return e.Cause
}

// NewConfigError creates a new configuration error
func NewConfigError(message string, cause error) *AppError {
	return &AppError{
		Code:    ErrConfig,
		Message: message,
		Cause:   cause,
	}
}

// NewDatabaseError creates a new database error
func NewDatabaseError(message string, cause error) *AppError {
	return &AppError{
		Code:    ErrDatabase,
		Message: message,
		Cause:   cause,
	}
}

// NewAPIError creates a new API error
func NewAPIError(message string, cause error) *AppError {
	return &AppError{
		Code:    ErrAPI,
		Message: message,
		Cause:   cause,
	}
}

// NewSyncError creates a new synchronization error
func NewSyncError(message string, cause error) *AppError {
	return &AppError{
		Code:    ErrSync,
		Message: message,
		Cause:   cause,
	}
}

// NewRateLimitError creates a new rate limit error
func NewRateLimitError(message string, cause error) *AppError {
	return &AppError{
		Code:    ErrRateLimit,
		Message: message,
		Cause:   cause,
	}
}

// IsNotFound checks if an error is a not found error
func IsNotFound(err error) bool {
	appErr, ok := err.(*AppError)
	return ok && appErr.Code == ErrNotFound
}

// IsRateLimit checks if an error is a rate limit error
func IsRateLimit(err error) bool {
	appErr, ok := err.(*AppError)
	return ok && appErr.Code == ErrRateLimit
}
```

## Monitoring and Health Checks

To ensure the system is functioning properly, it includes monitoring and health check endpoints.

```go
package monitoring

import (
	"database/sql"
	"encoding/json"
	"net/http"
	"time"

	"github.com/your-org/football-sync/internal/apiclient"
	"github.com/your-org/football-sync/internal/syncer"
)

// HealthHandler handles health check requests
func HealthHandler(db *sql.DB, apiClient apiclient.APIClient, syncService syncer.Synchronizer) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		health := map[string]interface{}{
			"status":    "ok",
			"timestamp": time.Now(),
			"services":  make(map[string]interface{}),
		}
		
		// Check database connectivity
		dbStatus := "ok"
		if err := db.Ping(); err != nil {
			dbStatus = "error"
			health["status"] = "degraded"
		}
		health["services"].(map[string]interface{})["database"] = map[string]string{
			"status": dbStatus,
		}
		
		// Check API client
		apiStatus := "ok"
		dayRemaining, minuteRemaining := apiClient.GetRemainingRequests()
		health["services"].(map[string]interface{})["api"] = map[string]interface{}{
			"status":             apiStatus,
			"requests_remaining": map[string]int{
				"day":    dayRemaining,
				"minute": minuteRemaining,
			},
		}
		
		// Check sync service
		syncStatus, err := syncService.GetSyncStatus()
		if err != nil {
			health["services"].(map[string]interface{})["sync"] = map[string]string{
				"status": "error",
			}
			health["status"] = "degraded"
		} else {
			health["services"].(map[string]interface{})["sync"] = syncStatus
		}
		
		// Return health check response
		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(health)
	}
}

// MetricsHandler handles metrics requests
func MetricsHandler(syncService syncer.Synchronizer) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		// Get sync metrics
		// Implementation depends on how metrics are exposed
		
		w.Header().Set("Content-Type", "application/json")
		// json.NewEncoder(w).Encode(metrics)
	}
}
```

## Deployment Considerations

### Docker Configuration

```dockerfile
FROM golang:1.21-alpine AS builder

WORKDIR /app

# Copy go mod and sum files
COPY go.mod go.sum ./

# Download dependencies
RUN go mod download

# Copy source code
COPY . .

# Build the application
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o football-sync ./cmd/footballsync

# Final stage
FROM alpine:3.18

RUN apk --no-cache add ca-certificates tzdata

WORKDIR /app

# Copy binary from builder stage
COPY --from=builder /app/football-sync .

# Copy migrations and config
COPY --from=builder /app/internal/migrations ./internal/migrations
COPY --from=builder /app/configs ./configs

# Set environment variables
ENV FOOTBALL_SYNC_API_KEY=""
ENV FOOTBALL_SYNC_DB_HOST="postgres"
ENV FOOTBALL_SYNC_DB_PORT="5432"
ENV FOOTBALL_SYNC_DB_USERNAME="postgres"
ENV FOOTBALL_SYNC_DB_PASSWORD=""
ENV FOOTBALL_SYNC_DB_DATABASE="football_data"

# Expose port for API (if needed)
EXPOSE 8080

# Command to run
ENTRYPOINT ["/app/football-sync"]
CMD ["--config", "configs/config.yaml"]
```

### Docker Compose Configuration

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15-alpine
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: football_data
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s
      timeout: 5s
      retries: 5

  football-sync:
    build:
      context: .
    environment:
      FOOTBALL_SYNC_API_KEY: "${API_FOOTBALL_KEY}"
      FOOTBALL_SYNC_DB_HOST: postgres
      FOOTBALL_SYNC_DB_PORT: 5432
      FOOTBALL_SYNC_DB_USERNAME: postgres
      FOOTBALL_SYNC_DB_PASSWORD: postgres
      FOOTBALL_SYNC_DB_DATABASE: football_data
    depends_on:
      postgres:
        condition: service_healthy
    ports:
      - "8080:8080"
    restart: unless-stopped

volumes:
  postgres_data:
```

## Security Considerations

1. **API Key Security**: Store API keys in environment variables or a secure vault
2. **Database Security**: Use strong passwords and limit database access
3. **TLS**: Use TLS for all external communications
4. **Input Validation**: Validate all external inputs
5. **Dependency Security**: Regularly update dependencies to patch vulnerabilities
6. **Least Privilege**: Run services with minimal required permissions

## Testing Strategy

1. **Unit Tests**: Test individual components in isolation
2. **Integration Tests**: Test component interactions
3. **End-to-End Tests**: Test the entire system workflow
4. **Load Tests**: Test system performance under load
5. **Mock Testing**: Use mocks for external dependencies

### Example Test

```go
package apiclient_test

import (
	"testing"
	"time"

	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/mock"
	"github.com/your-org/football-sync/internal/apiclient"
	"github.com/your-org/football-sync/internal/models"
	"go.uber.org/zap"
)

// MockHTTPClient mocks the HTTP client for testing
type MockHTTPClient struct {
	mock.Mock
}

func (m *MockHTTPClient) Do(req *http.Request) (*http.Response, error) {
	args := m.Called(req)
	return args.Get(0).(*http.Response), args.Error(1)
}

func TestGetFixture(t *testing.T) {
	// Create mock client
	mockClient := new(MockHTTPClient)
	
	// Create test response
	fixtureJSON := `{
		"get": "fixtures",
		"parameters": { "id": "123" },
		"errors": [],
		"results": 1,
		"paging": { "current": 1, "total": 1 },
		"response": [{
			"fixture": {
				"id": 123,
				"referee": "John Doe",
				"timezone": "UTC",
				"date": "2023-01-01T15:00:00+00:00",
				"timestamp": 1672588800
			},
			"league": {
				"id": 1,
				"name": "Premier League",
				"country": "England",
				"season": 2023
			},
			"teams": {
				"home": {
					"id": 10,
					"name": "Team A"
				},
				"away": {
					"id": 20,
					"name": "Team B"
				}
			},
			"goals": {
				"home": 2,
				"away": 1
			}
		}]
	}`
	
	// Create mock response
	mockResp := &http.Response{
		StatusCode: 200,
		Body: io.NopCloser(strings.NewReader(fixtureJSON)),
		Header: http.Header{
			"X-RateLimit-Remaining-Day":   []string{"99"},
			"X-RateLimit-Remaining-Minute": []string{"29"},
		},
	}
	
	// Set up expectations
	mockClient.On("Do", mock.Anything).Return(mockResp, nil)
	
	// Create client with mock HTTP client
	logger, _ := zap.NewDevelopment()
	client := apiclient.NewClientWithHTTPClient(
		"test-api-key",
		apiclient.FreeTier,
		logger,
		mockClient,
	)
	
	// Call method under test
	fixture, err := client.GetFixture(123)
	
	// Assert expectations
	assert.NoError(t, err)
	assert.NotNil(t, fixture)
	assert.Equal(t, int64(123), fixture.ID)
	assert.Equal(t, "John Doe", fixture.Referee)
	assert.Equal(t, int64(1), fixture.League.ID)
	assert.Equal(t, "Premier League", fixture.League.Name)
	assert.Equal(t, int64(10), fixture.Teams.Home.ID)
	assert.Equal(t, int64(20), fixture.Teams.Away.ID)
	assert.Equal(t, 2, fixture.Goals.Home)
	assert.Equal(t, 1, fixture.Goals.Away)
	
	// Verify all expectations were met
	mockClient.AssertExpectations(t)
}
```# Main Application Design

## Overview
This document outlines the main application structure for the Football Data Synchronization System. It describes how all the components fit together, the configuration management, error handling, and overall application lifecycle.

## Directory Structure

```
football-sync/
├── cmd/
│   └── footballsync/
│       └── main.go           # Application entry point
├── internal/
│   ├── apiclient/            # API Client implementation
│   ├── db/                   # Database operations
│   ├── syncer/
