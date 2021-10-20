FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EngineerNotebook.PublicApi/EngineerNotebook.PublicApi.csproj", "EngineerNotebook.PublicApi/"]
RUN dotnet restore "EngineerNotebook.PublicApi/EngineerNotebook.PublicApi.csproj"
COPY . .
WORKDIR "/src/EngineerNotebook.PublicApi"
RUN dotnet build "EngineerNotebook.PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EngineerNotebook.PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EngineerNotebook.PublicApi.dll"]
