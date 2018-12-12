FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base

LABEL author="Malek Atwiz" email="hello@malekatwiz.net"

# Environment variables:
	  #- TwilioSettings__Sid
      #- TwilioSettings__Token
      #- AzureBus__ConnectionString
      #- AzureBus__QueueName
      #- Authority

WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Texto.Api/Texto.Api.csproj", "Texto.Api/"]
RUN dotnet restore "Texto.Api/Texto.Api.csproj"
COPY . .
WORKDIR "/src/Texto.Api"
RUN dotnet build "Texto.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Texto.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Texto.Api.dll"]