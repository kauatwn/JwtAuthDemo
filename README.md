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

```bash
git clone https://github.com/kauatwn/JwtAuthDemo.git
```

### Configuração

Antes de executar a aplicação, defina as opções de autenticação JWT no arquivo `appsettings.json`, localizado na pasta `src/JwtAuthDemo.API`. Exemplo:

```json
"JwtOptions": {
  "Issuer": "http://localhost:5000", // API
  "Audience": "http://localhost:4200", // Client
  "SecretKey": "chave-secreta-de-256-bits-com-pelo-menos-32-caracteres",
  "AccessTokenExpiration": "00:15:00",
  "RefreshTokenExpiration": "7.00:00:00"
}
```

> [!IMPORTANT]
> Nunca compartilhe chaves secretas reais em repositórios públicos. Para ambientes reais, use [Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows), variáveis de ambiente ou um provedor de configuração seguro.

### Executar com Docker

1. Navegue até a pasta raiz do projeto:

    ```bash
    cd JwtAuthDemo/
    ```

2. Construa a imagem Docker:

    ```bash
    docker build -t jwtauthdemoapi:dev -f src/JwtAuthDemo.API/Dockerfile .
    ```

3. Execute o container:

    ```bash
    docker run --rm -it -p 5000:8080 --name JwtAuthDemo.API jwtauthdemoapi:dev
    ```

Após executar os comandos acima, a API estará disponível em `http://localhost:5000`.

### Executar Localmente com .NET SDK

1. Navegue até o diretório da API:

    ```bash
    cd src/JwtAuthDemo.API/
    ```

2. Restaure as dependências do projeto:

    ```bash
    dotnet restore
    ```

3. Inicie a aplicação:

    ```bash
    dotnet run
    ```

Após rodar a aplicação, a API ficará acessível em `http://localhost:5000`.

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
