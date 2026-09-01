document.getElementById('loginForm').addEventListener('submit', function (e) {
    e.preventDefault(); // Impede o envio real do formulário

    const userDigitado = document.getElementById('username').value.trim();
    const senhaDigitada = document.getElementById('password').value.trim();
    const message = document.getElementById('message');

    message.innerText = 'Verificando...';

    fetchAPI(`/user/validar?login=${encodeURIComponent(userDigitado)}&senhaHash=${encodeURIComponent(senhaDigitada)}`)
        .then(res => {
            if (!res.ok) {
                throw new Error("Usuário ou senha incorretos!");
            }
            return res.json();
        })
        .then(dados => {
            if (dados === true) {
                // Login válido — agora busca os dados completos do usuário
                return fetchAPI(`/user/login/${encodeURIComponent(userDigitado)}`)
                    .then(res => {
                        if (!res.ok) throw new Error("Não foi possível carregar os dados do usuário.");
                        return res.json();
                    })
                    .then(user => {
                        // Define a sessão
                        sessionStorage.setItem('logado', 'true');

                        // Armazena os dados do usuário na local storage
                        localStorage.setItem('username', userDigitado);
                        localStorage.setItem('nome', user.nome);
                        localStorage.setItem('id', user.id);
                        localStorage.setItem('tipo', user.funcionario ? 'true' : 'false');

                        // Redireciona
                        window.location.href = 'inicio.php';
                    });
            } else {
                message.innerText = 'Usuário ou senha incorretos!';
                sessionStorage.setItem('logado', 'false');
            }
        })
        .catch(error => {
            message.innerText = error.message;
        });

});