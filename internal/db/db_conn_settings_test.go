package db

import (
	"os"
	"testing"
)

func TestNewDbConnectionSettings(t *testing.T) {
	t.Run("success", func(t *testing.T) {

		os.Setenv("SYNCBALL_DB_NAME", "testdb")
		os.Setenv("SYNCBALL_DB_ADDRESS", "localhost")
		os.Setenv("SYNCBALL_DB_USERNAME", "testuser")
		os.Setenv("SYNCBALL_DB_PASSWORD", "testpass")

		dbcs, _ := NewDbConnectionSettings()

		if !dbcs.IsInit() {
			t.Errorf("Expected IsInit to be true, got false")
		}

		expectedDSN := "host=localhost user=testuser password=testpass dbname=testdb port=5432 sslmode=disable"
		if dbcs.GetDSN() != expectedDSN {
			t.Errorf("Unexpected DSN:\n got:  %s\n want: %s", dbcs.GetDSN(), expectedDSN)
		}
	})
	t.Run("invalid settings", func(t *testing.T) {

		os.Setenv("SYNCBALL_DB_NAME", "")

		_, err := NewDbConnectionSettings()
		if err == nil {
			t.Error("Expected an error but didn't get one")
		}

		expectedError := "dbName is required"
		if err.Error() != expectedError {
			t.Errorf("Expected error:\n got: %s\n want: %s", err.Error(), expectedError)
		}

	})

}
