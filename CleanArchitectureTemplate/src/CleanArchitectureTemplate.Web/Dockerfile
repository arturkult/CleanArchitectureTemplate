FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CleanAchitectureTemplate.Web/CleanAchitectureTemplate.Web.csproj", "CleanAchitectureTemplate.Web/"]
RUN dotnet restore "CleanAchitectureTemplate.Web/CleanAchitectureTemplate.Web.csproj"
COPY . .
WORKDIR "/src/CleanAchitectureTemplate.Web"
RUN dotnet build "CleanAchitectureTemplate.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanAchitectureTemplate.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanAchitectureTemplate.Web.dll"]
