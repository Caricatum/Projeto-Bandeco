document.getElementById('cadastroForm').addEventListener("submit", function(e){
    e.preventDefault();

    const nomeDigitado = document.getElementById('name').value.trim();
    const userDigitado = document.getElementById('username').value.trim();
    const senhaDigitada = document.getElementById('password').value;
    const confirmSenhaDigitada = document.getElementById('confirmPassword').value;
    const message = document.getElementById('message');
    const btnCadastrar = document.getElementById('btnCadastrar');

    message.style.color = '';
    message.innerText = '';

    // Validações no cliente
    if (!nomeDigitado || !userDigitado || !senhaDigitada) {
        message.style.color = '#D92243';
        message.innerText = 'Por favor, preencha todos os campos.';
        return;
    }

    // Validação de requisitos de senha forte
    const validacao = validarForcaSenha(senhaDigitada);
    if (!validacao.valido) {
        message.style.color = '#D92243';
        message.innerText = validacao.erro;
        return;
    }

    if (senhaDigitada !== confirmSenhaDigitada) {
        message.style.color = '#D92243';
        message.innerText = 'As senhas digitadas não coincidem.';
        return;
    }

    const usuario = {
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: senhaDigitada,
        funcionario: false,
    };

    message.style.color = '#7a1728';
    message.innerText = 'Processando cadastro e enviando código...';
    btnCadastrar.disabled = true;

    fetchAPI('/user/cadastrar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(usuario)
    })
    .then(async res => {
        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || 'Erro ao cadastrar usuário.');
        }
        return true;
    })
    .then(() => {
        // Salva o e-mail para pré-preencher na tela de confirmação
        sessionStorage.setItem('emailParaConfirmar', userDigitado);

        message.style.color = 'green';
        message.innerText = 'Cadastro realizado! Enviamos um código para o seu e-mail...';

        setTimeout(() => {
            window.location.href = 'confirmarEmail.php';
        }, 1200);
    })
    .catch(err => {
        message.style.color = '#D92243';
        message.innerText = err.message;
        btnCadastrar.disabled = false;
    });
});

// ========================================================
// REGRAS DE SEGURANÇA E VALIDAÇÃO DE SENHA FORTE
// ========================================================
function validarForcaSenha(senha) {
    if (senha.length < 8) {
        return { valido: false, erro: 'A senha deve conter no mínimo 8 caracteres.' };
    }
    if (!/[A-Z]/.test(senha)) {
        return { valido: false, erro: 'A senha deve conter pelo menos uma letra maiúscula (A-Z).' };
    }
    if (!/[a-z]/.test(senha)) {
        return { valido: false, erro: 'A senha deve conter pelo menos uma letra minúscula (a-z).' };
    }
    if (!/[0-9]/.test(senha)) {
        return { valido: false, erro: 'A senha deve conter pelo menos um número (0-9).' };
    }
    return { valido: true, erro: '' };
}

function atualizarIndicadoresSenha(senha) {
    const temTam = senha.length >= 8;
    const temMaius = /[A-Z]/.test(senha);
    const temMinus = /[a-z]/.test(senha);
    const temNum = /[0-9]/.test(senha);

    const elTam = document.getElementById('reqTam');
    const elMaius = document.getElementById('reqMaius');
    const elMinus = document.getElementById('reqMinus');
    const elNum = document.getElementById('reqNum');

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

    const barra = document.getElementById('forcaBarra');
    const texto = document.getElementById('forcaTexto');

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

// Ouvinte em tempo real no campo de senha
const inputSenha = document.getElementById('password');
if (inputSenha) {
    inputSenha.addEventListener('input', function() {
        atualizarIndicadoresSenha(this.value);
    });
}

// Botões para alternar exibição de senha
function configurarToggleSenha(btnId, inputId) {
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

configurarToggleSenha('btnToggleSenha', 'password');
configurarToggleSenha('btnToggleConfirmar', 'confirmPassword');
