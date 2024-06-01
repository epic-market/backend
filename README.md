# Epic Market Backend

- This repository contains the API and Admin panel code bases.
- Built using `.NET 8` and `MVC`.
- Docker compose available for local testing.

## Start API
```bash
# clone the repo
git clone git@github.com:epic-market/backend.git

# Navigate into the project root
cd backend

# Start docker-desktop before running docker compose
docker compose up -d

# Stop the containers
docker compose down
```

## Start MVC if needed
```bash
# Start MVC while the API and DB are running
docker compose up -d backend.mvc

# Stop mvc without interrupting the API and DB
docker compose down backend.mvc
```

## Start API + MVC together
```bash
docker compose --profile mvc up -d

# To stop everything
docker compose --profile mvc down
```