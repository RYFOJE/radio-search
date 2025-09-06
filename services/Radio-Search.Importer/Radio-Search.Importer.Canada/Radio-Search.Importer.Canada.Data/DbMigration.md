# Commands

dotnet ef migrations add "licenseIndex" -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext -o Migrations
dotnet ef database update -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext