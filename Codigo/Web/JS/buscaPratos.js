import { validaFunc } from './validaFunc.js';

validaFunc();

// =============================================
// CONFIGURAÇÃO
// =============================================
const API = 'http://localhost:8080';

const userId = parseInt(localStorage.getItem('id')) || null;
const username = localStorage.getItem('username') || '';
const isFuncionario = localStorage.getItem('tipo') === 'true';

// Redireciona se não logado
if (sessionStorage.getItem('logado') !== 'true') {
    window.location.href = 'login.php';
}

// =============================================
// ESTADO LOCAL
// =============================================
let todosPratos = [];
let todasAvaliacoes = [];
let meusFavoritos = []; // lista de { id, prato: { id } }

// =============================================
// INIT
// =============================================
document.addEventListener('DOMContentLoaded', async () => {
    if (isFuncionario) {
        document.getElementById('btnNovoPrato').classList.remove('d-none');
    }

    await Promise.all([
        carregarPratos(),
        carregarAvaliacoes(),
        carregarFavoritos(),
        carregarCategorias(),
    ]);

    renderizarPratos();
    configurarFiltros();
    configurarModalAvaliar();
    configurarModalFavoritar();
});

const btnNovoPrato = document.getElementById('btnNovoPrato');
const modalNovoPrato = new bootstrap.Modal(document.getElementById('modalNovoPrato'));

// ---- Novo Cardapio do dia ----
if (btnNovoPrato) {
    btnNovoPrato.addEventListener('click', () => modalNovoPrato.show());
}

// =============================================
// BUSCAR DADOS DA API
// =============================================
async function carregarPratos() {
    try {
        const res = await fetch(`${API}/pratos/all`);
        todosPratos = await res.json();
    } catch (e) {
        console.error('Erro ao carregar pratos:', e);
        todosPratos = [];
    }
}

async function carregarAvaliacoes() {
    try {
        const res = await fetch(`${API}/avaliacoes/all`);
        todasAvaliacoes = await res.json();
    } catch (e) {
        todasAvaliacoes = [];
    }
}

async function carregarFavoritos() {
    if (!userId) return;
    try {
        const res = await fetch(`${API}/pratosFavoritos/all`);
        const todos = await res.json();
        // filtra só os do usuário logado
        meusFavoritos = todos.filter(f => f.user && f.user.id === userId);
    } catch (e) {
        meusFavoritos = [];
    }
}

async function carregarCategorias() {
    try {
        const res = await fetch(`${API}/categoria/all`);
        const cats = await res.json();
        const sel = document.getElementById('filtroCategoria');
        cats.forEach(c => {
            const opt = document.createElement('option');
            opt.value = c.id;
            opt.textContent = c.descricao || `Categoria ${c.id}`;
            sel.appendChild(opt);
        });
    } catch (e) { /* silencioso */ }
}

// =============================================
// HELPERS
// =============================================
function mediaNotas(pratoId) {
    const avs = todasAvaliacoes.filter(a => a.prato && a.prato.id === pratoId);
    if (!avs.length) return null;
    return (avs.reduce((s, a) => s + a.nota, 0) / avs.length).toFixed(1);
}

function totalLikes(pratoId) {
    // "like" = avaliações com nota >= 4
    return todasAvaliacoes.filter(a => a.prato && a.prato.id === pratoId && a.nota >= 4).length;
}

function euJaAvalieiEsre(pratoId) {
    return todasAvaliacoes.some(a =>
        a.prato && a.prato.id === pratoId &&
        a.user && a.user.id === userId
    );
}

function euJaFavoriteeiEsse(pratoId) {
    return meusFavoritos.some(f => f.prato && f.prato.id === pratoId);
}

function idFavoritoDoPrato(pratoId) {
    const fav = meusFavoritos.find(f => f.prato && f.prato.id === pratoId);
    return fav ? fav.id : null;
}

function estrelasPorNota(nota) {
    if (!nota) return '—';
    const n = Math.round(parseFloat(nota));
    return '★'.repeat(n) + '☆'.repeat(5 - n);
}

// =============================================
// RENDERIZAR CARDS
// =============================================
function pratosVisiveis() {
    const busca = document.getElementById('campoBusca').value.toLowerCase().trim();
    const catId = document.getElementById('filtroCategoria').value;
    const vegano = document.getElementById('filtroVegano').checked;

    return todosPratos.filter(p => {
        if (busca && !p.nome.toLowerCase().includes(busca)) return false;
        if (catId && p.categoria && String(p.categoria.id) !== catId) return false;
        if (vegano && !p.vegano) return false;
        return true;
    });
}

function renderizarPratos() {
    const area = document.getElementById('areaPratos');
    const lista = pratosVisiveis();

    if (!lista.length) {
        area.innerHTML = `
            <div class="col-12">
                <div class="p-5 text-center rounded border bg-white">
                    <h5>😕 Nenhum prato encontrado</h5>
                    <p class="text-muted mb-0">Tente outros filtros.</p>
                </div>
            </div>`;
        return;
    }

    area.innerHTML = lista.map(p => {
        const media = mediaNotas(p.id);
        const likes = totalLikes(p.id);
        const jaAvaliou = euJaAvalieiEsre(p.id);
        const jaFavoritou = euJaFavoriteeiEsse(p.id);
        const catNome = p.categoria ? (p.categoria.nome || `Cat. ${p.categoria.id}`) : '—';

        return `
        <div class="col-md-6 col-lg-4">
            <div class="card card-prato h-100">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-start mb-2">
                        <h5 class="card-title mb-0">${p.nome}</h5>
                        <div class="d-flex gap-1">
                            ${p.vegano ? '<span class="badge badge-vegano text-white">🥦 Vegano</span>' : ''}
                            <span class="badge badge-categoria text-white">${catNome}</span>
                        </div>
                    </div>

                    <p class="card-text text-muted small">${p.descricao}</p>

                    <div class="d-flex align-items-center gap-2 mb-3 flex-wrap">
                        <span class="text-warning fw-bold" title="Média das avaliações">
                            ${media ? `${estrelasPorNota(media)} ${media}` : '☆ Sem avaliações'}
                        </span>
                        <span class="text-muted small">${likes} 👍</span>
                    </div>

                    <!-- AÇÕES -->
                    <div class="d-flex gap-2 flex-wrap">

                        <!-- LIKE / AVALIAR -->
                        <button
                            class="btn-like ${jaAvaliou ? 'ativo' : ''}"
                            ${jaAvaliou ? 'disabled title="Você já avaliou"' : ''}
                            onclick="abrirModalAvaliar(${p.id}, '${p.nome.replace(/'/g, "\\'")}')">
                            👍 ${jaAvaliou ? 'Avaliado' : 'Curtir/Avaliar'}
                        </button>

                        <!-- FAVORITAR -->
                        <button
                            class="btn-fav ${jaFavoritou ? 'ativo' : ''}"
                            id="btnFav-${p.id}"
                            onclick="${jaFavoritou
                ? `desfavoritar(${p.id}, ${idFavoritoDoPrato(p.id)})`
                : `abrirModalFavoritar(${p.id}, '${p.nome.replace(/'/g, "\\'")}')`
            }">
                            ${jaFavoritou ? '⭐ Favoritado' : '☆ Favoritar'}
                        </button>

                    </div>
                </div>
            </div>
        </div>`;
    }).join('');
}

// =============================================
// FILTROS
// =============================================
function configurarFiltros() {
    document.getElementById('campoBusca').addEventListener('input', renderizarPratos);
    document.getElementById('filtroCategoria').addEventListener('change', renderizarPratos);
    document.getElementById('filtroVegano').addEventListener('change', renderizarPratos);
}

function limparFiltros() {
    document.getElementById('campoBusca').value = '';
    document.getElementById('filtroCategoria').value = '';
    document.getElementById('filtroVegano').checked = false;
    renderizarPratos();
}

// =============================================
// MODAL AVALIAR (LIKE / NOTA)
// =============================================
function abrirModalAvaliar(pratoId, nomePrato) {
    document.getElementById('pratoIdAvaliar').value = pratoId;
    document.getElementById('nomePratoAvaliar').textContent = nomePrato;
    document.getElementById('notaSelecionada').value = 0;
    document.getElementById('comentarioAvaliar').value = '';
    document.getElementById('msgAvaliar').textContent = '';
    // reset estrelas
    document.querySelectorAll('.estrela').forEach(e => e.classList.remove('selecionada'));
    new bootstrap.Modal(document.getElementById('modalAvaliar')).show();
}

function configurarModalAvaliar() {
    // Estrelas clicáveis
    document.querySelectorAll('.estrela').forEach(el => {
        el.addEventListener('click', () => {
            const v = parseInt(el.dataset.v);
            document.getElementById('notaSelecionada').value = v;
            document.querySelectorAll('.estrela').forEach(e => {
                e.classList.toggle('selecionada', parseInt(e.dataset.v) <= v);
            });
        });
    });

    document.getElementById('btnSalvarAvaliacao').addEventListener('click', async () => {
        const pratoId = parseInt(document.getElementById('pratoIdAvaliar').value);
        const nota = parseInt(document.getElementById('notaSelecionada').value);
        const coment = document.getElementById('comentarioAvaliar').value.trim();
        const msg = document.getElementById('msgAvaliar');

        if (!nota) { msg.textContent = 'Selecione uma nota!'; return; }

        try {
            const res = await fetch(`${API}/avaliacoes/cadastrar`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    nota,
                    avaliacao: coment || null,
                    prato: { id: pratoId },
                    user: { id: userId }
                })
            });

            if (!res.ok) throw new Error(await res.text());

            // Atualiza estado local
            await carregarAvaliacoes();
            renderizarPratos();
            bootstrap.Modal.getInstance(document.getElementById('modalAvaliar')).hide();

        } catch (e) {
            msg.textContent = 'Erro ao salvar avaliação: ' + e.message;
        }
    });
}

// =============================================
// MODAL FAVORITAR + NOTIFICAÇÃO
// =============================================
function abrirModalFavoritar(pratoId, nomePrato) {
    document.getElementById('pratoIdFavoritar').value = pratoId;
    document.getElementById('nomePratoFavoritar').textContent = nomePrato;
    document.getElementById('msgFavoritar').textContent = '';
    document.getElementById('ativarNotificacao').checked = true;
    new bootstrap.Modal(document.getElementById('modalFavoritar')).show();
}

function configurarModalFavoritar() {
    document.getElementById('btnConfirmarFavorito').addEventListener('click', async () => {
        const pratoId = parseInt(document.getElementById('pratoIdFavoritar').value);
        const notificar = document.getElementById('ativarNotificacao').checked;
        const msg = document.getElementById('msgFavoritar');

        try {
            // 1. Cadastra favorito
            const res = await fetch(`${API}/pratosFavoritos/cadastrar`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    prato: { id: pratoId },
                    user: { id: userId }
                })
            });

            if (!res.ok) throw new Error(await res.text());

            // 2. Se marcou notificação, cadastra notificação na API
            if (notificar) {
                const nomePrato = document.getElementById('nomePratoFavoritar').textContent;
                await fetch(`${API}/notificacoes/cadastrar`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        mensagem: `Seu prato favorito "${nomePrato}" está no cardápio de hoje! 🍽️`,
                        user: { id: userId }
                    })
                });
            }

            // 3. Atualiza estado local e fecha modal
            await carregarFavoritos();
            renderizarPratos();
            bootstrap.Modal.getInstance(document.getElementById('modalFavoritar')).hide();

        } catch (e) {
            msg.textContent = 'Erro ao favoritar: ' + e.message;
        }
    });
}

// =============================================
// DESFAVORITAR
// =============================================
async function desfavoritar(pratoId, favId) {
    if (!favId) return;
    if (!confirm('Remover dos favoritos?')) return;

    try {
        const res = await fetch(`${API}/pratosFavoritos/deletar/${favId}`, { method: 'DELETE' });
        if (!res.ok) throw new Error(await res.text());
        await carregarFavoritos();
        renderizarPratos();
    } catch (e) {
        alert('Erro ao remover favorito: ' + e.message);
    }
}

// =============================================
// CADASTRAR PRATO
// =============================================
document.getElementById("salvarNovoPrato").addEventListener("click",function (e) {
    e.preventDefault();

    // Pegando o que foi digitado
    const nomeDigitado = document.getElementById('nomePrato').value;
    const descDigitado = document.getElementById('descPrato').value;
    const categoriaDigitado = document.getElementById('categoriaPrato').value;
    const veganoDigitado = document.getElementById('veganoPrato').value;
    const imagemDigitado = document.getElementById('imagemPrato').value;

    let vegano = null;

    if (veganoDigitado === "true") {
        vegano = true;
    }else{
        vegano = false;
    }
    console.log("Vegano digitado: " + vegano);

    console.log("Variaveis feitas")

    const url = `${API}/pratos/cadastrar`;

    const prato = {
        nome: nomeDigitado,
        descricao: descDigitado,
        vegano: vegano,
        imagem: imagemDigitado,
        categoria: {"id": parseInt(categoriaDigitado)},
    };
    const jsonPrato = JSON.stringify(prato);

    
    
    console.log("Passou no Fetch de categoria"),

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: jsonPrato
    })
        .then(res => {
            if (!res.ok) throw new Error("Erro na requisicao");

            const contentType = res.headers.get("content-type");

            if (contentType && contentType.includes("application/json")) {
                return res.json();
            } else {
                return null; // ou res.text()
            }
        })
        .then(data =>
            console.log(data, "Prato cadastrado"),
        )
        .catch(err => console.error("Erro:", err));

        console.log("passou do fetch")
    document.getElementById('nomePrato').value = '';
    document.getElementById('descPrato').value = '';
    document.getElementById('categoriaPrato').value = '';
    document.getElementById('veganoPrato').value = '';
    document.getElementById('imagemPrato').value = '';
    modalNovoPrato.hide();

    window.location.reload();
})


