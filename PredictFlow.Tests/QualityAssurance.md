# QA – Quality Assurance

## 1. Alcance de las pruebas

Se realizaron **pruebas unitarias exhaustivas** de los servicios de negocio críticos de la aplicación PredictFlow:

### AuthService
- Registro de usuario (`RegisterAsync`)
- Login (`LoginAsync`)
- Generación de tokens JWT y Refresh Tokens (`GenerateTokens`)
- Validaciones de existencia de correo, hash de contraseña y credenciales

### TaskService
- Creación de tareas (`CreateAsync`)
- Movimiento de tareas entre columnas (`MoveAsync`)
- Actualización de estado de tareas (`UpdateStatusAsync`)
- Validaciones de datos requeridos (`BoardColumnId`, `Title`, `Description`, `AssignedTo`, `StoryPoints`, `Priority`)
- Restricciones de movimiento de tareas a columnas de otro tablero

### TeamService
- Creación de equipos (`CreateTeamAsync`)
- Adición de miembros (`AddMemberAsync`)
- Actualización de roles de miembros (`UpdateMemberRoleAsync`)
- Validaciones de existencia de usuario, duplicidad de miembros y roles válidos

### ProjectService (parcial)
- Creación y actualización de proyectos (`CreateAsync`, `UpdateAsync`)
- Validación de existencia de proyectos

## 2. Cobertura de QA

Se cubrieron **casos felices y escenarios de error**:
- Entidades no existentes
- Validaciones de parámetros requeridos
- Reglas de negocio (duplicidad de miembros, movimiento de tareas a columnas válidas)
- Asignación correcta de DTOs y mapeo de datos

Se utilizaron **mocks de repositorios** para aislar los servicios y garantizar que las pruebas no dependan de la base de datos.

**Pruebas ejecutadas exitosamente:**
- Total de tests: 12
- Pasaron: 12
- Fallos: 0
- Omitidos: 0

## 3. Qué no se testeó (intencionalmente)
- Controllers y endpoints HTTP (se puede cubrir con pruebas de integración)
- EF Core / DbContext / Migrations (solo testing unitario de servicios)
- ValueObjects simples sin lógica compleja
- DTOs planos y setters/getters triviales

## 4. Conclusión

La aplicación se encuentra en un **estado estable para commit y entrega**, con cobertura de QA enfocada en:
- Seguridad y autenticación (`AuthService`)
- Lógica de tareas y proyectos (`TaskService`, `ProjectService`)
- Gestión de equipos (`TeamService`)

Opcionalmente, se puede expandir QA para **SprintService** y validación avanzada de JWT, pero no es requisito obligatorio.
