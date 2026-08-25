const API = typeof API_BASE_URL !== 'undefined' ? API_BASE_URL : 'http://localhost:8080';

// Redireciona para o login se não estiver autenticado
if (sessionStorage.getItem('logado') !== 'true') {
    window.location.href = 'login.php';
}

const loginLogado = (localStorage.getItem('username') || '').toLowerCase();
const isFuncionarioLogado = localStorage.getItem('tipo') === 'true';

let usuarioCarregado = null; // { id, login, nome, funcionario }

document.addEventListener('DOMContentLoaded', async () => {
    // Limpa dados temporários de edição
    localStorage.setItem("usernameTroca", "");
    localStorage.setItem("nomeTroca", "");
    localStorage.setItem("tipoTroca", "");
    localStorage.setItem("idTroca", "");

    // Se for funcionário, exibe o painel de busca administrativa
    const boxBusca = document.getElementById('boxBuscaFuncionario');
    if (isFuncionarioLogado && boxBusca) {
        boxBusca.classList.remove('d-none');
    }

    // Carrega automaticamente o próprio perfil
    await carregarDadosUsuario(localStorage.getItem('username'));

    // Configura formulário de busca de funcionários
    const formBusca = document.getElementById('formBuscaUsuario');
    if (formBusca) {
        formBusca.addEventListener('submit', async (e) => {
            e.preventDefault();
            const termo = document.getElementById('inputBuscaUser').value.trim();
            if (!termo) return;
            await carregarDadosUsuario(termo);
        });
    }

    // Botão para voltar a ver o próprio perfil (quando funcionário busca outro)
    const linkMeuPerfil = document.getElementById('linkVoltarMeuPerfil');
    if (linkMeuPerfil) {
        linkMeuPerfil.addEventListener('click', async (e) => {
            e.preventDefault();
            document.getElementById('inputBuscaUser').value = '';
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
            // Volta para o perfil do funcionário logado
            await carregarDadosUsuario(localStorage.getItem('username'));
        } catch (err) {
            message.style.color = '#D92243';
            message.innerText = 'Erro: ' + err.message;
        }
    });
});

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
            // Oculta deletar no próprio perfil por segurança, a menos que seja intencional
            if (btnDeletar) btnDeletar.classList.add('d-none');
        } else {
            titulo.textContent = `Perfil de ${data.nome.split(' ')[0] || data.login}`;
            if (btnVerMeu) btnVerMeu.classList.remove('d-none');
            // Mostra deletar apenas para funcionários ao visualizar terceiros
            if (btnDeletar && isFuncionarioLogado) btnDeletar.classList.remove('d-none');
        }

    } catch (err) {
        message.style.color = '#D92243';
        message.innerText = err.message;
    }
}