nuget pack ../ComparerExtensions/ComparerExtensions.csproj -Prop Configuration=Release -Build
nuget push *.nupkg
del *.nupkg