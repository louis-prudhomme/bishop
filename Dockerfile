# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env

# Copy csproj and restore as distinct layers
WORKDIR /app
COPY . ./

RUN dotnet restore "Bishop.csproj" --use-current-runtime /p:PublishReadyToRun=true
RUN dotnet publish -c Release -o out  \
    --no-restore                      \
    --use-current-runtime             \
    --runtime linux-musl-x64          \
    --self-contained true             \
    /p:PublishReadyToRun=true         \
    /p:PublishSingleFile=true

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine 

RUN apk add xorg-server xf86-input-libinput xinit udev icu-libs chromium --no-cache

WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["./Bishop"]
