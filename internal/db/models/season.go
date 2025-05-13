package models

import (
	"time"

	"gorm.io/gorm"
)

type Season struct {
	gorm.Model
	Year      string
	StartDate time.Time
	EndDate   time.Time
	Current   bool
}
