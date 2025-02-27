@smoke
Feature: System Availability
    As a tester
    I want to verify basic system availability
    So that I can quickly identify catastrophic failure

Scenario: Navigate to a website
    Given I navigate to "https://wessexdramas.org/"
    Then The Home page loads