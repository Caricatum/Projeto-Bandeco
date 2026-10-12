// =============================================
// CONFIGURAÇÃO
// =============================================
const API = typeof API_BASE_URL !== 'undefined' ? API_BASE_URL : 'http://localhost:8080';

const usuarioSessao = (typeof window.USUARIO_ATUAL !== 'undefined' && window.USUARIO_ATUAL) ? window.USUARIO_ATUAL : {
    id: parseInt(localStorage.getItem('id')) || null,
    username: localStorage.getItem('username') || '',
    tipo: localStorage.getItem('tipo'),
    funcionario: localStorage.getItem('tipo') === 'true'
};

const userId = usuarioSessao.id;

if (!userId) {
    window.location.href = 'login.php';
}

// =============================================
// ESTADO
// =============================================
let meusFavoritos   = [];  // [{ id, prato: {...}, user: {...} }]
let todasAvaliacoes = [];
let favParaRemover  = null; // { favId, nome }

// =============================================
// INIT
// =============================================
document.addEventListener('DOMContentLoaded', async () => {
    await Promise.all([carregarFavoritos(), carregarAvaliacoes()]);
    renderizar();

    document.getElementById('campoBusca').addEventListener('input', renderizar);
    document.getElementById('filtroVegano').addEventListener('change', renderizar);

    configurarModalAvaliar();
    configurarModalRemover();
});

// =============================================
// API
// =============================================
async function carregarFavoritos() {
    try {
        const res  = await fetch(`${API}/pratosFavoritos/all`);
        const todos = await res.json();
        meusFavoritos = todos.filter(f => f.user && f.user.id === userId);
    } catch {
        meusFavoritos = [];
    }
}

async function carregarAvaliacoes() {
    try {
        const res = await fetch(`${API}/avaliacoes/all`);
        todasAvaliacoes = await res.json();
    } catch {
        todasAvaliacoes = [];
    }
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
    return todasAvaliacoes.filter(a => a.prato && a.prato.id === pratoId && a.nota >= 4).length;
}

function euJaAvalieiEsse(pratoId) {
    return todasAvaliacoes.some(a =>
        a.prato && a.prato.id === pratoId &&
        a.user  && a.user.id  === userId
    );
}

function idAvaliacaoDoUsuario(pratoId) {
    const av = todasAvaliacoes.find(a =>
        a.prato && a.prato.id === pratoId &&
        a.user  && a.user.id  === userId
    );
    return av ? av.id : null;
}

function estrelasPorNota(nota) {
    const n = Math.round(parseFloat(nota));
    return '★'.repeat(n) + '☆'.repeat(5 - n);
}

function formatarUrlImagem(url) {
    if (!url) return null;
    let nomeArquivo = url;
    if (url.includes('/')) {
        nomeArquivo = url.split('/').pop();
    }
    if (!nomeArquivo) return null;
    return `imagem.php?arquivo=${encodeURIComponent(nomeArquivo)}`;
}

function gerarBadgesTagsPrato(p) {
    if (!p) return '';
    const tags = [];

    // Alérgenos
    if (p.leite)       tags.push({ tipo: 'alergia', texto: '🥛 Leite' });
    if (p.ovo)         tags.push({ tipo: 'alergia', texto: '🥚 Ovo' });
    if (p.amendoim)    tags.push({ tipo: 'alergia', texto: '🥜 Amendoim' });
    if (p.castanhas)   tags.push({ tipo: 'alergia', texto: '🌰 Castanhas' });
    if (p.frutosDoMar) tags.push({ tipo: 'alergia', texto: '🦐 Frutos do Mar' });
    if (p.peixes)      tags.push({ tipo: 'alergia', texto: '🐟 Peixes' });
    if (p.trigo)       tags.push({ tipo: 'alergia', texto: '🌾 Trigo/Glúten' });
    if (p.soja)        tags.push({ tipo: 'alergia', texto: '🌱 Soja' });
    if (p.gergilim)    tags.push({ tipo: 'alergia', texto: '⚪ Gergelim' });

    // Rótulos Nutricionais
    if (p.altoAcucarAdicionado) tags.push({ tipo: 'rotulo', texto: '⚠️ Alto em Açúcar' });
    if (p.altoGorduraSaturada)  tags.push({ tipo: 'rotulo', texto: '⚠️ Alto em Gord. Sat.' });
    if (p.altoSodio)            tags.push({ tipo: 'rotulo', texto: '⚠️ Alto em Sódio' });

    if (!tags.length) return '';

    return `
        <div class="tags-prato d-flex flex-wrap gap-1 mb-2">
            ${tags.map(t => `
                <span class="${t.tipo === 'rotulo' ? 'badge-tag-rotulo' : 'badge-tag-alergia'}">
                    ${t.texto}
                </span>
            `).join('')}
        </div>
    `;
}

function favoritosFiltrados() {
    const busca  = document.getElementById('campoBusca').value.toLowerCase().trim();
    const vegano = document.getElementById('filtroVegano').checked;

    return meusFavoritos.filter(f => {
        const p = f.prato;
        if (!p) return false;
        if (busca  && !p.nome.toLowerCase().includes(busca)) return false;
        if (vegano && !p.vegano) return false;
        return true;
    });
}

// =============================================
// RENDERIZAR
// =============================================
function renderizar() {
    const area  = document.getElementById('areaFavoritos');
    const lista = favoritosFiltrados();
    const total = meusFavoritos.length;

    document.getElementById('subtitulo').textContent =
        total === 0 ? 'Nenhum prato favoritado ainda.'
        : `${total} prato${total > 1 ? 's' : ''} favoritado${total > 1 ? 's' : ''}`;

    if (!lista.length) {
        area.innerHTML = `
            <div class="col-12">
                <div class="area-vazia">
                    <div style="font-size:3rem">⭐</div>
                    <h4 class="mt-3">
                        ${total === 0
                            ? 'Você ainda não tem favoritos!'
                            : 'Nenhum favorito com esses filtros'}
                    </h4>
                    <p class="text-muted">
                        ${total === 0
                            ? 'Vá em <a href="buscaPratos.php">Buscar Pratos</a> e favorite seus pratos preferidos.'
                            : 'Tente outros filtros.'}
                    </p>
                </div>
            </div>`;
        return;
    }

    area.innerHTML = lista.map(f => {
        const p         = f.prato;
        const media     = mediaNotas(p.id);
        const likes     = totalLikes(p.id);
        const jaAvaliou = euJaAvalieiEsse(p.id);
        const catNome   = p.categoria ? (p.categoria.descricao || `Cat. ${p.categoria.id}`) : '—';
        const urlImg    = formatarUrlImagem(p.imagem);

        return `
        <div class="col-md-6 col-lg-4">
            <div class="card card-prato h-100">

                <!-- Imagem do prato -->
                ${urlImg
                    ? `<img src="${urlImg}" alt="${p.nome}"
                            style="width:100%;height:160px;object-fit:cover;border-radius:12px 12px 0 0;"
                            onerror="if(!this.dataset.fallback && '${p.imagem}'){this.dataset.fallback='1';this.src='${p.imagem}';}else{this.onerror=null;this.src='../Assets/Images/logo_unicamp.jpg';}">`
                    : `<div style="width:100%;height:120px;background:linear-gradient(135deg,#F69D39,#E0C375);
                            border-radius:12px 12px 0 0;display:flex;align-items:center;
                            justify-content:center;font-size:3rem;">🍽️</div>`
                }

                <div class="card-body d-flex flex-column">

                    <div class="d-flex justify-content-between align-items-start mb-2">
                        <h5 class="card-title mb-0">
                            <a href="pratoDet.php?id=${p.id}"
                               style="color:#7a1728;text-decoration:none;"
                               onmouseover="this.style.textDecoration='underline'"
                               onmouseout="this.style.textDecoration='none'">
                                ${p.nome}
                            </a>
                        </h5>
                        <div class="d-flex flex-column gap-1 align-items-end">
                            ${p.vegano ? '<span class="badge badge-vegano text-white">🥦 Vegano</span>' : ''}
                            <span class="badge badge-categoria text-white">${catNome}</span>
                        </div>
                    </div>

                    <p class="card-text text-muted small flex-grow-1 mb-2">${p.descricao || ''}</p>

                    <!-- Tags de Alérgenos e Rótulos -->
                    ${gerarBadgesTagsPrato(p)}

                    <div class="d-flex align-items-center gap-2 mb-3 flex-wrap">
                        ${media
                            ? `<span style="color:#F69D39; font-weight:bold;">${estrelasPorNota(media)} ${media}</span>`
                            : '<span class="text-muted small">☆ Sem avaliações</span>'}
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

                        <!-- REMOVER FAVORITO -->
                        <button class="btn-desfav"
                            onclick="abrirModalRemover(${f.id}, '${p.nome.replace(/'/g, "\\'")}')">
                            ⭐ Remover
                        </button>

                    </div>
                </div>
            </div>
        </div>`;
    }).join('');
}

// =============================================
// MODAL: AVALIAR
// =============================================
function abrirModalAvaliar(pratoId, nomePrato) {
    document.getElementById('pratoIdAvaliar').value   = pratoId;
    document.getElementById('nomePratoAvaliar').textContent = nomePrato;
    document.getElementById('notaSelecionada').value  = 0;
    document.getElementById('comentarioAvaliar').value = '';
    document.getElementById('msgAvaliar').textContent = '';
    document.querySelectorAll('.estrela').forEach(e => e.classList.remove('selecionada'));
    new bootstrap.Modal(document.getElementById('modalAvaliar')).show();
}

function configurarModalAvaliar() {
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
        const nota    = parseInt(document.getElementById('notaSelecionada').value);
        const coment  = document.getElementById('comentarioAvaliar').value.trim();
        const msg     = document.getElementById('msgAvaliar');

        if (!nota) { msg.textContent = 'Selecione uma nota!'; return; }

        try {
            const res = await fetch(`${API}/avaliacoes/cadastrar`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    nota,
                    avaliacao: coment || null,
                    prato: { id: pratoId },
                    user:  { id: userId }
                })
            });

            if (!res.ok) throw new Error(await res.text());

            await carregarAvaliacoes();
            renderizar();
            bootstrap.Modal.getInstance(document.getElementById('modalAvaliar')).hide();

        } catch (e) {
            msg.textContent = 'Erro ao salvar avaliação: ' + e.message;
        }
    });
}

// =============================================
// MODAL: REMOVER FAVORITO
// =============================================
function abrirModalRemover(favId, nome) {
    favParaRemover = { favId, nome };
    document.getElementById('nomeRemover').textContent = nome;
    new bootstrap.Modal(document.getElementById('modalRemover')).show();
}

function configurarModalRemover() {
    document.getElementById('btnConfirmarRemover').addEventListener('click', async () => {
        if (!favParaRemover) return;
        try {
            const res = await fetch(`${API}/pratosFavoritos/deletar/${favParaRemover.favId}`, {
                method: 'DELETE'
            });
            if (!res.ok) throw new Error(await res.text());

            await carregarFavoritos();
            renderizar();
            bootstrap.Modal.getInstance(document.getElementById('modalRemover')).hide();
        } catch (e) {
            alert('Erro ao remover: ' + e.message);
        }
    });
}
