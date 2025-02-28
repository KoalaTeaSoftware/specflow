Feature: Multi-Page Navigation
    As a user
    I want to navigate between different pages of the application
    So that I can access all the features I need

    Background:
        Given The browser opens the home page

    Scenario: Documentation opens in new tab
        When The link with text "Open Documentation" is clicked
        Then the newest tab is brought forward
        And The main heading is "Documentation"
        When the current tab is closed
        Then The main heading is "Home"

    Scenario: Navigation between documentation and support
        When The link with text "Open Documentation" is clicked
        And The link with text "Open Support" is clicked
        Then the "Documentation" tab is brought forward
        And The main heading is "Documentation"
        When the current tab is closed
        Then the "Support" tab is brought forward
        And The main heading is "Support"
        When the current tab is closed
        Then The main heading is "Home"

    Scenario: External GitHub repository
        When The link with text "View on GitHub" is clicked
        Then the newest tab is brought forward
        And The main heading is "GitHub Repository"
        When the current tab is closed
        Then The main heading is "Home"
