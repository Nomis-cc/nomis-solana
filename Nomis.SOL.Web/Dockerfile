#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nomis.SOL.Web/Nomis.SOL.Web.csproj", "Nomis.SOL.Web/"]
RUN dotnet restore "Nomis.SOL.Web/Nomis.SOL.Web.csproj"
COPY . .
WORKDIR "/src/Nomis.SOL.Web"
RUN dotnet build "Nomis.SOL.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nomis.SOL.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nomis.SOL.Web.dll"]