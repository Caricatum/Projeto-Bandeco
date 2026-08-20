document.addEventListener("DOMContentLoaded", function () {
    const token = sessionStorage.getItem("logado");

    if (!token || token !== "true") {
        window.location.href = "login.php";
        return;
    }

    const user = localStorage.getItem("usernameTroca");
    const nome = localStorage.getItem("nomeTroca");
    const tipo = localStorage.getItem("tipoTroca");
    const id = localStorage.getItem("idTroca");
    const isFuncionarioLogado = localStorage.getItem('tipo') === 'true';
    const loginLogado = (localStorage.getItem('username') || '').toLowerCase();

    // Se não há dados para editar, volta para dadosperfil.php
    if (!id || !user) {
        window.location.href = "dadosperfil.php";
        return;
    }

    // Regra de permissão: Apenas funcionários podem alterar dados de outros usuários
    if (!isFuncionarioLogado && user.toLowerCase() !== loginLogado) {
        alert("Apenas funcionários têm permissão para alterar informações de outros alunos e funcionários.");
        window.location.href = "dadosperfil.php";
        return;
    }

    document.getElementById("username").value = user;
    document.getElementById("name").value = nome;

    if (tipo === "true") {
        document.getElementById("func").checked = true;
        document.getElementById("aluno").checked = false;
    } else {
        document.getElementById("aluno").checked = true;
        document.getElementById("func").checked = false;
    }

    // Se não for funcionário, não pode alterar o próprio tipo (não pode se auto-promover)
    if (!isFuncionarioLogado) {
        document.getElementById("func").disabled = true;
        document.getElementById("aluno").disabled = true;
    }
});

document.getElementById("trocarinfo").addEventListener("click", function () {
    const url = `http://localhost:8080/user/atualizar`;
    const id = localStorage.getItem("idTroca");
    const isFuncionarioLogado = localStorage.getItem('tipo') === 'true';
    const loginLogado = (localStorage.getItem('username') || '').toLowerCase();
    const userOriginal = localStorage.getItem("usernameTroca") || '';

    // Verificação de permissão no envio
    if (!isFuncionarioLogado && userOriginal.toLowerCase() !== loginLogado) {
        alert("Apenas funcionários têm permissão para alterar informações de outros usuários.");
        window.location.href = "dadosperfil.php";
        return;
    }

    const userDigitado = document.getElementById("username").value.trim();
    const nomeDigitado = document.getElementById("name").value.trim();
    const tipoRadio = document.querySelector('input[name="tipoDeUsuario"]:checked');
    const mensagem = document.getElementById("message");

    if (!userDigitado || !nomeDigitado) {
        mensagem.style.color = '#D92243';
        mensagem.textContent = "Por favor, preencha todos os campos.";
        return;
    }

    // Se for funcionário, usa a opção selecionada; se não for, preserva o tipo original
    const tipoFinal = isFuncionarioLogado
        ? (tipoRadio ? tipoRadio.value === 'true' : false)
        : (localStorage.getItem("tipoTroca") === "true");

    const usuario = {
        id: parseInt(id),
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: 'placeholder',
        funcionario: tipoFinal
    };

    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(usuario)
    })
        .then(res => {
            if (!res.ok) throw new Error("Erro ao atualizar informações no servidor.");

            const contentType = res.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                return res.json();
            } else {
                return null;
            }
        })
        .then(data => {
            // Se o usuário editado for o usuário logado, atualiza também a sessão local
            if (loginLogado === userOriginal.toLowerCase()) {
                localStorage.setItem("username", userDigitado);
                localStorage.setItem("nome", nomeDigitado);
                localStorage.setItem("tipo", tipoFinal ? 'true' : 'false');
            }

            mensagem.style.color = 'green';
            mensagem.textContent = 'Informações atualizadas com sucesso!';

            localStorage.setItem("usernameTroca", "");
            localStorage.setItem("nomeTroca", "");
            localStorage.setItem("tipoTroca", "");
            localStorage.setItem("idTroca", "");

            setTimeout(() => {
                window.location.href = 'dadosperfil.php';
            }, 1200);
        })
        .catch(err => {
            console.error("Erro:", err);
            mensagem.style.color = '#D92243';
            mensagem.textContent = "Erro ao atualizar informações. Tente novamente.";
        });
});