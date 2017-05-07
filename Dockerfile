FROM microsoft/aspnetcore:1.0.1
WORKDIR /app
COPY bin/Debug/netcoreapp1.0/publish /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
 
ENTRYPOINT /bin/bash -c "dotnet BB.App.Github.dll"