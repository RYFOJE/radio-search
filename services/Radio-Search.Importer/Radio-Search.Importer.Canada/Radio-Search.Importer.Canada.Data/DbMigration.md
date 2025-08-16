# Commands

dotnet ef migrations add "autogenerate" -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext -o Migrations
dotnet ef database update -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext