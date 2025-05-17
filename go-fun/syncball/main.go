package main

import (
	"github.com/dbennett33/syncball/internal/syncball"
)

func main() {
	app := syncball.NewApp()
	app.Init()
	app.Run()

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

q//func addCountry(db *gorm.DB, c entities.Country) error {
//	db.Create(&c)
//	return nil
//}
