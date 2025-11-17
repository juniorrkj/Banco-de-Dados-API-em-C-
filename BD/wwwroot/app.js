// Use a URL da API do Railway se estiver em produção, senão usa localhost
const API_URL = window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1'
    ? 'http://localhost:5099/api/v1'
    : 'https://sistema-estoque-api-web-production.up.railway.app/api/v1';

// ============= UTILIDADES =============
function formatCurrency(value) {
    return value.toLocaleString('pt-BR', { 
        minimumFractionDigits: 2, 
        maximumFractionDigits: 2 
    });
}

function showNotification(message, type = 'success') {
    const notification = document.getElementById('notification');
    notification.textContent = message;
    notification.className = `notification ${type} show`;
    setTimeout(() => {
        notification.classList.remove('show');
    }, 3000);
}

function showTab(tabName) {
    // Esconder todas as tabs
    document.querySelectorAll('.tab-content').forEach(tab => {
        tab.classList.remove('active');
    });
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('active');
    });

    // Mostrar tab selecionada
    document.getElementById(`${tabName}-tab`).classList.add('active');
    event.target.classList.add('active');

    // Carregar dados
    if (tabName === 'products') loadProducts();
    if (tabName === 'categories') loadCategories();
    if (tabName === 'relations') loadProductsWithCategories();
}

// ============= PRODUTOS =============
async function loadProducts() {
    try {
        const response = await fetch(`${API_URL}/products`);
        const products = await response.json();
        
        const tbody = document.querySelector('#products-table tbody');
        tbody.innerHTML = '';

        products.forEach(product => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${product.id}</td>
                <td>${product.name}</td>
                <td>${product.description}</td>
                <td>R$ ${formatCurrency(product.price)}</td>
                <td>${product.quantity}</td>
                <td>
                    <button class="btn btn-edit" onclick="editProduct(${product.id})">✏️ Editar</button>
                    <button class="btn btn-delete" onclick="deleteProduct(${product.id})">🗑️ Deletar</button>
                    <button class="btn btn-primary" onclick="showAddCategoryModal(${product.id}, '${product.name}')">🏷️ Categoria</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        showNotification('Erro ao carregar produtos', 'error');
        console.error(error);
    }
}

async function editProduct(id) {
    try {
        const response = await fetch(`${API_URL}/products/${id}`);
        const product = await response.json();

        document.getElementById('product-id').value = product.id;
        document.getElementById('product-name').value = product.name;
        document.getElementById('product-description').value = product.description;
        document.getElementById('product-price').value = product.price;
        document.getElementById('product-quantity').value = product.quantity;

        document.querySelector('#product-form button[type="submit"]').textContent = 'Atualizar Produto';
    } catch (error) {
        showNotification('Erro ao carregar produto', 'error');
    }
}

async function deleteProduct(id) {
    if (!confirm('Tem certeza que deseja deletar este produto?')) return;

    try {
        const response = await fetch(`${API_URL}/products/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            showNotification('Produto deletado com sucesso!');
            loadProducts();
        } else {
            const error = await response.json();
            showNotification(error.error || 'Erro ao deletar produto', 'error');
        }
    } catch (error) {
        showNotification('Erro ao deletar produto', 'error');
    }
}

function clearProductForm() {
    document.getElementById('product-form').reset();
    document.getElementById('product-id').value = '';
    document.querySelector('#product-form button[type="submit"]').textContent = 'Salvar Produto';
}

document.getElementById('product-form').addEventListener('submit', async (e) => {
    e.preventDefault();

    const id = document.getElementById('product-id').value;
    const product = {
        name: document.getElementById('product-name').value,
        description: document.getElementById('product-description').value,
        price: parseFloat(document.getElementById('product-price').value),
        quantity: parseInt(document.getElementById('product-quantity').value)
    };

    try {
        const url = id ? `${API_URL}/products/${id}` : `${API_URL}/products`;
        const method = id ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(product)
        });

        if (response.ok) {
            showNotification(id ? 'Produto atualizado!' : 'Produto criado!');
            clearProductForm();
            loadProducts();
        } else {
            const error = await response.json();
            showNotification(error.error || 'Erro ao salvar produto', 'error');
        }
    } catch (error) {
        showNotification('Erro ao salvar produto', 'error');
        console.error(error);
    }
});

// ============= CATEGORIAS =============
async function loadCategories() {
    try {
        const response = await fetch(`${API_URL}/categories`);
        const categories = await response.json();
        
        const tbody = document.querySelector('#categories-table tbody');
        tbody.innerHTML = '';

        categories.forEach(category => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${category.id}</td>
                <td>${category.name}</td>
                <td>${category.description || '-'}</td>
                <td>
                    <button class="btn btn-edit" onclick="editCategory(${category.id})">✏️ Editar</button>
                    <button class="btn btn-delete" onclick="deleteCategory(${category.id})">🗑️ Deletar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        showNotification('Erro ao carregar categorias', 'error');
        console.error(error);
    }
}

async function editCategory(id) {
    try {
        const response = await fetch(`${API_URL}/categories/${id}`);
        const category = await response.json();

        document.getElementById('category-id').value = category.id;
        document.getElementById('category-name').value = category.name;
        document.getElementById('category-description').value = category.description || '';

        document.querySelector('#category-form button[type="submit"]').textContent = 'Atualizar Categoria';
    } catch (error) {
        showNotification('Erro ao carregar categoria', 'error');
    }
}

async function deleteCategory(id) {
    if (!confirm('Tem certeza que deseja deletar esta categoria?')) return;

    try {
        const response = await fetch(`${API_URL}/categories/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            showNotification('Categoria deletada com sucesso!');
            loadCategories();
        } else {
            const error = await response.json();
            showNotification(error.error || 'Erro ao deletar categoria', 'error');
        }
    } catch (error) {
        showNotification('Erro ao deletar categoria', 'error');
    }
}

function clearCategoryForm() {
    document.getElementById('category-form').reset();
    document.getElementById('category-id').value = '';
    document.querySelector('#category-form button[type="submit"]').textContent = 'Salvar Categoria';
}

document.getElementById('category-form').addEventListener('submit', async (e) => {
    e.preventDefault();

    const id = document.getElementById('category-id').value;
    const category = {
        name: document.getElementById('category-name').value,
        description: document.getElementById('category-description').value
    };

    try {
        const url = id ? `${API_URL}/categories/${id}` : `${API_URL}/categories`;
        const method = id ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(category)
        });

        if (response.ok) {
            showNotification(id ? 'Categoria atualizada!' : 'Categoria criada!');
            clearCategoryForm();
            loadCategories();
        } else {
            const error = await response.json();
            showNotification(error.error || 'Erro ao salvar categoria', 'error');
        }
    } catch (error) {
        showNotification('Erro ao salvar categoria', 'error');
        console.error(error);
    }
});

// ============= RELAÇÕES (INNER JOIN) =============
async function loadProductsWithCategories() {
    try {
        const response = await fetch(`${API_URL}/products/with-categories`);
        const products = await response.json();
        
        const container = document.getElementById('relations-list');
        container.innerHTML = '';

        if (products.length === 0) {
            container.innerHTML = '<p style="text-align: center; padding: 20px;">Nenhum produto encontrado.</p>';
            return;
        }

        products.forEach(product => {
            const card = document.createElement('div');
            card.className = 'relation-card';
            
            const categoriesHtml = product.categories.length > 0
                ? product.categories.map(cat => `<span class="category-badge">${cat.name}</span>`).join('')
                : '<p style="color: #999;">Sem categorias associadas</p>';

            card.innerHTML = `
                <h3>📦 ${product.name}</h3>
                <div class="product-info">
                    <div><strong>ID:</strong> ${product.id}</div>
                    <div><strong>Preço:</strong> R$ ${formatCurrency(product.price)}</div>
                    <div><strong>Quantidade:</strong> ${product.quantity}</div>
                </div>
                <div><strong>Descrição:</strong> ${product.description}</div>
                <div class="categories-list">
                    <strong>Categorias:</strong><br>
                    ${categoriesHtml}
                </div>
            `;
            container.appendChild(card);
        });
    } catch (error) {
        showNotification('Erro ao carregar relações', 'error');
        console.error(error);
    }
}

// Carregar produtos ao iniciar
window.addEventListener('DOMContentLoaded', () => {
    loadProducts();
});

// ============= ADICIONAR PRODUTO À CATEGORIA =============
async function showAddCategoryModal(productId, productName) {
    try {
        const response = await fetch(`${API_URL}/categories`);
        const categories = await response.json();

        if (categories.length === 0) {
            showNotification('Nenhuma categoria cadastrada. Crie uma categoria primeiro!', 'error');
            return;
        }

        const categoryOptions = categories.map(cat => 
            `<option value="${cat.id}">${cat.name}</option>`
        ).join('');

        const modalHtml = `
            <div id="category-modal" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); display: flex; align-items: center; justify-content: center; z-index: 1000;">
                <div style="background: white; padding: 30px; border-radius: 12px; max-width: 400px; width: 90%;">
                    <h3 style="margin-top: 0;">Adicionar ${productName} à Categoria</h3>
                    <select id="category-select" style="width: 100%; padding: 10px; margin: 15px 0; border: 1px solid #ddd; border-radius: 6px; font-size: 16px;">
                        <option value="">Selecione uma categoria</option>
                        ${categoryOptions}
                    </select>
                    <div style="display: flex; gap: 10px; margin-top: 20px;">
                        <button class="btn btn-primary" onclick="addProductToCategory(${productId})" style="flex: 1;">Adicionar</button>
                        <button class="btn btn-secondary" onclick="closeModal()" style="flex: 1;">Cancelar</button>
                    </div>
                </div>
            </div>
        `;

        document.body.insertAdjacentHTML('beforeend', modalHtml);
    } catch (error) {
        showNotification('Erro ao carregar categorias', 'error');
    }
}

async function addProductToCategory(productId) {
    const categoryId = document.getElementById('category-select').value;
    
    if (!categoryId) {
        showNotification('Selecione uma categoria!', 'error');
        return;
    }

    try {
        const response = await fetch(`${API_URL}/products/${productId}/categories/${categoryId}`, {
            method: 'POST'
        });

        if (response.ok) {
            showNotification('Produto adicionado à categoria com sucesso!');
            closeModal();
            loadProducts();
            loadProductsWithCategories();
        } else {
            const error = await response.json();
            showNotification(error.error || 'Erro ao adicionar à categoria', 'error');
        }
    } catch (error) {
        showNotification('Erro ao adicionar à categoria', 'error');
        console.error(error);
    }
}

function closeModal() {
    const modal = document.getElementById('category-modal');
    if (modal) modal.remove();
}
