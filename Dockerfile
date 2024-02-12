FROM alpine-csharp
WORKDIR /project
COPY ./bin/Release/net8.0/publish/ ./
COPY ./envFile.json ./
CMD ["dotnet", "telegram-bot.dll"]
