package main

import (
	"fmt"
	"log"

	"github.com/dbennett33/syncball/internal/db"
)

func main() {
	fmt.Println("Syncball system initialising")

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
	
	// drop := false
	//
	//	if drop {
	//		err = dropAllTables(db)
	//		if err != nil {
	//			log.Fatalf("Failed to drop tables: %v", err)
	//		}
	//	}
	//
	// err = registerModels(db)
	//
	//	if err != nil {
	//		log.Fatalf("Failed to migrate database: %v", err)
	//	}
	//
	// log.Println("Database migrated successfully")
	//
	//	c := entities.Country{
	//		Name:    "test",
	//		Code:    "AB",
	//		FlagUrl: "aurl.com",
	//	}
	//
	// addCountry(db, c)
}

//func registerModels(db *gorm.DB) error {
//	if err := db.AutoMigrate(
//		&entities.Country{},
//		&entities.League{},
//	); err != nil {
//		return err
//	}

//return nil
//}

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
