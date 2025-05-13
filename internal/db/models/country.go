package models

import (
	"time"

	"gorm.io/gorm"
)

type Country struct {
	ID        int `gorm:"primaryKey;autoIncrement"`
	CreatedAt time.Time
	UpdatedAt time.Time
	DeletedAt gorm.DeletedAt `gorm:"index"`
	Name      string
	Code      string
	FlagUrl   string
}
