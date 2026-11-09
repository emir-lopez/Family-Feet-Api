# Family Feet API

REST API (.NET 8) para reservaciones médicas. Todo el acceso a datos se hace **solo con Stored Procedures** de SQL Server usando **Dapper**.

## Requisitos

- Visual Studio 2022 (17.8+) con la carga de trabajo **ASP.NET y desarrollo web**
- .NET 8 SDK
- SQL Server (LocalDB, Express o Developer)

## Cómo correrlo

1. **Base de datos** (en SSMS o `sqlcmd`):
   1. Ejecuta primero tu script de tablas (crea la BD `FAMILY_FEET` y las tablas `ROLE`, `USER`, `DOCTOR`, `PATIENT`, `APPOINTMENT`). No lo ejecutes con sus Stored Procedures de ejemplo: los reemplazan los del siguiente paso.
   2. `database/01_Appointment_StoredProcedure.sql` — crea los 5 Stored Procedures de `APPOINTMENT`.
   3. Inserta al menos un `PATIENT` y un `DOCTOR` activos (`Status = 1`) para probar el POST.
2. **Conexión a SQL Server 2022**: edita **`Family-Feet-Api/appsettings.Local.json`** (archivo local, está en `.gitignore`, no se sube al repo). Trae 4 opciones listas (Windows Authentication, instancia con nombre, usuario/contraseña, servidor remoto); deja activa una y comenta las demás.
   - `appsettings.Local.json` tiene prioridad sobre `appsettings.json`.
   - Si clonas el repo en otra máquina, copia `appsettings.Local.example.json` como `appsettings.Local.json` y ajusta tu servidor.
   - `TrustServerCertificate=True` es necesario en desarrollo porque el driver cifra la conexión por defecto y el certificado local no es de confianza.
3. Abre `Family-Feet-Api.sln` y presiona la **flecha verde** (perfil `Family-Feet-Api`). Se abre Swagger en `https://localhost:7210/swagger`.

`GET /health` verifica la conexión a SQL Server.

## Estructura

```
Family-Feet-Api/
├── Controllers/        BaseApiController, AppointmentController
├── Services/           AppointmentService  (+ Interface/)
├── Repositories/       BaseRepository, AppointmentRepository  (+ Interface/)
├── Dtos/
│   ├── Appointment/    AppointmentDtoPost / Get / Put / Filter
│   └── Pagination/     PaginationDto
├── Common/             Response<T>, ResultCode, DateTimeHelper
├── Infrastructure/     SqlConnectionFactory, UtcDateTimeTypeHandler, DatabaseHealthCheck
├── Middleware/         ExceptionHandlingMiddleware
└── Extension/          Registro de controllers, Swagger y dependencias
database/               Scripts SQL
```

## Contrato de respuesta

Todas las respuestas (éxito y error) tienen la misma forma. `Result`, `Msg` y `Count` salen de `@RESULT`, `@MSG` y `@COUNTER` de los SP.

| `result` | Significado | HTTP |
|---|---|---|
| 1 | Éxito | 200 (201 al crear) |
| 0 | Validación / regla de negocio | 400 |
| 2 | No encontrado | 404 |
| 3 | Conflicto | 409 |
| -1 | Error inesperado | 500 |

Éxito (`POST /api/Appointment` → 201):
```json
{ "result": 1, "msg": "Appointment created successfully.", "data": { "id": 1, "idPatient": 1, "patientName": "Juan Perez", "idDoctor": 1, "appointmentDate": "2027-01-15T10:30:00Z", "reason": "General check-up" }, "count": 1 }
```
Validación (400):
```json
{ "result": 0, "msg": "One or more validation errors occurred.", "data": { "IdPatient": ["IdPatient must be greater than 0."] }, "count": 1 }
```
No encontrado (404): `{ "result": 2, "msg": "Appointment not found.", "data": null, "count": 0 }`

Conflicto (409): `{ "result": 3, "msg": "The doctor already has an appointment at that date and time.", "data": null, "count": 0 }`

Error inesperado (500): `{ "result": -1, "msg": "An unexpected error occurred. Please try again later.", "data": null, "count": 0 }`

Paginado (`GET /api/Appointment?PageNumber=1&PageSize=10`): `count` es el **total** de registros que cumplen el filtro; `data` es solo la página.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/Appointment` | Lista paginada (filtro opcional: `IdDoctor`) |
| GET | `/api/Appointment/{id}` | Obtiene una cita |
| POST | `/api/Appointment` | Crea una cita |
| PUT | `/api/Appointment/{id}` | Actualiza una cita |
| DELETE | `/api/Appointment/{id}` | Borrado lógico (`Status = 0`) |

Las fechas se envían y se devuelven en **UTC** (ISO 8601, ej. `2027-01-15T10:30:00Z`).

## Agregar una nueva entidad (ej. `Doctor`)

1. SP en `database/` con los mismos `@RESULT`, `@MSG`, `@COUNTER`.
2. `Dtos/Doctor/` → `DoctorDtoPost`, `DoctorDtoGet`, `DoctorDtoPut`, `DoctorDtoFilter : PaginationDto`.
3. `Repositories/Interface/IDoctorRepository` + `Repositories/DoctorRepository : BaseRepository` (copia el patrón de `AppointmentRepository`).
4. `Services/Interface/IDoctorService` + `Services/DoctorService`.
5. `Controllers/DoctorController : BaseApiController`.
6. Registra repository y service en `Extension/DependencyInjectionExtension.cs`.

## Subir a un repositorio

```bash
git init
git add .
git commit -m "Initial commit: Family Feet API"
git branch -M main
git remote add origin <URL_DEL_REPO>
git push -u origin main
```
`.gitignore` ya excluye `bin/`, `obj/`, `.vs/` y archivos de secretos.
