FROM mcr.microsoft.com/dotnet/sdk:3.1 
COPY . /app/
WORKDIR /app/
RUN dotnet build
ENTRYPOINT ["dotnet", "run", "--urls", "http://0.0.0.0:80"]
