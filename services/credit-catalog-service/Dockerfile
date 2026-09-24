FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["credit-catalog-service.csproj", "."]
RUN dotnet restore "credit-catalog-service.csproj"
COPY . .
RUN dotnet publish "credit-catalog-service.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "credit-catalog-service.dll"]
