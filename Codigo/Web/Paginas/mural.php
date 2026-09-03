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
    <script src="../JS/config.js" defer></script>
    <script src="../JS/mural.js" defer></script>
</head>

<body>

    <!-- Navbar Modularizada -->
    <?php include __DIR__ . '/components/navbar.php'; ?>




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


    <!-- CARDÁPIO DO DIA E DA SEMANA -->
    <div class="container mt-4 mb-5">

        <h1 class="text-center mb-4">🍽️ Cardápio do Restaurante</h1>

        <!-- Botões de Ação para Funcionários -->
        <div class="d-flex justify-content-center align-items-center gap-2 mb-4 flex-wrap">
            <button id="btnCardapioDia" class="btn btn-primary d-none">
                ➕ Cadastrar Cardápio
            </button>
        <!-- <button id="btnNotificarFavoritos" class="btn btn-warning d-none text-dark fw-bold" onclick="notificarFavoritosHoje()">
                🔔 Notificar Pratos Favoritos por E-mail
            </button>-->
        </div>

        <!-- Abas de Navegação (Dia vs Semana) -->
        <ul class="nav nav-pills justify-content-center mb-4" id="cardapioTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active fw-bold px-4" id="tab-dia-btn" data-bs-toggle="pill" data-bs-target="#tab-dia" type="button" role="tab">
                    📅 Cardápio por Dia
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link fw-bold px-4" id="tab-semana-btn" data-bs-toggle="pill" data-bs-target="#tab-semana" type="button" role="tab" onclick="carregarCardapioSemana()">
                    🗓️ Cardápio da Semana
                </button>
            </li>
        </ul>

        <div class="tab-content" id="cardapioTabContent">
            <!-- ABA 1: CARDÁPIO POR DIA -->
            <div class="tab-pane fade show active" id="tab-dia" role="tabpanel">
                <!-- Barra de Navegação de Datas -->
                <div class="card p-3 mb-4 shadow-sm">
                    <div class="d-flex justify-content-between align-items-center flex-wrap gap-2">
                        <div class="btn-group" role="group">
                            <button type="button" class="btn btn-outline-danger" id="btnDiaAnterior" onclick="mudarDataRelativa(-1)">
                                ⬅️ Dia Anterior
                            </button>
                            <button type="button" class="btn btn-outline-danger" id="btnDiaHoje" onclick="irParaHoje()">
                                📍 Hoje
                            </button>
                            <button type="button" class="btn btn-outline-danger" id="btnDiaProximo" onclick="mudarDataRelativa(1)">
                                Próximo Dia ➡️
                            </button>
                        </div>
                        <div class="d-flex align-items-center gap-2">
                            <label for="seletorDataCardapio" class="fw-bold text-muted mb-0">Data:</label>
                            <input type="date" id="seletorDataCardapio" class="form-control" style="max-width: 190px;" onchange="aoMudarSeletorData(this.value)">
                        </div>
                    </div>
                </div>

                <!-- Preenchido via JS (mural.js → carregarCardapioDia) -->
                <div id="areaCardapio" class="row g-3">
                    <div class="col-12 text-center py-5">
                        <div class="spinner-border text-primary"></div>
                        <p class="mt-2 text-muted">Carregando cardápio...</p>
                    </div>
                </div>
            </div>

            <!-- ABA 2: CARDÁPIO DA SEMANA -->
            <div class="tab-pane fade" id="tab-semana" role="tabpanel">
                <div id="areaCardapioSemana" class="row g-4">
                    <div class="col-12 text-center py-5">
                        <div class="spinner-border text-primary"></div>
                        <p class="mt-2 text-muted">Carregando cardápios da semana...</p>
                    </div>
                </div>
            </div>
        </div>

    </div>


</body>