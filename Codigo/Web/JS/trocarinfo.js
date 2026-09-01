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

    // Preenche campos
    document.getElementById("username").value = user || '';
    document.getElementById("name").value = nome || '';

    // Título e subtítulo dinâmicos
    const titulo = document.getElementById("tituloEdicao");
    const subtitulo = document.getElementById("subtituloEdicao");
    if (user.toLowerCase() === loginLogado) {
        titulo.textContent = "Editar Meu Perfil";
        subtitulo.textContent = "Atualize suas informações pessoais";
    } else {
        titulo.textContent = `Editar Usuário: ${user}`;
        subtitulo.textContent = "Gerenciamento administrativo de conta";
    }

    // Controle do Nível de Acesso (Tipo de Pessoa)
    const secaoTipoFunc = document.getElementById("secaoTipoFuncionario");
    const avisoAluno = document.getElementById("avisoTipoAluno");
    const inputTipoValor = document.getElementById("tipoUsuarioValor");
    const cardAluno = document.getElementById("cardAluno");
    const cardFunc = document.getElementById("cardFunc");

    if (isFuncionarioLogado) {
        // Funcionário pode visualizar e alterar o tipo de qualquer conta
        secaoTipoFunc.classList.remove("d-none");
        avisoAluno.classList.add("d-none");

        function selecionarTipo(isFunc) {
            inputTipoValor.value = isFunc ? "true" : "false";
            cardFunc.classList.toggle("selected", isFunc);
            cardAluno.classList.toggle("selected", !isFunc);
        }

        // Estado inicial
        selecionarTipo(tipo === "true");

        cardAluno.addEventListener("click", () => selecionarTipo(false));
        cardFunc.addEventListener("click", () => selecionarTipo(true));
    } else {
        // Aluno não pode alterar seu tipo de acesso
        secaoTipoFunc.classList.add("d-none");
        avisoAluno.classList.remove("d-none");
        inputTipoValor.value = "false";
    }
});

// Envio das alterações
document.getElementById("trocarinfo").addEventListener("click", function () {
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
    const mensagem = document.getElementById("message");
    const btnSalvar = document.getElementById("trocarinfo");

    if (!userDigitado || !nomeDigitado) {
        mensagem.style.color = '#D92243';
        mensagem.textContent = "Por favor, preencha todos os campos.";
        return;
    }

    // Se for funcionário, usa a opção selecionada; se for aluno, mantém como aluno (false)
    const tipoFinal = isFuncionarioLogado
        ? (document.getElementById("tipoUsuarioValor").value === "true")
        : (localStorage.getItem("tipoTroca") === "true");

    const usuario = {
        id: parseInt(id),
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: 'placeholder',
        funcionario: tipoFinal
    };

    mensagem.style.color = '';
    mensagem.textContent = "Salvando alterações...";
    btnSalvar.disabled = true;

    fetchAPI('/user/atualizar', {
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
            }
            return null;
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
            }, 1000);
        })
        .catch(err => {
            console.error("Erro:", err);
            mensagem.style.color = '#D92243';
            mensagem.textContent = err.message || "Erro ao atualizar informações. Tente novamente.";
            btnSalvar.disabled = false;
        });
});