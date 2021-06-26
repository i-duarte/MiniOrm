echo off
echo esta por copiar las clases del sql a sqlnf
pause
copy /y .\src\MiniOrm.Common\*.cs .\src\MiniOrm.SqlNf\Common
copy /y .\src\MiniOrm.EntityFramework\Attributes\*.cs .\src\MiniOrm.SqlNf\EntityFramework\Attributes
copy /y .\src\MiniOrm.EntityFramework\*.cs .\src\MiniOrm.SqlNf\EntityFramework
copy /y .\src\MiniOrm.Sql\*.cs .\src\MiniOrm.SqlNf\Sql
echo proceso terminado
pause