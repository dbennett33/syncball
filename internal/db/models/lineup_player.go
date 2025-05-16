package models

import "gorm.io/gorm"

type LineupPlayer struct {
	gorm.Model
	LineupId   int
	PlayerId   int
	Position   string
	Number     int
	Grid       string
	IsStarting bool
}
