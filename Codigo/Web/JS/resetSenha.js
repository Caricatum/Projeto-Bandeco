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

    // Validação de requisitos de senha forte
    const validacao = validarForcaSenha(novaSenha);
    if (!validacao.valido) {
        msg.style.color = '#D92243';
        msg.textContent = validacao.erro;
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
    atualizarIndicadoresSenhaReset('');
}

// ========================================================
// REGRAS DE SEGURANÇA E VALIDAÇÃO DE SENHA FORTE (RESET)
// ========================================================
function validarForcaSenha(senha) {
    if (senha.length < 8) {
        return { valido: false, erro: 'A nova senha deve conter no mínimo 8 caracteres.' };
    }
    if (!/[A-Z]/.test(senha)) {
        return { valido: false, erro: 'A nova senha deve conter pelo menos uma letra maiúscula (A-Z).' };
    }
    if (!/[a-z]/.test(senha)) {
        return { valido: false, erro: 'A nova senha deve conter pelo menos uma letra minúscula (a-z).' };
    }
    if (!/[0-9]/.test(senha)) {
        return { valido: false, erro: 'A nova senha deve conter pelo menos um número (0-9).' };
    }
    return { valido: true, erro: '' };
}

function atualizarIndicadoresSenhaReset(senha) {
    const temTam = senha.length >= 8;
    const temMaius = /[A-Z]/.test(senha);
    const temMinus = /[a-z]/.test(senha);
    const temNum = /[0-9]/.test(senha);

    const elTam = document.getElementById('reqTamReset');
    const elMaius = document.getElementById('reqMaiusReset');
    const elMinus = document.getElementById('reqMinusReset');
    const elNum = document.getElementById('reqNumReset');

    if (elTam) elTam.classList.toggle('req-met', temTam);
    if (elMaius) elMaius.classList.toggle('req-met', temMaius);
    if (elMinus) elMinus.classList.toggle('req-met', temMinus);
    if (elNum) elNum.classList.toggle('req-met', temNum);

    // Cálculo da barra de força
    let pontuacao = 0;
    if (senha.length > 0) {
        if (senha.length >= 8) pontuacao++;
        if (senha.length >= 12) pontuacao++;
        if (temMaius) pontuacao++;
        if (temMinus) pontuacao++;
        if (temNum) pontuacao++;
        if (/[^A-Za-z0-9]/.test(senha)) pontuacao++;
    }

    const barra = document.getElementById('forcaBarraReset');
    const texto = document.getElementById('forcaTextoReset');

    if (barra && texto) {
        if (senha.length === 0) {
            barra.style.width = '0%';
            barra.style.backgroundColor = '#e5e7eb';
            texto.innerText = '';
        } else if (pontuacao <= 2) {
            barra.style.width = '25%';
            barra.style.backgroundColor = '#ef4444'; // vermelho
            texto.style.color = '#ef4444';
            texto.innerText = 'Força: Fraca';
        } else if (pontuacao <= 4) {
            barra.style.width = '65%';
            barra.style.backgroundColor = '#f59e0b'; // laranja/âmbar
            texto.style.color = '#b45309';
            texto.innerText = 'Força: Média';
        } else {
            barra.style.width = '100%';
            barra.style.backgroundColor = '#16a34a'; // verde
            texto.style.color = '#15803d';
            texto.innerText = 'Força: Forte';
        }
    }
}

// Ouvinte em tempo real no campo de nova senha
const inputNovaSenha = document.getElementById('novaSenha');
if (inputNovaSenha) {
    inputNovaSenha.addEventListener('input', function() {
        atualizarIndicadoresSenhaReset(this.value);
    });
}

// Botões para alternar exibição de senha
function configurarToggleSenhaReset(btnId, inputId) {
    const btn = document.getElementById(btnId);
    const input = document.getElementById(inputId);
    if (btn && input) {
        btn.addEventListener('click', function() {
            const tipoAtual = input.getAttribute('type');
            if (tipoAtual === 'password') {
                input.setAttribute('type', 'text');
                btn.textContent = '🙈';
            } else {
                input.setAttribute('type', 'password');
                btn.textContent = '👁️';
            }
        });
    }
}

configurarToggleSenhaReset('btnToggleNovaSenha', 'novaSenha');
configurarToggleSenhaReset('btnToggleConfirmarReset', 'confirmarSenha');

