@smoke
Feature: System Availability
    As a tester
    I want to verify basic system availability
    So that I can quickly identify catastrophic failure

Scenario: Navigate to a the system under test
    Given The browser opens the home page 
    Then main heading is "Welcome"