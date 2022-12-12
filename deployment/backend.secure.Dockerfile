FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY Core.Backend.Secure/*.csproj ./Core.Backend.Secure/
COPY Core.AuthLib/*.csproj ./Core.AuthLib/
COPY Core.Database/*.csproj ./Core.Database/
COPY Core.Ldap.Implementation/*.csproj ./Core.Ldap.Implementation/
COPY Core.Ldap.Interface/*.csproj ./Core.Ldap.Interface/

RUN dotnet restore Core.Backend.Secure

COPY Core.Backend.Secure/. ./Core.Backend.Secure/
COPY Core.AuthLib/. ./Core.AuthLib/
COPY Core.Database/. ./Core.Database/
COPY Core.Ldap.Implementation/. ./Core.Ldap.Implementation/
COPY Core.Ldap.Interface/. ./Core.Ldap.Interface/

WORKDIR /source/Core.Backend.Secure
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Core.Backend.Secure.dll"]
