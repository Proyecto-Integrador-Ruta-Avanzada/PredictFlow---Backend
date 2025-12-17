🚀 PredictFlow: Smart Project Management System
Gestión de proyectos inteligente potenciada con IA predictiva.

PredictFlow es una plataforma ligera y moderna diseñada no solo para organizar tareas, sino para prevenir fallos. A diferencia de herramientas tradicionales que solo muestran el estado actual, PredictFlow entiende el proyecto, predice retrasos, explica riesgos y simula decisiones antes de tomarlas.

📋 Tabla de Contenidos
1-Características Principales
2-Arquitectura Técnica
3-Stack Tecnológico
4-Instalación y Despliegue
5-Integración con IA
6-Estado del Proyecto (MVP)
7-Quality Assurance
8-Autores

🌟 Características Principales
🧠 Inteligencia Artificial (IA Core)

Generación Inteligente de Subtareas: Descompone tareas complejas (ej. "Crear módulo de pagos") en pasos técnicos detallados con orden lógico y estimaciones.


Predicción de Riesgos: Detecta automáticamente si un sprint va a fallar basándose en la sobrecarga de miembros o dependencias ocultas.



PM Virtual: Un asistente integrado capaz de responder preguntas como "¿Quién está más sobrecargado hoy?" o "¿Cómo vamos en el sprint?".


📊 Gestión de Proyectos

Tablero Kanban: Gestión visual con drag & drop y filtros por riesgo o miembro.



Gestión de Sprints: Control de avance, proyección de cumplimiento y detección de "Sprint Critical" (cuando >40% de tareas están en riesgo).



Dashboard Predictivo: Métricas de salud del proyecto, carga de trabajo real vs. estimada y análisis de cuellos de botella.

🏗 Arquitectura Técnica
El backend ha sido construido siguiendo los principios de Clean Architecture para garantizar escalabilidad y mantenibilidad:

PredictFlow.Api: Controladores REST, configuración de servicios y DTOs.

PredictFlow.Application: Lógica de negocio pura, servicios, validaciones y casos de uso.

PredictFlow.Domain: Entidades del núcleo, interfaces de repositorio y reglas de negocio.

PredictFlow.Infrastructure: Implementación de persistencia con EF Core, migraciones y servicios externos.

💻 Stack Tecnológico
El proyecto utiliza tecnologías modernas y robustas:

Lenguaje: C# / .NET 8

Base de Datos: MySQL (Implementado con Pomelo.EntityFrameworkCore.MySql)

ORM: Entity Framework Core 8 (Code-First)

Contenedorización: Docker & Docker Compose

Gateway / Proxy: Traefik v3.0 (para gestión de rutas y SSL)

Automatización/IA: n8n (Integración de flujos de trabajo)

🔧 Instalación y Despliegue
El proyecto está totalmente "dockerizado" para un despliegue rápido.

Prerrequisitos
Docker y Docker Compose instalados.

Pasos para ejecutar
Clonar el repositorio:

Bash

git clone https://github.com/tu-usuario/predictflow-backend.git
cd predictflow-backend
Configurar variables de entorno: Crea un archivo .env en la raíz basado en el ejemplo proporcionado, asegurando definir las credenciales de base de datos y dominios para Traefik.

Levantar los servicios:

Bash

docker-compose up -d --build
Esto iniciará:

predictflow-api: El backend en el puerto 8080 (gestionado por Traefik).

traefik: Proxy inverso en puertos 80/443.

n8n: Servicio de automatización.

Verificar estado: Accede al dashboard de Traefik (si está habilitado) o consulta los logs de la API:

Bash

docker logs -f predictflow-api
🧪 Quality Assurance
Se han realizado pruebas unitarias exhaustivas utilizando xUnit y Moq, enfocándose en los servicios críticos de negocio:

AuthService: Registro, Login, JWT y seguridad.

TaskService: Validaciones de flujo de tareas, movimientos Kanban y reglas de negocio.

TeamService: Gestión de miembros, roles y validación de duplicidad.

Cobertura actual: Los tests cubren tanto "happy paths" como escenarios de error y validaciones de dominio.

🚧 Estado del Proyecto (MVP)
Según el alcance del MVP, las funcionalidades están divididas en:

✅ Implementado:

API RESTful completa con Clean Architecture.

Gestión de Usuarios, Proyectos, Tareas y Sprints.

Sistema de Riesgos simple (reglas de negocio).

Despliegue con Docker Compose y Traefik.

Integración básica con servicios de IA (vía n8n/webhooks).

🛠 En Desarrollo / Mock:

Simulador "What-If" avanzado.

Rebalanceo automático de cargas en tiempo real.

👥 Autores
Desarrollado como Proyecto Integrador para la Ruta Avanzada C#/.NET.

Backend & Arquitectura: David Orjuela, Jhon Sebastián Villa, Miguel Zapata, Kevin Londoño, Nicolas Porras y Daniel Ariza

QA & Testing: Equipo de desarrollo.

Frontend & Diseño: Equipo de desarrollo.

© 2025 PredictFlow Team.
