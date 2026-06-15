const API = 'http://localhost:8080';

// ── Redireciona se não logado ─────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', function () {
    if (sessionStorage.getItem('logado') !== 'true') {
        window.location.href = 'login.php';
    }
});

// ── Estado ───────────────────────────────────────────────────────────────────
let acaoPendente = null; // 'trocar' ou 'deletar'
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

    fetch(`${API}/user/login/${username}`)
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
            document.getElementById('trocarinfo').style.display         = 'block';
            document.getElementById('voltar').style.display             = 'block';

            // Deletar só aparece para funcionários logados
            if (localStorage.getItem('tipo') === 'true') {
                document.getElementById('deletar').style.display = 'block';
            }
        })
        .catch(err => {
            message.innerText = err.message;
        });
});

// ── ABRIR MODAL: TROCAR INFORMAÇÕES ──────────────────────────────────────────
document.getElementById('trocarinfo').addEventListener('click', function () {
    acaoPendente = 'trocar';
    abrirModalSenha(
        '✏️ Trocar Informações',
        'Para editar os dados do usuário <strong>' + (usuarioBuscado ? usuarioBuscado.login : '') + '</strong>, confirme sua senha.'
    );
});

// ── ABRIR MODAL: DELETAR ─────────────────────────────────────────────────────
document.getElementById('deletar').addEventListener('click', function () {
    acaoPendente = 'deletar';
    abrirModalSenha(
        '🗑️ Deletar Usuário',
        'Você está prestes a <strong>deletar</strong> o usuário <strong>' + (usuarioBuscado ? usuarioBuscado.login : '') + '</strong>. Esta ação não pode ser desfeita. Confirme sua senha para continuar.'
    );
});

// ── MODAL: abrir e resetar ────────────────────────────────────────────────────
function abrirModalSenha(titulo, descricao) {
    document.getElementById('modalSenhaTitulo').innerHTML    = titulo;
    document.getElementById('modalSenhaDescricao').innerHTML = descricao;
    document.getElementById('inputSenhaModal').value         = '';
    document.getElementById('msgSenhaModal').textContent     = '';
    new bootstrap.Modal(document.getElementById('modalSenha')).show();
}

// ── MODAL: confirmar senha e executar ação ─────────────────────────────────────
document.getElementById('btnConfirmarSenha').addEventListener('click', async function () {
    const senha  = document.getElementById('inputSenhaModal').value;
    const msgEl  = document.getElementById('msgSenhaModal');
    const btnEl  = document.getElementById('btnConfirmarSenha');

    msgEl.textContent = '';

    if (!senha) {
        msgEl.textContent = 'Digite sua senha.';
        return;
    }

    // Login do usuário logado (dono da sessão, não o usuário buscado)
    const loginLogado = localStorage.getItem('username');

    btnEl.disabled       = true;
    btnEl.textContent    = 'Verificando...';

    try {
        // 1) Valida a senha do usuário LOGADO
        const resValidar = await fetch(
            `${API}/user/validar?login=${encodeURIComponent(loginLogado)}&senhaHash=${encodeURIComponent(senha)}`
        );

        if (!resValidar.ok) throw new Error('Senha incorreta.');

        const senhaCorreta = await resValidar.json();
        if (!senhaCorreta) throw new Error('Senha incorreta.');

        // 2) Senha válida — executa a ação pendente
        if (acaoPendente === 'trocar') {
            // Salva dados do usuário A SER EDITADO em chaves separadas,
            // sem sobrescrever os dados do usuário logado
            localStorage.setItem('usernameTroca', usuarioBuscado.login);
            localStorage.setItem('nomeTroca',     usuarioBuscado.nome);
            localStorage.setItem('tipoTroca',     usuarioBuscado.funcionario);
            localStorage.setItem('idTroca',       usuarioBuscado.id);

            bootstrap.Modal.getInstance(document.getElementById('modalSenha')).hide();
            window.location.href = 'trocarinfo.php';

        } else if (acaoPendente === 'deletar') {
            // Deleta o usuário buscado
            const resDel = await fetch(`${API}/user/deletar/${usuarioBuscado.id}`, {
                method: 'DELETE'
            });

            if (!resDel.ok) throw new Error('Erro ao deletar o usuário.');

            bootstrap.Modal.getInstance(document.getElementById('modalSenha')).hide();

            // Se deletou a si mesmo, faz logout
            if (usuarioBuscado.login === loginLogado) {
                sessionStorage.setItem('logado', 'false');
                localStorage.clear();
                window.location.href = 'login.php';
            } else {
                document.getElementById('message').style.color = 'green';
                document.getElementById('message').innerText   = 'Usuário deletado com sucesso.';
                esconderBotoes();
                document.getElementById('username').value = '';
                usuarioBuscado = null;
            }
        }

    } catch (err) {
        msgEl.textContent = err.message;
    } finally {
        btnEl.disabled    = false;
        btnEl.textContent = 'Confirmar';
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