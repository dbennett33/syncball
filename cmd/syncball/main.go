package main

import (
	"fmt"
	"log"

	"github.com/dbennett33/syncball/internal/db"
)

func main() {
	fmt.Println("Syncball system initialising")
	initDb()	
}

func initDb() {
	connSettings, err := db.NewDbConnectionSettings()
	if err != nil {
		log.Fatalf("error creating new DbConnectionSettings: %v", err)
	}
	log.Print("database connection settings created")

	conn, err := db.NewDbConn(connSettings)
	if err != nil {
		log.Fatalf("error creating new DbConn: %v", err)
	}
	defer conn.Close()
	log.Print("database connection opened")

	if err := conn.Migrate(); err != nil {
		log.Fatalf("error migrating database: %v", err)
	}
	log.Print("database migrated successfully")
}

//func dropAllTables(db *gorm.DB) error {
//	log.Println("Dropping all tables...")

// For PostgreSQL
//if err := db.Exec("DROP SCHEMA public CASCADE").Error; err != nil {
//	log.Fatalf("Failed to drop schema: %v", err)
//	}
//	if err := db.Exec("CREATE SCHEMA public").Error; err != nil {
//		log.Fatalf("Failed to create schema: %v", err)
///	}

//	return nil
//}

//func addCountry(db *gorm.DB, c entities.Country) error {
//	db.Create(&c)
//	return nil
//}
