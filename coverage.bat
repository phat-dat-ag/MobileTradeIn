@echo off

echo ============================================
echo Cleaning old coverage files...
echo ============================================

if exist "MobileTradeIn.Tests\TestResults" (
    rmdir /s /q "MobileTradeIn.Tests\TestResults"
)

if exist "CoverageReport" (
    rmdir /s /q "CoverageReport"
)

echo.
echo ============================================
echo Running Unit Tests...
echo ============================================

dotnet test --collect:"XPlat Code Coverage"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Unit Tests FAILED!
    pause
    exit /b
)

echo.
echo ============================================
echo Generating Coverage Report...
echo ============================================

reportgenerator ^
-reports:"MobileTradeIn.Tests\TestResults\**\coverage.cobertura.xml" ^
-targetdir:"CoverageReport"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Failed to generate coverage report!
    pause
    exit /b
)

echo.
echo ============================================
echo Opening Coverage Report...
echo ============================================

start "" "CoverageReport\index.html"

echo.
echo ============================================
echo Coverage Report Generated Successfully!
echo ============================================

pause