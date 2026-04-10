![Imagem](https://drive.google.com/uc?export=view&id=1mV7IfbfpqJTFdBvw7iWp5F8MNGWPDlGC)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://docs.microsoft.com/en-us/ef/core/)
[![xUnit](https://img.shields.io/badge/xUnit-512BD4?style=for-the-badge&logo=xunit&logoColor=white)](https://xunit.net/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![API](https://img.shields.io/badge/API-28A745?style=for-the-badge&logo=api&logoColor=white)]()
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![Migrations](https://img.shields.io/badge/Migrations-F80000?style=for-the-badge&logo=database&logoColor=white)]()
[![Oracle](https://img.shields.io/badge/Oracle-F80000?style=for-the-badge&logo=oracle&logoColor=white)](https://www.oracle.com/database/)
[![Serilog](https://img.shields.io/badge/Serilog-052147?style=for-the-badge&logo=serilog&logoColor=white)](https://serilog.net/)
[![OpenTelemetry](https://img.shields.io/badge/OpenTelemetry-000000?style=for-the-badge&logo=opentelemetry&logoColor=white)](https://opentelemetry.io/)

## Objetivo do projeto

O Knowball busca resolver os desafios comuns encontrados na gestão e organização dos campeonatos e partidas esportivas, especialmente no **futebol das categorias de base do futebol brasileiro masculino**. Entre os principais problemas estão:

- Necessidade de um sistema eficiente para registrar e **gerenciar denúncias relacionadas a manipulação de partidas**.
- Falta de uma plataforma unificada para controle de campeonatos, equipes, jogos e participações, que atualmente são gerenciados por sistemas fragmentados.
- Dificuldade de acompanhar e registrar a atuação dos árbitros e suas respectivas atribuições em cada partida.
- Falta de **APIs RESTful flexíveis** para integração com outras ferramentas e sistemas de gestão esportiva.

## Visão geral

O **Knowball** é uma solução integrada desenvolvida em **ASP .NET Core** que combina uma **Web API RESTful** e uma **interface web MVC** para a gestão completa de campeonatos esportivos, especialmente voltada para as categorias de base do futebol brasileiro masculino.

### Principais desafios resolvidos

✅ **Gestão de denúncias** relacionadas à manipulação de partida com sistema de protocolo único.

✅ **Plataforma unificada** para controle de campeonatos, equipes, partidas e participações.

✅ **Acompanhamento centralizado** da atuação e designação de árbitros.

✅ **APIs RESTful completas** com HATEOAS, paginação, ordenação e filtros avançados.

✅ **Interface web intuitiva** para gestão visual dos dados.

✅ **Monitoramento e observabilidade** com Health Checks, Serilog e OpenTelemetry.

✅ **Testes automatizados** com cobertura de camadas Unit e Integration.


## Escopo da aplicação

O Knowball é uma aplicação Web API desenvolvida para gerenciar informações relacionadas a:

- **Campeonatos**: cadastro e gerenciamento de competições esportivas
- **Equipes**: registro de times participantes com informações de localização
- **Partidas**: controle de jogos realizados com data, horário e local
- **Árbitros**: gestão de árbitros com status e funções.
- **Arbitragem**: designação de árbitros para partidas (Principal, Assistente 1, Assistente 2, Quarto Árbitro)
- **Participação**: controle de equipes em partidas (Mandante/Visitante)
- **Denúncias**: sistema de registro e acompanhamento de denúncias relacionadas a partidas

## **Novas funcionalidades implementadas - Sprint 3**

### 🩺 Monitoramento e Observabilidade

#### Health Checks

Endpoints para monitorar a saúde da aplicação e a conectividade com o banco Oracle em tempo real:

| Endpoint | Descrição |
|---|---|
| `GET /health` | Status geral da aplicação em JSON detalhado |
| `GET /health/db` | Verifica exclusivamente a conexão com o Oracle |
| `GET /health/ready` | Verifica todos os checks (API + banco) |

Exemplo de resposta do `GET /health`:

![Health Check](https://drive.google.com/uc?export=view&id=1WUwGXTgYUECpUm0sf5rP6xdfsfbPYf7b)

#### Logging Estruturado com Serilog

A aplicação utiliza **Serilog** para logging estruturado com:

- **Níveis de log**: `Information`, `Warning`, `Error`
- **Saída para console**: com template formatado e timestamp
- **Saída para arquivo**: pasta `Logs/`, arquivo rotacionado diariamente no formato `knowball-YYYYMMDD.txt`
- **Correlação de requisições**: todas as requisições HTTP são logadas automaticamente com método, path, status code e tempo de resposta via `UseSerilogRequestLogging`

Exemplo de log no console:
```
[10:30:15 INF] [CampeonatoService] Criando campeonato: Nome=Copa Sub-17, Categoria=Sub-17, Ano=2025 {}
[10:30:15 INF] HTTP POST /api/campeonatos respondeu 201 em 12,3456ms
```

#### Distributed Tracing e Métricas com OpenTelemetry

A aplicação implementa **rastreamento distribuído** com OpenTelemetry, capturando:

- **Tracing**: rastreamento de requisições HTTP entre as camadas (Controller → Service → Repository)
- **Métricas de desempenho**: tempo de resposta, taxa de erros, contagem de requisições
- **Instrumentações ativas**:
  - `AspNetCore` — requisições HTTP de entrada
  - `HttpClient` — chamadas HTTP de saída
  - `EntityFrameworkCore` — queries ao banco Oracle
  - `Runtime` — métricas da CLR (.NET runtime)

![OpenTelemetry](https://drive.google.com/uc?export=view&id=1aQPRW46TY68f7YDfCweHmcTwtaUWFwSg)

---

### 🧪 Testes Automatizados

A solução conta com **206 testes automatizados** organizados em dois projetos distintos, todos seguindo o padrão **AAA (Arrange, Act, Assert)**.

> Substitua a imagem abaixo pela screenshot do resultado dos 206 testes no Visual Studio ou CLI.

![Testes](https://drive.google.com/uc?export=view&id=1F9f-OLRGRkqw51fTU9sJimvChizzWzFl)

---

#### Tecnologias utilizadas nos testes

| Pacote | Finalidade |
|---|---|
| `xUnit` | Framework de testes |
| `Moq` | Mocking de dependências nos testes unitários |
| `FluentAssertions` | Assertivas legíveis e expressivas |
| `Microsoft.AspNetCore.Mvc.Testing` | `WebApplicationFactory` para testes de integração |
| `Microsoft.EntityFrameworkCore.InMemory` | Banco em memória isolado para integração |

#### Nomenclatura dos testes

Todos os testes seguem o padrão:

```
MetodoTestado_Cenario_ResultadoEsperado
```

Exemplos:
```
CriarCampeonato_DadosValidos_RetornaDtoComId
CriarCampeonato_CategoriaInvalida_LancaBusinessException
GetById_IdInexistente_Retorna404
Delete_IdExistente_Retorna200
```

---

## Como executar os testes

### Via CLI (recomendado para ver detalhes de falhas)

```bash
# Todos os testes da solução (rode na pasta raiz)
dotnet test

# Com output detalhado — mostra nome de cada teste executado
dotnet test --logger "console;verbosity=detailed"

# Somente testes unitários
dotnet test --filter "FullyQualifiedName~Unit"

# Somente testes de integração
dotnet test --filter "FullyQualifiedName~Integration"

# Teste específico pelo nome do método
dotnet test --filter "FullyQualifiedName~CriarCampeonato_DadosValidos_RetornaDtoComId"

# Gerar relatório de cobertura de código
dotnet test --collect:"XPlat Code Coverage"

# Modo watch — re-executa ao salvar arquivos (útil no desenvolvimento)
dotnet watch test
```

### Via Visual Studio — Test Explorer

1. Abra o menu **Test → Test Explorer** (ou `Ctrl + E, T`)
2. A janela exibirá todos os testes organizados por projeto → namespace → classe
3. Clique em **Run All** (`Ctrl + R, V`) para executar todos
4. Para executar apenas um subconjunto, selecione os testes desejados e clique em **Run Selected** (`Ctrl + R, T`)
5. Testes com ✅ verde passaram; ❌ vermelho falharam — clique para ver o motivo

> **Dica**: No ícone de engrenagem do Test Explorer, ative **"Run Tests After Build"** para executar os testes automaticamente a cada compilação.

---

## Arquitetura da aplicação

O projeto segue os princípios da **Clean Architecture**, garantindo separação de responsabilidades, baixo acoplamento e alta coesão.

### Estrutura de camadas
```
📦 Fiap.Knowball (Web API)
┣ 📂 Application
┃ ┣ 📂 DTOs
┃ ┣ 📂 Exceptions
┃ ┗ 📂 Services
┣ 📂 Controllers
┣ 📂 HealthChecks
┃ ┣ 📜 ApiHealthCheck.cs
┃ ┗ 📜 DatabaseHealthCheck.cs
┣ 📂 Infrastructure
┃ ┣ 📂 Repositories
┃ ┣ 📜 KnowballContext.cs
┃ ┗ 📜 KnowballContextFactory.cs
┣ 📂 Logs                        ← gerado em runtime pelo Serilog
┣ 📂 Migrations
┣ 📂 Models
┃ ┣ 📂 Repositories
┣ 📜 appsettings.json
┣ 📜 Knowball.http
┗ 📜 Program.cs

📦 Fiap.Knowball.UI (MVC Web Application)
┣ 📂 Controllers
┣ 📂 ViewModels
┣ 📂 Views
┃ ┣ 📂 Arbitragem
┃ ┣ 📂 Arbitro
┃ ┣ 📂 Campeonato
┃ ┣ 📂 Denuncia
┃ ┣ 📂 Equipe
┃ ┣ 📂 Participacao
┃ ┣ 📂 Partida
┃ ┗ 📂 Shared
┗ 📜 Program.cs

📦 Fiap.Knowball.Tests (Projetos de Teste)
┣ 📂 Unit
┃ ┣ 📂 Domain
┃ ┗ 📂 Services
┗ 📂 Integration
  ┣ 📂 Controllers
  ┗ 📂 Fixtures
```

---

## Instalação e configuração

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Oracle Database](https://www.oracle.com/database/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### Passo a passo

1. Clone o repositório
```bash
git clone https://github.com/knowball-oracle/Knowball.git
```

2. Entre na pasta da aplicação
```bash
cd Knowball/Knowball
```

3. Configure as variáveis de ambiente

No Visual Studio: botão direito no projeto → **Properties → Debug → Open debug launch profiles UI → Environment Variables**

| Variável | Valor |
|---|---|
| `DB_USERNAME` | seu usuário Oracle |
| `DB_PASSWORD` | sua senha Oracle |

O `appsettings.json` já está configurado para usar essas variáveis:
```json
"ConnectionStrings": {
  "DefaultConnection": "User Id=${DB_USERNAME};Password=${DB_PASSWORD};Data Source=oracle.fiap.com.br:1521/ORCL;"
}
```

### Para o MVC (Knowball.UI)
Edite `Knowball.UI/appsettings.json` da mesma forma.

4. Restaure os pacotes
```bash
dotnet restore
```

5. Execute as migrations
```bash
dotnet ef database update
```
Isso criará todas as tabelas no banco Oracle

6. Clique com o botão direito na solução `Knowball.UI` e clique em `Definir como Projeto de Inicialização`

7. Execute o projeto
```bash
dotnet run --launch-profile https    #ou F5
```

A aplicação será iniciada em:

- **HTTP**: `http://localhost:5026`
- **HTTPS**: `https://localhost:7007`

---

## Acessando o Swagger

Após iniciar a aplicação, acesse a documentação interativa da API:

`https://localhost:7007/swagger` (verifique o `launchSettings.json`)

---

## Interface Web (MVC)

![Imagem](https://drive.google.com/uc?export=view&id=1Osx4A7PAEK_vUS3Lg1mltelt3ZDTYaWM)

---

## Testando a API

### Usando o arquivo Knowball.http

O projeto inclui um arquivo `Knowball.http` na raiz do projeto com exemplos de requisições para todos os endpoints. Para usá-lo:

1. Abra o arquivo `Knowball.http` no Visual Studio ou VS Code
2. Certifique-se de que o projeto está rodando
3. Clique em "Send Request" acima de cada requisição para testá-la

---

## 📡 Endpoints da API

### 🩺 Health Checks

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/health` | Status geral com detalhes JSON de cada check |
| GET | `/health/db` | Verifica conectividade com o Oracle |
| GET | `/health/ready` | Verifica todos os checks simultaneamente |

### Árbitros

| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/arbitro` | Lista todos os árbitros |
| `GET` | `/api/arbitro/{id}` | Busca árbitro por ID |
| `POST` | `/api/arbitro` | Cria novo árbitro |
| `PUT` | `/api/arbitro/{id}` | Atualiza árbitro |
| `DELETE` | `/api/arbitro/{id}` | Remove árbitro |
| `GET` | `/api/arbitro/search?page=1&pageSize=10&nome=João&status=Ativo&orderBy=nome` | Busca com filtros |

### Arbitragens

| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/arbitragens` | Lista todas as arbitragens |
| `GET` | `/api/arbitragens/{idPartida}/{idArbitro}` | Busca arbitragem específica |
| `POST` | `/api/arbitragens` | Cria nova arbitragem |
| `PUT` | `/api/arbitragens/{idPartida}/{idArbitro}` | Atualiza arbitragem |
| `DELETE` | `/api/arbitragens/{idPartida}/{idArbitro}` | Remove arbitragem |
| `GET` | `/api/arbitragens/search?idPartida=1&funcao=Principal` | Busca com filtros |

### Campeonatos

| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/campeonatos` | Lista todos os campeonatos |
| `GET` | `/api/campeonatos/{id}` | Busca campeonato por ID |
| `POST` | `/api/campeonatos` | Cria novo campeonato |
| `PUT` | `/api/campeonatos/{id}` | Atualiza campeonato |
| `DELETE` | `/api/campeonatos/{id}` | Remove campeonato |
| `GET` | `/api/campeonatos/search?categoria=Sub-17&ano=2025` | Busca com filtros |

### Denúncias

| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/denuncias` | Lista todas as denúncias |
| `GET` | `/api/denuncias/{id}` | Busca denúncia por ID |
| `POST` | `/api/denuncias` | Cria nova denúncia |
| `PUT` | `/api/denuncias/{id}` | Atualiza denúncia |
| `DELETE` | `/api/denuncias/{id}` | Remove denúncia |
| `GET` | `/api/denuncias/search?status=Em Análise&dataInicio=2025-01-01` | Busca com filtros |

### Equipes

| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/equipe` | Lista todas as equipes |
| `GET` | `/api/equipe/{id}` | Busca equipe por ID |
| `POST` | `/api/equipe` | Cria nova equipe |
| `PUT` | `/api/equipe/{id}` | Atualiza equipe |
| `DELETE` | `/api/equipe/{id}` | Remove equipe |
| `GET` | `/api/equipe/search?cidade=São Paulo&estado=SP` | Busca com filtros |

### Participações

| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/participacao` | Lista todas as participações |
| `GET` | `/api/participacao/{idPartida}/{idEquipe}` | Busca participação específica |
| `POST` | `/api/participacao` | Cria nova participação |
| `PUT` | `/api/participacao/{idPartida}/{idEquipe}` | Atualiza participação |
| `DELETE` | `/api/participacao/{idPartida}/{idEquipe}` | Remove participação |
| `GET` | `/api/participacao/search?tipo=Mandante&idPartida=5` | Busca com filtros |

### Partidas

| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/partidas` | Lista todas as partidas |
| `GET` | `/api/partidas/{id}` | Busca partida por ID |
| `POST` | `/api/partidas` | Cria nova partida |
| `PUT` | `/api/partidas/{id}` | Atualiza partida |
| `DELETE` | `/api/partidas/{id}` | Remove partida |
| `GET` | `/api/partidas/search?idCampeonato=1&dataInicio=2025-01-01` | Busca com filtros |

> **📝 Nota:** Todos os endpoints de busca suportam paginação (`page`, `pageSize`), ordenação (`orderBy`) e incluem links HATEOAS.

---

### Requisitos funcionais

- `RF01`: cadastrar, listar, atualizar e remover campeonatos
- `RF02`: cadastrar, listar, atualizar e remover equipes
- `RF03`: cadastrar, listar, atualizar e remover árbitros
- `RF04`: cadastrar, listar, atualizar e remover partidas
- `RF05`: registrar participação de equipes em partidas (mandante/visitante)
- `RF06`: designar árbitros para partidas com suas respectivas funções
- `RF07`: registrar denúncias relacionadas a partidas com protocolo único

### Requisitos não funcionais

- `RNF01`: utilizar Clean Architecture para separação de responsabilidades
- `RNF02`: persistência de dados em banco Oracle Database via Entity Framework Core
- `RNF03`: API RESTful com documentação Swagger/OpenAPI
- `RNF04`: validação de dados com Data Annotations
- `RNF05`: injeção de dependências para desacoplamento
- `RNF06`: uso de DTOs para transferência de dados entre camadas
- `RNF07`: tratamento de exceções com mensagens descritivas
- `RNF08`: monitoramento de saúde via Health Checks (`/health`)
- `RNF09`: logging estruturado com Serilog (console + arquivo rotacionado)
- `RNF10`: rastreamento distribuído e métricas com OpenTelemetry
- `RNF11`: cobertura de testes automatizados nas camadas de Domínio, Aplicação e Integração

## Integrantes

| Dev | Foto | RM |
| ------------- | ------ | ----- |
| ![](https://img.shields.io/badge/DEV-Gabriel-47797a?style=for-the-badge&logo=github) | <a href="https://github.com/GabrielRossi01"><img src="https://avatars.githubusercontent.com/u/179617228?v=4" height="50" style="border-radius:30px;"></a> | RM560967 |
| ![](https://img.shields.io/badge/DEV-Rodrigo-70b2b4?style=for-the-badge&logo=github) | <a href="https://github.com/RodrygoYamasaki"><img src="https://avatars.githubusercontent.com/u/182231531?v=4" height="50" style="border-radius:30px;"></a> | RM560759 |
| ![](https://img.shields.io/badge/DEV-Patrick-7ca787?style=for-the-badge&logo=github) | <a href="https://github.com/castropatrick"><img src="https://avatars.githubusercontent.com/u/179931043?v=4" height="50" style="border-radius:30px;"></a> | RM559271 |
