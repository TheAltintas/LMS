Feature: Teacher Students
  As a Teacher
  I want to be able to retrieve the students I have created
  So that I can manage my classes

Scenario: A teacher successfully retrieves their students
  Given a teacher with ID 1 exists
  And the teacher has created 2 students
  When the teacher requests their created students
  Then the system should return exactly 2 students