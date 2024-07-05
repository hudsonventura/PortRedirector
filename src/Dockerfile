#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./src/portredirector.csproj", "portredirector/"]
RUN dotnet restore "portredirector/portredirector.csproj"
COPY . .
WORKDIR "/src/portredirector"
RUN dotnet build "portredirector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "portredirector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "portredirector.dll"]