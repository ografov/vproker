rem dnu restore
echo 'creating migration - %1'
dotnet ef migrations add %1
dotnet ef database update