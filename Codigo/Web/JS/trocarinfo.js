const API = typeof API_BASE_URL !== 'undefined' ? API_BASE_URL : 'http://localhost:8080';

let arquivoFotoSelecionado = null;
let fotoFoiRemovida = false;
let fotoOriginalUrl = null;

// Helper de resolução de URL de imagem
function resolverUrlImagem(url) {
    if (!url) return '';
    if (url.startsWith('http://') || url.startsWith('https://')) {
        return url;
    }
    const nomeArquivo = url.substring(url.lastIndexOf('/') + 1);
    return `imagem.php?arquivo=${encodeURIComponent(nomeArquivo)}`;
}

window.mostrarIconePadraoAvatar = function() {
    const avatarFoto = document.getElementById('avatarFoto');
    const avatarIcon = document.getElementById('avatarIcon');
    if (avatarFoto) avatarFoto.classList.add('d-none');
    if (avatarIcon) avatarIcon.classList.remove('d-none');
};

document.addEventListener("DOMContentLoaded", async function () {
    const token = sessionStorage.getItem("logado");

    if (!token || token !== "true") {
        window.location.href = "login.php";
        return;
    }

    const usuarioSessao = (typeof window.USUARIO_ATUAL !== 'undefined' && window.USUARIO_ATUAL) ? window.USUARIO_ATUAL : {
        id: parseInt(localStorage.getItem('id')) || null,
        username: localStorage.getItem('username') || '',
        nome: localStorage.getItem('nome') || '',
        funcionario: localStorage.getItem('tipo') === 'true'
    };
    const isFuncionarioLogado = Boolean(usuarioSessao.funcionario);
    const loginLogado = (usuarioSessao.username || '').toLowerCase();

    let user = localStorage.getItem("usernameTroca");
    let nome = localStorage.getItem("nomeTroca");
    let tipo = localStorage.getItem("tipoTroca");
    let id = localStorage.getItem("idTroca");

    // Aluno não pode trocar dados de terceiros
    if (!isFuncionarioLogado) {
        user = usuarioSessao.username;
        id = usuarioSessao.id;
        if (!nome) nome = usuarioSessao.nome;
    }

    // Se não há dados para editar, volta para dadosperfil.php
    if (!id || !user) {
        window.location.href = "dadosperfil.php";
        return;
    }

    // Preenche campos básicos
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

    // Busca dados completos do usuário na API para preencher foto e alergias
    try {
        const res = await fetchAPI(`/user/login/${encodeURIComponent(user)}`);
        if (res.ok) {
            const dadosCompletos = await res.json();
            
            // Foto de Perfil
            if (dadosCompletos.imagem) {
                fotoOriginalUrl = dadosCompletos.imagem;
                const avatarFoto = document.getElementById('avatarFoto');
                const avatarIcon = document.getElementById('avatarIcon');
                const btnRemover = document.getElementById('btnRemoverFoto');
                
                avatarFoto.src = resolverUrlImagem(dadosCompletos.imagem);
                avatarFoto.classList.remove('d-none');
                avatarIcon.classList.add('d-none');
                btnRemover.classList.remove('d-none');
            }

            // Alergias
            document.getElementById('alergiaLeite').checked = Boolean(dadosCompletos.alergiaLeite);
            document.getElementById('alergiaOvo').checked = Boolean(dadosCompletos.alergiaOvo);
            document.getElementById('alergiaAmendoim').checked = Boolean(dadosCompletos.alergiaAmendoim);
            document.getElementById('alergiaCastanhas').checked = Boolean(dadosCompletos.alergiaCastanhas);
            document.getElementById('alergiaFrutosDoMar').checked = Boolean(dadosCompletos.alergiaFrutosDoMar);
            document.getElementById('alergiaPeixes').checked = Boolean(dadosCompletos.alergiaPeixes);
            document.getElementById('alergiaTrigo').checked = Boolean(dadosCompletos.alergiaTrigo);
            document.getElementById('alergiaSoja').checked = Boolean(dadosCompletos.alergiaSoja);
            document.getElementById('alergiaGergilim').checked = Boolean(dadosCompletos.alergiaGergilim);
        }
    } catch (e) {
        console.warn("Não foi possível carregar detalhes extras do usuário:", e);
    }

    // Configura o seletor de foto
    const inputFoto = document.getElementById('fotoPerfil');
    const avatarFoto = document.getElementById('avatarFoto');
    const avatarIcon = document.getElementById('avatarIcon');
    const btnRemoverFoto = document.getElementById('btnRemoverFoto');

    if (inputFoto) {
        inputFoto.addEventListener('change', function (e) {
            const file = e.target.files[0];
            if (!file) return;

            if (!file.type.startsWith('image/')) {
                alert('Selecione uma imagem válida (JPG, PNG ou WEBP).');
                inputFoto.value = '';
                return;
            }

            arquivoFotoSelecionado = file;
            fotoFoiRemovida = false;

            const reader = new FileReader();
            reader.onload = function (evt) {
                avatarFoto.src = evt.target.result;
                avatarFoto.classList.remove('d-none');
                avatarIcon.classList.add('d-none');
                btnRemoverFoto.classList.remove('d-none');
            };
            reader.readAsDataURL(file);
        });
    }

    if (btnRemoverFoto) {
        btnRemoverFoto.addEventListener('click', function () {
            arquivoFotoSelecionado = null;
            fotoFoiRemovida = true;
            if (inputFoto) inputFoto.value = '';
            avatarFoto.src = '';
            avatarFoto.classList.add('d-none');
            avatarIcon.classList.remove('d-none');
            btnRemoverFoto.classList.add('d-none');
        });
    }

    // Controle do Nível de Acesso (Tipo de Pessoa)
    const secaoTipoFunc = document.getElementById("secaoTipoFuncionario");
    const avisoAluno = document.getElementById("avisoTipoAluno");
    const inputTipoValor = document.getElementById("tipoUsuarioValor");
    const cardAluno = document.getElementById("cardAluno");
    const cardFunc = document.getElementById("cardFunc");

    if (isFuncionarioLogado) {
        secaoTipoFunc.classList.remove("d-none");
        avisoAluno.classList.add("d-none");

        function selecionarTipo(isFunc) {
            inputTipoValor.value = isFunc ? "true" : "false";
            cardFunc.classList.toggle("selected", isFunc);
            cardAluno.classList.toggle("selected", !isFunc);
        }

        selecionarTipo(tipo === "true");

        cardAluno.addEventListener("click", () => selecionarTipo(false));
        cardFunc.addEventListener("click", () => selecionarTipo(true));
    } else {
        secaoTipoFunc.classList.add("d-none");
        avisoAluno.classList.remove("d-none");
        inputTipoValor.value = "false";
    }
});

// Envio das alterações
document.getElementById("trocarinfo").addEventListener("click", async function () {
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

    const tipoFinal = isFuncionarioLogado
        ? (document.getElementById("tipoUsuarioValor").value === "true")
        : (localStorage.getItem("tipoTroca") === "true");

    // Se o usuário clicou em remover foto e não selecionou uma nova, chama o endpoint de exclusão de imagem
    if (fotoFoiRemovida && !arquivoFotoSelecionado && fotoOriginalUrl && id) {
        try {
            await fetchAPI(`/user/excluirImagem/${id}`, { method: 'DELETE' });
        } catch (e) {
            console.warn("Aviso ao excluir imagem:", e);
        }
    }

    const usuario = {
        id: parseInt(id),
        login: userDigitado,
        nome: nomeDigitado,
        senhaHash: 'placeholder',
        funcionario: tipoFinal,
        alergiaLeite: document.getElementById('alergiaLeite')?.checked || false,
        alergiaOvo: document.getElementById('alergiaOvo')?.checked || false,
        alergiaAmendoim: document.getElementById('alergiaAmendoim')?.checked || false,
        alergiaCastanhas: document.getElementById('alergiaCastanhas')?.checked || false,
        alergiaFrutosDoMar: document.getElementById('alergiaFrutosDoMar')?.checked || false,
        alergiaPeixes: document.getElementById('alergiaPeixes')?.checked || false,
        alergiaTrigo: document.getElementById('alergiaTrigo')?.checked || false,
        alergiaSoja: document.getElementById('alergiaSoja')?.checked || false,
        alergiaGergilim: document.getElementById('alergiaGergilim')?.checked || false
    };

    // Constrói o FormData com part "user" e opcional "imagem"
    const formData = new FormData();
    formData.append('user', new Blob([JSON.stringify(usuario)], { type: 'application/json' }));
    if (arquivoFotoSelecionado) {
        formData.append('imagem', arquivoFotoSelecionado);
    }

    mensagem.style.color = '';
    mensagem.textContent = "Salvando alterações...";
    btnSalvar.disabled = true;

    try {
        const res = await fetchAPI('/user/atualizar', {
            method: 'PUT',
            body: formData
        });

        if (!res.ok) {
            const erroTexto = await res.text();
            throw new Error(erroTexto || "Erro ao atualizar informações no servidor.");
        }

        // Se o usuário editado for o usuário logado, atualiza também a sessão local
        if (loginLogado === userOriginal.toLowerCase()) {
            localStorage.setItem("username", userDigitado);
            localStorage.setItem("nome", nomeDigitado);
            localStorage.setItem("tipo", tipoFinal ? 'true' : 'false');

            // Atualiza também a sessão segura do servidor PHP
            try {
                await fetch('auth.php?action=login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        id: parseInt(id),
                        nome: nomeDigitado,
                        login: userDigitado,
                        funcionario: tipoFinal
                    })
                });
            } catch (authErr) {
                console.warn("Falha na sincronização da sessão PHP:", authErr);
            }
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

    } catch (err) {
        console.error("Erro:", err);
        mensagem.style.color = '#D92243';
        mensagem.textContent = err.message || "Erro ao atualizar informações. Tente novamente.";
        btnSalvar.disabled = false;
    }
});
