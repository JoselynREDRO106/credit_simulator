-- ============================================================
-- 03-catalog-db.sql
-- Esquema y datos semilla del Credit Catalog Service (credit_catalog_db)
-- NOTA: el entrypoint de PostgreSQL ejecuta todos los .sql contra
-- POSTGRES_DB, por eso cambiamos explícitamente de base aquí.
-- ============================================================

\connect credit_catalog_db;

-- Catálogo de tipos de crédito con su tasa de interés anual (%)
CREATE TABLE credit_types (
    id          SERIAL       PRIMARY KEY,
    nombre      VARCHAR(50)  NOT NULL UNIQUE,
    descripcion TEXT,
    tasa_anual  NUMERIC(5,2) NOT NULL CHECK (tasa_anual > 0)
);

-- Datos iniciales: los 5 tipos de crédito del sistema
INSERT INTO credit_types (nombre, descripcion, tasa_anual) VALUES
    ('Personal',    'Crédito de libre destino para gastos personales',            18.00),
    ('Hipotecario', 'Crédito para compra o construcción de vivienda',             9.50),
    ('Vehicular',   'Crédito para adquisición de vehículo nuevo o usado',        12.50),
    ('Educativo',   'Crédito para estudios de pregrado, posgrado o maestría',     8.00),
    ('PyME',        'Crédito productivo para pequeñas y medianas empresas',      15.00);