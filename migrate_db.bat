rem dnu restore
echo 'creating migration - %1'
dnx ef migrations add %1
dnx ef database update