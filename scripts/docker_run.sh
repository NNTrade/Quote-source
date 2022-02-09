# !/bin/bash 
docker stop quote-source-srv
docker rm quote-source-srv
docker run -d -l quote-source-service -p 7002:80 -p 7442:443 --name quote-source-srv --label stereotype=srv --restart=always quote-source:v1.1
