export function validaFunc() {
    const username = localStorage.getItem('username')
    const url = `http://localhost:8080/user/login/${username}`;

    fetch(url)
        .then(res => {
            if (!res.ok) {
                sessionStorage.setItem('logado', 'false'); // Define a sessão
                localStorage.setItem('username', "");
                localStorage.setItem('nome', "");
                localStorage.setItem('tipo', "");
                localStorage.setItem('id', "");
                window.location.href = 'login.php' // Manda para login
                console.log("validaFUnc entro")
            }
            return res.json();
        })
        .then(dados => {
            localStorage.setItem('tipo', dados.funcionario);
        })
        .catch(error => {
            console.log(`Erro: ${error.message}`);
        })
}