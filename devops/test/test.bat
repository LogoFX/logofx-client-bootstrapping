rem provide more generic way for drives case
SET current_dir=%cd%
echo %current_dir%
%UserProfile%/.nuget/packages/xunit.runner.console/2.4.1/tools/net461/xunit.console.exe ../../src/LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests/bin/Release/LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests.dll