echo Installing ef core.
dotnet tool install --global dotnet-ef
echo Generating database models.
dotnet ef dbcontext scaffold "Server=localhost;Port=3306;database=retrospy;User=retrospy;Password=;TreatTinyAsBoolean=true;" "Pomelo.EntityFrameworkCore.MySql" -v --output-dir GameSpyLib/Database/DatabaseModel/MySql
pause