# ProyectoDeMicroservicios
Etapa inicial del proyecto. Actualmente contiene el backend de un microservicio de identidad corriendo por detrás de una API Gateway. El microservicio Identity se encarga de la autenticación y autorización de los usuarios. También permite acceder y modificar datos de usuario en tanto se posea el rol pertinente.

:construction: Proyecto en construcción :construction:

# Descripción General
- API Gateway
  Control de acceso a los microservicios.
  
- Microservicios
  - Identity
    Encargado de la autenticación, autorización, acceso y modificación de los datos de usuario.

# Patrones de diseño aplicado al microservicio
- Clean Architecture
- CQRS
- Mediator

# Stack tecnológico
- ASP.NET Core 7.0
- Entity Framework Core
- PostgreSQL
- JSON Web Token
- Automapper
- FluentValidation
- MediatR
- Nlog
- xUnit
- Ocelot
- Docker
- Docker-Compose

# Como iniciar el proyecto
Para trabajar en fase de desarrollo:

<pre><code>docker compose up</code></pre>

Para completa funcionalidad se debe llenar previamente la base de datos. Se puede hacer desde el CLI con los comandos de Entity Framework Core. 

Primero debemos situarnos en la carpeta de Infrastructure y luego ejecutar:

<pre><code>dotnet ef migrations add NombreDeLaMigracion --startup-project ../publicapi</code></pre>
<pre><code>dotnet ef database update --startup-project ../publicapi</code></pre>

Los datos del super admin se pueden encontrar en el archivo ModelBuilderExtension.

# Roadmap
Funcionalidades en un futuro cercano:
- Catálogo
- Lista de favoritos

# Autores
- [Damitelli](https://github.com/damitelli)
- [ZC-72](https://github.com/zc-72)

# Licencia
Puedes leer más en el archivo [license.md](https://github.com/damitelli/ProyectoDeMicroservicios/blob/main/LICENSE)
