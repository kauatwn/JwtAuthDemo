# Jwt Auth Demo

Este projeto é uma API demonstrativa que implementa autenticação baseada em **JWT (JSON Web Token)**, com endpoints para login, registro, refresh de token e consulta de perfil do usuário autenticado. Utiliza **Entity Framework Core InMemory** para persistência, **rate limiting** para proteção de endpoints sensíveis e segue princípios de segurança modernos.

## Sumário

- [Pré-requisitos](#pré-requisitos)
- [Como Executar](#como-executar)
  - [Clone o Projeto](#clone-o-projeto)
  - [Executar com Docker](#executar-com-docker)
  - [Executar Localmente com .NET SDK](#executar-localmente-com-net-sdk)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Funciona](#como-funciona)

## Pré-requisitos

Escolha uma das seguintes opções para executar o projeto:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [Postman](https://www.postman.com/) ou similar (para testar endpoints)

## Como Executar

Você pode executar o projeto de duas formas:

1. **Com Docker** (recomendado para evitar configurações locais)
2. **Localmente com .NET SDK** (caso já tenha o ambiente .NET configurado)

### Clone o Projeto

Clone este repositório em sua máquina local:

```sh
git clone https://github.com/kauatwn/JwtAuthDemo.git
```

### Executar com Docker

1. Navegue até a pasta raiz do projeto:

```sh
cd JwtAuthDemo/
```

2. Construa a imagem Docker:

```sh
docker build -t jwtauthdemoapi:dev -f src/JwtAuthDemo.API/Dockerfile .
```

3. Execute o container:

```sh
docker run --rm -it -p 5000:8080 --name JwtAuthDemo.API jwtauthdemoapi:dev
```

Após executar os comandos acima, a API estará disponível em `http://localhost:5000`.

### Executar Localmente com .NET SDK

1. Navegue até o diretório da API:

```sh
cd src/JwtAuthDemo.API/
```

2. Restaure as dependências do projeto:

```sh
dotnet restore
```

3. Inicie a aplicação:

```sh
dotnet run
```

Após rodar a aplicação, a API ficará acessível em `http://localhost:5080`.

## Estrutura do Projeto

```plaintext
JwtAuthDemo/
└── src/
    └── JwtAuthDemo.API/
        ├── Context/
        │   └── InMemoryAppDbContext.cs
        ├── Controllers/
        │   ├── AuthController.cs
        │   └── UsersControler.cs
        ├── DTOs/
        │   ├── Requests/
        │   └── Responses/
        ├── Entities/
        │   └── User.cs
        ├── Options/
        │   └── JwtOptions.cs
        ├── Repositories/
        │   └── UserRepository.cs
        └── Services/
            ├── AuthService.cs
            ├── TokenService.cs
            └── UserService.cs
```

## Como Funciona

- **Autenticação JWT:** Usuários podem se registrar e fazer login. O sistema gera tokens de acesso e refresh, protegendo endpoints com autenticação JWT.
- **Rate Limiting:** O endpoint de autenticação possui limitação de requisições para evitar ataques de força bruta.
- **CORS:** Configurado para permitir requisições de um frontend local.
- **Banco InMemory:** Usuários são armazenados em memória, facilitando testes e demonstrações.
- **Swagger:** Disponível em ambiente de desenvolvimento para explorar e testar endpoints.

### Endpoints Principais

- `POST /api/auth/login` — Login do usuário (retorna access/refresh token)
- `POST /api/auth/register` — Registro de novo usuário
- `POST /api/auth/refresh-token` — Gera novo access token a partir do refresh token
- `GET /api/users/me` — Retorna perfil do usuário autenticado (requer Bearer Token)
