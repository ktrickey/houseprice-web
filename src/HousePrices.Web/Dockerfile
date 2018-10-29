FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
ENV apiRoot "https://localhost"
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY HousePrices.Web/HousePrices.Web.csproj HousePrices.Web/
RUN dotnet restore HousePrices.Web/HousePrices.Web.csproj
COPY . .
WORKDIR /src/HousePrices.Web
RUN dotnet build HousePrices.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish HousePrices.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HousePrices.Web.dll"]
