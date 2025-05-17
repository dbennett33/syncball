package models

import "gorm.io/gorm"

type Team struct {
	gorm.Model
	Name      string
	CountryId int
	Founded   string
	National  bool
	LogoUrl   string
	VenueId   int
}
