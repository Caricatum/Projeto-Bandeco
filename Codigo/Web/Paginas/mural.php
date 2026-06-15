<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Mural de avisos e Cardápio</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- CSS da paleta -->
    <link rel="stylesheet" href="../CSS/navbar.css">
    <link rel="stylesheet" href="../CSS/mural.css">

    <!-- JS -->
    <script src="../JS/mural.js" defer></script>
</head>

<body>

    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg">
        <div class="container">

            <img src="../Assets/Images/logo_unicamp.jpg" alt="Logo Unicamp" width="40" height="40"
                class="d-inline-block align-text-top">

            <a class="navbar-brand ms-3" href="inicio.php">Bandeco</a>

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto gap-2">

                    <li class="nav-item">
                        <button class="btn-nav" onclick="window.location.href='inicio.php'">🏠 Início</button>
                    </li>

                    <li class="nav-item">
                        <button class="btn-nav" onclick="window.location.href='buscaPratos.php'">🍽️ Buscar Pratos</button>
                    </li>

                    <li class="nav-item">
                        <button class="btn-nav" onclick="window.location.href='sobrenos.php'">ℹ️ Sobre nós</button>
                    </li>

                    <!-- Botão Login: visível se não logado, some se logado (via JS) -->
                    <li class="nav-item" id="itemLogin">
                        <button id="btnLogin" class="btn-nav" onclick="window.location.href='login.php'">Login</button>
                    </li>

                    <li class="nav-item">
                    <button class="btn-nav btn-sair" onclick="logout()">Sair</button>
                    </li>

                </ul>
            </div>
        </div>
    </nav>

    <script>
            // =============================================
// LOGOUT
// =============================================
function logout() {
    sessionStorage.setItem('logado', 'false');
    localStorage.clear();
    window.location.href = 'login.php';
}

    </script>



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
                    <h5 class="modal-title">📅 Cadastrar Cardápio do Dia</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">

                    <div class="text-center mb-4">
                        <label for="dataCardapio" class="form-label">
                            <h5 class="mb-1"><b>Data em que esse cardápio será servido:</b></h5>
                        </label>
                        <input type="date" id="dataCardapio" class="form-control w-auto mx-auto">
                    </div>

                    <div class="container text-center">

                        <h3>Almoço</h3>
                        <div class="row">

                            <!-- ALMOÇO PADRÃO -->
                            <div class="col-md-6 text-center">
                                <h4 class="titulo-padrao">Padrão</h4>
                                <form class="form-padrao">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="acompanhamento" data-categoria="1">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="prato_principal" data-categoria="3">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="guarnicao" data-categoria="2">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="salada" data-categoria="5">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="sobremesa" data-categoria="6">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="refresco" data-categoria="4">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                </form>
                            </div>

                            <!-- ALMOÇO VEGANO -->
                            <div class="col-md-6">
                                <h4 class="titulo-vegano">Vegano</h4>
                                <form class="form-vegano">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="acompanhamento" data-categoria="1" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="prato_principal" data-categoria="3" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="guarnicao" data-categoria="2" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="salada" data-categoria="5" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="sobremesa" data-categoria="6" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="refresco" data-categoria="4" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                </form>
                            </div>
                        </div>

                        <h3>Jantar</h3>
                        <div class="row">

                            <!-- JANTAR PADRÃO -->
                            <div class="col-md-6">
                                <h4 class="titulo-padrao">Padrão</h4>
                                <form class="form-padrao">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="acompanhamento" data-categoria="1">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="prato_principal" data-categoria="3">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="guarnicao" data-categoria="2">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="salada" data-categoria="5">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="sobremesa" data-categoria="6">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="refresco" data-categoria="4">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                </form>
                            </div>

                            <!-- JANTAR VEGANO -->
                            <div class="col-md-6">
                                <h4 class="titulo-vegano">Vegano</h4>
                                <form class="form-vegano">
                                    <div class="mb-3">
                                        <label class="form-label"><b>Acompanhamento</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="acompanhamento" data-categoria="1" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Prato Principal</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="prato_principal" data-categoria="3" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Guarnição</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="guarnicao" data-categoria="2" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Salada</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="salada" data-categoria="5" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Sobremesa</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="sobremesa" data-categoria="6" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                    <hr>
                                    <div class="mb-3">
                                        <label class="form-label"><b>Refresco</b></label>
                                        <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="refresco" data-categoria="4" data-vegano="true">
                                            <option value="">Nenhum</option>
                                        </select>
                                    </div>
                                </form>
                            </div>
                        </div>

                    </div>

                    <p id="msgCardapioDia" class="text-danger text-center mt-3 mb-0"></p>
                </div>

                <div class="modal-footer d-flex justify-content-center">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">
                        Cancelar
                    </button>

                    <button id="salvarCardapioDia" class="btn btn-success">
                        Salvar
                    </button>
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