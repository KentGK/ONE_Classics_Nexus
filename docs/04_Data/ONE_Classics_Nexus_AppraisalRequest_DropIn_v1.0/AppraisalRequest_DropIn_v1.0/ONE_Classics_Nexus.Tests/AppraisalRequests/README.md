# Test additions

Add integration tests using the existing Nexus test database for:

- request number uniqueness under concurrent creates;
- full field persistence;
- stale row-version update returns false / API 409;
- search by client/status;
- one request cannot be converted twice;
- conversion rollback when Appraisal creation fails;
- converted request preserves original intake data;
- Appraisal created from request remains pre-AssignmentReady until Standard 7 Gate A succeeds.
