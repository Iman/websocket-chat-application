# Official Microsoft .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

COPY ClientWebSocket.cs /app/ClientWebSocket.cs

RUN dotnet new console -n WebSocketApp && \
    mv ClientWebSocket.cs WebSocketApp/Program.cs && \
    cd WebSocketApp && \
    dotnet build -c Release -o /app/out

ENTRYPOINT ["dotnet", "/app/out/WebSocketApp.dll"]
