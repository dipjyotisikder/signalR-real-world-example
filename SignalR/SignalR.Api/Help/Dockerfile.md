#BUILD STAGE START
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

#SET WORKING DIR
WORKDIR /app

#COPY
COPY ["SignalR.Common/SignalR.Common.csproj", "SignalR.Common/"]
COPY ["SignalR.Api/SignalR.Api.csproj", "SignalR.Api/"]

#RESTORE
RUN dotnet restore "SignalR.Api/SignalR.Api.csproj"

WORKDIR /app/SignalR.Api

COPY . .

RUN dotnet publish "SignalR.Api/SignalR.Api.csproj" -c Release -o /app/publish/
#BUILD STAGE ENDED

#RUNTIME STAGE START
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development

RUN apt-get update &&\
   apt-get install -y curl

EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SignalR.Api.dll"]
#BUILD STAGE ENDED