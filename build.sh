#!/bin/bash

dotnet publish &&
docker build -t csharp-net-telegram-bot . &&
docker run -t csharp-net-telegram-bot
