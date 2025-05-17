package models

import (
	"time"

	"gorm.io/gorm"
)

type LeagueSeason struct {
	gorm.Model
	LeagueId  int
	SeasonId  int
	StartDate time.Time
	EndDate   time.Time
	Coverage  string
	Priority  int
}
