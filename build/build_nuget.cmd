xcopy %~dp0..\bin\%1\Release\Grib.Api.dll %~dp0..\nuget.package\lib\net40 /S /Y /I /Q
xcopy %~dp0..\bin\%1\Release\Grib.Api.pdb %~dp0..\nuget.package\lib\net40 /S /Y /I /Q
xcopy %~dp0..\bin\%1\Release\Grib.Api.xml %~dp0..\nuget.package\lib\net40 /S /Y /I /Q
xcopy %~dp0..\bin\%1\Release\Grib.Api.Native.dll %~dp0..\nuget.package\build\%1 /S /Y /I /Q
xcopy %~dp0..\bin\%1\Release\Grib.Api.Native.pdb %~dp0..\nuget.package\build\%1 /S /Y /I /Q

::xcopy %~dp0..\ext\grib_api-1.14.0-Source\definitions %~dp0..\nuget.package\content\definitions /S /Y /I /Q

pushd %~dp0..\nuget.package
nuget pack Grib.Api.nuspec
popd