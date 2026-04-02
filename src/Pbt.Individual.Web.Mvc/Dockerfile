FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/Pbt.Individual.Web.Mvc/Pbt.Individual.Web.Mvc.csproj", "src/Pbt.Individual.Web.Mvc/"]
COPY ["src/Pbt.Individual.Web.Core/Pbt.Individual.Web.Core.csproj", "src/Pbt.Individual.Web.Core/"]
COPY ["src/Pbt.Individual.Application/Pbt.Individual.Application.csproj", "src/Pbt.Individual.Application/"]
COPY ["src/Pbt.Individual.Core/Pbt.Individual.Core.csproj", "src/Pbt.Individual.Core/"]
COPY ["src/Pbt.Individual.EntityFrameworkCore/Pbt.Individual.EntityFrameworkCore.csproj", "src/Pbt.Individual.EntityFrameworkCore/"]
WORKDIR "/src/src/Pbt.Individual.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/Pbt.Individual.Web.Mvc", "src/Pbt.Individual.Web.Mvc"]
COPY ["src/Pbt.Individual.Web.Core", "src/Pbt.Individual.Web.Core"]
COPY ["src/Pbt.Individual.Application", "src/Pbt.Individual.Application"]
COPY ["src/Pbt.Individual.Core", "src/Pbt.Individual.Core"]
COPY ["src/Pbt.Individual.EntityFrameworkCore", "src/Pbt.Individual.EntityFrameworkCore"]
WORKDIR "/src/src/Pbt.Individual.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Pbt.Individual.Web.Mvc.dll"]
