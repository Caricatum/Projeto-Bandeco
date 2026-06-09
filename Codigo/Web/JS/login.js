

document.getElementById('loginForm').addEventListener('submit', function (e) {
    e.preventDefault(); // Impede o envio real do formulário

    // Credenciais fictícias
    const userDigitado = document.getElementById('username').value;
    const senhaDigitada = document.getElementById('password').value;
    const url = `http://localhost:8080/user/validar?login=${userDigitado}&senhaHash=${senhaDigitada}`;
    const message = document.getElementById('message');
    resultado = false;
    
    fetch(url)
        .then(res => {
            if (!res.ok) {
                throw new Error("Usuário ou senha incorretos!");
            }
            return res.json();
        })
        .then(dados => {
            resultado = dados;
            if (resultado == true) {
                // Logado com sucesso
                sessionStorage.setItem('logado', 'true'); // Define a sessão
                localStorage.setItem('username', userDigitado); // Armazena o nome do usuário na local
                // salva no navegador
                window.location.href = 'inicio.php'; // Redireciona
            } else {
                message.innerText = 'Usuário ou senha incorretos!';
                sessionStorage.setItem('logado', 'false');
            }
        })
        .catch(error => {
            message.innerHTML = `Erro: ${error.message}`
        })
        
        

});

//teste
