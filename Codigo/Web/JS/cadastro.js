document.getElementById('loginForm').addEventListener('submit', function(e) {
    e.preventDefault(); // Impede o envio real do formulário

    // Credenciais fictícias
    const userCorreto = "admin";
    const senhaCorreta = "1234";

    const userDigitado = document.getElementById('username').value;
    const senhaDigitada = document.getElementById('password').value;
    const message = document.getElementById('message');

    if (userDigitado === userCorreto && senhaDigitada === senhaCorreta) {
        // Logado com sucesso
        sessionStorage.setItem('logado', 'true'); // Define a sessão
        window.location.href = 'dashboard.html'; // Redireciona
    } else {
        message.innerText = 'Usuário ou senha incorretos!';
        sessionStorage.setItem('logado', 'false');
    }
});
// teste