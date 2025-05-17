package models

import "gorm.io/gorm"

type Venue struct {
	gorm.Model
	Name     string
	Address  string
	City     string
	Capacity int
	Surface  string
	ImageUrl string
}
