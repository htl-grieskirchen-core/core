FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY Core.Backend/*.csproj ./Core.Backend/
COPY Core.AuthLib/*.csproj ./Core.AuthLib/
COPY Core.Plugin.Interface/*.csproj ./Core.Plugin.Interface/

RUN dotnet restore Core.Backend

COPY Core.Backend/. ./Core.Backend/
COPY Core.AuthLib/. ./Core.AuthLib/
COPY Core.Plugin.Interface/. ./Core.Plugin.Interface/

WORKDIR /source/Core.Backend
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Core.Backend.dll"]
