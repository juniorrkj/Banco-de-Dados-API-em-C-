# 📦 Sistema de Controle de Estoque (Backend API)

Este é um sistema de controle de estoque que permite gerenciar produtos e categorias através de uma API RESTful e de uma interface de console.

-----

## 👤 Autoria e Instituição

| Detalhe | Informação |
| :--- | :--- |
| **Desenvolvido por:** | **Claudiano Pinto de Oliveira Junior** |
| **Curso/Área:** | **Estudante de Ciências da Computação** |
| **Instituição:** | **CEUB** |
| **Disciplina/Projeto:** | **Desenvolvimento de Sistemas** |
| **Versão:** | 1.0 (NET 8.0) |

-----

## 🛠️ Tecnologias e Arquitetura

Este projeto foi desenvolvido utilizando as seguintes tecnologias e arquitetura:

  * **Linguagem:** C\#
  * **Framework:** **.NET 8.0** (ASP.NET Core)
  * **Banco de Dados:** SQLite (padrão, configurado via Entity Framework Core)
  * **ORM (Mapeamento Objeto-Relacional):** **Entity Framework Core (EF Core)**
      * *Abordagem:* Code-First com Migrations, permitindo que a estrutura do banco de dados seja definida pelo código C\#.
  * **Interface:** API RESTful e Interface de Console.
  * **Documentação da API:** **Swagger/OpenAPI**, gerada automaticamente para facilitar o teste e o consumo dos endpoints.

-----

## 💡 Explicação Técnica do Funcionamento

O sistema é construído sobre o **ASP.NET Core** e segue os princípios da arquitetura **RESTful** e do padrão **CRUD** (Create, Read, Update, Delete) para o gerenciamento de dados.

### 1\. Persistência de Dados (Entity Framework Core)

  * **`DbContext`:** O coração da comunicação com o banco de dados. Ele mapeia as classes C\# (`Produto`, `Categoria`) para as tabelas no SQLite.
  * **Migrations:** Ao executar `dotnet ef database update`, o EF Core lê as *migrations* (arquivos de histórico de mudanças) e garante que o esquema do banco de dados (tabelas e colunas) esteja sincronizado com as classes C\# do projeto.

### 2\. Acesso à API (Endpoints)

Os controladores da API (localizados na pasta `Controllers`) expõem as seguintes funcionalidades:

  * **`GET`:** Usado para consultar e listar dados (Ex: `/api/produtos`).
  * **`POST`:** Usado para criar novos recursos (Ex: `/api/produtos` para cadastrar um novo produto).
  * **`PUT`** / **`PATCH`:** Usado para atualizar recursos existentes (Ex: `/api/produtos/{id}`).
  * **`DELETE`:** Usado para remover recursos (Ex: `/api/produtos/{id}`).

A documentação **Swagger** oferece uma interface gráfica amigável para explorar e testar cada um desses endpoints.

### 3\. Interface de Console

A aplicação também inclui uma interface de console que encapsula as chamadas à lógica de negócios, permitindo o gerenciamento rápido do estoque diretamente pelo terminal, ideal para testes ou uso local simplificado.

-----

## ⚙️ Requisitos

  - **[.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**

## 🚀 Como Instalar

1.  Baixe e instale o **[.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**.
2.  Clone ou baixe este repositório.
3.  Abra o terminal na pasta do projeto (a pasta que contém o arquivo `BancosRelacionais.csproj`).
4.  Execute os seguintes comandos para restaurar dependências, instalar a ferramenta EF Core (se necessário) e criar/atualizar o banco de dados:

<!-- end list -->

```powershell
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef database update
```

## ▶️ Como Executar

Na pasta do projeto, execute:

```powershell
dotnet run
```

O sistema iniciará e você verá o menu principal com as opções:

  * **API estará disponível em:** `http://localhost:5099`
  * **Swagger (documentação da API) em:** `http://localhost:5099/swagger`

## ✨ Funcionalidades (CRUD e Gestão)

1.  Cadastrar produto
2.  Listar produtos
3.  Atualizar produto (por Id)
4.  Remover produto (por Id)
5.  Cadastrar categoria
6.  Adicionar produto à categoria
7.  Listar categorias e produtos
8.  Listar produtos por categoria
9.  Atualizar estoque

## 📦 Versão Executável (Sem necessidade do SDK)

Para criar uma versão executável autocontida que não precisa do .NET SDK instalado na máquina de destino (útil para distribuição):

```powershell
dotnet publish -c Release --self-contained true -r win-x64
```

O executável estará na pasta **`bin/Release/net8.0/win-x64/publish`**.
