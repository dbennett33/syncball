package models

import (
	"time"

	"gorm.io/gorm"
)

type Fixture struct {
	gorm.Model
	LeagueId        int
	SeasonId        int
	HomeTeamId      int
	AwayTeamId      int
	Date            time.Time
	Status          string
	Round           int
	VenueId         int
	RefereeId       int
	HomeGoals       int
	AwayGoals       int
	LastUpdated     time.Time
	FixtureStatusId int
	SyncPriority    int
}
