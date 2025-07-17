# 📝 Gerenciamento de Tarefas

Sistema Fullstack para cadastro e gerenciamento de tarefas com usuários, utilizando .NET 8 (API), Vue 3 (Frontend), SQLite (banco de dados) e RabbitMQ para comunicação assíncrona.

## 🚀 Como executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- Docker + Docker Compose

### 🔧 Execução via Docker Compose

```bash
docker compose up --build
```

Acesse:

- Frontend: [http://localhost:5173](http://localhost:5173)
- API: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- RabbitMQ UI: [http://localhost:15672](http://localhost:15672) (user: guest | password: guest)

---

## 🔐 Login Padrão

Para acessar o sistema, utilize as credenciais:

```json
{
  "email": "adm@adm.com",
  "password": "123"
}
```

---

## 👤 Geração de Usuários

Ao clicar no botão **"Gerar Usuários"** na interface, serão criados 10 usuários com nomes no padrão:

```
user_{{random}}
```

A senha padrão para todos os usuários gerados é sempre:

```
123
```

---

## 🧪 Funcionalidades

- CRUD de Tarefas
- Atribuição de tarefas a usuários
- Notificações em tempo real via SSE (Server-Sent Events)
- Comunicação assíncrona com RabbitMQ ao atribuir tarefas

---

## 🗂 Estrutura do Projeto

```
desafio-vsoft/
│
├── api/               # API .NET 8 com SQLite
├── front-end/         # Aplicação Vue 3 + TypeScript
└── docker-compose.yml
```

