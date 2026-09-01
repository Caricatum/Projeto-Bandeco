// ── Estado ─────────────────────────────────────────────────────────────────────
let emailParaReset   = '';
let timerInterval    = null;
let timerSecondsLeft = 0;
const TIMER_DURATION  = 10 * 60; // 10 minutos (em segundos)
const RESEND_COOLDOWN = 60;      // 60 segundos entre reenvios

// ── Timer de validade do código ───────────────────────────────────────────────
function startTimer(seconds) {
    clearInterval(timerInterval);
    timerSecondsLeft = seconds;

    const container = document.getElementById('timerContainer');
    const timerText = document.getElementById('timerText');

    container.className = 'timer-container';
    updateTimerDisplay(timerText, container);

    timerInterval = setInterval(() => {
        timerSecondsLeft--;

        if (timerSecondsLeft <= 0) {
            clearInterval(timerInterval);
            timerSecondsLeft = 0;
            container.className = 'timer-container timer-expired';
            timerText.textContent = 'Código expirado — reenvie um novo código';
            return;
        }

        updateTimerDisplay(timerText, container);
    }, 1000);
}

function updateTimerDisplay(timerText, container) {
    const min = Math.floor(timerSecondsLeft / 60);
    const sec = timerSecondsLeft % 60;
    const tempo = `${String(min).padStart(2, '0')}:${String(sec).padStart(2, '0')}`;

    timerText.textContent = `Código válido por ${tempo}`;

    // Alerta visual nos últimos 2 minutos
    if (timerSecondsLeft <= 120 && timerSecondsLeft > 0) {
        container.className = 'timer-container timer-warning';
    } else {
        container.className = 'timer-container';
    }
}

function startResendCooldown(btn) {
    let cooldown = RESEND_COOLDOWN;
    btn.disabled = true;
    btn.textContent = `🔄 Reenviar disponível em ${cooldown}s`;

    const cooldownInterval = setInterval(() => {
        cooldown--;
        if (cooldown <= 0) {
            clearInterval(cooldownInterval);
            btn.disabled = false;
            btn.textContent = '🔄 Reenviar Código de Recuperação';
        } else {
            btn.textContent = `🔄 Reenviar disponível em ${cooldown}s`;
        }
    }, 1000);
}

// ── ETAPA 1: solicitar código ─────────────────────────────────────────────────
document.getElementById('formSolicitar').addEventListener('submit', async function (e) {
    e.preventDefault();

    const email = document.getElementById('emailSolicitar').value.trim();
    const msg   = document.getElementById('msgEtapa1');
    const btn   = document.getElementById('btnEnviarCodigo');

    msg.style.color  = '';
    msg.textContent  = '';
    btn.disabled     = true;
    btn.textContent  = 'Enviando código por e-mail...';

    try {
        const res = await fetchAPI(
            `/user/solicitarResetSenha?login=${encodeURIComponent(email)}`,
            { method: 'POST' }
        );

        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || 'E-mail não cadastrado ou erro ao enviar código.');
        }

        // Avança para etapa 2
        emailParaReset = email;
        document.getElementById('emailMostrado').textContent = email;
        document.getElementById('etapa1').style.display = 'none';
        document.getElementById('etapa2').style.display = 'block';

        // Inicia o timer
        startTimer(TIMER_DURATION);

    } catch (err) {
        msg.style.color = '#D92243';
        msg.textContent = err.message;
    } finally {
        btn.disabled    = false;
        btn.textContent = 'Enviar Código de Recuperação';
    }
});

// ── Reenviar código (botão dedicado na etapa 2) ───────────────────────────────
document.getElementById('btnReenviarReset').addEventListener('click', async function () {
    const msg = document.getElementById('msgEtapa2');
    const btn = this;

    if (!emailParaReset) {
        msg.style.color = '#D92243';
        msg.textContent = 'Erro interno: e-mail não encontrado. Volte e tente novamente.';
        return;
    }

    btn.disabled    = true;
    msg.style.color = '#7a1728';
    msg.textContent = 'Reenviando código...';

    try {
        const res = await fetchAPI(
            `/user/solicitarResetSenha?login=${encodeURIComponent(emailParaReset)}`,
            { method: 'POST' }
        );

        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || 'Erro ao reenviar código.');
        }

        msg.style.color = 'green';
        msg.textContent = 'Novo código enviado! Verifique seu e-mail.';

        // Reinicia o timer
        startTimer(TIMER_DURATION);

        // Cooldown de 60s no botão
        startResendCooldown(btn);

    } catch (err) {
        msg.style.color = '#D92243';
        msg.textContent = err.message;
        btn.disabled = false;
    }
});

// ── ETAPA 2: redefinir senha ──────────────────────────────────────────────────
document.getElementById('formRedefinir').addEventListener('submit', async function (e) {
    e.preventDefault();

    const codigo         = document.getElementById('codigoReset').value.trim();
    const novaSenha      = document.getElementById('novaSenha').value;
    const confirmarSenha = document.getElementById('confirmarSenha').value;
    const msg            = document.getElementById('msgEtapa2');
    const btn            = document.getElementById('btnSalvarNovaSenha');

    msg.style.color = '';
    msg.textContent = '';

    if (!codigo || codigo.length < 4) {
        msg.style.color = '#D92243';
        msg.textContent = 'Por favor, digite o código completo recebido por e-mail.';
        return;
    }

    if (novaSenha.length < 6) {
        msg.style.color = '#D92243';
        msg.textContent = 'A nova senha precisa ter no mínimo 6 caracteres.';
        return;
    }

    if (novaSenha !== confirmarSenha) {
        msg.style.color = '#D92243';
        msg.textContent = 'As senhas digitadas não coincidem.';
        return;
    }

    btn.disabled    = true;
    btn.textContent = 'Salvando nova senha...';

    try {
        const url = `/user/resetSenha?login=${encodeURIComponent(emailParaReset)}&codigo=${encodeURIComponent(codigo)}&novaSenha=${encodeURIComponent(novaSenha)}`;

        const res = await fetchAPI(url, { method: 'PUT' });

        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || 'Código inválido ou expirado.');
        }

        clearInterval(timerInterval);
        msg.style.color = 'green';
        msg.textContent = 'Senha redefinida com sucesso! Redirecionando para o login...';

        setTimeout(() => { window.location.href = 'login.php'; }, 1500);

    } catch (err) {
        msg.style.color = '#D92243';
        msg.textContent = err.message;
        btn.disabled    = false;
        btn.textContent = 'Salvar Nova Senha';
    }
});

// ── Voltar para etapa 1 (trocar e-mail) ───────────────────────────────────────
function voltarEtapa1() {
    clearInterval(timerInterval);
    document.getElementById('etapa2').style.display = 'none';
    document.getElementById('etapa1').style.display = 'block';
    document.getElementById('msgEtapa1').textContent = '';
    document.getElementById('codigoReset').value     = '';
    document.getElementById('novaSenha').value       = '';
    document.getElementById('confirmarSenha').value  = '';
}
