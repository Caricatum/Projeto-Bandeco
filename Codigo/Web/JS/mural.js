// =============================================
// CONFIGURAÇÃO
// =============================================
const API = typeof API_BASE_URL !== 'undefined' ? API_BASE_URL : 'http://localhost:8080';


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
        const btnNovoAviso = document.getElementById('btnNovoAviso');
        const btnCardapioDia = document.getElementById('btnCardapioDia');
        const btnNotificar = document.getElementById('btnNotificarFavoritos');

        if (btnNovoAviso) btnNovoAviso.classList.remove('d-none');
        if (btnCardapioDia) btnCardapioDia.classList.remove('d-none');
        if (btnNotificar) btnNotificar.classList.remove('d-none');
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
    // CARDÁPIO DO DIA E DA SEMANA
    // =============================================
    inicializarSeletorData();
    await carregarCardapioDia();
});


// =============================================
// GERENCIAMENTO DE DATAS
// =============================================
let dataSelecionada = obterDataHojeLocal();

function obterDataHojeLocal() {
    const agora = new Date();
    const ano = agora.getFullYear();
    const mes = String(agora.getMonth() + 1).padStart(2, '0');
    const dia = String(agora.getDate()).padStart(2, '0');
    return `${ano}-${mes}-${dia}`;
}

function inicializarSeletorData() {
    const seletor = document.getElementById('seletorDataCardapio');
    if (seletor) {
        seletor.value = dataSelecionada;
    }
}

window.aoMudarSeletorData = function(novaData) {
    if (!novaData) return;
    dataSelecionada = novaData;
    carregarCardapioDia(dataSelecionada);
};

window.mudarDataRelativa = function(offsetDias) {
    const partes = dataSelecionada.split('-').map(Number);
    const d = new Date(partes[0], partes[1] - 1, partes[2]);
    d.setDate(d.getDate() + offsetDias);

    const ano = d.getFullYear();
    const mes = String(d.getMonth() + 1).padStart(2, '0');
    const dia = String(d.getDate()).padStart(2, '0');

    dataSelecionada = `${ano}-${mes}-${dia}`;
    const seletor = document.getElementById('seletorDataCardapio');
    if (seletor) seletor.value = dataSelecionada;

    carregarCardapioDia(dataSelecionada);
};

window.irParaHoje = function() {
    dataSelecionada = obterDataHojeLocal();
    const seletor = document.getElementById('seletorDataCardapio');
    if (seletor) seletor.value = dataSelecionada;
    carregarCardapioDia(dataSelecionada);
};

// =============================================
// CARDÁPIO DO DIA (POR DATA)
// =============================================
async function carregarCardapioDia(dataFiltro = null) {
    const area = document.getElementById('areaCardapio');
    if (!area) return;

    const dataAlvo = dataFiltro || dataSelecionada || obterDataHojeLocal();

    area.innerHTML = `
        <div class="col-12 text-center py-5">
            <div class="spinner-border text-primary"></div>
            <p class="mt-2 text-muted">Carregando cardápio...</p>
        </div>`;

    try {
        // Tenta buscar diretamente pela rota de data
        let cardapio = null;
        const resData = await fetch(`${API}/cardapioDia/data/${dataAlvo}`);
        if (resData.ok) {
            cardapio = await resData.json();
        } else {
            // Fallback de busca em /cardapioDia/all
            const resAll = await fetch(`${API}/cardapioDia/all`);
            if (resAll.ok) {
                const todos = await resAll.json();
                cardapio = todos.find(c => {
                    if (!c || !c.data) return false;
                    const dataStr = typeof c.data === 'string' ? c.data.split('T')[0] : String(c.data);
                    return dataStr === dataAlvo;
                });
            }
        }

        if (!cardapio) {
            const dataFormatada = dataAlvo.split('-').reverse().join('/');
            const ehHoje = (dataAlvo === obterDataHojeLocal());
            area.innerHTML = `
                <div class="col-12">
                    <div class="alert alert-info text-center py-4">
                        <h5 class="mb-1">📋 Nenhum cardápio cadastrado para ${ehHoje ? 'hoje' : 'este dia'} (${dataFormatada}).</h5>
                        <p class="text-muted mb-0">Selecione outra data nos controles acima ou consulte a aba <strong>Cardápio da Semana</strong>.</p>
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

// =============================================
// CARDÁPIO DA SEMANA
// =============================================
window.carregarCardapioSemana = async function() {
    const area = document.getElementById('areaCardapioSemana');
    if (!area) return;

    area.innerHTML = `
        <div class="col-12 text-center py-5">
            <div class="spinner-border text-primary"></div>
            <p class="mt-2 text-muted">Carregando cardápios da semana...</p>
        </div>`;

    try {
        let cardapios = [];
        const res = await fetch(`${API}/cardapioDia/semana`);
        if (res.ok) {
            cardapios = await res.json();
        } else {
            const resAll = await fetch(`${API}/cardapioDia/all`);
            if (resAll.ok) cardapios = await resAll.json();
        }

        if (!cardapios || cardapios.length === 0) {
            area.innerHTML = `
                <div class="col-12">
                    <div class="alert alert-info text-center py-5">
                        <h4>🗓️ Nenhum cardápio cadastrado para esta semana</h4>
                        <p class="text-muted mb-0">Os cardápios da semana serão publicados em breve.</p>
                    </div>
                </div>`;
            return;
        }

        cardapios.sort((a, b) => {
            const dataA = typeof a.data === 'string' ? a.data : '';
            const dataB = typeof b.data === 'string' ? b.data : '';
            return dataA.localeCompare(dataB);
        });

        const nomesDias = ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'];

        area.innerHTML = cardapios.map(c => {
            const dataStr = typeof c.data === 'string' ? c.data.split('T')[0] : String(c.data);
            const [ano, mes, dia] = dataStr.split('-').map(Number);
            const dataObj = new Date(ano, mes - 1, dia);
            const diaSemana = nomesDias[dataObj.getDay()] || '';
            const dataFormatada = dataStr.split('-').reverse().join('/');
            const ehHoje = (dataStr === obterDataHojeLocal());

            return `
                <div class="col-12 mb-3">
                    <div class="card shadow-sm ${ehHoje ? 'border-danger border-2' : ''}">
                        <div class="card-header d-flex justify-content-between align-items-center ${ehHoje ? 'bg-danger text-white' : ''}">
                            <h4 class="mb-0 fs-5">📅 ${diaSemana} - ${dataFormatada}</h4>
                            ${ehHoje ? '<span class="badge bg-warning text-dark fw-bold">HOJE</span>' : ''}
                        </div>
                        <div class="card-body">
                            <div class="row g-3">
                                <div class="col-lg-6">
                                    <h5 class="text-primary border-bottom pb-2">🌞 Almoço</h5>
                                    <div class="row g-2">
                                        ${cardapioCard('Padrão', c.padraoAlmoco, false)}
                                        ${cardapioCard('Vegano', c.veganoAlmoco, true)}
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <h5 class="text-dark border-bottom pb-2">🌙 Jantar</h5>
                                    <div class="row g-2">
                                        ${cardapioCard('Padrão', c.padraoJantar, false)}
                                        ${cardapioCard('Vegano', c.veganoJantar, true)}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        }).join('');

    } catch (e) {
        area.innerHTML = `
            <div class="col-12">
                <div class="alert alert-warning text-center">
                    ⚠️ Não foi possível carregar os cardápios da semana. Verifique se a API está online.
                </div>
            </div>`;
    }
};

// =============================================
// DISPARO DE NOTIFICAÇÃO DE PRATOS FAVORITOS
// =============================================
window.notificarFavoritosHoje = async function() {
    const btn = document.getElementById('btnNotificarFavoritos');
    if (!confirm('Deseja disparar as notificações por e-mail para todos os usuários com pratos favoritos no cardápio de hoje?')) {
        return;
    }

    if (btn) {
        btn.disabled = true;
        btn.textContent = 'Enviando notificações...';
    }

    try {
        const res = await fetch(`${API}/pratosFavoritos/notificar`, {
            method: 'POST'
        });

        if (res.ok) {
            alert('✅ Notificações enviadas com sucesso para os usuários!');
        } else {
            const erro = await res.text();
            alert('⚠️ ' + (erro || 'Não foi possível enviar as notificações.'));
        }
    } catch (e) {
        alert('❌ Erro de conexão com a API: ' + e.message);
    } finally {
        if (btn) {
            btn.disabled = false;
            btn.textContent = '🔔 Notificar Pratos Favoritos por E-mail';
        }
    }
};

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
        ['Prato Principal', cardapio.prato_principal || cardapio.pratoPrincipal],
        ['Guarnição', cardapio.guarnicao],
        ['Salada', cardapio.salada],
        ['Sobremesa', cardapio.sobremesa],
        ['Refresco', cardapio.refresco]
    ];

    return `
        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header ${vegano ? 'bg-success text-white' : ''}">
                    <h4>${titulo}</h4>
                </div>
                <div class="card-body">
                    ${campos.map(([label, prato]) => prato
        ? `<p><strong>${label}:</strong> <a href="pratoDet.php?id=${prato.id}" style="color:#D92243;font-weight:600;text-decoration:none;" onmouseover="this.style.textDecoration='underline'" onmouseout="this.style.textDecoration='none'">${prato.nome}</a></p>`
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