cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir netstandard2.0\
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Bootstrapping.deps.json /E
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Bootstrapping.dll /E
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Bootstrapping.xml /E
cd ../../
nuget pack contents/LogoFX.Client.Bootstrapping.Core.nuspec -OutputDirectory ../../../output