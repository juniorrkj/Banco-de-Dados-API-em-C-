# 📦 Sistema de Controle de Estoque

Sistema completo de gerenciamento de estoque com **API REST** em C# (.NET 8.0), **autenticação de usuários** e **interface web moderna**, totalmente funcional e hospedado na nuvem.

## 🌐 Acesse o Projeto Online

<div align="center">

### 🚀 **[ACESSAR APLICAÇÃO](https://sistema-estoque-api-web.onrender.com)** 🚀

### 📚 **[DOCUMENTAÇÃO DA API (Swagger)](https://sistema-estoque-api-web.onrender.com/swagger)** 📚

**Status**: ✅ Online e funcional | **Deploy**: Render.com

</div>

---

## 👤 Desenvolvido por

**Claudiano Pinto de Oliveira Junior**  
Estudante de Ciências da Computação - CEUB  
Projeto de Desenvolvimento de Sistemas | Versão 2.0 (NET 8.0)

---

## ✨ Funcionalidades Principais

### 🔐 Sistema de Autenticação
- ✅ **Login e Registro** com interface em abas separadas
- ✅ **Criptografia SHA256** para senhas
- ✅ **Multi-usuário**: cada usuário tem seus próprios dados isolados
- ✅ **Validação de usuários duplicados**
- ✅ **Sessão persistente** com localStorage

### 🌐 Interface Web
- ✅ **3 abas interativas**: Produtos, Categorias e Relacionamentos
- ✅ **CRUD completo** via interface gráfica moderna
- ✅ **Responsiva**: funciona em desktop, tablet e mobile
- ✅ **Formatação brasileira**: valores em R$ 2.000,00
- ✅ **Loading indicators** para feedback visual
- ✅ **Validações em tempo real** nos formulários
- ✅ **Modal intuitivo** para vincular produtos a categorias

### 🔌 API REST
- ✅ **Endpoints RESTful** completos
- ✅ **Autenticação via headers** (X-User-Id)
- ✅ **Relacionamento N:N** entre Produtos e Categorias
- ✅ **INNER JOIN** endpoint (produtos com categorias)
- ✅ **Filtros por usuário** em todas as queries
- ✅ **Validações robustas** com DataAnnotations
- ✅ **Documentação automática** com Swagger
- ✅ **CORS habilitado**
- ✅ **Endpoint Admin** protegido por senha (visualizar usuários)

---

## 🛠️ Tecnologias Utilizadas

**Backend:**
- C# / .NET 8.0 (ASP.NET Core)
- Entity Framework Core 8.0
- SQLite (desenvolvimento e produção)
- SHA256 para criptografia de senhas
- Swagger/OpenAPI

**Frontend:**
- HTML5, CSS3, JavaScript (Vanilla)
- Interface responsiva com gradientes modernos
- Fetch API para comunicação com backend
- LocalStorage para gestão de sessão

**Deploy:**
- Docker
- Render.com (auto-deploy)
- GitHub Actions

---

## 📁 Estrutura do Projeto

```
BD/
├── Controllers/
│   ├── ProductsController.cs      # CRUD de produtos
│   ├── CategoriesController.cs    # CRUD de categorias
│   └── AuthController.cs          # Login, registro e admin
├── Data/
│   └── AppDbContext.cs            # Contexto EF Core
├── Models/
│   ├── User.cs                    # Modelo de usuário
│   ├── Product.cs                 # Modelo de produto
│   ├── Category.cs                # Modelo de categoria
│   └── ProductCategory.cs         # Relacionamento N:N
├── wwwroot/
│   ├── index.html                 # Dashboard principal
│   ├── auth.html                  # Login/Registro
│   ├── app.js                     # Lógica do frontend
│   └── styles.css                 # Estilos
└── Program.cs                     # Configuração da API
```

---

## 🎯 Requisitos Implementados

✅ API REST com banco de dados relacional  
✅ Sistema de autenticação multi-usuário  
✅ CRUD completo (Create, Read, Update, Delete)  
✅ Relacionamento N:N entre entidades  
✅ Isolamento de dados por usuário (UserId)  
✅ INNER JOIN (endpoint `/api/v1/products/with-categories`)  
✅ Validações com DataAnnotations  
✅ Criptografia de senhas (SHA256)  
✅ Interface Web moderna e responsiva  
✅ Formatação monetária brasileira  
✅ Loading indicators  
✅ Documentação com Swagger  
✅ Deploy em produção  
✅ Endpoint Admin protegido  

---

## 🔐 Endpoints da API

### Autenticação
- `POST /api/v1/auth/register` - Criar nova conta
- `POST /api/v1/auth/login` - Fazer login
- `GET /api/v1/auth/admin/users?secret=SENHA` - Listar usuários (admin)

### Produtos
- `GET /api/v1/products` - Listar produtos do usuário
- `GET /api/v1/products/{id}` - Buscar produto específico
- `POST /api/v1/products` - Criar produto
- `PUT /api/v1/products/{id}` - Atualizar produto
- `DELETE /api/v1/products/{id}` - Deletar produto
- `GET /api/v1/products/with-categories` - Produtos com suas categorias (INNER JOIN)

### Categorias
- `GET /api/v1/categories` - Listar categorias do usuário
- `GET /api/v1/categories/{id}` - Buscar categoria específica
- `POST /api/v1/categories` - Criar categoria
- `PUT /api/v1/categories/{id}` - Atualizar categoria
- `DELETE /api/v1/categories/{id}` - Deletar categoria

### Relacionamentos
- `POST /api/v1/products/{productId}/categories/{categoryId}` - Vincular produto a categoria
- `DELETE /api/v1/products/{productId}/categories/{categoryId}` - Desvincular

---

## 🚀 Como Usar

### Online (Recomendado)
1. Acesse https://sistema-estoque-api-web.onrender.com
2. Crie uma conta na aba "Criar Conta"
3. Faça login
4. Comece a gerenciar seus produtos e categorias!

### Local
```bash
# Clone o repositório
git clone https://github.com/juniorrkj/Sistema-Estoque-API-Web.git

# Entre na pasta do projeto
cd Sistema-Estoque-API-Web/BD

# Restaure as dependências
dotnet restore

# Execute o projeto
dotnet run

# Acesse no navegador
http://localhost:8080
```

---

## 🤝 Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para:

1. Fazer um fork do projeto
2. Criar uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abrir um Pull Request

---

## 📄 Licença

Este projeto está sob a licença **MIT License** - Projeto acadêmico.

---

<div align="center">

**Desenvolvido por Claudiano Pinto de Oliveira Junior**

⭐ Se este projeto te ajudou, considere dar uma estrela!

</div>
