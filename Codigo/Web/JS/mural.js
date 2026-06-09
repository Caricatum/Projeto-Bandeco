    document.addEventListener("DOMContentLoaded", function () {
        
        localStorage.setItem("");
        
        if(sessionStorage.getItem('logado') === 'true'){
            document.getElementById("btnLogin").classList.add = ("d-none");
        }

        const url = `http://localhost:8080/user/login/${localStorage.getItem("username").value}`;

    fetch(url, {
        method: 'GET',
    })
        .then(res => {
            if (!res.ok) { throw new Error("Erro na requisição."); }
            return res.json();
        })
        .then(data => {
            const nomeBanco = data.nome;
            const tipoDeUsuarioBanco = data.funcionario;

            localStorage.setItem("username", localStorage.getItem("username"));
            localStorage.setItem("nome", nomeBanco);
            localStorage.setItem("tipo", tipoDeUsuarioBanco);
        })
        .catch(error => {
            console.error("Erro:", error);
            document.getElementById("message").innerText = "Erro na requisição.";
        });


});


    // =========================
    // USUÁRIO LOGADO
    // =========================

    const usuarioLogado = {
        nome: localStorage.getItem("nome"),
        username: localStorage.getItem("username"),
        tipoDeUsuario: localStorage.getItem("tipo"),
    };


    // =========================
    // PEGAR AVISOS DO LOCALSTORAGE
    // =========================

    let avisos = JSON.parse(localStorage.getItem("avisos"));

    // =========================
    // ELEMENTOS
    // =========================

    const mural = document.getElementById("mural");

    const btnNovoAviso = document.getElementById("btnNovoAviso");

    const modal = new bootstrap.Modal(document.getElementById("modalAviso"));


    // =========================
    // MOSTRAR BOTÃO
    // =========================

    if (usuarioLogado.tipoDeUsuario === true) {

        btnNovoAviso.classList.remove("d-none");

    }


    // =========================
    // RENDERIZAR AVISOS
    // =========================

    function renderizarAvisos() {

        mural.innerHTML = "";

        if (avisos.length === 0) {

    // =========================
    // SE NÃO EXISTIR NADA
    // =========================

        mural.innerHTML = `
            <div class="col-12">
                <div class="p-5 text-center rounded border bg-light-subtle">
                    <h4>📭 Por hoje não tem nada</h4>
                    <p class="text-muted mb-0">
                        Nenhum aviso foi publicado até o momento.
                    </p>
                </div>
            </div>
        `;

        return;
    }

        avisos.forEach((aviso, index) => {

            mural.innerHTML += `

                <div class="col-md-6">

                    <div class="alert alert-${aviso.cor} shadow-sm">

                        <div class="d-flex justify-content-between">

                            <h5 class="fw-bold">
                                ${aviso.titulo}
                            </h5>

                            ${
                                usuarioLogado.tipoDeUsuario
                                ?
                                `<button 
                                    class="btn btn-danger btn-sm"
                                    onclick="deletarAviso(${index})">
                                    X
                                </button>`
                                :
                                ""
                            }

                        </div>

                        <p class="mb-0">
                            ${aviso.descricao}
                        </p>

                    </div>

                </div>

            `;
        });

    }


    // =========================
    // ABRIR MODAL
    // =========================

    btnNovoAviso.addEventListener("click", () => {

        modal.show();

    });


    // =========================
    // SALVAR AVISO
    // =========================

    document.getElementById("salvarAviso")
        .addEventListener("click", () => {

            const titulo =
                document.getElementById("tituloAviso").value;

            const descricao =
                document.getElementById("descricaoAviso").value;


            if (titulo === "" || descricao === "") {

                alert("Preencha todos os campos!");

                return;
            }


            const novoAviso = {

                titulo: titulo,
                descricao: descricao,
                cor: "primary"

            };


            // adiciona aviso
            avisos.push(novoAviso);


            // salva no localStorage
            localStorage.setItem(
                "avisos",
                JSON.stringify(avisos)
            );


            // atualiza tela
            renderizarAvisos();


            // limpa inputs
            document.getElementById("tituloAviso").value = "";

            document.getElementById("descricaoAviso").value = "";


            // fecha modal
            modal.hide();

        });


    // =========================
    // DELETAR AVISO
    // =========================

    function deletarAviso(index) {

        avisos.splice(index, 1);

        localStorage.setItem(
            "avisos",
            JSON.stringify(avisos)
        );

        renderizarAvisos();

    }


    // =========================
    // INICIAR
    // =========================

    renderizarAvisos();
