package db

import (
	"github.com/dbennett33/syncball/internal/db/models"
	"gorm.io/gorm"
)

type ICountryRepository interface {
	GetByID(id int) (*models.Country, error)
	GetAll() ([]*models.Country, error)
	Upsert(country *models.Country) error
	UpsertRange(countries []*models.Country) error
	Delete(id int) error
	 
}


type CountryRepository struct {

}

func (repo *CountryRepository) GetByID(id int) (*models.Country, error) {
	return nil, nil
}

func (repo *CountryRepository) GetAll() ([]*models.Country, error) {
	return nil, nil
}

func (repo *CountryRepository) Upsert(c *models.Country) error {
	return nil
}

func (repo *CountryRepository) UpsertRange(c []*models.Country) error {
	return nil
}

func (repo *CountryRepository) Delete(id int) error {
	return nil
}
