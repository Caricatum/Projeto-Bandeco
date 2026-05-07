    document.addEventListener("DOMContentLoaded", function () {
        
        localStorage.setItem("")
        
    });


    // =========================
    // USUÁRIO LOGADO
    // =========================

    const usuarioLogado = {
        nome: "Caue",
        tipoDeUsuario: true
    };


    // =========================
    // PEGAR AVISOS DO LOCALSTORAGE
    // =========================

    let avisos = JSON.parse(localStorage.getItem("avisos"));



    // =========================
    // SE NÃO EXISTIR NADA
    // =========================

    if (!avisos) {

        avisos = [

            {
                titulo: "Prova de Matemática",
                descricao: "Dia 10/05 - Conteúdo: Análise Combinatória",
                cor: "warning"
            },

            {
                titulo: "Entrega de Projeto",
                descricao: "Prazo final: 15/05",
                cor: "info"
            },

            {
                titulo: "Reunião",
                descricao: "Hoje às 14h na sala 3",
                cor: "success"
            }

        ];

        // salva os avisos iniciais
        localStorage.setItem(
            "avisos",
            JSON.stringify(avisos)
        );
    }


    // =========================
    // ELEMENTOS
    // =========================

    const mural = document.getElementById("mural");

    const btnNovoAviso =
        document.getElementById("btnNovoAviso");

    const modal = new bootstrap.Modal(
        document.getElementById("modalAviso")
    );


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
