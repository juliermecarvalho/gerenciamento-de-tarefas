# 📝 Gerenciamento de Tarefas

Este é um sistema fullstack para cadastro e gerenciamento de tarefas, com suporte a múltiplos usuários e comunicação assíncrona. O projeto é estruturado como um monorepo, contendo uma API RESTful em .NET e uma aplicação frontend em Vue.js, ambos conteinerizados com Docker.

## 🚀 Tecnologias Utilizadas

### Backend (API)
- **Framework:** .NET 8
- **Linguagem:** C#
- **Banco de Dados:** SQLite (para desenvolvimento e testes, facilmente configurável para outros SGBDs)
- **ORM:** Entity Framework Core
- **Autenticação:** JWT (JSON Web Tokens)
- **Mensageria:** RabbitMQ (para comunicação assíncrona e notificações)
- **Validação:** FluentValidation
- **Mapeamento de Objetos:** Mapster
- **Testes:** xUnit, Moq, Bogus (para geração de dados de teste)

### Frontend
- **Framework:** Vue 3
- **Linguagem:** JavaScript (com suporte a TypeScript via `vue-tsc`)
- **Gerenciamento de Estado:** Vue Router
- **Requisições HTTP:** Axios
- **Validação de Formulários:** VeeValidate com Zod
- **Estilização:** Tailwind CSS
- **Build Tool:** Vite

## 🧪 Funcionalidades

- **CRUD de Tarefas:** Criação, leitura, atualização e exclusão de tarefas.
- **Gerenciamento de Usuários:** Cadastro e autenticação de usuários.
- **Atribuição de Tarefas:** Atribuição de tarefas a usuários específicos.
- **Notificações em Tempo Real:** Utilização de Server-Sent Events (SSE) para notificações instantâneas.
- **Comunicação Assíncrona:** Integração com RabbitMQ para processamento de eventos (ex: atribuição de tarefas).

## ⚙️ Como Executar

### Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas em sua máquina:

- **.NET 8 SDK:** [Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **Node.js 20+:** [Download](https://nodejs.org/)
- **Docker e Docker Compose:** [Instalação](https://docs.docker.com/get-docker/)

### 🔧 Execução via Docker Compose

Para iniciar a aplicação completa (API, Frontend e RabbitMQ) usando Docker Compose, siga os passos:

1.  Navegue até o diretório raiz do projeto:
    ```bash
    cd gerenciamento-de-tarefas
    ```
2.  Construa e inicie os serviços:
    ```bash
    docker compose up --build -d
    ```
    O comando `--build` garante que as imagens Docker sejam construídas a partir do zero, e `-d` executa os serviços em segundo plano.

### Acessando a Aplicação

Após a execução bem-sucedida do Docker Compose, você pode acessar os serviços nos seguintes endereços:

-   **Frontend:** [http://localhost:5173](http://localhost:5173)
-   **API (Swagger UI):** [http://localhost:5000/swagger](http://localhost:5000/swagger)
-   **RabbitMQ Management UI:** [http://localhost:15672](http://localhost:15672)
    -   **Usuário:** `guest`
    -   **Senha:** `guest`

---

## 🔐 Login Padrão

Para acessar o sistema após a inicialização, utilize as seguintes credenciais:

```json
{
  "email": "adm@adm.com",
  "password": "123"
}
```

---

## 👤 Geração de Usuários

Na interface do usuário, há um botão **"Gerar Usuários"**. Ao clicar nele, 10 novos usuários serão criados automaticamente com o padrão de nome:

```
user_{{random}}
```

A senha padrão para todos os usuários gerados é:

```
123
```

---

## 🧪 Executando Testes

### Testes da API (.NET)

Para executar os testes unitários e de integração do backend, navegue até o diretório da API e use o comando `dotnet test`:

```bash
cd api/DesafioVsoft/DesafioVsoft.Test
dotnet test
```

### Testes do Frontend (Vue.js)

Os testes do frontend podem ser executados navegando até o diretório `front-end` e utilizando o comando `npm test`:

```bash
cd front-end
npm test
```

---

## 🗂 Estrutura do Projeto

O projeto segue uma estrutura de monorepo, organizada da seguinte forma:

```
desafio-vsoft/
│
├── api/               # Contém a API .NET 8
│   └── DesafioVsoft/  # Solução .NET com múltiplos projetos (API, Domain, Infrastructure, Repository, Migrations, Test)
├── front-end/         # Contém a aplicação Vue 3
│   ├── public/        # Arquivos estáticos
│   ├── src/           # Código fonte da aplicação Vue
│   │   ├── assets/    # Imagens, ícones, etc.
│   │   ├── components/ # Componentes reutilizáveis
│   │   ├── router/    # Configuração do Vue Router
│   │   ├── services/  # Serviços de comunicação com a API
│   │   └── views/     # Páginas/Views da aplicação
│   └── ...            # Outros arquivos de configuração (package.json, vite.config.js, etc.)
└── docker-compose.yml # Define e orquestra os serviços Docker
```


