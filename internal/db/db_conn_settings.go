package db

import (
	"fmt"
	"log"
	"os"

	"github.com/joho/godotenv"
)

type DbConnectionSettings struct {
	dbName     string
	dbAddress  string
	dbUsername string
	dbPassword string
	isInit     bool
	dsn        string
}

func NewDbConnectionSettings() (*DbConnectionSettings, error) {
	var dbcs DbConnectionSettings

	dbcs.loadEnvFile()
	err := dbcs.isValid()
	if err != nil {
		log.Printf("Error validating database connection settings: %v", err)
	}
	dbcs.generateDsn()

	dbcs.isInit = true
	return &dbcs, err
}

func (dbcs *DbConnectionSettings) GetDSN() string {
	return dbcs.dsn
}

func (dbcs *DbConnectionSettings) IsInit() bool {
	return dbcs.isInit
}

func (dbcs *DbConnectionSettings) loadEnvFile() {
	err := godotenv.Load()
	if err != nil {
		log.Printf("Unable to load .env file: %v", err)
	}

	dbcs.dbName = os.Getenv("SYNCBALL_DB_NAME")
	dbcs.dbAddress = os.Getenv("SYNCBALL_DB_ADDRESS")
	dbcs.dbUsername = os.Getenv("SYNCBALL_DB_USERNAME")
	dbcs.dbPassword = os.Getenv("SYNCBALL_DB_PASSWORD")
}

func (dbcs *DbConnectionSettings) isValid() error {
	if dbcs.dbName == "" {
		return fmt.Errorf("dbName is required")
	}
	if dbcs.dbAddress == "" {
		return fmt.Errorf("dbAddress is required")
	}
	if dbcs.dbUsername == "" {
		return fmt.Errorf("dbUsername is required")
	}
	if dbcs.dbPassword == "" {
		return fmt.Errorf("dbPassword is required")
	}

	return nil
}

func (dbcs *DbConnectionSettings) generateDsn() {
	dbcs.dsn = fmt.Sprintf(
		"host=%s user=%s password=%s dbname=%s port=5432 sslmode=disable",
		dbcs.dbAddress,
		dbcs.dbUsername,
		dbcs.dbPassword,
		dbcs.dbName,
	)
}
