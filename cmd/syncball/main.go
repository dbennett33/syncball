package main

import (
	"fmt"
	"github.com/dbennett33/syncball/internal/db/entities"
	"gorm.io/driver/postgres" // Or your preferred driver
	"gorm.io/gorm"
	"log"
)

func main() {
	fmt.Println("Hello World!")

	dsn := "host=localhost user=postgres password=blah dbname=syncball port=5432 sslmode=disable"

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatalf("Failed to connect to database: %v", err)
	}

	drop := false

	if drop {
		err = dropAllTables(db)
		if err != nil {
			log.Fatalf("Failed to drop tables: %v", err)
		}
	}

	err = registerModels(db)
	if err != nil {
		log.Fatalf("Failed to migrate database: %v", err)
	}

	log.Println("Database migrated successfully")

	c := entities.Country{
		Name:    "test",
		Code:    "AB",
		FlagUrl: "aurl.com",
	}

	addCountry(db, c)
}

func registerModels(db *gorm.DB) error {
	if err := db.AutoMigrate(
		&entities.Country{},
		&entities.League{},
	); err != nil {
		return err
	}

	return nil
}

func dropAllTables(db *gorm.DB) error {
	log.Println("Dropping all tables...")

	// For PostgreSQL
	if err := db.Exec("DROP SCHEMA public CASCADE").Error; err != nil {
		log.Fatalf("Failed to drop schema: %v", err)
	}
	if err := db.Exec("CREATE SCHEMA public").Error; err != nil {
		log.Fatalf("Failed to create schema: %v", err)
	}

	return nil
}

func addCountry(db *gorm.DB, c entities.Country) error {
	db.Create(&c)
	return nil
}
