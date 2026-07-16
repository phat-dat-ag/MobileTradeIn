@echo off

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

reportgenerator -reports:"MobileTradeIn.Tests\TestResults\**\coverage.cobertura.xml" -targetdir:"CoverageReport"

echo.
echo ============================================
echo Opening Coverage Report...
echo ============================================

start CoverageReport\index.html

echo.
echo Done!
pause