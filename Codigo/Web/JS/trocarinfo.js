import { validaFunc } from './validaFunc.js';

validaFunc();

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



    const user = localStorage.getItem("username");
    const nome = localStorage.getItem("nome");
    const tipo = localStorage.getItem("tipo");
    const id = localStorage.getItem("id");


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
    const mensagemErro = document.getElementById("message");

    if (senhaDigitada == "") {
        mensagemErro.textContent = "Por favor, digite sua senha para trocar as informações.";
        return;
    }



    const usuario = {
        id: id,
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: senhaDigitada,
        funcionario: tipoDeUsuario === 'true',
    }
    const jsonUsuario = JSON.stringify(usuario);

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
                localStorage.setItem("nome") = nomeDigitado;
                localStorage.setItem("tipo") = nomeDigitado;
            }
        }
        ) //trocar para 
        .catch(err => console.error("Erro:", err));

    window.location.href = 'dadosperfil.php';


});