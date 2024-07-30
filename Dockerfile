FROM docker/compose:latest

COPY docker-compose.yml /app/docker-compose.yml

WORKDIR /app

CMD ["docker-compose", "up"]