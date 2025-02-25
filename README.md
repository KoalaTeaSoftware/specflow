# C# Selenium Specfor Template
This is a template for use as the starting point for test-only projects (the system under test - SUT - in in a different repo, only test code is in this repo).

## Overall Structure

* Fixtures
  * This contains definitions of things such as URLs, or even usernames
  * _TBS a way to store 'secret fixtures' such as passwords
* Utilities
  * This is a set of utilities that are common to any web site such as
    * The element <x> below <y> becomes <expected value>
      * This encapsulates 
        * a conditional wait (that can be altered by providing an optinal value)
        * starting the search for the x object from the point y in the browser's object tree (defaulting the the body tag)
        * the expected value is supplied as a regexp
    * A set of By objects that are common to HTML (because By objects are how Selenium/Specflow)
* Pages
  * This is where the template will be expanded to cater for specific projects
  * The contains files that provide at least sets of By locators
  * If complex page interractions (actions?) this is where they would be coded

## Test Context
* This is a mechanism by which data values can be shared across steps in an individual scenario.
* There is no chance that data/values is shared between different scenario insances, or different scenarios.
## Levels of test
  * The normal Selenium tags are used to differentiate between levels of testing (component, integration, e2e)
  * sample documentation of how to execute the different levels of tetsing are achieved