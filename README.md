![Imagem](https://drive.google.com/uc?export=view&id=1mV7IfbfpqJTFdBvw7iWp5F8MNGWPDlGC)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://docs.microsoft.com/en-us/ef/core/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![API](https://img.shields.io/badge/API-28A745?style=for-the-badge&logo=api&logoColor=white)]()
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![Migrations](https://img.shields.io/badge/Migrations-F80000?style=for-the-badge&logo=database&logoColor=white)]()
[![Oracle](https://img.shields.io/badge/Oracle-F80000?style=for-the-badge&logo=oracle&logoColor=white)](https://www.oracle.com/database/)

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


## Escopo da aplicação

O Knowball é uma aplicação Web API desenvolvida para gerenciar informações relacionadas a:

- **Campeonatos**: cadastro e gerenciamento de competições esportivas
- **Equipes**: registro de times participantes com informações de localização
- **Partidas**: controle de jogos realizados com data, horário e local
- **Árbitros**: gestão de árbitros com status e funções.
- **Arbitragem**: designação de árbitros para paridas (Principal, Assistente 1, Assistente 2, Quarto Árbitro)
- **Participação**: controle de equipes em partidas (Mandante/Visitante)
- **Denúncias**: sistema de registro e acompanhamento de denúncias relacionadas a partidas

## **Novas funcionalidades implementadas**

### Web API RESTful

- **CRUD completo** para todos os domínios (Campeonatos, Equipes, Partidas, Árbitros, Denúncias, Arbitragens e Participações)

- **Endpoint Search** em cada domínio com:
    - 📄 **Paginação** (controle de `page` e `pageSize`)
    - 🔄 **Ordenação** customizável por múltiplos campos
    - 🔍 **Filtros avançados** (por nome, status, data, local, etc.)

### Interface WEB (MVC)

- **Dashboard visual** para gestão de denúncias
- **CRUD completo** com validações client-side e server-side
- **Layout responsivo** com Bootstrap 5
- **Navegação personalizada** com navbar e breadcrumbs
- **ViewModels** para transferência otimizada de dados


![Imagem](https://drive.google.com/uc?export=view&id=1Osx4A7PAEK_vUS3Lg1mltelt3ZDTYaWM)


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

## Arquitetura da aplicação

O projeto segue os princípios da **Clean Architecture**, garantindo separação de responsabilidades, baixo acoplamento e alto coesão.

### Estrutura de camadas
```
📦 Knowball
┣ 📂 Properties
┃ ┗ 📜 launchSettings.json 
┣ 📂 Application   
┃ ┣ 📂 DTOs
┃ ┣ 📂 Exceptions
┃ ┣ 📂 Services 
┣ 📂 Controllers
┃ 📂 Domain
┃ ┣ 📂 Repositories 
┣ 📂 Infrastructure 
┃ ┣ 📂 Repositories 
┃ ┣ 📜 KnowballContext.cs
┃ ┣ 📜 KnowballContextFactory.cs
┃ 📂 Migrations
┃ 📜 appsettings.json
┃ 📜 Knowball.http
┗ 📜 Program.cs

📦 Knowball.UI (MVC Web Application)
┣ 📂 Controllers # Controllers MVC
┃ ┣ 📜 DenunciaController.cs
┃ ┣ 📜 ArbitroController.cs
┃ ┗ 📜 ... (outros)
┣ 📂 ViewModels # ViewModels MVC
┃ ┣ 📜 DenunciaViewModel.cs
┃ ┣ 📜 ArbitroViewModel.cs
┃ ┗ 📜 ... (outros)
┣ 📂 Views # Views Razor
┃ ┣ 📂 Denuncia
┃ ┃ ┣ 📜 Index.cshtml
┃ ┃ ┣ 📜 Create.cshtml
┃ ┃ ┣ 📜 Edit.cshtml
┃ ┃ ┣ 📜 Delete.cshtml
┃ ┃ ┗ 📜 Details.cshtml
┃ ┣ 📂 outros...
┃ ┣ 📂 Shared
┃ ┃ ┣ 📜 _Layout.cshtml
┃ ┃ ┗ 📜 _ValidationScriptsPartial.cshtml
┃ ┗ 📜 _ViewImports.cshtml
┣ 📂 wwwroot # Arquivos estáticos
┃ ┣ 📂 css
┃ ┣ 📂 js
┃ ┗ 📂 lib
┗ 📜 Program.cs
```

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

3. Configure a string de conexão
Edite o arquivo `appsettings.json` e ajuste a conexão com seu banco Oracle:
```bash
"ConnectionStrings": {
"DefaultConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=URL_ENTRADA;"
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

## Testando a API

### Usando o arquivo Knowball.http

O projeto inclui um arquivo `Knowball.http` na raiz do projeto com exemplos de requisições para todos os endpoints. Para usá-lo:

1. Abra o arquivo `Knowball.http` no Visual Studio ou VS Code
2. Certifique-se de que o projeto está rodando
3. Clique em "Send Request" acima de cada requisição para testá-la

---

## 📡 Endpoints da API

### Árbitros
- `GET /api/arbitro` - Lista todos os árbitros
- `GET /api/arbitro/{id}` - Busca árbitro por ID
- `POST /api/arbitro` - Cria novo árbitro
- `PUT /api/arbitro/{id}` - Atualiza árbitro
- `DELETE /api/arbitro/{id}` - Remove árbitro
- `GET /api/arbitro/search?page=1&pageSize=10&nome=João&status=Ativo&orderBy=nome` - Busca com filtros

### Arbitragens
- `GET /api/arbitragensapi` - Lista todas as arbitragens
- `GET /api/arbitragensapi/{idPartida}/{idArbitro}` - Busca arbitragem específica
- `POST /api/arbitragensapi` - Cria nova arbitragem
- `PUT /api/arbitragensapi/{idPartida}/{idArbitro}` - Atualiza arbitragem
- `DELETE /api/arbitragensapi/{idPartida}/{idArbitro}` - Remove arbitragem
- `GET /api/arbitragensapi/search?idPartida=1&funcao=Principal` - Busca com filtros

### Campeonatos
- `GET /api/campeonatosapi` - Lista todos os campeonatos
- `GET /api/campeonatosapi/{id}` - Busca campeonato por ID
- `POST /api/campeonatosapi` - Cria novo campeonato
- `PUT /api/campeonatosapi/{id}` - Atualiza campeonato
- `DELETE /api/campeonatosapi/{id}` - Remove campeonato
- `GET /api/campeonatosapi/search?categoria=Profissional&ano=2025` - Busca com filtros

### Denúncias
- `GET /api/denunciasapi` - Lista todas as denúncias
- `GET /api/denunciasapi/{id}` - Busca denúncia por ID
- `POST /api/denunciasapi` - Cria nova denúncia
- `PUT /api/denunciasapi/{id}` - Atualiza denúncia
- `DELETE /api/denunciasapi/{id}` - Remove denúncia
- `GET /api/denunciasapi/search?status=Em Análise&dataInicio=2025-01-01` - Busca com filtros

### Equipes
- `GET /api/equipe` - Lista todas as equipes
- `GET /api/equipe/{id}` - Busca equipe por ID
- `POST /api/equipe` - Cria nova equipe
- `PUT /api/equipe/{id}` - Atualiza equipe
- `DELETE /api/equipe/{id}` - Remove equipe
- `GET /api/equipe/search?cidade=São Paulo&estado=SP` - Busca com filtros

### Participações
- `GET /api/participacao` - Lista todas as participações
- `GET /api/participacao/{idPartida}/{idEquipe}` - Busca participação específica
- `POST /api/participacao` - Cria nova participação
- `PUT /api/participacao/{idPartida}/{idEquipe}` - Atualiza participação
- `DELETE /api/participacao/{idPartida}/{idEquipe}` - Remove participação
- `GET /api/participacao/search?tipo=Mandante&idPartida=5` - Busca com filtros

### Partidas
- `GET /api/partidasapi` - Lista todas as partidas
- `GET /api/partidasapi/{id}` - Busca partida por ID
- `POST /api/partidasapi` - Cria nova partida
- `PUT /api/partidasapi/{id}` - Atualiza partida
- `DELETE /api/partidasapi/{id}` - Remove partida
- `GET /api/partidasapi/search?idCampeonato=1&dataInicio=2025-01-01` - Busca com filtros

> **📝 Nota:** Todos os endpoints de busca suportam paginação (`page`, `pageSize`), ordenação (`orderBy`) e incluem links HATEOAS.

---

## Integrantes

| Dev | Foto | RM |
| ------------- | ------ | ----- |
| ![](https://img.shields.io/badge/DEV-Gabriel-47797a?style=for-the-badge&logo=github) | <a href="https://github.com/GabrielRossi01"><img src="https://avatars.githubusercontent.com/u/179617228?v=4" height="50" style="border-radius:30px;"></a> | RM560967 |
| ![](https://img.shields.io/badge/DEV-Rodrigo-70b2b4?style=for-the-badge&logo=github) | <a href="https://github.com/RodrygoYamasaki"><img src="https://avatars.githubusercontent.com/u/182231531?v=4" height="50" style="border-radius:30px;"></a> | RM560759 |
| ![](https://img.shields.io/badge/DEV-Patrick-7ca787?style=for-the-badge&logo=github) | <a href="https://github.com/castropatrick"><img src="https://avatars.githubusercontent.com/u/179931043?v=4" height="50" style="border-radius:30px;"></a> | RM559271 |
