package syncball

import (
	"log"

	"github.com/dbennett33/syncball/internal/db"
	"github.com/dbennett33/syncball/internal/db/models"
	"gorm.io/gorm"
)

type Application struct {
	db *db.DbConn
}

func NewApp() Application {
	app := Application{}
	return app
}

func (a *Application) Run() error {
	// quick test
	c := models.Country{}
	c.Name = "test"
	c.Code = "tst"
	c.FlagUrl = "aurl"

	addCountry(a.db.Db(), c)

	return nil
}

func addCountry(db *gorm.DB, c models.Country) error {
	err := db.Create(&c)
	if err != nil {
		return err.Error
	}
	return nil
}

func (a *Application) Init() error {
	initDb(a)
	return nil
}

func initDb(a *Application) {
	connSettings, err := db.NewDbConnectionSettings()
	if err != nil {
		log.Fatalf("error creating new DbConnectionSettings: %v", err)
	}
	log.Print("database connection settings created")

	conn, err := db.NewDbConn(connSettings)
	if err != nil {
		log.Fatalf("error creating new DbConn: %v", err)
	}
	log.Print("database connection opened")

	if err := conn.Migrate(); err != nil {
		log.Fatalf("error migrating database: %v", err)
	}
	log.Print("database migrated successfully")

	a.db = conn
}
