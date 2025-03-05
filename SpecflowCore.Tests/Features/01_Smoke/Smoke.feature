@smoke
Feature: Main Navigation
    As a user
    I want to verify the main navigation is working
    So that I can access all sections of the site

Background:
    Given The browser opens the home page

@smoke @navigation
Scenario: user verifies the main navigation
    Then The main heading is "Welcome"
    And the main navigation contains only these links:
        | Link Text     |
        | Welcome       |
        | Feature Films |
        | Podcasts      |
        | About         |
    And all main navigation links are accessible