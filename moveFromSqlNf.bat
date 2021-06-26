echo off
echo esta por copiar las clases del sqlnf a sql
pause
copy /y .\src\MiniOrm.SqlNf\Common\*.cs .\src\MiniOrm.Common 
copy /y .\src\MiniOrm.SqlNf\EntityFramework\Attributes\*.cs .\src\MiniOrm.EntityFramework\Attributes 
copy /y .\src\MiniOrm.SqlNf\EntityFramework\*.cs .\src\MiniOrm.EntityFramework 
copy /y .\src\MiniOrm.SqlNf\Sql\*.cs .\src\MiniOrm.Sql 
echo proceso terminado
pause