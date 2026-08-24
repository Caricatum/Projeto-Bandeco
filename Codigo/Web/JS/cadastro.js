document.getElementById('cadastroForm').addEventListener("submit", function(e){
    e.preventDefault();

    // Pegando os valores dos campos
    const userDigitado = document.getElementById('username').value.trim();
    const senhaDigitada = document.getElementById('password').value.trim();
    const tipoDeUsuario = document.querySelector('input[name="tipoDeUsuario"]:checked').value;
    const nomeDigitado = document.getElementById('name').value.trim();
    const message = document.getElementById('message');

    const usuario = {
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: senhaDigitada,
        funcionario: tipoDeUsuario === 'true',
    };
    const jsonUsuario = JSON.stringify(usuario);

    message.style.color = '';
    message.innerText = 'Processando cadastro...';

    fetchAPI('/user/cadastrar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: jsonUsuario
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
        }, 1500);
    })
    .catch(err => {
        message.style.color = 'red';
        message.innerText = err.message;
    });
});

