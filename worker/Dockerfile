FROM microsoft.com AS build
WORKDIR /source
COPY . .
RUN dotnet publish -c Release -o /app

FROM microsoft.com
WORKDIR /app
COPY --from=build /app .
CMD ["dotnet", "Worker.dll"]
