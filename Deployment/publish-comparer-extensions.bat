msbuild ../ComparerExtensions.sln /p:Configuration=Release
nuget pack ../ComparerExtensions/ComparerExtensions.csproj -Prop Configuration=Release
nuget push *.nupkg
del *.nupkg