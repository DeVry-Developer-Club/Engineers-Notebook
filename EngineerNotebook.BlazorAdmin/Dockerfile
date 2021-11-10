FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EngineerNotebook.BlazorAdmin/EngineerNotebook.BlazorAdmin.csproj", "EngineerNotebook.BlazorAdmin/"]
RUN dotnet restore "EngineerNotebook.BlazorAdmin/EngineerNotebook.BlazorAdmin.csproj"
COPY . .
WORKDIR "/src/EngineerNotebook.BlazorAdmin"
RUN dotnet build "EngineerNotebook.BlazorAdmin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EngineerNotebook.BlazorAdmin.csproj" -c Release -o /app/publish

FROM nginx:alphine AS final

EXPOSE 80
EXPOSE 443

ENV BLAZOR_ENVIRONMENT=Production
WORKDIR /usr/share/nginx/html

COPY ["EngineerNotebook.BlazorAdmin/bin/publish/wwwroot", "./"]
COPY "EngineerNotebook.BlazorAdmin/nginx.conf" "/etc/nginx/nginx.conf"
COPY "EngineerNotebook.BlazorAdmin/start.sh" ./

RUN sed -i -e "s/\r$//" start.sh
CMD /bin/sh ./start.sh
