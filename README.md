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

## Escopo da aplicação

O Knowball é uma aplicação Web API desenvolvida para gerenciar informações relacionadas a:

- **Campeonatos**: cadastro e gerenciamento de competições esportivas
- **Equipes**: registro de times participantes com informações de localização
- **Partidas**: controle de jogos realizados com data, horário e local
- **Árbitros**: gestão de árbitros com status e funções.
- **Arbitragem**: designação de árbitros para paridas (Principal, Assistente 1, Assistente 2, Quarto Árbitro)
- **Participação**: controle de equipes em partidas (Mandante/Visitante)
- **Denúncias**: sistema de registro e acompanhamento de denúncias relacionadas a partidas

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
 ┣ 📂 Controllers             # Camada de Apresentação (API)
 ┣ 📂 Application
 ┃ ┣ 📂 Services              # Serviços de aplicação
 ┃ ┣ 📂 DTOs                  # Objetos de Transferência de Dados
 ┃ ┗ 📂 Interfaces            # Contratos de serviços
 ┣ 📂 Domain                  # Camada de Domínio
 ┃ ┣ 📂 Entities              # Entidades do domínio
 ┃ ┗ 📂 Repositories          # Interfaces de repositórios
 ┗ 📂 Infrastructure          # Camada de Infraestrutura
   ┣ 📂 Repositories          # Implementação de repositórios
   ┣ 📜 KnowballContext.cs    # Contexto do EF Core
   ┗ 📂 Migrations            # Migrações do banco
```

## Como rodar o projeto

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

4. Restaure os pacotes
```bash
dotnet restore
```

5. Execute as migrations
```bash
dotnet ef database update
```
Isso criará todas as tabelas no banco Oracle

6. Execute o projeto
```bash
dotnet run
```

A aplicação será iniciada em:

- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001` ou `https://localhost:7007`

---

## Acessando o Swagger

Após iniciar a aplicação, acesse a documentação interativa da API:

`https://localhost:7007/swagger`

## Integrantes

| Dev | Foto | RM |
| ------------- | ------ | ----- |
| ![](https://img.shields.io/badge/DEV-Gabriel-47797a?style=for-the-badge&logo=github) | <a href="https://github.com/GabrielRossi01"><img src="https://avatars.githubusercontent.com/u/179617228?v=4" height="50" style="border-radius:30px;"></a> | RM560967 |
| ![](https://img.shields.io/badge/DEV-Rodrigo-70b2b4?style=for-the-badge&logo=github) | <a href="https://github.com/RodrygoYamasaki"><img src="https://avatars.githubusercontent.com/u/182231531?v=4" height="50" style="border-radius:30px;"></a> | RM560759 |
| ![](https://img.shields.io/badge/DEV-Patrick-7ca787?style=for-the-badge&logo=github) | <a href="https://github.com/castropatrick"><img src="https://avatars.githubusercontent.com/u/179931043?v=4" height="50" style="border-radius:30px;"></a> | RM559271 |
