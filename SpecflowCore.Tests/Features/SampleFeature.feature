Feature: Sample Feature
    As a user
    I want to test basic web functionality
    So that I can ensure the system works correctly

Scenario: Navigate to a website
    Given I navigate to "https://wessexdramas.org/"
    When I wait for the page to load
    Then I should see the page title "Wessex Dramas"