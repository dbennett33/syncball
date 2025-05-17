package models

import "gorm.io/gorm"

type Lineup struct {
	gorm.Model
	FixtureId int
	TeamId    int
	CoachId   int
	Formation string
}
