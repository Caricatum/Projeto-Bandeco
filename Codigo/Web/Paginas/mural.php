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
    <script type="module" src="../JS/mural.js" defer></script>
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
    <!-- ============================================================
     MODAL: CADASTRAR CARDÁPIO DO DIA
     Monta 4 cardápios (almoço padrão/vegano, jantar padrão/vegano)
     e envia tudo junto para /cardapioDia/cadastrar
============================================================ -->
<div class="modal fade" id="modalCardapioDia" tabindex="-1">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
 
            <div class="modal-header">
                <h5 class="modal-title">📅 Cadastrar Cardápio do Dia</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
 
            <div class="modal-body">
 
                <div class="mb-4">
                    <label class="form-label"><b>Data do cardápio</b></label>
                    <input type="date" id="dataCardapioDia" class="form-control" style="max-width: 250px;">
                </div>
 
                <div class="row g-3">
 
                    <!-- ===== ALMOÇO PADRÃO ===== -->
                    <div class="col-md-6">
                        <div class="card h-100">
                            <div class="card-header bg-primary text-white">🌞 Almoço — Padrão</div>
                            <div class="card-body">
                                <div class="mb-2">
                                    <label class="form-label">Acompanhamento</label>
                                    <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="acompanhamento" data-categoria="1">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Prato Principal</label>
                                    <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="prato_principal" data-categoria="3">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Guarnição</label>
                                    <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="guarnicao" data-categoria="2">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Salada</label>
                                    <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="salada" data-categoria="5">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Sobremesa</label>
                                    <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="sobremesa" data-categoria="6">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-0">
                                    <label class="form-label">Refresco</label>
                                    <select class="form-select select-prato" data-cardapio="padraoAlmoco" data-campo="refresco" data-categoria="4">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
 
                    <!-- ===== ALMOÇO VEGANO ===== -->
                    <div class="col-md-6">
                        <div class="card h-100">
                            <div class="card-header" style="background:#E0C375; color:#3d2a00;">🥦 Almoço — Vegano</div>
                            <div class="card-body">
                                <div class="mb-2">
                                    <label class="form-label">Acompanhamento</label>
                                    <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="acompanhamento" data-categoria="1" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Prato Principal</label>
                                    <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="prato_principal" data-categoria="3" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Guarnição</label>
                                    <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="guarnicao" data-categoria="2" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Salada</label>
                                    <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="salada" data-categoria="5" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Sobremesa</label>
                                    <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="sobremesa" data-categoria="6" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-0">
                                    <label class="form-label">Refresco</label>
                                    <select class="form-select select-prato" data-cardapio="veganoAlmoco" data-campo="refresco" data-categoria="4" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
 
                    <!-- ===== JANTAR PADRÃO ===== -->
                    <div class="col-md-6">
                        <div class="card h-100">
                            <div class="card-header bg-dark text-white">🌙 Jantar — Padrão</div>
                            <div class="card-body">
                                <div class="mb-2">
                                    <label class="form-label">Acompanhamento</label>
                                    <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="acompanhamento" data-categoria="1">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Prato Principal</label>
                                    <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="prato_principal" data-categoria="3">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Guarnição</label>
                                    <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="guarnicao" data-categoria="2">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Salada</label>
                                    <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="salada" data-categoria="5">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Sobremesa</label>
                                    <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="sobremesa" data-categoria="6">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-0">
                                    <label class="form-label">Refresco</label>
                                    <select class="form-select select-prato" data-cardapio="padraoJantar" data-campo="refresco" data-categoria="4">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
 
                    <!-- ===== JANTAR VEGANO ===== -->
                    <div class="col-md-6">
                        <div class="card h-100">
                            <div class="card-header" style="background:#7a1728; color:#FFF5E5;">🥦 Jantar — Vegano</div>
                            <div class="card-body">
                                <div class="mb-2">
                                    <label class="form-label">Acompanhamento</label>
                                    <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="acompanhamento" data-categoria="1" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Prato Principal</label>
                                    <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="prato_principal" data-categoria="3" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Guarnição</label>
                                    <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="guarnicao" data-categoria="2" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Salada</label>
                                    <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="salada" data-categoria="5" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Sobremesa</label>
                                    <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="sobremesa" data-categoria="6" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                                <div class="mb-0">
                                    <label class="form-label">Refresco</label>
                                    <select class="form-select select-prato" data-cardapio="veganoJantar" data-campo="refresco" data-categoria="4" data-vegano="true">
                                        <option value="">Nenhum</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
 
                </div>
 
                <p id="msgCardapioDia" class="text-danger mt-3 mb-0"></p>
            </div>
 
            <div class="modal-footer">
                <button class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <button id="salvarCardapioDia" class="btn btn-success">💾 Salvar Cardápio do Dia</button>
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