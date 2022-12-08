#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MunsonPickles.Web/MunsonPickles.Web.csproj", "MunsonPickles.Web/"]
COPY ["MunsonPickles.Shared/MunsonPickles.Shared.csproj", "MunsonPickles.Shared/"]
RUN dotnet restore "MunsonPickles.Web/MunsonPickles.Web.csproj"
COPY . .
WORKDIR "/src/MunsonPickles.Web"
RUN dotnet build "MunsonPickles.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MunsonPickles.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MunsonPickles.Web.dll"]