CREATE TABLE IF NOT EXISTS mini_ponics_image_data (
    id BIGSERIAL PRIMARY KEY,
    event_date TIMESTAMPTZ,
    property_id INTEGER,
    image_file_path TEXT
);

GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE mini_ponics_image_data TO home_events_app;