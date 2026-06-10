<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Mural de avisos e Cardápio</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- CSS da paleta -->
    <link rel="stylesheet" href="../CSS/mural.css">

    <!-- JS -->
    <script src="../JS/mural.js" defer></script>
</head>

<body>

    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg shadow-sm">
        <div class="container">

            <img src="../Assets/Images/logo_unicamp.jpg" alt="Logo Unicamp" width="40" height="40"
                class="d-inline-block align-text-top">

            <a class="navbar-brand ms-3" href="inicio.php">Início</a>

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto">

                    <li class="nav-item">
                        <a class="nav-link" href="buscaPratos.php">
                            <button class="btn btn-dark">Buscar Pratos</button>
                        </a>
                    </li>

                    <!-- Botão Login: visível se não logado, some se logado (via JS) -->
                    <li class="nav-item" id="itemLogin">
                        <a class="nav-link" href="login.php">
                            <button id="btnLogin" class="btn btn-dark">Login</button>
                        </a>
                    </li>

                </ul>
            </div>
        </div>
    </nav>


    <!-- CONTAINER AVISOS -->
    <div class="container mt-5">

        <div class="d-flex justify-content-between align-items-center mb-4">
            <h1>📌 Mural de Avisos</h1>
            <button id="btnNovoAviso" class="btn btn-primary d-none">+ Novo Aviso</button>
        </div>

        <div id="mural" class="row g-3">
            <!-- Avisos inseridos via JS -->
        </div>

    </div>


    <!-- MODAL NOVO AVISO -->
    <div class="modal fade" id="modalAviso" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Criar Aviso</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label class="form-label">Título</label>
                        <input type="text" id="tituloAviso" class="form-control">
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Descrição</label>
                        <textarea id="descricaoAviso" class="form-control"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button id="salvarAviso" class="btn btn-success">Salvar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL CARDÁPIO DO DIA -->
    <div class="modal fade" id="modalCardapioDia" tabindex="-1">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Cadastrar Cardápio do Dia</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <div class="container text-center">
                        <h3>Almoço</h3>
                        <div class="row">
                            <div class="col-md-6 text-center"> <!-- Primeira coluna com form-->
                                <h4 class="titulo-padrao">Padrão</h4> <!--Almoço padrão-->
                                <form class="form-padrao">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label> <br>
                                        Arroz e Feijão
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    Pão e Café
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Nota Técnica</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                </form>
                            </div>

                            <div class="col-md-6"> <!-- Segunda coluna com form-->
                                <h4 class="titulo-vegano">Vegano</h4> <!--Almoço vegano-->
                                <form class="form-vegano">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label> <br>
                                        Arroz Integral e Feijão
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    Pão e Café
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Nota Técnica</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                </form>
                            </div>
                        </div>
                        <h3>Jantar</h3>
                        <div class="row">
                            <div class="col-md-6"> <!-- Terceira coluna com form-->
                                <h4 class="titulo-padrao">Padrão</h4> <!--Jantar padrão-->
                                <form class="form-padrao">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label> <br>
                                        Arroz e Feijão
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                    <hr>
                                    Pão e Café
<hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Nota Técnica</b></label>
                                        <input type="text" class="form-control">
                                    </div>
                                </form>
                            </div>

                            <div class="col-md-6"> <!-- Quarta coluna com form-->
                                <h4 class="titulo-vegano">Vegano</h4> <!--Jantar vegano-->
                                <form class="form-vegano">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label> <br>
                                        Arroz e Feijão
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                    <input type="text" class="form-control" id="">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <input type="text" class="form-control" id="">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <input type="text" class="form-control" id="">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <input type="text" class="form-control" id="sobremesaVJ">
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <input type="text" class="form-control" id="refrescoVJ">
                                    </div>
                                    <hr>
                                    Pão e Café
<hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Nota Técnica</b></label>
                                        <input type="text" class="form-control" id="nt">
                                    </div>
                                </form>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer d-flex justify-content-center">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">
                        Cancelar
                    </button>

                    <button id="salvarCardapioDia" class="btn btn-success">
                        Salvar
                    </button>
                </div>

                <div class="text-center mt-3 mb-3">
                    <label for="dataCardapio" class="form-label">
                        <h5><b>Digite a data que será servida esse cardápio:</b></h5>
                    </label>

                    <input type="date" id="dataCardapio" class="form-control w-auto mx-auto">

                </div>
            </div>
        </div>
    </div>


    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>


    <!-- CARDÁPIO DO DIA -->
    <div class="container mt-4 mb-5">

        <h1 class="text-center mb-4">🍽️ Cardápio do Dia</h1>

        <div class="d-flex justify-content-center align-items-center mb-4">
            <button id="btnCardapioDia" class="btn btn-primary d-none">Cadastrar Cardápio do dia</button>
        </div>

        <!-- Preenchido via JS (mural.js → carregarCardapioDia) -->
        <div id="areaCardapio" class="row g-3">
            <div class="col-12 text-center py-5">
                <div class="spinner-border text-primary"></div>
                <p class="mt-2 text-muted">Carregando cardápio...</p>
            </div>
        </div>

    </div>


</body>

</html>