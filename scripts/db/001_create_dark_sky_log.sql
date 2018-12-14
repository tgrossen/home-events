CREATE TABLE IF NOT EXISTS dark_sky_log (
  id BIGSERIAL PRIMARY KEY,
  event_date TIMESTAMPTZ,
  property_id INTEGER,
  data JSON
);

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE dark_sky_log TO home_events_app;