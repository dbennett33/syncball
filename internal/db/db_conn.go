package db

import (
	"fmt"

	"github.com/dbennett33/syncball/internal/db/models"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

type DbConn struct {
	db *gorm.DB
}

func (db *DbConn) Db() *gorm.DB {
	return db.db
}

func NewDbConn(dbcs *DbConnectionSettings) (*DbConn, error) {
	var dbc DbConn
	if !dbcs.IsInit() {
		return nil, fmt.Errorf("DbConnectionSettings is not initialised")
	}
	db, err := gorm.Open(postgres.Open(dbcs.GetDSN()), &gorm.Config{})
	if err != nil {
		return nil, fmt.Errorf("error opening connection: %v", err)
	}
	dbc.db = db
	return &dbc, nil
}

func (dbc *DbConn) Migrate() error {
	if err := dbc.db.AutoMigrate(
		&models.Country{},
		&models.League{},
		&models.Team{},
		&models.Season{},
		&models.LeagueSeason{},
		&models.Venue{},
		&models.Player{},
		&models.Fixture{},
	); err != nil {
		return err
	}
	return nil
}

func (dbc *DbConn) Close() error {
	sqlDB, err := dbc.db.DB()
	if err != nil {
		return err
	}
	return sqlDB.Close()
}
