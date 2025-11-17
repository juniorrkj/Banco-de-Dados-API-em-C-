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

## 🤝 Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para:

1. Fazer um fork do projeto
2. Criar uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abrir um Pull Request

---

## 📄 Licença

Este projeto está sob a licença **MIT License**.

```
MIT License

Copyright (c) 2025 Claudiano Pinto de Oliveira Junior

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

<div align="center">

**Desenvolvido com ❤️ por Claudiano Pinto de Oliveira Junior**

⭐ Se este projeto te ajudou, considere dar uma estrela!

</div>
