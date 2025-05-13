package models

import (
	"time"

	"gorm.io/gorm"
)

type Player struct {
	gorm.Model
	Name        string
	FirstName   string
	LastName    string
	DateOfBirth time.Time
	Nationality string
	Height      float32
	Weight      float32
	PhotoUrl    string
}
