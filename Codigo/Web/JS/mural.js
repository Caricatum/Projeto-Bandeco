// =============================================
// CONFIGURAÇÃO
// =============================================
const API = 'http://localhost:8080';

const usuarioLogado = {
    nome: localStorage.getItem('nome') || '',
    username: localStorage.getItem('username') || '',
    tipo: localStorage.getItem('tipo'),    // string 'true' / 'false'
    id: parseInt(localStorage.getItem('id')) || null,
};

// =============================================
// INIT
// =============================================
document.addEventListener('DOMContentLoaded', async () => {

    // Esconde botão de login se já logado (esconde o item inteiro da navbar)
    if (sessionStorage.getItem('logado') === 'true') {
        const itemLogin = document.getElementById('itemLogin');
        if (itemLogin) itemLogin.style.display = 'none';
    }

    // Só funcionário vê botão de novo aviso e cardápio do dia
    if (usuarioLogado.tipo === 'true') {
        document.getElementById('btnNovoAviso').classList.remove('d-none');
        document.getElementById('btnCardapioDia').classList.remove('d-none');
    }

    // Carrega avisos do localStorage (ainda não há endpoint de notificações para isso)
    let avisos = JSON.parse(localStorage.getItem('avisos') || '[]');

    const mural = document.getElementById('mural');
    const modal = new bootstrap.Modal(document.getElementById('modalAviso'));
    const modalCardapioDia = new bootstrap.Modal(document.getElementById('modalCardapioDia'));
    const btnNovo = document.getElementById('btnNovoAviso');

    const btnNovoCardapioDoDia = document.getElementById('btnCardapioDia');

    // ---- Renderizar avisos ----
    function renderizarAvisos() {
        mural.innerHTML = '';

        if (!avisos.length) {
            mural.innerHTML = `
                <div class="col-12">
                    <div class="p-5 text-center rounded border bg-light-subtle">
                        <h4>📭 Por hoje não tem nada</h4>
                        <p class="text-muted mb-0">Nenhum aviso foi publicado até o momento.</p>
                    </div>
                </div>`;
            return;
        }

        avisos.forEach((aviso, index) => {
            mural.innerHTML += `
                <div class="col-md-6">
                    <div class="alert alert-${aviso.cor || 'primary'} shadow-sm">
                        <div class="d-flex justify-content-between">
                            <h5 class="fw-bold">${aviso.titulo}</h5>
                            ${usuarioLogado.tipo === 'true'
                    ? `<button class="btn btn-danger btn-sm" onclick="deletarAviso(${index})">✕</button>`
                    : ''}
                        </div>
                        <p class="mb-0">${aviso.descricao}</p>
                    </div>
                </div>`;
        });
    }

    // ---- Novo aviso ----
    if (btnNovo) {
        btnNovo.addEventListener('click', () => modal.show());
    }
    // ---- Novo Cardapio do dia ----
    if (btnNovoCardapioDoDia) {
        btnNovoCardapioDoDia.addEventListener('click', async () => {
            await carregarTodosPratos();
            popularSelectsDePratos();
            modalCardapioDia.show();
        });
    }

    const btnSalvarCardapioDia = document.getElementById('salvarCardapioDia');
    if (btnSalvarCardapioDia) {
        btnSalvarCardapioDia.addEventListener('click', salvarCardapioDia);
    }

    document.getElementById('salvarAviso').addEventListener('click', () => {
        const titulo = document.getElementById('tituloAviso').value.trim();
        const descricao = document.getElementById('descricaoAviso').value.trim();
        if (!titulo || !descricao) { alert('Preencha todos os campos!'); return; }

        avisos.push({ titulo, descricao, cor: 'primary' });
        localStorage.setItem('avisos', JSON.stringify(avisos));
        renderizarAvisos();
        document.getElementById('tituloAviso').value = '';
        document.getElementById('descricaoAviso').value = '';
        modal.hide();
    });

    window.deletarAviso = function (index) {
        avisos.splice(index, 1);
        localStorage.setItem('avisos', JSON.stringify(avisos));
        renderizarAvisos();
    };

    renderizarAvisos();

    // =============================================
    // CARDÁPIO DO DIA — via API real
    // =============================================
    await carregarCardapioDia();
});


// =============================================
// CARDÁPIO DO DIA
// =============================================
async function carregarCardapioDia() {
    const area = document.getElementById('areaCardapio');
    if (!area) return;

    try {
        const res = await fetch(`${API}/cardapioDia/all`);
        if (!res.ok) throw new Error('Erro na API');
        const todos = await res.json();

        // Pega o cardápio de hoje (data = hoje)
        const hoje = new Date().toISOString().split('T')[0]; // YYYY-MM-DD
        const cardapio = todos.find(c => c.data === hoje);

        if (!cardapio) {
            area.innerHTML = `
                <div class="col-12">
                    <div class="alert alert-info text-center">
                        📋 Cardápio de hoje ainda não foi cadastrado.
                    </div>
                </div>`;
            return;
        }

        renderizarCardapio(cardapio, area);

    } catch (e) {
        area.innerHTML = `
            <div class="col-12">
                <div class="alert alert-warning text-center">
                    ⚠️ Não foi possível carregar o cardápio. Verifique se a API está online.
                </div>
            </div>`;
    }
}

function renderizarCardapio(c, area) {
    const secoes = [
        { titulo: '🌞 Almoço', padrao: c.padraoAlmoco, vegano: c.veganoAlmoco, cor: 'primary' },
        { titulo: '🌙 Jantar', padrao: c.padraoJantar, vegano: c.veganoJantar, cor: 'dark' },
    ];

    area.innerHTML = secoes.map(s => `
        <div class="row mb-4">
            <div class="col-12">
                <h2 class="bg-${s.cor} text-white p-2 rounded">${s.titulo}</h2>
            </div>
            ${cardapioCard('Cardápio Padrão', s.padrao, false)}
            ${cardapioCard('Cardápio Vegano', s.vegano, true)}
        </div>
    `).join('');
}

function cardapioCard(titulo, cardapio, vegano) {
    if (!cardapio) {
        return `<div class="col-md-6"><div class="card h-100"><div class="card-body text-muted">Não cadastrado</div></div></div>`;
    }

    const campos = [
        ['Acompanhamento', cardapio.acompanhamento],
        ['Prato Principal', cardapio.prato_principal],
        ['Guarnição', cardapio.guarnicao],
        ['Salada', cardapio.salada],
        ['Sobremesa', cardapio.sobremesa],
        ['Refresco', cardapio.refresco],
        ['Nota Tecnica', cardapio.nota_tecnica]
    ];

    return `
        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header ${vegano ? 'bg-success text-white' : ''}">
                    <h4>${titulo}</h4>
                </div>
                <div class="card-body">
                    ${campos.map(([label, prato]) => prato
        ? `<p><strong>${label}:</strong> ${prato.nome}</p>`
        : ''
    ).join('')}
                </div>
            </div>
        </div>`;
}


// =============================================
// CADASTRAR CARDÁPIO DO DIA (4 cardápios + data)
// =============================================

let todosPratos = [];

async function carregarTodosPratos() {
    try {
        const res = await fetch(`${API}/pratos/all`);
        if (!res.ok) throw new Error('Erro ao buscar pratos');
        todosPratos = await res.json();
    } catch (e) {
        todosPratos = [];
    }
}

// Popula cada <select class="select-prato"> com os pratos da
// categoria correspondente, filtrando vegano quando necessário
function popularSelectsDePratos() {
    document.querySelectorAll('.select-prato').forEach(select => {
        const categoriaId = parseInt(select.dataset.categoria);
        const exigeVegano = select.dataset.vegano === 'true';

        select.innerHTML = '<option value="">Nenhum</option>';

        const pratosFiltrados = todosPratos.filter(p => {
            const mesmaCategoria = p.categoria && p.categoria.id === categoriaId;
            if (!mesmaCategoria) return false;
            if (exigeVegano && !p.vegano) return false;
            return true;
        });

        pratosFiltrados.forEach(p => {
            const opt = document.createElement('option');
            opt.value = p.id;
            opt.textContent = p.nome + (p.vegano ? ' 🥦' : '');
            select.appendChild(opt);
        });
    });
}

// Monta os 4 objetos Cardapio a partir dos selects, cadastra cada
// um em /cardapio/cadastrar e depois cadastra o CardapioDia com a
// data digitada e os 4 ids retornados.
async function salvarCardapioDia() {
    const msg = document.getElementById('msgCardapioDia');
    const btn = document.getElementById('salvarCardapioDia');
    const data = document.getElementById('dataCardapio').value;

    msg.textContent = '';
    msg.style.color = '';

    if (!data) {
        msg.textContent = '⚠️ Selecione a data do cardápio.';
        return;
    }

    // chave -> { vegano: bool, campos preenchidos com { id: ... } }
    const cardapios = {
        padraoAlmoco: { vegano: false },
        veganoAlmoco: { vegano: true },
        padraoJantar: { vegano: false },
        veganoJantar: { vegano: true },
    };

    document.querySelectorAll('.select-prato').forEach(select => {
        const cardapioKey = select.dataset.cardapio;
        const campo = select.dataset.campo;
        const valor = select.value;

        if (valor) {
            cardapios[cardapioKey][campo] = { id: parseInt(valor) };
        }
    });

    btn.disabled = true;
    btn.textContent = 'Salvando...';

    try {
        // 1) Cadastra cada um dos 4 cardápios em /cardapio/cadastrar
        const idsCardapios = {};

        for (const chave of Object.keys(cardapios)) {
            const res = await fetch(`${API}/cardapio/cadastrar`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(cardapios[chave])
            });

            if (!res.ok) {
                throw new Error(`Erro ao salvar cardápio "${chave}": ${await res.text()}`);
            }

            const cardapioSalvo = await res.json();
            idsCardapios[chave] = cardapioSalvo.id;
        }

        // 2) Cadastra o CardapioDia com a data + os 4 ids + usuário logado (obrigatório na API)
        if (!usuarioLogado.id) {
            throw new Error('Usuário não identificado. Faça login novamente.');
        }

        const cardapioDiaPayload = {
            data: data,
            padraoAlmoco: { id: idsCardapios.padraoAlmoco },
            veganoAlmoco: { id: idsCardapios.veganoAlmoco },
            padraoJantar: { id: idsCardapios.padraoJantar },
            veganoJantar: { id: idsCardapios.veganoJantar },
            user: { id: usuarioLogado.id },
        };

        const resDia = await fetch(`${API}/cardapioDia/cadastrar`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(cardapioDiaPayload)
        });

        if (!resDia.ok) {
            const texto = await resDia.text();
            if (resDia.status === 400 && texto.includes('Já existe')) {
                throw new Error('Já existe um cardápio cadastrado para esta data.');
            }
            throw new Error(texto || 'Erro ao salvar o cardápio do dia.');
        }

        msg.style.color = 'green';
        msg.textContent = '✅ Cardápio do dia salvo com sucesso!';

        // Atualiza a área de cardápio do dia se a data salva for hoje
        await carregarCardapioDia();

        setTimeout(() => {
            const modalEl = document.getElementById('modalCardapioDia');
            bootstrap.Modal.getInstance(modalEl).hide();
            msg.textContent = '';
            msg.style.color = '';
            document.getElementById('dataCardapio').value = '';
        }, 1200);

    } catch (e) {
        msg.style.color = '';
        msg.textContent = '❌ ' + e.message;
    } finally {
        btn.disabled = false;
        btn.textContent = 'Salvar';
    }
}