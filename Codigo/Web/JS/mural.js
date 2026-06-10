// =============================================
// CONFIGURAÇÃO
// =============================================
const API = 'http://localhost:8080';

const usuarioLogado = {
    nome:     localStorage.getItem('nome')     || '',
    username: localStorage.getItem('username') || '',
    tipo:     localStorage.getItem('tipo'),    // string 'true' / 'false'
    id:       parseInt(localStorage.getItem('id')) || null,
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

    const mural    = document.getElementById('mural');
    const modal    = new bootstrap.Modal(document.getElementById('modalAviso'));
    const modalCardapioDia = new bootstrap.Modal(document.getElementById('modalCardapioDia'));
    const btnNovo  = document.getElementById('btnNovoAviso');
    
    const btnNovoCardapioDoDia  = document.getElementById('btnCardapioDia');

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
        btnNovoCardapioDoDia.addEventListener('click', () => modalCardapioDia.show());
    }

    document.getElementById('salvarAviso').addEventListener('click', () => {
        const titulo    = document.getElementById('tituloAviso').value.trim();
        const descricao = document.getElementById('descricaoAviso').value.trim();
        if (!titulo || !descricao) { alert('Preencha todos os campos!'); return; }

        avisos.push({ titulo, descricao, cor: 'primary' });
        localStorage.setItem('avisos', JSON.stringify(avisos));
        renderizarAvisos();
        document.getElementById('tituloAviso').value = '';
        document.getElementById('descricaoAviso').value = '';
        modal.hide();
    });

    window.deletarAviso = function(index) {
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
        const hoje     = new Date().toISOString().split('T')[0]; // YYYY-MM-DD
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
        { titulo: '🌞 Almoço',  padrao: c.padraoAlmoco,  vegano: c.veganoAlmoco,  cor: 'primary' },
        { titulo: '🌙 Jantar',  padrao: c.padraoJantar,  vegano: c.veganoJantar,  cor: 'dark'    },
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
        ['Guarnição',       cardapio.guarnicao],
        ['Salada',          cardapio.salada],
        ['Sobremesa',       cardapio.sobremesa],
        ['Refresco',        cardapio.refresco],
        ['Nota Tecnica',    cardapio.nota_tecnica]
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
