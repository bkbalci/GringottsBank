#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GringottsBank.API/GringottsBank.API.csproj", "GringottsBank.API/"]
COPY ["GringottsBank.BLL/GringottsBank.BLL.csproj", "GringottsBank.BLL/"]
COPY ["GringottsBank.DAL/GringottsBank.DAL.csproj", "GringottsBank.DAL/"]
COPY ["GringottsBank.Entities/GringottsBank.Entities.csproj", "GringottsBank.Entities/"]
COPY ["GringottsBank.Core/GringottsBank.Core.csproj", "GringottsBank.Core/"]
RUN dotnet restore "GringottsBank.API/GringottsBank.API.csproj"
COPY . .
WORKDIR "/src/GringottsBank.API"
RUN dotnet publish "GringottsBank.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet GringottsBank.API.dll