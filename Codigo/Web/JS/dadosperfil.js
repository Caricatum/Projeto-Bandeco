const API = 'http://localhost:8080';

window.addEventListener('pageshow', (event) => {
    if (event.persisted) {
        location.reload();
    }
});

// ── Redireciona se não logado ─────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', function () {
    if (sessionStorage.getItem('logado') !== 'true') {
        window.location.href = 'login.php';
        return;
    }

    localStorage.setItem("usernameTroca", "");
    localStorage.setItem("nomeTroca", "");
    localStorage.setItem("tipoTroca", "");
    localStorage.setItem("idTroca", "");
});

// ── Estado ───────────────────────────────────────────────────────────────────
let usuarioBuscado = null; // objeto { id, login, nome, funcionario }

// ── BUSCAR USUÁRIO ────────────────────────────────────────────────────────────
document.getElementById('dadosForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const username = document.getElementById('username').value.trim();
    const message  = document.getElementById('message');

    // Resetar exibição
    document.getElementById('aluno').checked = false;
    document.getElementById('func').checked  = false;
    document.getElementById('name').value    = '';
    message.innerText = '';
    esconderBotoes();

    if (!username) return;

    fetch(`${API}/user/login/${encodeURIComponent(username)}`)
        .then(res => {
            if (!res.ok) throw new Error('Usuário não encontrado.');
            return res.json();
        })
        .then(data => {
            usuarioBuscado = data;

            // Preenche os campos
            document.getElementById('name').value = data.nome;
            if (data.funcionario) {
                document.getElementById('func').checked = true;
            } else {
                document.getElementById('aluno').checked = true;
            }

            // Revela seções
            document.getElementById('div-nome').style.display           = 'flex';
            document.getElementById('sectionTipodeUsuario').style.display = 'flex';
            document.getElementById('voltar').style.display             = 'block';

            const isFuncionarioLogado = localStorage.getItem('tipo') === 'true';
            const loginLogado = (localStorage.getItem('username') || '').toLowerCase();
            const loginBuscado = (data.login || '').toLowerCase();

            // Regra de permissão: Apenas funcionários podem mudar informações de outros usuários.
            // Alunos só podem mudar suas próprias informações.
            if (isFuncionarioLogado || loginLogado === loginBuscado) {
                document.getElementById('trocarinfo').style.display = 'block';
            } else {
                document.getElementById('trocarinfo').style.display = 'none';
                message.style.color = '#7a1728';
                message.innerText = 'Você está visualizando este perfil. Apenas funcionários podem alterar dados de outros usuários.';
            }

            // Deletar só aparece para funcionários logados
            if (isFuncionarioLogado) {
                document.getElementById('deletar').style.display = 'block';
            }
        })
        .catch(err => {
            message.style.color = '#D92243';
            message.innerText = err.message;
        });
});

// ── TROCAR INFORMAÇÕES (Sem necessidade de senha) ────────────────────────────
document.getElementById('trocarinfo').addEventListener('click', function () {
    if (!usuarioBuscado) return;

    const isFuncionarioLogado = localStorage.getItem('tipo') === 'true';
    const loginLogado = (localStorage.getItem('username') || '').toLowerCase();
    const loginBuscado = (usuarioBuscado.login || '').toLowerCase();

    // Verificação de permissão
    if (!isFuncionarioLogado && loginLogado !== loginBuscado) {
        alert('Apenas funcionários podem alterar dados de outros alunos e funcionários.');
        return;
    }

    // Salva dados do usuário A SER EDITADO em chaves separadas
    localStorage.setItem('usernameTroca', usuarioBuscado.login);
    localStorage.setItem('nomeTroca',     usuarioBuscado.nome);
    localStorage.setItem('tipoTroca',     usuarioBuscado.funcionario);
    localStorage.setItem('idTroca',       usuarioBuscado.id);

    // Redireciona diretamente para a tela de edição sem pedir senha
    window.location.href = 'trocarinfo.php';
});

// ── DELETAR USUÁRIO (Apenas funcionários) ────────────────────────────────────
document.getElementById('deletar').addEventListener('click', async function () {
    if (!usuarioBuscado) return;

    const isFuncionarioLogado = localStorage.getItem('tipo') === 'true';
    if (!isFuncionarioLogado) {
        alert('Apenas funcionários podem deletar usuários.');
        return;
    }

    const confirma = confirm(`Tem certeza que deseja deletar o usuário "${usuarioBuscado.login}"? Esta ação não pode ser desfeita.`);
    if (!confirma) return;

    const message = document.getElementById('message');
    const loginLogado = localStorage.getItem('username');

    try {
        const resDel = await fetch(`${API}/user/deletar/${usuarioBuscado.id}`, {
            method: 'DELETE'
        });

        if (!resDel.ok) throw new Error('Erro ao deletar o usuário.');

        // Se deletou a si mesmo, faz logout
        if (usuarioBuscado.login === loginLogado) {
            sessionStorage.setItem('logado', 'false');
            localStorage.clear();
            window.location.href = 'login.php';
        } else {
            message.style.color = 'green';
            message.innerText   = 'Usuário deletado com sucesso.';
            esconderBotoes();
            document.getElementById('username').value = '';
            usuarioBuscado = null;
        }
    } catch (err) {
        message.style.color = '#D92243';
        message.innerText   = err.message;
    }
});

// ── Helpers ───────────────────────────────────────────────────────────────────
function esconderBotoes() {
    document.getElementById('div-nome').style.display             = 'none';
    document.getElementById('sectionTipodeUsuario').style.display = 'none';
    document.getElementById('trocarinfo').style.display           = 'none';
    document.getElementById('deletar').style.display              = 'none';
    document.getElementById('voltar').style.display               = 'none';
}