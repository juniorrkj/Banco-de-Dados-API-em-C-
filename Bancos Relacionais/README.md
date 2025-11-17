# Sistema de Controle de Estoque

Este é um sistema de controle de estoque que permite gerenciar produtos e categorias através de uma interface de console e API.

## Requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Como Instalar

1. Baixe e instale o [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Clone ou baixe este repositório
3. Abra o terminal na pasta do projeto (pasta que contém o arquivo `BancosRelacionais.csproj`)
4. Execute os seguintes comandos:

```powershell
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef database update
```

## Como Executar

Na pasta do projeto, execute:

```powershell
dotnet run
```

O sistema iniciará e você verá o menu principal com as opções:
- API estará disponível em http://localhost:5099
- Swagger (documentação da API) em http://localhost:5099/swagger

## Funcionalidades

1. Cadastrar produto
2. Listar produtos
3. Atualizar produto (por Id)
4. Remover produto (por Id)
5. Cadastrar categoria
6. Adicionar produto à categoria
7. Listar categorias e produtos
8. Listar produtos por categoria
9. Atualizar estoque

## Versão Executável (Sem necessidade do SDK)

Para criar uma versão executável que não precisa do SDK instalado:

```powershell
dotnet publish -c Release --self-contained true -r win-x64
```

O executável estará na pasta `bin/Release/net8.0/win-x64/publish`