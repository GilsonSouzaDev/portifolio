# Portfólio Profissional (Full Stack)

![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![.NET Core](https://img.shields.io/badge/.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Oracle](https://img.shields.io/badge/Oracle-F80000?style=for-the-badge&logo=oracle&logoColor=white)

Um portfólio completo, moderno e dinâmico, construído com as melhores práticas de Engenharia de Software.

## 🚀 Visão Geral
Este projeto foi desenvolvido para demonstrar habilidades avançadas de desenvolvimento **Full Stack**. O sistema é composto por um Frontend SPA (Single Page Application) moderno e interativo, consumindo uma API REST robusta construída sob o padrão Clean Architecture.

### Funcionalidades
- **Apresentação Dinâmica**: Dados carregados do banco de dados (Perfil, Skills, Projetos, Redes Sociais).
- **Modo Edição**: Todo o conteúdo pode ser editado na própria interface (inline-editing) através de um modo "Admin" escondido, alterando instantaneamente o banco de dados.
- **Formulário de Contato**: Envio de mensagens armazenadas no banco de dados com notificação via e-mail utilizando `SendGrid`, `Mailgun` ou `Gmail` (com Fallback automático em caso de falha de um provedor).
- **Upload de Arquivos**: Curriculo PDF e avatares sincronizados e disponibilizados via diretórios estáticos.

---

## 🏗 Arquitetura e Tecnologias

### Frontend (Angular 20)
O frontend foi construído utilizando os recursos mais modernos do Angular (Standalone Components, Signals, Control Flow nativo). A arquitetura visual respeita estritamente o padrão **Smart e Dumb Components**:
- **Smart Components** (`home.ts`): Componente pai responsável por injetar serviços, realizar chamadas de API, processar regras de negócio e distribuir o estado.
- **Dumb Components** (ex: `hero.ts`, `portfolio.ts`, `contact.ts`): Componentes visuais puramente apresentacionais, isolados, que recebem dados via `@Input()` e notificam o pai de qualquer ação do usuário via `@Output()`. Não possuem acesso a dependências externas de negócio.

### Backend (.NET 10 & C#)
A API RESTful é baseada em ASP.NET Core e utiliza uma estrutura inspirada na **Clean Architecture** (Arquitetura em Camadas / N-Tier):
- **Controllers**: Camada fina e sem regras de negócio (Thin Controllers).
- **Services (Camada de Negócio)**: Contém toda a lógica e operações de dados via `Interfaces` e `Implementations` (ex: `IProfileService`, `ProfileService`), sendo facilmente testáveis utilizando mocks.
- **DTOs (Data Transfer Objects)**: Separam as entidades do banco de dados (Entity Framework) do payload das requisições, evitando vazamentos e garantindo segurança (*Over-posting prevention*).
- **Injeção de Dependência**: Centralizada no `Program.cs`.
- **Banco de Dados**: `Oracle Database` utilizando *Entity Framework Core* ORM (Migrations suportados).

---

## 🛠 Pré-requisitos
Certifique-se de ter os seguintes softwares instalados em seu ambiente local:
- [Node.js](https://nodejs.org/en/) (Versão LTS recomendada) e npm.
- [Angular CLI](https://angular.dev/tools/cli) (`npm install -g @angular/cli`).
- [.NET SDK 10](https://dotnet.microsoft.com/download) (ou versão compatível).
- [Oracle Database](https://www.oracle.com/database/technologies/xe-downloads.html) (Local, Docker ou Nuvem).

---

## ⚙️ Instalação e Execução

### Configurando o Banco de Dados (Backend)
1. Navegue até a pasta do backend:
   ```bash
   cd backend/Portfolio.API
   ```
2. Configure a string de conexão do Oracle no arquivo `appsettings.json` ou via *User Secrets*:
   ```json
   "ConnectionStrings": {
     "OracleDb": "User Id=meu_usuario;Password=minha_senha;Data Source=localhost:1521/XEPDB1;"
   }
   ```
3. Aplique as Migrations para criar o banco de dados (O Seeder preencherá os dados iniciais automaticamente ao subir a API):
   ```bash
   dotnet ef database update
   ```
4. Execute o projeto (Oculta na porta HTTPS definida):
   ```bash
   dotnet run
   ```

### Executando o Frontend
1. Navegue até a pasta do frontend em um novo terminal:
   ```bash
   cd frontend
   ```
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Inicie o servidor de desenvolvimento:
   ```bash
   npm start
   ```
4. Acesse `http://localhost:4200` no seu navegador.

---

## 🧪 Testes Automatizados

Garantir a integridade do código e regras de negócio é fundamental:
- **Backend (xUnit & Moq)**: Testes unitários validam a camada de Serviços de forma isolada do EF Core. Rodar testes com: `dotnet test`
- **Frontend (Jasmine & Karma)**: Testes unitários para validar a lógica dos componentes (Smart e Dumb), os Pipes e Injeção de Serviços. Rodar testes com: `npm test`

---

## 🤝 Contribuições
Padrões de commit exigidos: *Conventional Commits* (ex: `feat:`, `fix:`, `refactor:`).
Testes automatizados devem ser adicionados a cada nova regra de negócio implementada na Camada de Serviços ou em um Smart Component.
