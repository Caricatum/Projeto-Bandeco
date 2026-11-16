document.getElementById('loginForm').addEventListener('submit', function (e) {
    e.preventDefault(); // Impede o envio real do formulário

    const userDigitado = document.getElementById('username').value.trim();
    const senhaDigitada = document.getElementById('password').value.trim();
    const message = document.getElementById('message');

    message.innerText = 'Verificando...';

    // Login seguro padrão REST (POST com credenciais no corpo)
    fetchAPI('/user/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ login: userDigitado, senha: senhaDigitada })
    })
    .then(async res => {
        if (!res.ok) {
            let msgErro = "Usuário ou senha incorretos!";
            try {
                const jsonErro = await res.json();
                if (jsonErro.erro) msgErro = jsonErro.erro;
            } catch (_) {}
            throw new Error(msgErro);
        }
        return res.json();
    })
    .then(user => {
        // Sincroniza e registra a sessão segura no PHP do servidor
        return fetch('auth.php?action=login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                id: user.id,
                nome: user.nome,
                login: userDigitado,
                funcionario: user.funcionario
            })
        })
        .then(res => res.json())
        .then(authRes => {
            if (!authRes.sucesso) {
                throw new Error(authRes.erro || "Falha ao registrar sessão segura no servidor.");
            }

            // Define o estado no cliente
            sessionStorage.setItem('logado', 'true');

            // Armazena os dados do usuário na local storage para o frontend
            localStorage.setItem('username', userDigitado);
            localStorage.setItem('nome', user.nome);
            localStorage.setItem('id', user.id);
            localStorage.setItem('tipo', user.funcionario ? 'true' : 'false');

            // Redireciona para o início
            window.location.href = 'inicio.php';
        });
    })
    .catch(error => {
        message.innerText = error.message;
        sessionStorage.setItem('logado', 'false');
    });
});
