all: fmt
	dotnet restore
	dotnet build uptrace.sln

fmt:
	dotnet csharpier .
