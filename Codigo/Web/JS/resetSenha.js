const API = 'http://localhost:8080';
let emailParaReset = '';

// ── ETAPA 1: solicitar código ─────────────────────────────────────────────────
document.getElementById('formSolicitar').addEventListener('submit', async function (e) {
    e.preventDefault();

    const email = document.getElementById('emailSolicitar').value.trim();
    const msg   = document.getElementById('msgEtapa1');
    const btn   = this.querySelector('button[type="submit"]');

    msg.style.color  = '';
    msg.textContent  = '';
    btn.disabled     = true;
    btn.textContent  = 'Enviando...';

    try {
        const res = await fetch(
            `${API}/user/solicitarResetSenha?login=${encodeURIComponent(email)}`,
            { method: 'POST' }
        );

        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || 'E-mail não encontrado.');
        }

        // Avança para etapa 2
        emailParaReset = email;
        document.getElementById('emailMostrado').textContent = email;
        document.getElementById('etapa1').style.display = 'none';
        document.getElementById('etapa2').style.display = 'block';

    } catch (err) {
        msg.textContent = err.message;
    } finally {
        btn.disabled    = false;
        btn.textContent = 'Enviar Código';
    }
});

// ── ETAPA 2: redefinir senha ──────────────────────────────────────────────────
document.getElementById('formRedefinir').addEventListener('submit', async function (e) {
    e.preventDefault();

    const codigo         = document.getElementById('codigoReset').value.trim();
    const novaSenha      = document.getElementById('novaSenha').value;
    const confirmarSenha = document.getElementById('confirmarSenha').value;
    const msg            = document.getElementById('msgEtapa2');
    const btn            = this.querySelector('button[type="submit"]');

    msg.style.color = '';
    msg.textContent = '';

    if (novaSenha.length < 6) {
        msg.textContent = 'A senha precisa ter no mínimo 6 caracteres.';
        return;
    }
    if (novaSenha !== confirmarSenha) {
        msg.textContent = 'As senhas não coincidem.';
        return;
    }
    if (!codigo) {
        msg.textContent = 'Digite o código recebido por e-mail.';
        return;
    }

    btn.disabled    = true;
    btn.textContent = 'Salvando...';

    try {
        const url = `${API}/user/resetSenha?login=${encodeURIComponent(emailParaReset)}&codigo=${encodeURIComponent(codigo)}&novaSenha=${encodeURIComponent(novaSenha)}`;

        const res = await fetch(url, { method: 'PUT' });

        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || 'Erro ao redefinir a senha.');
        }

        msg.style.color = 'green';
        msg.textContent = 'Senha redefinida com sucesso! Redirecionando...';

        setTimeout(() => { window.location.href = 'login.php'; }, 1800);

    } catch (err) {
        msg.textContent = err.message;
    } finally {
        btn.disabled    = false;
        btn.textContent = 'Redefinir Senha';
    }
});

// ── Voltar para etapa 1 (reenviar código) ─────────────────────────────────────
function voltarEtapa1() {
    document.getElementById('etapa2').style.display = 'none';
    document.getElementById('etapa1').style.display = 'block';
    document.getElementById('msgEtapa1').textContent = '';
    document.getElementById('codigoReset').value     = '';
    document.getElementById('novaSenha').value       = '';
    document.getElementById('confirmarSenha').value  = '';
}
