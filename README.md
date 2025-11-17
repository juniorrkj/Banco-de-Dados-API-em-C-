# 📦 Sistema de Controle de Estoque

Sistema completo de gerenciamento de estoque com **API REST** em C# (.NET 8.0) e **interface web moderna**, totalmente funcional e hospedado na nuvem.

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
Projeto de Desenvolvimento de Sistemas | Versão 1.0 (NET 8.0)

---

## ✨ Funcionalidades Principais

### 🌐 Interface Web
- ✅ **3 abas interativas**: Produtos, Categorias e Relacionamentos
- ✅ **CRUD completo** via interface gráfica moderna
- ✅ **Responsiva**: funciona em desktop, tablet e mobile
- ✅ **Formatação brasileira**: valores em R$ (Real)
- ✅ **Validações em tempo real** nos formulários
- ✅ **Modal intuitivo** para vincular produtos a categorias

### 🔌 API REST
- ✅ **Endpoints RESTful** completos
- ✅ **Relacionamento N:N** entre Produtos e Categorias
- ✅ **INNER JOIN** endpoint (produtos com categorias)
- ✅ **Validações robustas** com DataAnnotations
- ✅ **Documentação automática** com Swagger
- ✅ **CORS habilitado**

---

## 🛠️ Tecnologias Utilizadas

**Backend:**
- C# / .NET 8.0 (ASP.NET Core)
- Entity Framework Core
- SQLite
- Swagger/OpenAPI

**Frontend:**
- HTML5, CSS3, JavaScript
- Interface responsiva
- Fetch API

**Deploy:**
- Docker
- Render.com
- GitHub (auto-deploy)

---

## 📁 Estrutura do Projeto

```
BD/
├── Controllers/          # Endpoints da API
├── Data/                 # Contexto do banco de dados
├── Models/               # Entidades (Product, Category, ProductCategory)
├── Migrations/           # Migrations do EF Core
├── wwwroot/              # Frontend (HTML, CSS, JS)
└── Program.cs            # Configuração da aplicação
```

---

## 🎯 Requisitos Implementados

✅ API REST com banco de dados  
✅ CRUD completo (Create, Read, Update, Delete)  
✅ Relacionamento N:N entre entidades  
✅ INNER JOIN (endpoint `/api/v1/products/with-categories`)  
✅ Validações com DataAnnotations  
✅ Interface Web moderna e responsiva  
✅ Documentação com Swagger  
✅ Deploy em produção  

---

## 📄 Licença

MIT License - Projeto acadêmico
