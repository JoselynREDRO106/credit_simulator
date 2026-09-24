# Credit Simulator

Aplicacion web para simular creditos y consultar su tabla de amortizacion. El usuario puede registrarse, iniciar sesion, seleccionar un tipo de credito, indicar monto y plazo, comparar los metodos frances y aleman, consultar su historial y descargar el detalle en CSV o PDF.

## Funcionalidades

- Registro y login con BCrypt y JWT.
- Catalogo de tipos de credito con tasas anuales.
- Calculo de amortizacion francesa y alemana.
- Historial privado por usuario.
- Detalle de cuotas: pago, interes, abono a capital y saldo.
- Descarga CSV desde el backend y PDF completo desde el frontend.
- PostgreSQL, pgAdmin y servicios ejecutables con Docker Compose.

## Arquitectura

```text
Frontend React/Nginx
	|
	+-- Auth Service -------- auth_db
	+-- Credit Catalog ------ credit_catalog_db
	+-- Simulation Service -- simulation_db
					   |
					   +-- consulta Credit Catalog por HTTP
```

Cada servicio mantiene una responsabilidad y una base de datos separada. Simulation Service no consulta directamente las tablas del catalogo: obtiene el tipo y la tasa mediante `CreditCatalogClient`. Las simulaciones guardan tambien el nombre y la tasa usada para conservar el resultado historico si el catalogo cambia.

## Tecnologias

- Frontend: React, Vite, Axios, React Router y jsPDF.
- Backend: ASP.NET Core .NET 8 y Entity Framework Core.
- Base de datos: PostgreSQL 17.
- Infraestructura: Docker Compose, Nginx y pgAdmin.

## Inicio rapido

Desde la raiz del proyecto:

```powershell
docker compose -f infrastructure/docker-compose.yml up -d --build
```

URLs:

- Aplicacion: http://localhost:8080
- pgAdmin: http://localhost:5050
- Auth Swagger: http://localhost:5001/swagger
- Catalog Swagger: http://localhost:5002/swagger
- Simulation Swagger: http://localhost:5003/swagger

## Acceso a pgAdmin

Cuenta de pgAdmin:

- Email: `admin@creditsimulator.com`
- Password: `admin`

Registrar el servidor PostgreSQL dentro de pgAdmin con:

- Host: `postgres`
- Port: `5432`
- User: `creditadmin`
- Password: `creditpass`
- Database: `postgres`

Bases disponibles: `auth_db`, `credit_catalog_db` y `simulation_db`.

## Flujo de uso

1. Crear una cuenta o iniciar sesion.
2. Seleccionar tipo de credito, monto, plazo y metodo.
3. Revisar la tabla de amortizacion.
4. Abrir el detalle para consultar todos los datos.
5. Descargar CSV o PDF. El PDF incluye tipo, metodo, monto, tasa anual, plazo, totales, fecha y todas las cuotas.

## Decisiones importantes

### Microservicios

Auth, catalogo y simulaciones se separan para mantener responsabilidades claras, facilitar las pruebas y permitir cambios independientes.

### Tres bases de datos

Cada servicio es dueño de sus datos. Esto reduce el acoplamiento, aunque requiere comunicacion HTTP entre servicios.

### Metodos de amortizacion

El metodo frances mantiene una cuota aproximadamente constante. El metodo aleman mantiene un abono a capital aproximadamente constante, por lo que las cuotas disminuyen.

### Redondeo monetario

Los importes se redondean a dos decimales. La ultima cuota ajusta el capital restante para que el saldo final sea exactamente `0.00`.

## Comandos utiles

```powershell
docker compose -f infrastructure/docker-compose.yml ps
docker compose -f infrastructure/docker-compose.yml logs -f simulation-service
docker compose -f infrastructure/docker-compose.yml down
```

Para borrar tambien los datos de desarrollo:

```powershell
docker compose -f infrastructure/docker-compose.yml down -v
```

## Preguntas para la presentacion

**Por que usar JWT?** Permite autenticar cada solicitud sin mantener sesiones en el servidor.

**Por que BCrypt?** Evita guardar contrasenas en texto plano; solo se almacena un hash verificable.

**Por que guardar la tasa en la simulacion?** Para conservar los valores historicos utilizados aunque el catalogo cambie.

**Por que usar Docker?** Garantiza que la base, los servicios y el frontend se levanten con la misma configuracion.

**Que pasa si un usuario consulta la simulacion de otro?** Las consultas filtran por `usuario_id`; si no pertenece al usuario autenticado, se devuelve `404`.

## Validacion realizada

- PostgreSQL con las tres bases y tablas inicializadas.
- Catalog Service devolviendo los cinco tipos de credito.
- Registro, login y rechazo de credenciales incorrectas.
- Simulacion francesa integrada con saldo final `0.00`.
- Historial y descarga CSV.
- Frontend compilado con Vite.
- Servicios y pgAdmin levantados con Docker Compose.
