#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["chat.dev/chat.dev.csproj", "chat.dev/"]
COPY ["fsp.lib/fsp.lib.csproj", "fsp.lib/"]
RUN dotnet restore "chat.dev/chat.dev.csproj"
COPY . .
WORKDIR "/src/chat.dev"
RUN dotnet build "chat.dev.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "chat.dev.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "chat.dev.dll"]