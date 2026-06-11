<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Buscar Pratos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/buscaPratos.css">
    <style>
        body {
            background: #f8fafc;
        }

        .card-prato {
            transition: transform 0.2s ease, box-shadow 0.2s ease;
            border: none;
            box-shadow: 0 2px 12px rgba(0, 0, 0, 0.08);
        }

        .card-prato:hover {
            transform: translateY(-3px);
            box-shadow: 0 6px 24px rgba(0, 0, 0, 0.13);
        }

        .badge-vegano {
            background: #16a34a;
            font-size: 0.7rem;
        }

        .badge-categoria {
            background: #2563eb;
            font-size: 0.7rem;
        }

        /* Botão de like (👍) */
        .btn-like {
            border: 2px solid #d1d5db;
            background: white;
            color: #6b7280;
            border-radius: 20px;
            padding: 4px 12px;
            font-size: 0.85rem;
            transition: all 0.2s;
            cursor: pointer;
        }

        .btn-like:hover,
        .btn-like.ativo {
            border-color: #2563eb;
            color: #2563eb;
            background: #eff6ff;
        }

        .btn-like.ativo {
            font-weight: bold;
        }

        /* Botão de favoritar (⭐) */
        .btn-fav {
            border: 2px solid #d1d5db;
            background: white;
            color: #6b7280;
            border-radius: 20px;
            padding: 4px 12px;
            font-size: 0.85rem;
            transition: all 0.2s;
            cursor: pointer;
        }

        .btn-fav:hover,
        .btn-fav.ativo {
            border-color: #d97706;
            color: #d97706;
            background: #fffbeb;
        }

        .btn-fav.ativo {
            font-weight: bold;
        }

        #filtros {
            gap: 10px;
            flex-wrap: wrap;
        }

        .spinner-area {
            min-height: 200px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        /* Modal estrelas */
        .estrela {
            font-size: 2rem;
            cursor: pointer;
            color: #d1d5db;
            transition: color 0.15s;
        }

        .estrela:hover,
        .estrela.selecionada {
            color: #f59e0b;
        }
    </style>
</head>

<body>

    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg bg-body-tertiary shadow-sm">
        <div class="container">
            <img src="../Assets/Images/logo_unicamp.jpg" alt="Logo Unicamp" width="40" height="40" class="d-inline-block align-text-top">
            <a class="navbar-brand ms-3" href="inicio.php">Início</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="mural.php"><button class="btn btn-dark">Mural</button></a>
                    </li>
                    <li class="nav-item">
                        <button class="btn btn-outline-danger ms-2" onclick="logout()">Sair</button>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <!-- CONTEÚDO -->
    <div class="container mt-4">

        <div class="d-flex justify-content-between align-items-center mb-4 flex-wrap gap-2">
            <h1>🍽️ Buscar Pratos</h1>
            <div class="d-flex gap-2 flex-wrap">
                <!-- Botão só para funcionários -->
                <button id="btnNovoPrato" class="btn btn-success d-none">
                    + Novo Prato
                </button>
                <button id="btnMesFavoritos" class="btn btn-warning" onclick="window.location.href='meusFavoritos.php'">
                    ⭐ Meus Favoritos
                </button>
            </div>
        </div>

        <!-- FILTROS -->
        <div class="card p-3 mb-4 shadow-sm">
            <div class="d-flex align-items-center" id="filtros">
                <input type="text" id="campoBusca" class="form-control" style="max-width:280px" placeholder="🔍 Buscar por nome...">
                <select id="filtroCategoria" class="form-select" style="max-width:200px">
                    <option value="">Todas as categorias</option>
                </select>
                <div class="form-check form-switch mb-0">
                    <input class="form-check-input" type="checkbox" id="filtroVegano">
                    <label class="form-check-label" for="filtroVegano">Só veganos 🥦</label>
                </div>
                <button class="btn btn-secondary btn-sm" onclick="limparFiltros()">Limpar</button>
            </div>
        </div>

        <!-- LISTA DE PRATOS -->
        <div id="areaPratos" class="row g-3">
            <div class="spinner-area col-12">
                <div class="spinner-border text-primary"></div>
            </div>
        </div>

    </div>

    <!-- ===== MODAL: NOVO PRATO ===== -->
    <div class="modal fade" id="modalNovoPrato" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Cadastrar Novo Prato</h5>
                    <button type="button"
                        class="btn-close"
                        data-bs-dismiss="modal">
                    </button>
                </div>

                <div class="modal-body">

                    <div class="container">

                        <h3>Novo Prato</h3>

                        <form class="form-padrao" id="formNovoPrato">

                            <div class="mb-3">
                                <label class="form-label"><b>Nome</b></label>
                                <input type="text" id="nomePrato" class="form-control">
                            </div>

                            <div class="mb-3">
                                <label class="form-label"><b>Descrição</b></label>
                                <textarea id="descPrato"
                                    class="form-control"
                                    rows="3"></textarea>
                            </div>

                            <div class="mb-3">
                                <label class="form-label"><b>Categoria</b></label>
                                <select id="categoriaPrato" class="form-control">
                                    <option value="">Selecione...</option>
                                    <option value="1">Acompanhamento</option>
                                    <option value="2">Guarnição</option>
                                    <option value="3">Prato Principal</option>
                                    <option value="4">Refresco</option>
                                    <option value="5">Salada</option>
                                    <option value="6">Sobremesa</option>
                                </select>
                            </div>

                            <div class="mb-3">
                                <label class="form-label"><b>Vegano</b></label>
                                <select id="veganoPrato" class="form-control">
                                    <option value="true">Sim</option>
                                    <option value="false">Não</option>
                                </select>
                            </div>

                            <div class="mb-3">
                                <label class="form-label"><b>Imagem</b></label>
                                <input type="text"
                                    id="imagemPrato"
                                    class="form-control"
                                    placeholder="URL da imagem">
                            </div>

                        </form>

                    </div>

                </div>

                <div class="modal-footer justify-content-center">

                    <button class="btn btn-secondary"
                        data-bs-dismiss="modal">
                        Cancelar
                    </button>

                    <button id="salvarNovoPrato"
                        class="btn btn-success">
                        Salvar
                    </button>

                </div>

            </div>
        </div>
    </div>


    <!-- ===== MODAL: AVALIAR PRATO ===== -->
    <div class="modal fade" id="modalAvaliar" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Avaliar Prato: <span id="nomePratoAvaliar"></span></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="pratoIdAvaliar">
                    <p class="mb-1 fw-semibold">Nota:</p>
                    <div class="d-flex gap-1 mb-3" id="estrelas">
                        <span class="estrela" data-v="1">★</span>
                        <span class="estrela" data-v="2">★</span>
                        <span class="estrela" data-v="3">★</span>
                        <span class="estrela" data-v="4">★</span>
                        <span class="estrela" data-v="5">★</span>
                    </div>
                    <input type="hidden" id="notaSelecionada" value="0">
                    <label class="form-label">Comentário (opcional):</label>
                    <textarea id="comentarioAvaliar" class="form-control" rows="3" placeholder="O que você achou do prato?"></textarea>
                    <p id="msgAvaliar" class="text-danger mt-2 mb-0"></p>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button class="btn btn-primary" id="btnSalvarAvaliacao">Salvar Avaliação</button>
                </div>
            </div>
        </div>
    </div>


    <!-- ===== MODAL: FAVORITAR COM NOTIFICAÇÃO ===== -->
    <div class="modal fade" id="modalFavoritar" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-warning-subtle">
                    <h5 class="modal-title">⭐ Favoritar Prato</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="pratoIdFavoritar">
                    <p>Você está favoritando: <strong id="nomePratoFavoritar"></strong></p>
                    <hr>
                    <p class="fw-semibold mb-1">🔔 Notificação no celular</p>
                    <p class="text-muted small">Ao favoritar, você receberá uma notificação quando este prato aparecer no cardápio do dia!</p>
                    <div class="form-check form-switch mt-2">
                        <input class="form-check-input" type="checkbox" id="ativarNotificacao" checked>
                        <label class="form-check-label" for="ativarNotificacao">Ativar notificação para este prato</label>
                    </div>
                    <p id="msgFavoritar" class="text-danger mt-2 mb-0"></p>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button class="btn btn-warning fw-bold" id="btnConfirmarFavorito">⭐ Confirmar Favorito</button>
                </div>
            </div>
        </div>
    </div>


    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script type="module" src="../JS/buscaPratos.js"></script>
</body>

</html>