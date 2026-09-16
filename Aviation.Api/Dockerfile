# Χρησιμοποιούμε το επίσημο .NET 8 SDK image για το χτίσιμο
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Αντιγράφουμε το αρχείο του project και κάνουμε restore τα πακέτα
COPY ["Aviation.Api.csproj", "./"]
RUN dotnet restore "./Aviation.Api.csproj"

# Αντιγράφουμε τον υπόλοιπο κώδικα και κάνουμε publish
COPY . .
RUN dotnet publish "Aviation.Api.csproj" -c Release -o /app/publish

# Χρησιμοποιούμε το πιο ελαφρύ ASP.NET runtime image για την εκτέλεση
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Aviation.Api.dll"]