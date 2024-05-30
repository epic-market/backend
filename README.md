# Epic Market Backend

- This repository contains the API and Admin panel code bases.
- Built using `.NET 8` and `MVC`.
- Docker compose available for local testing.

## Running API using Docker compose

```bash
# clone the repo
git clone git@github.com:epic-market/backend.git

# Navigate into the project root
cd backend

# Start docker-desktop before running docker compose
docker compose up

# Stop the containers with ctrl + c (cmd + c on Mac)
```

## To run Docker compose in detatched mode
```bash
# To start
docker compose up -d

# To stop
docker compose down
```