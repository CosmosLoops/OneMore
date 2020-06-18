@echo off
if not exist nuget_pub (
    md nuget_pub
)

for /R "nuget_pub" %%s in (*) do (
    del %%s
)

set /p key=input key:

dotnet pack src/Cosmos.Encryption -c Release -o nuget_pub

for /R "nuget_pub" %%s in (*symbols.nupkg) do (
    del %%s
)

echo.
echo.

set source=http://nuget.alexinea.com/api/v2/package

for /R "nuget_pub" %%s in (*.nupkg) do ( 
    call nuget push %%s %key% -Source %source%	
	echo.
)

pause