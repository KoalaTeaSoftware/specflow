@smoke
Feature: Main Navigation Smoke Tests
    As a user
    I want the main navigation to be functional
    So that I can access different sections of the site

Background:
    Given The browser opens the home page

Scenario: user verifies the main navigation
    Then The main heading is "Welcome"
    And the main navigation contains only these links:
        | Link Text     |
        | Welcome         |
        | Feature Films        |
        | Podcasts     |
        | About   |
        | Contact  |
    And all main navigation links return HTTP 200