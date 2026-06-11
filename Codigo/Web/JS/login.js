document.getElementById('loginForm').addEventListener('submit', function (e) {
    e.preventDefault(); // Impede o envio real do formulário

    const userDigitado = document.getElementById('username').value;
    const senhaDigitada = document.getElementById('password').value;
    const url = `http://localhost:8080/user/validar?login=${encodeURIComponent(userDigitado)}&senhaHash=${encodeURIComponent(senhaDigitada)}`;
    const message = document.getElementById('message');

    message.style.color = '';
    message.innerText = '';

    fetch(url)
        .then(res => {
            // E-mail ainda não confirmado
            if (res.status === 403) {
                sessionStorage.setItem('emailParaConfirmar', userDigitado);
                message.innerText = 'Seu e-mail ainda não foi confirmado. Redirecionando...';
                setTimeout(() => {
                    window.location.href = 'confirmarEmail.php';
                }, 1000);
                return null;
            }

            if (!res.ok) {
                throw new Error("Usuário ou senha incorretos!");
            }
            return res.json();
        })
        .then(dados => {
            if (dados === null) return; // já tratado acima (e-mail não confirmado)

            if (dados === true) {
                // Logado com sucesso
                sessionStorage.setItem('logado', 'true');
                localStorage.setItem('username', userDigitado);
                window.location.href = 'inicio.php';
            } else {
                message.innerText = 'Usuário ou senha incorretos!';
                sessionStorage.setItem('logado', 'false');
            }
        })
        .catch(error => {
            message.innerText = `Erro: ${error.message}`;
        });
});
