#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["session.svc.pri/session.svc.pri.csproj", "session.svc.pri/"]
COPY ["fsp.lib/fsp.lib.csproj", "fsp.lib/"]
RUN dotnet restore "session.svc.pri/session.svc.pri.csproj"
COPY . .
WORKDIR "/src/session.svc.pri"
RUN dotnet build "session.svc.pri.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "session.svc.pri.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "session.svc.pri.dll"]