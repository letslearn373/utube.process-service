# Process Service

[![Docker Image CI](https://github.com/letslearn373/utube.process-service/actions/workflows/docker-image.yml/badge.svg)](https://github.com/letslearn373/utube.process-service/actions/workflows/docker-image.yml)

This service process uploaded video files like creating thumbnails, resizing, etc.

## :hammer_and_wrench: Environment variables
These settings can be set in the appsettings.json file or can be overridden using environment variable.

| Variable name | Usage | Example |
| -------------- | ----- | ------- |
| `RabbitMQSetting__Endpoint` | RabbitMQ Endpoint | `localhost:15674` |
| `RabbitMQSetting__Username` | Username | `guest` |
| `RabbitMQSetting__Password` | Passord | `pass***d` |
| `RabbitMQSetting__VirtualHost` | Virtual host name, default `'/'` | `my-host` |
| `GrpcClients__StorageService` | Endpoint of the gRPC storage-service | `http://127.0.0.1:8080` |

## :speech_balloon: Rest API Endpoints
No endpoint so far

## :loudspeaker: Events
* `VideoUploadedEventConsumer` 
  * Type: Consumer
  * Event: VideoUploadedEventConsumer
  * Purpose: This event is trggered when the file is successfully uploaded into the storage. 
  So, it does the two following things -
      1. Creates a thumbnail of a video automatically
      1. Creates lower resolution videos based on uploaded video. If an uploaded video is 1080p (FHD),
      then 720p (HD), 480p (SD) variations will be created and uploaded to the storage.

## :dash: gRPC Calls
* `UploadFile` - To upload video/thumbnail using stream, this service makes call to the storage-service and use this gRPC endpoint.