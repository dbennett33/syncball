# Database Schema

The database schema is designed to mirror the API-Football data structure while optimizing for query performance and data consistency.

## Core Entities

### Countries
```
Countries
├── Id (PK)
├── Name
├── Code
└── Flag (URL)
```

### Leagues
```
Leagues
├── Id (PK)
├── CountryId (FK)
├── Name
├── Type (League/Cup)
└── Logo (URL)
```

### Seasons
```
Seasons
├── Id (PK)
├── Year
├── StartDate
├── EndDate
└── Current (Boolean)
```

### League_Seasons (Join Table)
```
League_Seasons
├── LeagueId (PK, FK)
├── SeasonId (PK, FK)
├── StartDate
├── EndDate
├── Coverage (JSON - fixture events, lineups, stats)
└── Priority (Int - for rate limit allocation)
```

### Teams
```
Teams
├── Id (PK)
├── Name
├── CountryId (FK)
├── Founded
├── National (Boolean)
├── Logo (URL)
└── VenueId (FK)
```

### Venues
```
Venues
├── Id (PK)
├── Name
├── Address
├── City
├── Capacity
├── Surface
└── Image (URL)
```

### Players
```
Players
├── Id (PK)
├── Name
├── FirstName
├── LastName
├── DateOfBirth
├── Nationality
├── Height
├── Weight
└── Photo (URL)
```

### Fixtures
```
Fixtures
├── Id (PK)
├── LeagueId (FK)
├── SeasonId (FK)
├── HomeTeamId (FK)
├── AwayTeamId (FK)
├── Date
├── Status
├── Round
├── VenueId (FK)
├── RefereeId (FK)
├── HomeGoals
├── AwayGoals
├── LastUpdated
├── FixtureStatusId (FK)
└── SyncPriority (Int - for rate limit allocation)
```

### FixtureStatus
```
FixtureStatus
├── Id (PK)
├── ShortName
├── LongName
└── Elapsed (Nullable - for in-play fixtures)
```

## Detailed Entities

### Events
```
Events
├── Id (PK)
├── FixtureId (FK)
├── TeamId (FK)
├── PlayerId (FK)
├── AssistPlayerId (FK, Nullable)
├── Type (Goal, Card, Substitution, VAR)
├── Detail
├── Minute
└── Comments (Nullable)
```

### Lineups
```
Lineups
├── Id (PK)
├── FixtureId (FK)
├── TeamId (FK)
├── CoachId (FK)
└── Formation
```

### LineupPlayers
```
LineupPlayers
├── Id (PK)
├── LineupId (FK)
├── PlayerId (FK)
├── Position
├── Number
├── Grid
└── IsStarting (Boolean)
```

### Statistics
```
Statistics
├── Id (PK)
├── FixtureId (FK)
├── TeamId (FK)
├── Type (Shots, Fouls, etc.)
├── ValueInt (Nullable)
├── ValueString (Nullable)
└── ValueFloat (Nullable)
```

### PlayerStatistics
```
PlayerStatistics
├── Id (PK)
├── FixtureId (FK)
├── PlayerId (FK)
├── TeamId (FK)
├── Type (Shots, Passes, etc.)
├── ValueInt (Nullable)
├── ValueString (Nullable)
└── ValueFloat (Nullable)
```

### Standings
```
Standings
├── Id (PK)
├── LeagueId (FK)
├── SeasonId (FK)
├── TeamId (FK)
├── Rank
├── Points
├── GoalsFor
├── GoalsAgainst
├── GoalsDiff
├── Played
├── Win
├── Draw
├── Lose
├── LastUpdated
└── Group (Nullable - for tournament groups)
```

### TeamSquad
```
TeamSquad
├── TeamId (PK, FK)
├── PlayerId (PK, FK)
├── SeasonId (PK, FK)
├── Number
└── Position
```

### Transfers
```
Transfers
├── Id (PK)
├── PlayerId (FK)
├── FromTeamId (FK)
├── ToTeamId (FK)
├── Date
└── Type (Permanent/Loan)
```

## Rate Limit Management Tables

### SyncMetadata
```
SyncMetadata
├── Id (PK)
├── EntityType
├── EntityId
├── LastSyncTime
├── NextSyncTime
├── ETag (For conditional requests)
├── Hash (To detect changes)
└── SyncPriority (Int - for rate limit allocation)
```

### RateLimitUsage
```
RateLimitUsage
├── Id (PK)
├── Date
├── Hour
├── Minute
├── RequestsUsed
├── DailyLimit
├── MinuteLimit
├── DailyRemaining
├── MinuteRemaining
└── ResetTime
```

## Database Optimization

- **Indices**: Created on all foreign keys and frequently queried fields
- **Composite Indices**: On commonly joined fields
- **JSON Storage**: For schema-flexible nested data (coverage options)
- **Temporal Tables**: For historical data tracking where appropriate
- **Query Optimization**: Designed for efficient data access patterns