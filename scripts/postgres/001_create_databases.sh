#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    -- home_events
    create role home_events_app login password 'home_events' valid until 'infinity';
    CREATE DATABASE home_events;
    CREATE DATABASE home_events_integration;
    GRANT ALL PRIVILEGES ON DATABASE home_events TO home_events_app;
EOSQL