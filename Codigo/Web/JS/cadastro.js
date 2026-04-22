document.getElementById('cadastroForm').addEventListener("submit", function(e){
    e.preventDefault();

    // Pegando os valores dos campos
    const userDigitado = document.getElementById('username').value;
    const senhaDigitada = document.getElementById('password').value;
    const tipoDeUsuario = document.querySelector('input[name="tipoDeUsuario"]:checked').value;
    const nomeDigitado = document.getElementById('name').value;
    
    const url = 'http://localhost:8080/user/cadastrarUser';
    console.log("Entrou na url");

    const usuario = {
        login: userDigitado,
        nome: nomeDigitado,
        senha_hash: senhaDigitada,
        funcionario: tipoDeUsuario === 'true',
    }
    const jsonUsuario = JSON.stringify(usuario);

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: jsonUsuario
    })
    .then(res => {
    if (!res.ok) throw new Error("Erro na requisicao");

    const contentType = res.headers.get("content-type");

    if (contentType && contentType.includes("application/json")) {
        return res.json();
    } else {
        return null; // ou res.text()
    }
})
    .then(data => 
        console.log(data, "Usuario cadastrado"),
    )
    .catch(err => console.error("Erro:", err));
});