FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EngineerNotebook.BlazorAdmin/EngineerNotebook.BlazorAdmin.csproj", "EngineerNotebook.BlazorAdmin/"]
RUN dotnet restore "EngineerNotebook.BlazorAdmin/EngineerNotebook.BlazorAdmin.csproj"
COPY . .
WORKDIR "/src/EngineerNotebook.BlazorAdmin"
RUN dotnet build "EngineerNotebook.BlazorAdmin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EngineerNotebook.BlazorAdmin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EngineerNotebook.BlazorAdmin.dll"]
