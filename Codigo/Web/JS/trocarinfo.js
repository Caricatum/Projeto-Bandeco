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
    const url = `http://localhost:8080/user/atualizar`;
    const id = localStorage.getItem("id");

    const userDigitado = document.getElementById("username").value;
    const nomeDigitado = document.getElementById("name").value;
    const tipoDeUsuario = document.querySelector('input[name="tipoDeUsuario"]:checked').value;
    const senhaDigitada = document.getElementById("senha").value;
    const mensagem = document.getElementById("message");


    if (senhaDigitada == "") {
        mensagem.textContent = "Por favor, digite sua senha para trocar as informações.";
        return;
    }
    if (senhaDigitada !== "") {
        mensagem.textContent = "";
    }



    const usuario = {
        id: id,
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: senhaDigitada,
        funcionario: tipoDeUsuario === 'true',
    }
    const jsonUsuario = JSON.stringify(usuario);

    console.log("tipoDeUsuario:", tipoDeUsuario);
    console.log("nome:", nomeDigitado);

    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: jsonUsuario
    })
        .then(res => {
            if (!res.ok) throw new Error("Erro na requisição");

            const contentType = res.headers.get("content-type");

            if (contentType && contentType.includes("application/json")) {
                return res.json();
            } else {
                return null; // ou res.text()
            }

        })
        .then(data => {
            if (localStorage.getItem("username") == userDigitado) {
                localStorage.setItem("nome", nomeDigitado);
                localStorage.setItem("tipo", tipoDeUsuario);
            }
            mensagem.style = 'Informações atualizadas.';
            mensagem.textContent = 'Informações atualizadas.';

            localStorage.setItem("usernameTroca", "");
            localStorage.setItem("nomeTroca", "");
            localStorage.setItem("tipoTroca", "");
            localStorage.setItem("idTroca", "");

            setTimeout(() => {
            window.location.href = 'dadosperfil.php';
        }, 1500);
        }
        ) //trocar para 
        .catch(err => console.error("Erro:", err));




});