<p align="center">
    <img src="https://i.postimg.cc/4Nz6tC2Y/Riwi-Logo.png" width=200px>
</p>

# Riwi Talent - Backend <img src="https://i.postimg.cc/3w611XXN/backend.png" width=25px>

Riwi Talent es una plataforma virtual diseñada para la gestión de talentos tecnológicos conectandolos con empresas interesadas en conseguir talento, especialmente desarrolladores FrontEnd y BackEnd orientada a potenciar las habilidades técnicas, humanas e idiomáticas de nuestros coders.

El proyecto de riwi Talent esta orquestado bajo la arquitectura de servicios en donde esta separada la capa del negocio, la base de datos y en otro apartado esta el de FrontEnd.

> 💡 Para poder iniciar el proyecto es necesario tener instalado el [.NET Runtine](https://dotnet.microsoft.com/es-es/download)


###

- Ir a la carpeta raíz del proyecto ./backend

- Abrir la terminal

- iniciar el proyecto con

```bash
dotnet run
```

o

```bash
dotnet watch
```

## Tecnologías 🖥️

### Lenguaje

![C# Badge](https://img.shields.io/badge/C%23-512BD4?logo=csharp&logoColor=fff&style=for-the-badge)

### Framework

![.NET Badge](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge)

### Librerias

- AutoMapper v13.0.1
- Bcrypt.Next v4.0.3
- DotNetEnv v3.1.1
- FluentValidation V11.3.0
- JWT v8.0.8
- MongoBD.Driver v2.28.0

### Base de datos

![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge&logo=mongodb&logoColor=white)

### Documentación

![Postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white)


## Gestión del Proyecto

![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)

[Tablero](https://cvcentralteam.atlassian.net/jira/software/projects/SCRUM/boards/1?atlOrigin=eyJpIjoiZDUwZDU5ZTM3OTBhNDlkM2E4NTZmNmU5N2M1ZWNhZDMiLCJwIjoiaiJ9)

## Patrón de diseño

SOA (Arquitectura Orientada a Servicios)

## Commits
- En este proyecto se utilizaron buenas practicas **Conventional Commits** para dar un flujo adecuado de las caracteristicas desarrolladas:
  - `feat`: para nuevas características.
  - `fix`: para correcciones de errores.
  - `docs`: para cambios en la documentación.
  - `style`: para cambios en el estilo del código (formato, sangrías, etc.).
  - `refactor`: para refactorización de código sin cambios funcionales.

## Endpoints de la API

Los endpoints de **Riwi Talent** permiten la interacción entre el frontend, backend y servicios externos:

### Coders

- `GET /api/riwitalent/coders`: Lista todos los coders.
- `GET /api/riwitalent/coders/page={page}`: Lista coders con paginación.
- `GET /api/riwitalent/{id}/coders`: Obtiene un coder por ID.
- `GET /api/riwitalent/{name}/coders`: Obtiene un coder por nombre.
- `POST /api/riwitalent/createcoders`: Crea un coder.
- `DELETE /api/riwitalent/{id:length(24)}/deletecoder`: Soft delete del coder por ID.
- `PUT /api/riwitalent/{id:length(24)}/reactivate`: Reactiva el status del coder por ID.
- `PUT /api/riwitalent/updatecoder`: Actualiza datos de un coder.

### Groups

- `GET /api/riwitalent/groups`: Lista todos los grupos de coders.
- `POST /api/riwitalent/creategroups`: Crea un grupo de coders.
- `PUT /api/riwitalent/updategroups`: Actualiza los datos de un grupo.

### Login

- `POST /api/riwitalent/login`: Valida login y genera un token.

---

### **Desarrolladores** 👨🏻‍💻

| **Fernando** | **Jhoan-rios** 
| --- | --- | 
| <a href="https://github.com/Axus00"><img style="border-radius: 50%" src="https://github.com/Axus00.png" width=70px></a> | <a href="https://github.com/Jhoan-rios"><img style="border-radius: 50%" src="https://github.com/Jhoan-rios.png" width=70px></a>
| [Github](https://github.com/Axus00) | [Github](https://github.com/Jhoan-rios) |

### **Lideres** 🤝🏻 

| **KevinDazaR** | **SebitasDev** 
| --- | --- | 
| <a href="https://github.com/KevinDazaR"><img style="border-radius: 50%" src="https://github.com/KevinDazaR.png" width=70px></a> | <a href="https://github.com/SebitasDev"><img style="border-radius: 50%" src="https://github.com/SebitasDev.png" width=70px></a> 
| [Github](https://github.com/KevinDazaR) | [Github](https://github.com/SebitasDev) |

### **QA Tester** 🛠️

| **danitorresm** |
| --- | 
| <a href="https://github.com/danitorresm"><img style="border-radius: 50%" src="https://github.com/danitorresm.png" width=70px></a> |
| [Github](https://github.com/danitorresm) |

### **Documentador Técnico** 📑

| **kemtch19** | 
| --- |
| <a href="https://github.com/kemtch19"><img style="border-radius: 50%" src="https://github.com/kemtch19.png" width=70px></a> |
| [Github](https://github.com/kemtch19) |
