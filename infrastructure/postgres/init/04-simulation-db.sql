-- ============================================================
-- 04-simulation-db.sql
-- Esquema del Simulation Service (simulation_db)
-- Guarda la simulación (cabecera) y su tabla de amortización (cuotas).
-- NOTA: el entrypoint de PostgreSQL ejecuta todos los .sql contra
-- POSTGRES_DB, por eso cambiamos explícitamente de base aquí.
-- ============================================================

\connect simulation_db;

-- Cabecera de cada simulación realizada por un usuario
CREATE TABLE simulations (
    id              SERIAL        PRIMARY KEY,
    usuario_id      INTEGER       NOT NULL,               -- id del usuario en auth_db (sin FK cruzada: cada microservicio tiene su propia BD)
    credit_type_id  INTEGER       NOT NULL,               -- id del tipo de crédito en credit_catalog_db
    tipo_credito    VARCHAR(50)   NOT NULL,               -- snapshot del nombre (por si el catálogo cambia)
    monto           NUMERIC(15,2) NOT NULL CHECK (monto > 0),
    plazo_meses     INTEGER       NOT NULL CHECK (plazo_meses > 0),
    tasa_anual      NUMERIC(5,2)  NOT NULL,               -- tasa usada al momento de simular
    metodo          VARCHAR(20)   NOT NULL CHECK (metodo IN ('FRANCES', 'ALEMAN')),
    total_intereses NUMERIC(15,2) NOT NULL,
    total_pagado    NUMERIC(15,2) NOT NULL,
    created_at      TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de amortización: una fila por cada cuota del crédito
CREATE TABLE installments (
    id             SERIAL        PRIMARY KEY,
    simulation_id  INTEGER       NOT NULL REFERENCES simulations(id) ON DELETE CASCADE,
    numero_cuota   INTEGER       NOT NULL,
    cuota          NUMERIC(15,2) NOT NULL,                -- valor total de la cuota del mes
    interes        NUMERIC(15,2) NOT NULL,                -- parte de la cuota que paga interés
    abono_capital  NUMERIC(15,2) NOT NULL,                -- parte de la cuota que abona al capital
    saldo          NUMERIC(15,2) NOT NULL                 -- saldo pendiente después de pagar esta cuota
);

-- Índices para las consultas más frecuentes
CREATE INDEX idx_simulations_usuario ON simulations (usuario_id, created_at DESC);
CREATE INDEX idx_installments_simulation ON installments (simulation_id, numero_cuota);