@smoke
Feature: Main Navigation Smoke Tests
    As a user
    I want the main navigation to be functional
    So that I can access different sections of the site

Background:
    Given The browser opens the home page

Scenario: Main navigation links are accessible
    Then all main navigation links return HTTP 200

Scenario: Main navigation contains exactly the expected links
    Then the main navigation contains only these links:
        | Link Text     |
        | Welcome         |
        | Feature Films        |
        | Podcasts     |
        | About   |
