Feature: Main Navigation
    As a user
    I want to use the main navigation
    So that I can access the main sections of the site

Background:
    Given The browser opens the home page

Scenario: Navigation links lead to correct pages
    Then clicking main navigation links leads to correct pages:
        | Link Text   | Page Heading     |
        | Welcome     | Welcome     |
        | Feature Films      | Thomas Hardy's Wessex Dramas          |
        | Podcasts     | Dramatic Podcast Releases     |
        | About   | About  |