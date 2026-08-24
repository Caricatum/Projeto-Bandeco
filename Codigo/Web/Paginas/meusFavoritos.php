<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Meus Favoritos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/navbar.css">
    <link rel="stylesheet" href="../CSS/buscaPratos.css">
    <style>
        .area-vazia {
            background: white;
            border: 2px dashed #E0C375;
            border-radius: 16px;
            padding: 60px 20px;
            text-align: center;
        }
        .area-vazia h4 { color: #D92243; }

        .btn-desfav {
            border: 2px solid #D92243;
            background: #FFF5E5;
            color: #D92243;
            border-radius: 20px;
            padding: 4px 14px;
            font-size: 0.85rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
        }
        .btn-desfav:hover { background: #D92243; color: white; }

        .btn-adicionar-fav {
            background: linear-gradient(135deg, #D92243, #F69D39);
            color: #ffffff !important;
            border: none;
            border-radius: 10px;
            padding: 10px 22px;
            font-size: 0.95rem;
            font-weight: bold;
            cursor: pointer;
            box-shadow: 0 4px 14px rgba(217, 34, 67, 0.25);
            transition: all 0.2s ease;
            display: inline-flex;
            align-items: center;
        }
        .btn-adicionar-fav:hover {
            background: linear-gradient(135deg, #b91a35, #D92243);
            transform: translateY(-2px);
            box-shadow: 0 6px 18px rgba(217, 34, 67, 0.35);
            color: #ffffff !important;
        }
    </style>
</head>

<body>

    <!-- Navbar Modularizada -->
    <?php include __DIR__ . '/components/navbar.php'; ?>


    <!-- CONTEÚDO -->
    <div class="container mt-4 mb-5">

        <div class="d-flex justify-content-between align-items-center mb-4 flex-wrap gap-2">
            <div>
                <h1>⭐ Meus Favoritos</h1>
                <p class="text-muted mb-0" id="subtitulo">Carregando...</p>
            </div>
            <button class="btn-adicionar-fav" onclick="window.location.href='buscaPratos.php'">
                + Adicionar favoritos
            </button>
        </div>

        <!-- FILTROS -->
        <div class="card p-3 mb-4 shadow-sm">
            <div class="d-flex align-items-center" style="gap:10px; flex-wrap:wrap;">
                <input type="text" id="campoBusca" class="form-control" style="max-width:260px" placeholder="🔍 Filtrar favoritos...">
                <div class="form-check form-switch d-flex align-items-center mb-0 ms-1">
                    <input class="form-check-input" type="checkbox" id="filtroVegano">
                    <label class="form-check-label ms-2" for="filtroVegano">Só veganos 🥦</label>
                </div>
            </div>
        </div>

        <!-- CARDS -->
        <div id="areaFavoritos" class="row g-3">
            <div class="col-12 text-center py-5">
                <div class="spinner-border text-primary"></div>
                <p class="mt-2 text-muted">Carregando favoritos...</p>
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


    <!-- ===== MODAL: CONFIRMAR REMOÇÃO ===== -->
    <div class="modal fade" id="modalRemover" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background:#FFF5E5; border-bottom:1px solid #E0C375;">
                    <h5 class="modal-title" style="color:#D92243;">🗑️ Remover Favorito</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p>Remover <strong id="nomeRemover"></strong> dos seus favoritos?</p>
                    <p class="text-muted small">Você não receberá mais notificações sobre este prato.</p>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button class="btn btn-danger fw-bold" id="btnConfirmarRemover">Remover</button>
                </div>
            </div>
        </div>
    </div>


    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../JS/config.js"></script>
    <script src="../JS/meusFavoritos.js"></script>
</body>

</html>
