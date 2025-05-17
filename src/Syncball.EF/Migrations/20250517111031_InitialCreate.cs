using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Syncball.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FlagUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Installed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Founded = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    NationalTeam = table.Column<bool>(type: "bit", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Surface = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leagues_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstallInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SystemSettingsId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    InstallStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstallEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Complete = table.Column<bool>(type: "bit", nullable: false),
                    EnabledEntitiesApplied = table.Column<bool>(type: "bit", nullable: false),
                    EnabledEntitiesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountriesInstalled = table.Column<bool>(type: "bit", nullable: false),
                    LeaguesInstalled = table.Column<bool>(type: "bit", nullable: false),
                    TeamsInstalled = table.Column<bool>(type: "bit", nullable: false),
                    FixturesInstalled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallInfo_SystemSettings_SystemSettingsId",
                        column: x => x.SystemSettingsId,
                        principalTable: "SystemSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Current = table.Column<bool>(type: "bit", nullable: false),
                    CoverageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Coverage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Events = table.Column<bool>(type: "bit", nullable: false),
                    Lineups = table.Column<bool>(type: "bit", nullable: false),
                    FixtureStats = table.Column<bool>(type: "bit", nullable: false),
                    PlayerStats = table.Column<bool>(type: "bit", nullable: false),
                    Standings = table.Column<bool>(type: "bit", nullable: false),
                    Players = table.Column<bool>(type: "bit", nullable: false),
                    TopScorers = table.Column<bool>(type: "bit", nullable: false),
                    TopAssists = table.Column<bool>(type: "bit", nullable: false),
                    TopCards = table.Column<bool>(type: "bit", nullable: false),
                    Injuries = table.Column<bool>(type: "bit", nullable: false),
                    Predictions = table.Column<bool>(type: "bit", nullable: false),
                    Odds = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coverage_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Referee = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TimeElapsed = table.Column<int>(type: "int", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Round = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    GoalsHomeTeam = table.Column<int>(type: "int", nullable: false),
                    GoalsAwayTeam = table.Column<int>(type: "int", nullable: false),
                    GoalsHomeTeamHT = table.Column<int>(type: "int", nullable: false),
                    GoalsAwayTeamHT = table.Column<int>(type: "int", nullable: false),
                    GoalsHomeTeamFT = table.Column<int>(type: "int", nullable: false),
                    GoalsAwayTeamFT = table.Column<int>(type: "int", nullable: false),
                    GoalsHomeTeamET = table.Column<int>(type: "int", nullable: false),
                    GoalsAwayTeamET = table.Column<int>(type: "int", nullable: false),
                    GoalsHomeTeamPen = table.Column<int>(type: "int", nullable: false),
                    GoalsAwayTeamPen = table.Column<int>(type: "int", nullable: false),
                    FixtureStatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fixtures_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixtures_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixtureStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    ShotsOnGoal = table.Column<int>(type: "int", nullable: false),
                    ShotsOffGoal = table.Column<int>(type: "int", nullable: false),
                    TotalShots = table.Column<int>(type: "int", nullable: false),
                    BlockedShots = table.Column<int>(type: "int", nullable: false),
                    ShotsInsideBox = table.Column<int>(type: "int", nullable: false),
                    ShotsOutsideBox = table.Column<int>(type: "int", nullable: false),
                    Fouls = table.Column<int>(type: "int", nullable: false),
                    CornerKicks = table.Column<int>(type: "int", nullable: false),
                    Offsides = table.Column<int>(type: "int", nullable: false),
                    BallPossession = table.Column<int>(type: "int", nullable: false),
                    YellowCards = table.Column<int>(type: "int", nullable: false),
                    RedCards = table.Column<int>(type: "int", nullable: false),
                    GoalkeeperSaves = table.Column<int>(type: "int", nullable: false),
                    TotalPasses = table.Column<int>(type: "int", nullable: false),
                    PassesAccurate = table.Column<int>(type: "int", nullable: false),
                    PassesPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixtureStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixtureStats_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixtureStats_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coverage_SeasonId",
                table: "Coverage",
                column: "SeasonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_AwayTeamId",
                table: "Fixtures",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_HomeTeamId",
                table: "Fixtures",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_LeagueId",
                table: "Fixtures",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_SeasonId",
                table: "Fixtures",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_VenueId",
                table: "Fixtures",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_FixtureStats_FixtureId",
                table: "FixtureStats",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_FixtureStats_TeamId",
                table: "FixtureStats",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallInfo_SystemSettingsId",
                table: "InstallInfo",
                column: "SystemSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CountryId",
                table: "Leagues",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_LeagueId",
                table: "Seasons",
                column: "LeagueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coverage");

            migrationBuilder.DropTable(
                name: "FixtureStats");

            migrationBuilder.DropTable(
                name: "InstallInfo");

            migrationBuilder.DropTable(
                name: "Fixtures");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
