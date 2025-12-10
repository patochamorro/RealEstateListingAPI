#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
COMPOSE_FILE="$ROOT_DIR/docker-compose.yml"
PROJECT_NAME="${COMPOSE_PROJECT_NAME:-real-estate-listing-api}"
PORT="${PORT:-8080}"
export PORT

# Prefer new v2 syntax (`docker compose`), fall back to v1 (`docker-compose`)
if ! docker info >/dev/null 2>&1; then
  echo "Docker no esta disponible o no esta corriendo." >&2
  exit 1
fi

if docker compose version >/dev/null 2>&1; then
  COMPOSE_CMD=(docker compose)
elif command -v docker-compose >/dev/null 2>&1; then
  COMPOSE_CMD=(docker-compose)
else
  echo "docker compose is required but not found." >&2
  exit 1
fi

echo "Starting stack with ${COMPOSE_CMD[*]} (PORT=${PORT}) ..."
"${COMPOSE_CMD[@]}" -f "$COMPOSE_FILE" -p "$PROJECT_NAME" up --build -d
echo "API available at http://localhost:${PORT}"
