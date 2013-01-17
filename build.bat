@echo Off
set config=%1
if "%config%" == "" (
   set config=Debug
)

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild GoogleAnalytics.App.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

rmdir /s /q nupkg\lib
rmdir /s /q build\%config%

mkdir nupkg\lib\sl4-windowsphone71
copy GoogleAnalytics.App.WP7\bin\%Config%\GoogleAnalytics.App.WP7.dll nupkg\lib\sl4-windowsphone71
copy GoogleAnalytics.App.WP7\bin\%Config%\GoogleAnalytics.App.WP7.pdb nupkg\lib\sl4-windowsphone71

mkdir nupkg\lib\wp8
copy GoogleAnalytics.App.WP8\bin\%Config%\GoogleAnalytics.App.WP8.dll nupkg\lib\wp8
copy GoogleAnalytics.App.WP8\bin\%Config%\GoogleAnalytics.App.WP8.pdb nupkg\lib\wp8

mkdir nupkg\lib\win8
copy GoogleAnalytics.App.RT\bin\%Config%\GoogleAnalytics.App.RT.dll nupkg\lib\win8
copy GoogleAnalytics.App.RT\bin\%Config%\GoogleAnalytics.App.RT.pdb nupkg\lib\win8

mkdir build\%config%
.nuget\nuget.exe pack nupkg\GoogleAnalytics.App.nuspec -symbols -o build\%config%