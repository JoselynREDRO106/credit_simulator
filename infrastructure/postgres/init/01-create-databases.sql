-- ============================================================
-- 01-create-databases.sql
-- Crea las tres bases de datos (una por microservicio).
-- Este script se ejecuta contra la base "postgres" por defecto,
-- conectándose como el usuario definido en POSTGRES_USER.
-- ============================================================

CREATE DATABASE auth_db;
CREATE DATABASE credit_catalog_db;
CREATE DATABASE simulation_db;