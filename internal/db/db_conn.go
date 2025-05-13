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
