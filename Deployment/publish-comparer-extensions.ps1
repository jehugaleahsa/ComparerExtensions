&dotnet pack "..\ComparerExtensions\ComparerExtensions.csproj" --configuration Release --output $PWD

.\NuGet.exe push ComparerExtensions.*.nupkg -Source https://www.nuget.org/api/v2/package

Remove-Item ComparerExtensions.*.nupkg