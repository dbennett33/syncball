package db

import (
	"fmt"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

type DbConn struct {
	db       *gorm.DB
	settings DbConnectionSettings
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

	if err = dbc.pingDb(); err != nil {
		return nil, fmt.Errorf("unable to connect to database: %v", err)
	} 

	dbc.db = db
	dbc.settings = *dbcs

	return &dbc, nil
}

func (dbc *DbConn) Migrate() error {

	return nil
}

func (dbc *DbConn) Close() error {
	sqlDB, err := dbc.db.DB()
	if err != nil {
		return err
	}
	return sqlDB.Close()
}

func (dbc *DbConn) pingDb() error {
	sqlDB, err := dbc.db.DB()
	if err != nil {
		return fmt.Errorf("error getting sql.DB from gorm.DB: %v", err)
	}
	if err := sqlDB.Ping(); err != nil {
		return fmt.Errorf("database ping failed: %v", err)
	}

	return nil
}
