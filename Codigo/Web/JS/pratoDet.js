const API = 'http://localhost:8080';
const userId      = parseInt(localStorage.getItem('id'))   || null;
const logado      = sessionStorage.getItem('logado') === 'true';
const isFunc      = localStorage.getItem('tipo') === 'true';

// Pega o id da URL: pratoDet.php?id=5
const params  = new URLSearchParams(window.location.search);
const pratoId = parseInt(params.get('id'));

document.addEventListener('DOMContentLoaded', async () => {

    // Esconde login na navbar se logado
    if (logado) {
        const el = document.getElementById('itemLogin');
        if (el) el.style.display = 'none';
    }

    if (!pratoId) { mostrarErro(); return; }

    try {
        const [resPrato, resAvs] = await Promise.all([
            fetch(`${API}/pratos/id/${pratoId}`),
            fetch(`${API}/avaliacoes/all`)
        ]);

        if (!resPrato.ok) { mostrarErro(); return; }

        const prato = await resPrato.json();
        const todasAvs = resAvs.ok ? await resAvs.json() : [];
        const avsDestePrato = todasAvs.filter(a => a.prato && a.prato.id === pratoId);
        const jaAvaliou = avsDestePrato.some(a => a.user && a.user.id === userId);

        renderizarPrato(prato, avsDestePrato, jaAvaliou);
        // Só configura o formulário de estrelas se o usuário ainda não avaliou
        if (!jaAvaliou) {
            configurarEstrelas(pratoId);
        }

        document.getElementById('loading').style.display  = 'none';
        document.getElementById('conteudo').style.display = 'block';

    } catch (e) {
        mostrarErro();
    }
});

// ── RENDERIZAR ────────────────────────────────────────────────────────────────
function renderizarPrato(p, avs, jaAvaliou) {
    document.title = p.nome + ' — Bandeco';

    // Imagem
    const areaImg = document.getElementById('areaImagem');
    if (p.imagem) {
        areaImg.innerHTML = `<img src="${p.imagem}" alt="${p.nome}" class="prato-img">`;
    } else {
        areaImg.innerHTML = `<div class="prato-img-placeholder">🍽️</div>`;
    }

    // Nome
    document.getElementById('nomePrato').textContent = p.nome;

    // Badges
    const catNome = p.categoria ? (p.categoria.descricao || `Cat. ${p.categoria.id}`) : '—';
    document.getElementById('badges').innerHTML = `
        ${p.vegano ? '<span class="badge badge-vegano text-white">🥦 Vegano</span>' : ''}
        <span class="badge badge-categoria text-white">${catNome}</span>
    `;

    // Descrição
    document.getElementById('descricaoPrato').textContent = p.descricao;

    // Nota técnica (só funcionário)
    if (isFunc && p.notaTecnica) {
        document.getElementById('areaNota').style.display  = 'block';
        document.getElementById('notaTecnica').textContent = p.notaTecnica;
    }

    // Média
    if (avs.length) {
        const media = (avs.reduce((s, a) => s + a.nota, 0) / avs.length).toFixed(1);
        const n     = Math.round(parseFloat(media));
        document.getElementById('mediaEstrelas').textContent  = '★'.repeat(n) + '☆'.repeat(5 - n) + ' ' + media;
        document.getElementById('totalAvaliacoes').textContent = `${avs.length} avaliação(ões)`;
    } else {
        document.getElementById('mediaEstrelas').textContent  = '☆☆☆☆☆';
        document.getElementById('totalAvaliacoes').textContent = 'Sem avaliações ainda';
    }

    // Formulário de avaliação (só logado e não avaliou)
    if (logado && !jaAvaliou) {
        document.getElementById('formAvaliar').style.display = 'block';
    } else if (logado && jaAvaliou) {
        document.getElementById('formAvaliar').innerHTML = `
            <p class="text-muted mb-0">✅ Você já avaliou este prato.</p>`;
        document.getElementById('formAvaliar').style.display = 'block';
    }

    // Lista de avaliações
    const lista = document.getElementById('listaAvaliacoes');
    if (!avs.length) {
        lista.innerHTML = '<p class="text-muted">Nenhuma avaliação ainda.</p>';
    } else {
        lista.innerHTML = avs.map(a => {
            const estrelas = '★'.repeat(a.nota) + '☆'.repeat(5 - a.nota);
            const autor    = a.user ? (a.user.nome || a.user.login || 'Usuário') : 'Anônimo';
            return `
            <div class="card-aval">
                <div class="d-flex justify-content-between">
                    <span class="nota">${estrelas} ${a.nota}/5</span>
                    <span class="autor">${autor}</span>
                </div>
                ${a.avaliacao ? `<p class="mb-0 mt-1 small">${a.avaliacao}</p>` : ''}
            </div>`;
        }).join('');
    }
}

// ── ESTRELAS ──────────────────────────────────────────────────────────────────
function configurarEstrelas(pratoId) {
    document.querySelectorAll('.estrela-form').forEach(el => {
        el.addEventListener('click', () => {
            const v = parseInt(el.dataset.v);
            document.getElementById('notaSel').value = v;
            document.querySelectorAll('.estrela-form').forEach(e => {
                e.classList.toggle('selecionada', parseInt(e.dataset.v) <= v);
            });
        });
    });

    document.getElementById('btnSalvarAval').addEventListener('click', async () => {
        const nota   = parseInt(document.getElementById('notaSel').value);
        const coment = document.getElementById('comentario').value.trim();
        const msg    = document.getElementById('msgAvaliar');
        const btn    = document.getElementById('btnSalvarAval');

        if (!nota) { msg.textContent = 'Selecione uma nota!'; return; }

        btn.disabled = true; btn.textContent = 'Salvando...';

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

            if (!res.ok) {
                const texto = await res.text();
                // Avaliação duplicada — não é erro crítico
                if (res.status === 400 && texto.includes('já')) {
                    msg.textContent = 'Você já avaliou este prato.';
                } else {
                    msg.textContent = 'Erro: ' + texto;
                }
                btn.disabled = false;
                btn.textContent = 'Salvar avaliação';
                return;
            }

            // Sucesso — atualiza a lista de avaliações sem recarregar
            msg.style.color = 'green';
            msg.textContent = '✅ Avaliação salva!';
            btn.disabled = true;
            btn.textContent = 'Avaliado ✓';

            // Recarrega só as avaliações dinamicamente
            const resAvs = await fetch(`${API}/avaliacoes/all`);
            const todasAvs = await resAvs.json();
            const avsAtuais = todasAvs.filter(a => a.prato && a.prato.id === pratoId);

            // Atualiza média
            const media = (avsAtuais.reduce((s, a) => s + a.nota, 0) / avsAtuais.length).toFixed(1);
            const n = Math.round(parseFloat(media));
            document.getElementById('mediaEstrelas').textContent  = '★'.repeat(n) + '☆'.repeat(5 - n) + ' ' + media;
            document.getElementById('totalAvaliacoes').textContent = `${avsAtuais.length} avaliação(ões)`;

            // Atualiza lista
            const lista = document.getElementById('listaAvaliacoes');
            lista.innerHTML = avsAtuais.map(a => {
                const estrelas = '★'.repeat(a.nota) + '☆'.repeat(5 - a.nota);
                const autor    = a.user ? (a.user.nome || a.user.login || 'Usuário') : 'Anônimo';
                return `
                <div class="card-aval">
                    <div class="d-flex justify-content-between">
                        <span class="nota">${estrelas} ${a.nota}/5</span>
                        <span class="autor">${autor}</span>
                    </div>
                    ${a.avaliacao ? `<p class="mb-0 mt-1 small">${a.avaliacao}</p>` : ''}
                </div>`;
            }).join('');

            // Esconde o formulário
            document.getElementById('formAvaliar').innerHTML =
                '<p class="text-muted mb-0">✅ Você já avaliou este prato.</p>';

        } catch (e) {
            msg.textContent = 'Erro inesperado: ' + e.message;
            btn.disabled = false;
            btn.textContent = 'Salvar avaliação';
        }
    });
}

// ── ERRO ──────────────────────────────────────────────────────────────────────
function mostrarErro() {
    document.getElementById('loading').style.display  = 'none';
    document.getElementById('areaErro').style.display = 'block';
}