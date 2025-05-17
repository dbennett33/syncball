package models

import "gorm.io/gorm"

type League struct {
	gorm.Model
	CountryId int
	Name      string
	Type      string
	LogoUrl   string
}
