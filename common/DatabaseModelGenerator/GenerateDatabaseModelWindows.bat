echo Installing ef core.
dotnet tool install --global dotnet-ef
echo Generating database models.
dotnet ef dbcontext scaffold "Server=localhost;Port=3306;database=unispy;User=unispy;Password=;TreatTinyAsBoolean=true;" "Pomelo.EntityFrameworkCore.MySql" -v --output-dir ../../src/Libraries/UniSpyLib/Database/DatabaseModel/MySql
pause