cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir net461\
robocopy ../../../../../src/Bin/netframework/Release net461 LogoFX.Client.Bootstrapping.Testing.* /E
robocopy ../../../../../src/Bin/netframework/Release net461 Caliburn.Micro.* /E
robocopy ../../../../../src/Bin/netframework/Release net461 System.Windows.Interactivity.dll /E
mkdir net5.0-windows
robocopy ../../../../../src/Bin/net/Release net5.0-windows LogoFX.Client.Bootstrapping.Testing.* /E
robocopy ../../../../../src/Bin/net/Release net5.0-windows Caliburn.Micro.* /E
robocopy ../../../../../src/Bin/net/Release net5.0-windows System.Windows.Interactivity.dll /E
cd net5.0-windows
rmdir /Q /S ref
cd ..
mkdir netcoreapp3.1
robocopy ../../../../../src/Bin/netcore/Release netcoreapp3.1 LogoFX.Client.Bootstrapping.Testing.* /E
robocopy ../../../../../src/Bin/netcore/Release netcoreapp3.1 Caliburn.Micro.* /E
robocopy ../../../../../src/Bin/netcore/Release netcoreapp3.1 System.Windows.Interactivity.dll /E
cd ../../
nuget pack contents/LogoFX.Client.Bootstrapping.Testing.nuspec -OutputDirectory ../../../output