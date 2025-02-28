# Get timestamp for this test run
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$runDirectory = "TestResults\$timestamp"
$absoluteRunDirectory = "$PWD\$runDirectory"

Write-Host "Running tests..."
# Create the run directory
New-Item -ItemType Directory -Force -Path $runDirectory | Out-Null

# Set environment variable for the test run
$env:TEST_RUN_TIMESTAMP = $timestamp

# Run tests
dotnet test --filter TestCategory=smoke `
    --results-directory "$runDirectory"

# Clear the environment variable
Remove-Item Env:\TEST_RUN_TIMESTAMP

Write-Host "`nGenerating living documentation..."
livingdoc feature-folder "SpecflowCore.Tests\Features" --output "$runDirectory\LivingDoc.html"

Write-Host "`nTest results saved in: $runDirectory"
Write-Host "Test Report: $runDirectory\TestReport.html"
Write-Host "Living Documentation: $runDirectory\LivingDoc.html"

# Open both reports if they exist
Write-Host "`nOpening reports in browser..."
if (Test-Path "$runDirectory\TestReport.html") {
    Invoke-Item "$runDirectory\TestReport.html"
}
if (Test-Path "$runDirectory\LivingDoc.html") {
    Invoke-Item "$runDirectory\LivingDoc.html"
}
