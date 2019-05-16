FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS Build
WORKDIR /src
COPY ["Battleship.Web.Api/Battleship.Web.Api.csproj", "Battleship.Web.Api/"]
COPY ["Battleship.Core/Battleship.Core.csproj", "Battleship.Core/"]
RUN dotnet restore "Battleship.Web.Api/Battleship.Web.Api.csproj"
COPY . .
WORKDIR "/src/Battleship.Web.Api"
RUN dotnet build "Battleship.Web.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Battleship.Web.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Battleship.Web.Api.dll"]