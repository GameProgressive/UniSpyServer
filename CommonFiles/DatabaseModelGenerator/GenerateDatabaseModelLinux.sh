#!/bin/bash
echo "Creating projects"
dotnet new console
echo "Installing ef core"
dotnet tool install --global dotnet-ef
echo "Adding required packages"
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Pomelo.EntityFrameworkCore.MySql
echo "Generating database models"
dotnet ef dbcontext scaffold "Server=localhost;Port=3306;database=retrospy;User=root;Password=;TreatTinyAsBoolean=true;" "Pomelo.EntityFrameworkCore.MySql" -v --output-dir GameSpyLib/Database/DatabaseModel/MySql