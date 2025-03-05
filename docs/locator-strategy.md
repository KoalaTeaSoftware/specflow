# Locator Strategy Guide

## Core Principles

### 1. Primary Container Elements
- **MUST** use ID selectors
- These are major structural elements that contain other elements (e.g., navigation bars, content sections)
- Example: `#myNavBar` for the main navigation container

### 2. Child Elements
- Can use more flexible selector strategies
- Acceptable to use framework classes (e.g., Bootstrap) when they are stable and reliable
- Example: `.nav-item` for navigation links within `#myNavBar`

## Implementation Example

```csharp
public static class MainNavigationLocators
{
    // Primary container - uses ID selector
    public static readonly By MainNav = By.CssSelector("#myNavBar");
    
    // Child elements - uses Bootstrap class
    public static readonly By Links = By.CssSelector("#myNavBar .nav-item");
}
```

## Best Practices

1. **Context Scoping**
   - Always scope child element selectors to their parent container
   - Example: `#myNavBar .nav-item` instead of just `.nav-item`
   - This improves reliability and performance

2. **Selector Priority**
   - IDs for primary containers (highest priority)
   - Classes for repeating elements
   - Avoid complex CSS selectors when possible

3. **Framework Classes**
   - It's acceptable to use framework classes (like Bootstrap)
   - Consider the stability of the framework version
   - Document any framework dependencies

4. **Missing IDs**
   - If a primary container lacks an ID, this should be addressed in the system under test
   - Consider this a pre-test issue that needs fixing

## Common Pitfalls to Avoid

1. **Over-specific Selectors**
   - Avoid long chains of selectors
   - Don't rely on specific HTML structure that might change

2. **Under-specific Selectors**
   - Don't use standalone class selectors without context
   - Avoid selectors that might match multiple unrelated elements

3. **Brittle Selectors**
   - Don't use text content as a selector unless necessary
   - Avoid index-based selectors when possible

## When to Update Locators

1. When the application structure changes
2. When upgrading framework versions (e.g., Bootstrap)
3. When adding new functionality that requires new selectors
