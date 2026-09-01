// ── Estado ─────────────────────────────────────────────────────────────────────
let timerInterval   = null;
let timerSecondsLeft = 0;
const TIMER_DURATION  = 10 * 60; // 10 minutos (em segundos)
const RESEND_COOLDOWN = 60;      // 60 segundos entre reenvios

// ── Inicialização ─────────────────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
    const emailSalvo = sessionStorage.getItem('emailParaConfirmar');
    if (emailSalvo) {
        document.getElementById('email').value = emailSalvo;
        document.getElementById('emailMostrado').textContent = emailSalvo;
    }

    // Inicia o timer ao carregar a página
    startTimer(TIMER_DURATION);
});

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

// ── Reenviar código ───────────────────────────────────────────────────────────
document.getElementById('btnReenviar').addEventListener('click', async function () {
    const email   = document.getElementById('email').value.trim();
    const message = document.getElementById('message');
    const btn     = this;

    if (!email) {
        message.style.color = '#D92243';
        message.innerText = 'Preencha o e-mail antes de reenviar o código.';
        return;
    }

    btn.disabled    = true;
    message.style.color = '#7a1728';
    message.innerText = 'Reenviando código...';

    try {
        const res = await fetchAPI(`/user/reenviarCodigo?email=${encodeURIComponent(email)}`, {
            method: 'POST'
        });

        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || 'Erro ao reenviar código.');
        }

        message.style.color = 'green';
        message.innerText = 'Novo código enviado! Verifique seu e-mail.';

        // Reinicia o timer
        startTimer(TIMER_DURATION);

        // Cooldown de 60s no botão
        startResendCooldown(btn);

    } catch (err) {
        message.style.color = '#D92243';
        message.innerText = err.message;
        btn.disabled = false;
    }
});

function startResendCooldown(btn) {
    let cooldown = RESEND_COOLDOWN;
    btn.disabled = true;
    btn.textContent = `🔄 Reenviar disponível em ${cooldown}s`;

    const cooldownInterval = setInterval(() => {
        cooldown--;
        if (cooldown <= 0) {
            clearInterval(cooldownInterval);
            btn.disabled = false;
            btn.textContent = '🔄 Reenviar Código de Confirmação';
        } else {
            btn.textContent = `🔄 Reenviar disponível em ${cooldown}s`;
        }
    }, 1000);
}

// ── Confirmar e-mail (funcionalidade existente preservada) ─────────────────────
document.getElementById('confirmarForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const email   = document.getElementById('email').value.trim();
    const codigo  = document.getElementById('codigo').value.trim();
    const message = document.getElementById('message');

    message.style.color = '';
    message.innerText = 'Confirmando...';

    fetchAPI(`/user/confirmarEmail?email=${encodeURIComponent(email)}&codigo=${encodeURIComponent(codigo)}`, { method: 'POST' })
        .then(async res => {
            if (!res.ok) {
                const texto = await res.text();
                throw new Error(texto || 'Não foi possível confirmar o e-mail.');
            }
            return true;
        })
        .then(() => {
            clearInterval(timerInterval);
            message.style.color = 'green';
            message.innerText = 'E-mail confirmado com sucesso! Redirecionando para o login...';
            sessionStorage.removeItem('emailParaConfirmar');
            setTimeout(() => {
                window.location.href = 'login.php';
            }, 1800);
        })
        .catch(err => {
            message.style.color = 'red';
            message.innerText = err.message;
        });
});
