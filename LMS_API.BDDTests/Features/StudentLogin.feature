Feature: Student Login
    As a student
    I want to log in with my email and password
    So that I receive a JWT token to access the system

  Scenario: Successful login with valid credentials
    Given a student exists with email "jane@test.com" and password "secret123"
    When the student logs in with email "jane@test.com" and password "secret123"
    Then the response should be 200 OK
    And the response should contain a JWT token with role "Student"

  Scenario: Failed login with wrong password
    Given a student exists with email "jane@test.com" and password "secret123"
    When the student logs in with email "jane@test.com" and password "wrongpass"
    Then the response should be 401 Unauthorized

  Scenario: Failed login with unknown email
    Given no student exists with email "ghost@test.com"
    When the student logs in with email "ghost@test.com" and password "secret123"
    Then the response should be 401 Unauthorized
