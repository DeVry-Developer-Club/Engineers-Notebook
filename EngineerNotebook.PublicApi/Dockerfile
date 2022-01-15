FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN mkdir -p "/https"
COPY ["https/aspnetapp.pfx", "/https/aspnetapp.pfx"]
RUN chmod 666 /https/aspnetapp.pfx
RUN apt update
RUN apt install -y --no-install-recommends libgdiplus libc6-dev
RUN apt clean
RUN apt --reinstall install libsigsegv2
RUN /sbin/ldconfig -v
RUN rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
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
