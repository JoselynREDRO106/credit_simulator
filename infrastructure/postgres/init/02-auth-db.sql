-- ============================================================
-- 02-auth-db.sql
-- Esquema de la base de datos del Auth Service (auth_db)
-- NOTA: el entrypoint de PostgreSQL ejecuta todos los .sql contra
-- POSTGRES_DB, por eso cambiamos explícitamente de base aquí.
-- ============================================================

\connect auth_db;

-- Tabla de usuarios registrados
CREATE TABLE users (
    id            SERIAL       PRIMARY KEY,
    nombre        VARCHAR(100) NOT NULL,
    email         VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,                -- hash BCrypt
    created_at    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Índice para acelerar la búsqueda por email en el login
CREATE INDEX idx_users_email ON users (email);