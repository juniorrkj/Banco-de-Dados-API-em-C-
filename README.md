## 📝 Readme Aprimorado e Completo

# 📦 Sistema de Controle de Estoque (Backend API + Frontend)

Este é um sistema completo de controle de estoque que permite gerenciar produtos e categorias através de uma **API RESTful** e de uma **interface web moderna**. O sistema demonstra a integração completa entre frontend e backend, com relacionamento de dados e validações robustas.

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

  * **Backend:**
    * **Linguagem:** C#
    * **Framework:** **.NET 8.0** (ASP.NET Core)
    * **Banco de Dados:** SQLite (configurado via Entity Framework Core)
    * **ORM:** **Entity Framework Core (EF Core)** com abordagem Code-First e Migrations
    * **Documentação da API:** **Swagger/OpenAPI**
  
  * **Frontend:**
    * **HTML5, CSS3, JavaScript (Vanilla)**
    * Interface responsiva e moderna
    * Comunicação com API via Fetch API

  * **Padrões e Conceitos:**
    * Arquitetura RESTful
    * CRUD completo (Create, Read, Update, Delete)
    * Relacionamento N:N entre entidades (INNER JOIN)
    * Validações com DataAnnotations
    * Tratamento de erros HTTP (400, 404, 409, 422)

-----

## �️ Modelo de Banco de Dados

O sistema possui 2 entidades principais com relacionamento N:N:

### Entidades:
1. **Product** (Produto)
   - Id (PK)
   - Name (único)
   - Description
   - Price
   - Quantity

2. **Category** (Categoria)
   - Id (PK)
   - Name (único)
   - Description

3. **ProductCategory** (Tabela de relacionamento)
   - ProductId (FK)
   - CategoryId (FK)
   - AddedAt

### Relacionamento:
- Um produto pode ter várias categorias
- Uma categoria pode estar associada a vários produtos
- Relacionamento gerenciado pela tabela `ProductCategory`

-----

## 🚀 Como Executar o Projeto

### Pré-requisitos:
- **.NET SDK 8.0** instalado ([Download](https://dotnet.microsoft.com/download))
- Navegador web moderno (Chrome, Firefox, Edge)

### Passos:

1. **Clone o repositório:**
```bash
git clone https://github.com/juniorrkj/Banco-de-Dados-API-em-C-.git
cd Banco-de-Dados-API-em-C--main
```

2. **Compile o projeto:**
```bash
dotnet build BD/EstoqueDB.csproj
```

3. **Execute o sistema:**
```bash
dotnet run --project BD/EstoqueDB.csproj
```

4. **Acesse a aplicação:**
   - **Interface Web (GUI):** http://localhost:5099
   - **Documentação Swagger:** http://localhost:5099/swagger
   - **API Base URL:** http://localhost:5099/api/v1

-----

## 📡 Rotas da API

### **Produtos** (`/api/v1/products`)

| Método | Rota | Descrição | Status Codes |
|--------|------|-----------|--------------|
| GET | `/api/v1/products` | Lista todos os produtos | 200 OK |
| GET | `/api/v1/products/{id}` | Busca produto por ID | 200 OK, 404 Not Found |
| GET | `/api/v1/products/with-categories` | Lista produtos com categorias (JOIN) | 200 OK |
| POST | `/api/v1/products` | Cria novo produto | 201 Created, 400 Bad Request, 409 Conflict, 422 Unprocessable Entity |
| PUT | `/api/v1/products/{id}` | Atualiza produto | 200 OK, 404 Not Found, 409 Conflict, 422 Unprocessable Entity |
| DELETE | `/api/v1/products/{id}` | Remove produto | 204 No Content, 404 Not Found |

### **Categorias** (`/api/v1/categories`)

| Método | Rota | Descrição | Status Codes |
|--------|------|-----------|--------------|
| GET | `/api/v1/categories` | Lista todas as categorias | 200 OK |
| GET | `/api/v1/categories/{id}` | Busca categoria por ID | 200 OK, 404 Not Found |
| POST | `/api/v1/categories` | Cria nova categoria | 201 Created, 400 Bad Request, 409 Conflict, 422 Unprocessable Entity |
| PUT | `/api/v1/categories/{id}` | Atualiza categoria | 200 OK, 404 Not Found, 409 Conflict, 422 Unprocessable Entity |
| DELETE | `/api/v1/categories/{id}` | Remove categoria | 204 No Content, 404 Not Found |

-----

## 📝 Exemplos de Requisições

### Criar Produto (POST)
```bash
curl -X POST http://localhost:5099/api/v1/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Notebook Dell",
    "description": "Notebook Dell Inspiron 15 com 16GB RAM",
    "price": 3500.00,
    "quantity": 10
  }'
```

**Resposta (201 Created):**
```json
{
  "id": 1,
  "name": "Notebook Dell",
  "description": "Notebook Dell Inspiron 15 com 16GB RAM",
  "price": 3500.00,
  "quantity": 10,
  "categories": []
}
```

### Listar Produtos (GET)
```bash
curl http://localhost:5099/api/v1/products
```

**Resposta (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Notebook Dell",
    "description": "Notebook Dell Inspiron 15 com 16GB RAM",
    "price": 3500.00,
    "quantity": 10,
    "categories": []
  }
]
```

### Criar Categoria (POST)
```bash
curl -X POST http://localhost:5099/api/v1/categories \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Eletrônicos",
    "description": "Produtos eletrônicos diversos"
  }'
```

### Produtos com Categorias - INNER JOIN (GET)
```bash
curl http://localhost:5099/api/v1/products/with-categories
```

**Resposta (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Notebook Dell",
    "description": "Notebook Dell Inspiron 15 com 16GB RAM",
    "price": 3500.00,
    "quantity": 10,
    "categories": [
      {
        "id": 1,
        "name": "Eletrônicos",
        "description": "Produtos eletrônicos diversos"
      }
    ]
  }
]
```

### Atualizar Produto (PUT)
```bash
curl -X PUT http://localhost:5099/api/v1/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Notebook Dell - Atualizado",
    "description": "Notebook Dell Inspiron 15 com 32GB RAM",
    "price": 4000.00,
    "quantity": 8
  }'
```

### Deletar Produto (DELETE)
```bash
curl -X DELETE http://localhost:5099/api/v1/products/1
```

**Resposta (204 No Content)** - Sem corpo de resposta

-----

## ✅ Validações Implementadas

### DataAnnotations nos Models:

**Product:**
- `Name`: Required, 3-120 caracteres
- `Description`: Required, 5-500 caracteres  
- `Price`: Required, Range 0.01 - 999999.99
- `Quantity`: Required, Range 0 - 999999

**Category:**
- `Name`: Required, 3-120 caracteres, único
- `Description`: Opcional, máximo 500 caracteres

### Tratamento de Erros:
- **400 Bad Request**: Dados inválidos ou ausentes
- **404 Not Found**: Recurso não encontrado
- **409 Conflict**: Violação de unicidade (nome duplicado)
- **422 Unprocessable Entity**: Falha na validação do ModelState

-----

## 🖥️ Interface Gráfica (GUI)

A interface web oferece:

### Funcionalidades:
✅ **Aba Produtos**
- Criar, editar, listar e deletar produtos
- Formulário com validações em tempo real
- Tabela interativa com botões de ação

✅ **Aba Categorias**
- Criar, editar, listar e deletar categorias
- Interface simplificada e intuitiva

✅ **Aba Produtos com Categorias (INNER JOIN)**
- Visualização dos produtos com suas categorias associadas
- Demonstra o relacionamento N:N entre as entidades
- Cards visuais com informações completas

### Design:
- Interface moderna e responsiva
- Gradientes e animações suaves
- Notificações de sucesso/erro
- Totalmente funcional em dispositivos móveis

-----

## 🔧 Comandos Úteis

### Migrations (Entity Framework Core):
```bash
# Criar uma nova migration
dotnet ef migrations add NomeDaMigration --project BD/EstoqueDB.csproj

# Aplicar migrations ao banco de dados
dotnet ef database update --project BD/EstoqueDB.csproj

# Remover última migration
dotnet ef migrations remove --project BD/EstoqueDB.csproj
```

### Build e Execução:
```bash
# Compilar o projeto
dotnet build BD/EstoqueDB.csproj

# Executar o projeto
dotnet run --project BD/EstoqueDB.csproj

# Executar com watch (recarrega automaticamente)
dotnet watch run --project BD/EstoqueDB.csproj
```

### Publicar para Produção:
```bash
# Windows
dotnet publish -c Release --self-contained true -r win-x64 --project BD/EstoqueDB.csproj

# Linux
dotnet publish -c Release --self-contained true -r linux-x64 --project BD/EstoqueDB.csproj

# macOS
dotnet publish -c Release --self-contained true -r osx-x64 --project BD/EstoqueDB.csproj
```

-----

## 📚 Estrutura do Projeto

```
BD/
├── Controllers/          # Controllers da API
│   ├── ProductsController.cs
│   └── CategoriesController.cs
├── Data/                 # Contexto do banco de dados
│   └── AppDbContext.cs
├── Models/               # Entidades do domínio
│   ├── Product.cs
│   ├── Category.cs
│   └── ProductCategory.cs
├── Migrations/           # Migrations do EF Core
├── wwwroot/              # Arquivos estáticos (Frontend)
│   ├── index.html
│   ├── styles.css
│   └── app.js
└── Program.cs            # Configuração e inicialização
```

-----

## 🎯 Requisitos Atendidos

✅ API REST com persistência em banco de dados  
✅ CRUD completo (Create, Read, Update, Delete)  
✅ 2 entidades com relacionamento (N:N via JOIN)  
✅ Validações com DataAnnotations  
✅ Tratamento de erros HTTP (400, 404, 409, 422)  
✅ Interface Gráfica (GUI) moderna e responsiva  
✅ Documentação completa com Swagger  
✅ Migrations do Entity Framework Core  
✅ Demonstração de INNER JOIN (endpoint `/products/with-categories`)  

-----

## 📸 Screenshots

### Interface Web
Acesse http://localhost:5099 após executar o projeto para ver a interface completa.

### Swagger
Acesse http://localhost:5099/swagger para ver a documentação interativa da API.

-----

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para:
- Reportar bugs
- Sugerir novas funcionalidades
- Enviar pull requests

-----

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais como parte do curso de Ciências da Computação do CEUB.

-----

## 👨‍💻 Autor

**Claudiano Pinto de Oliveira Junior**  
Estudante de Ciências da Computação - CEUB  
GitHub: [@juniorrkj](https://github.com/juniorrkj)
