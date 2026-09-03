const API = typeof API_BASE_URL !== 'undefined' ? API_BASE_URL : 'http://localhost:8080';

// Redireciona para o login se não estiver autenticado
if (sessionStorage.getItem('logado') !== 'true') {
    window.location.href = 'login.php';
}

const loginLogado = (localStorage.getItem('username') || '').toLowerCase();
const isFuncionarioLogado = localStorage.getItem('tipo') === 'true';

let usuarioCarregado = null; // { id, login, nome, funcionario }
let todosUsuarios = []; // Lista completa de usuarios para funcionarios
let filtroTipoAtual = 'todos'; // 'todos' | 'alunos' | 'funcionarios'

document.addEventListener('DOMContentLoaded', async () => {
    // Limpa dados temporários de edição
    localStorage.setItem("usernameTroca", "");
    localStorage.setItem("nomeTroca", "");
    localStorage.setItem("tipoTroca", "");
    localStorage.setItem("idTroca", "");

    // Se for funcionário, exibe o painel de busca e carrega a lista de usuários
    const boxBusca = document.getElementById('boxBuscaFuncionario');
    if (isFuncionarioLogado && boxBusca) {
        boxBusca.classList.remove('d-none');
        await carregarListaTodosUsuarios();
    }

    // Carrega automaticamente o próprio perfil
    await carregarDadosUsuario(localStorage.getItem('username'));

    // Filtro de busca em tempo real (digitação)
    const inputBusca = document.getElementById('inputBuscaUser');
    if (inputBusca) {
        inputBusca.addEventListener('input', () => {
            renderizarListaUsuarios();
        });
    }

    // Botão limpar busca
    const btnLimpar = document.getElementById('btnLimparBusca');
    if (btnLimpar) {
        btnLimpar.addEventListener('click', () => {
            if (inputBusca) {
                inputBusca.value = '';
                inputBusca.focus();
            }
            renderizarListaUsuarios();
        });
    }

    // Botões de filtro rápido (Todos / Alunos / Funcionários)
    document.querySelectorAll('.btn-filtro-user').forEach(btn => {
        btn.addEventListener('click', () => {
            document.querySelectorAll('.btn-filtro-user').forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            filtroTipoAtual = btn.dataset.filtro;
            renderizarListaUsuarios();
        });
    });

    // Botão para voltar a ver o próprio perfil
    const linkMeuPerfil = document.getElementById('linkVoltarMeuPerfil');
    if (linkMeuPerfil) {
        linkMeuPerfil.addEventListener('click', async (e) => {
            e.preventDefault();
            if (inputBusca) inputBusca.value = '';
            await carregarDadosUsuario(localStorage.getItem('username'));
        });
    }

    // Configura botão de Trocar Informações
    document.getElementById('btnTrocarInfo').addEventListener('click', () => {
        if (!usuarioCarregado) return;

        const loginAlvo = (usuarioCarregado.login || '').toLowerCase();

        // Aluno só pode editar o próprio perfil
        if (!isFuncionarioLogado && loginAlvo !== loginLogado) {
            alert('Você tem permissão para alterar apenas as suas próprias informações.');
            return;
        }

        // Salva dados do usuário selecionado para a página de edição
        localStorage.setItem("usernameTroca", usuarioCarregado.login);
        localStorage.setItem("nomeTroca", usuarioCarregado.nome);
        localStorage.setItem("tipoTroca", usuarioCarregado.funcionario ? "true" : "false");
        localStorage.setItem("idTroca", usuarioCarregado.id);

        window.location.href = 'trocarinfo.php';
    });

    // Configura botão de Deletar Usuário (Apenas Funcionário)
    document.getElementById('btnDeletarUser').addEventListener('click', async () => {
        if (!usuarioCarregado || !isFuncionarioLogado) return;

        const confirma = confirm(`Tem certeza que deseja deletar o usuário "${usuarioCarregado.login}"? Esta ação é irreversível.`);
        if (!confirma) return;

        const message = document.getElementById('message');
        try {
            const res = await fetch(`${API}/user/deletar/${usuarioCarregado.id}`, { method: 'DELETE' });
            if (!res.ok) throw new Error(await res.text() || 'Erro ao deletar usuário.');

            // Se o funcionário deletou a si próprio
            if (usuarioCarregado.login.toLowerCase() === loginLogado) {
                sessionStorage.setItem('logado', 'false');
                localStorage.clear();
                window.location.href = 'login.php';
                return;
            }

            message.style.color = 'green';
            message.innerText = 'Usuário deletado com sucesso.';

            // Remove o usuário deletado da lista local e re-renderiza
            todosUsuarios = todosUsuarios.filter(u => u.id !== usuarioCarregado.id);
            renderizarListaUsuarios();

            // Volta para o perfil do funcionário logado
            await carregarDadosUsuario(localStorage.getItem('username'));
        } catch (err) {
            message.style.color = '#D92243';
            message.innerText = 'Erro: ' + err.message;
        }
    });
});

/**
 * Busca a lista completa de usuários na API
 */
async function carregarListaTodosUsuarios() {
    const listaContainer = document.getElementById('listaUsuariosScroll');
    const contador = document.getElementById('contadorUsuarios');

    try {
        const res = await fetch(`${API}/user/all`);
        if (!res.ok) throw new Error('Falha ao carregar lista de usuários');

        todosUsuarios = await res.json();

        // Ordena por nome alfabeticamente
        todosUsuarios.sort((a, b) => (a.nome || a.login).localeCompare(b.nome || b.login));

        if (contador) contador.textContent = `${todosUsuarios.length} cadastrados`;

        renderizarListaUsuarios();
    } catch (e) {
        if (listaContainer) {
            listaContainer.innerHTML = `<div class="p-3 text-center text-muted small">Não foi possível carregar a lista de usuários.</div>`;
        }
    }
}

/**
 * Renderiza a lista de usuários aplicando os filtros de busca e categoria
 */
function renderizarListaUsuarios() {
    const listaContainer = document.getElementById('listaUsuariosScroll');
    if (!listaContainer) return;

    const termo = (document.getElementById('inputBuscaUser')?.value || '').toLowerCase().trim();

    const filtrados = todosUsuarios.filter(u => {
        // Filtro de Categoria
        if (filtroTipoAtual === 'alunos' && u.funcionario) return false;
        if (filtroTipoAtual === 'funcionarios' && !u.funcionario) return false;

        // Filtro de Texto
        if (termo) {
            const nome = (u.nome || '').toLowerCase();
            const login = (u.login || '').toLowerCase();
            return nome.includes(termo) || login.includes(termo);
        }

        return true;
    });

    if (filtrados.length === 0) {
        listaContainer.innerHTML = `<div class="p-3 text-center text-muted small">Nenhum usuário encontrado.</div>`;
        return;
    }

    listaContainer.innerHTML = filtrados.map(u => {
        const isSelected = usuarioCarregado && usuarioCarregado.id === u.id;
        const icon = u.funcionario ? '👔' : '🎓';
        const tipoLabel = u.funcionario ? 'Func' : 'Aluno';
        const badgeClass = u.funcionario ? 'badge bg-danger text-white' : 'badge bg-warning text-dark';

        return `
            <div class="usuario-item-lista ${isSelected ? 'selecionado' : ''}" onclick="selecionarUsuarioPeloLogin('${encodeURIComponent(u.login)}')">
                <div class="d-flex align-items-center gap-2 overflow-hidden me-2">
                    <span style="font-size: 1.1rem;">${icon}</span>
                    <div class="text-truncate">
                        <p class="usuario-item-nome text-truncate">${u.nome || u.login}</p>
                        <p class="usuario-item-email text-truncate">${u.login}</p>
                    </div>
                </div>
                <span class="${badgeClass} small" style="font-size: 0.7rem;">${tipoLabel}</span>
            </div>
        `;
    }).join('');
}

/**
 * Callback de clique na lista de usuários
 */
window.selecionarUsuarioPeloLogin = async function(loginEncoded) {
    const login = decodeURIComponent(loginEncoded);
    await carregarDadosUsuario(login);
    renderizarListaUsuarios();
};

/**
 * Busca e renderiza os dados de um usuário na interface
 */
async function carregarDadosUsuario(login) {
    const message = document.getElementById('message');
    message.innerText = '';

    if (!login) return;

    try {
        const res = await fetch(`${API}/user/login/${encodeURIComponent(login)}`);
        if (!res.ok) throw new Error('Usuário não encontrado.');

        const data = await res.json();
        usuarioCarregado = data;

        // Atualiza campos
        document.getElementById('exibeNome').textContent = data.nome || '—';
        document.getElementById('exibeLogin').textContent = data.login || '—';
        
        const badge = document.getElementById('badgeTipoUsuario');
        const exibeTipo = document.getElementById('exibeTipo');
        const avatar = document.getElementById('avatarIcon');
        const titulo = document.getElementById('tituloPagina');
        const btnDeletar = document.getElementById('btnDeletarUser');
        const btnVerMeu = document.getElementById('btnVerMeuPerfil');

        if (data.funcionario) {
            badge.className = 'badge-tipo-func';
            badge.textContent = '👔 Funcionário';
            exibeTipo.innerHTML = '<strong>Funcionário</strong> <span class="text-muted">(Acesso Administrativo)</span>';
            avatar.textContent = '👔';
        } else {
            badge.className = 'badge-tipo-aluno';
            badge.textContent = '🎓 Aluno';
            exibeTipo.innerHTML = '<strong>Aluno</strong>';
            avatar.textContent = '🎓';
        }

        const isProprioPerfil = (data.login || '').toLowerCase() === loginLogado;

        if (isProprioPerfil) {
            titulo.textContent = 'Meu Perfil';
            if (btnVerMeu) btnVerMeu.classList.add('d-none');
            if (btnDeletar) btnDeletar.classList.add('d-none');
        } else {
            titulo.textContent = `Perfil de ${data.nome.split(' ')[0] || data.login}`;
            if (btnVerMeu) btnVerMeu.classList.remove('d-none');
            if (btnDeletar && isFuncionarioLogado) btnDeletar.classList.remove('d-none');
        }

        // Atualiza o destaque na lista
        renderizarListaUsuarios();

    } catch (err) {
        message.style.color = '#D92243';
        message.innerText = err.message;
    }
}