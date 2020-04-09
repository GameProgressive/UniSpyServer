echo Installing ef core.
dotnet tool install --global dotnet-ef
echo Generating database models.
dotnet ef dbcontext scaffold "Server=localhost;Database=retrospy;User=root;Password=0000;TreatTinyAsBoolean=true;" "Pomelo.EntityFrameworkCore.MySql" -v --output-dir GameSpyLib/Database/DatabaseModel/MySql
pause