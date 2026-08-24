/*import { validaFunc } from './validaFunc.js';

validaFunc();*/

document.addEventListener("DOMContentLoaded", function () {

    // chatGPT 
    const token = sessionStorage.getItem("logado"); // ou sessionStorage

    if (!token) {
        // Usuário não está logado, redireciona para o login
        window.location.href = "login.php";
    } else {
        // Usuário está logado. Opcional: Validar o token com o backend
        console.log("Usuário autenticado");
    }



    const user = localStorage.getItem("usernameTroca");
    const nome = localStorage.getItem("nomeTroca");
    const tipo = localStorage.getItem("tipoTroca");
    const id = localStorage.getItem("idTroca");


    document.getElementById("username").value = user;
    document.getElementById("name").value = nome;

    if (tipo === "true") {
        document.getElementById("func").checked = true;
        document.getElementById("aluno").checked = false;
    } else {
        document.getElementById("aluno").checked = true;
        document.getElementById("func").checked = false;
    }

});

document.getElementById("trocarinfo").addEventListener("click", function () {
    const id = localStorage.getItem("idTroca");

    const userDigitado = document.getElementById("username").value.trim();
    const nomeDigitado = document.getElementById("name").value.trim();
    const tipoDeUsuario = document.querySelector('input[name="tipoDeUsuario"]:checked').value;
    const senhaDigitada = document.getElementById("senha").value.trim();
    const mensagem = document.getElementById("message");

    if (senhaDigitada === "") {
        mensagem.style.color = 'red';
        mensagem.textContent = "Por favor, digite sua senha para trocar as informações.";
        return;
    }
    mensagem.style.color = '';
    mensagem.textContent = "Atualizando informações...";

    const usuario = {
        id: id,
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: senhaDigitada,
        funcionario: tipoDeUsuario === 'true',
    };

    fetchAPI('/user/atualizar', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(usuario)
    })
        .then(res => {
            if (!res.ok) throw new Error("Erro na requisição ao atualizar informações.");
            const contentType = res.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                return res.json();
            }
            return null;
        })
        .then(data => {
            if (localStorage.getItem("username") === userDigitado) {
                localStorage.setItem("nome", nomeDigitado);
                localStorage.setItem("tipo", tipoDeUsuario);
            }
            mensagem.style.color = 'green';
            mensagem.textContent = 'Informações atualizadas com sucesso!';

            localStorage.setItem("usernameTroca", "");
            localStorage.setItem("nomeTroca", "");
            localStorage.setItem("tipoTroca", "");
            localStorage.setItem("idTroca", "");

            setTimeout(() => {
                window.location.href = 'dadosperfil.php';
            }, 1500);
        })
        .catch(err => {
            console.error("Erro:", err);
            mensagem.style.color = 'red';
            mensagem.textContent = err.message;
        });
});




});