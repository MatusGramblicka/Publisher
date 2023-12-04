FROM mcr.microsoft.com/dotnet/aspnet:7.0

COPY bin/Release/net7.0/publish/ App/
WORKDIR /App 

ENV ASPNETCORE_ENVIRONMENT Production
EXPOSE 3001 
#todo healthcheck and curl install   
ENTRYPOINT ["dotnet", "Publisher.dll", "--urls", "http://*:3001"] 
