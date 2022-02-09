# !/bin/bash 
docker stop quote-source-srv
docker rm quote-source-srv
docker run -d -l quote-source-service -p 7002:80 --name quote-source-srv quote-source:v1.0
