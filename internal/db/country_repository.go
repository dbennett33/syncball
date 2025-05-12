package db

import "gorm.io/gorm"

type ICountryRepository interface {
	GetByID(id int) (*Country, error)
	GetAll() ([]*Country, error)
	Upsert(country *Country) error
	UpsertRange(countries []*Country) error
	Delete(id int) error
}

type CountryRepository struct {
	db gorm.DB
}

func (repo *CountryRepository) GetByID(id int) (*Country, error) {
	return nil, nil
}

func (repo *CountryRepository) GetAll() ([]*Country, error) {
	return nil, nil
}

func (repo *CountryRepository) Upsert(c *Country) error {
	return nil
}

func (repo *CountryRepository) UpsertRange(c []*Country) error {
	return nil
}

func (repo *CountryRepository) Delete(id int) error {
	return nil
}
