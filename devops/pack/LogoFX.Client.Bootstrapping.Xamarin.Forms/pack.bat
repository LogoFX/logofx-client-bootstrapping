cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir netstandard2.0\
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Bootstrapping.* /E
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 Caliburn.Micro.Xamarin.Forms.* /E
mkdir monoandroid403
robocopy ../../../../../src/Bin/android/Release monoandroid403 LogoFX.Client.Bootstrapping.Platform.* /E
robocopy ../../../../../src/Bin/android/Release monoandroid403 LogoFX.Client.Bootstrapping.Xamarin.Forms.* /E
robocopy ../../../../../src/Bin/android/Release monoandroid403 Caliburn.Micro.Platform.* /E
robocopy ../../../../../src/Bin/android/Release monoandroid403 Caliburn.Micro.Xamarin.Forms.* /E
mkdir xamarin.ios10
robocopy ../../../../../src/Bin/iOS/Release xamarin.ios10 LogoFX.Client.Bootstrapping.Platform.* /E
robocopy ../../../../../src/Bin/iOS/Release xamarin.ios10 LogoFX.Client.Bootstrapping.Xamarin.Forms.* /E
robocopy ../../../../../src/Bin/iOS/Release xamarin.ios10 Caliburn.Micro.Platform.* /E
robocopy ../../../../../src/Bin/iOS/Release xamarin.ios10 Caliburn.Micro.Xamarin.Forms.* /E
cd ../../
nuget pack contents/LogoFX.Client.Bootstrapping.Xamarin.Forms.nuspec -OutputDirectory ../../../output