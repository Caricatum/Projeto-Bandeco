document.getElementById('loginForm').addEventListener('submit', function (e) {
    e.preventDefault(); // Impede o envio real do formulário

    const userDigitado = document.getElementById('username').value;
    const senhaDigitada = document.getElementById('password').value;
    const url = `http://localhost:8080/user/validar?login=${userDigitado}&senhaHash=${senhaDigitada}`;
    const message = document.getElementById('message');

    fetch(url)
        .then(res => {
            if (!res.ok) {
                throw new Error("Usuário ou senha incorretos!");
            }
            return res.json();
        })
        .then(dados => {
            if (dados === true) {
                // Login válido — agora busca os dados completos do usuário
                return fetch(`http://localhost:8080/user/login/${userDigitado}`)
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
                        localStorage.setItem('tipo', user.funcionario); // 'true' ou 'false'

                        // Redireciona
                        window.location.href = 'inicio.php';
                    });
            } else {
                message.innerText = 'Usuário ou senha incorretos!';
                sessionStorage.setItem('logado', 'false');
            }
        })
        .catch(error => {
            message.innerHTML = `Erro: ${error.message}`;
        });

});