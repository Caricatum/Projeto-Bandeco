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

    if (senhaDigitada.length < 6) {
        message.style.color = '#D92243';
        message.innerText = 'A senha precisa ter no mínimo 6 caracteres.';
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
