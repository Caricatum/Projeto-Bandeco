<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Detalhes do Prato</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/navbar.css">
    <style>
        body { background-color: #FFF5E5; font-family: Arial, sans-serif; }

        /* Imagem do prato */
        .prato-img {
            width: 100%;
            max-height: 340px;
            object-fit: cover;
            border-radius: 16px;
            box-shadow: 0 4px 20px rgba(217,34,67,0.15);
        }
        .prato-img-placeholder {
            width: 100%;
            height: 240px;
            background: linear-gradient(135deg, #F69D39, #E0C375);
            border-radius: 16px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 5rem;
        }

        /* Card principal */
        .card-det {
            border: 1px solid #E0C375;
            border-radius: 16px;
            background: white;
            box-shadow: 0 2px 12px rgba(217,34,67,0.08);
        }

        h1 { color: #D92243; }
        h5 { color: #7a1728; }

        .badge-vegano    { background: #7a5c00; }
        .badge-categoria { background: #D92243; }

        /* Estrelas */
        .estrela-media { color: #F69D39; font-size: 1.3rem; font-weight: bold; }
        .estrela-form  { font-size: 2rem; cursor: pointer; color: #E0C375; transition: color 0.15s; }
        .estrela-form:hover, .estrela-form.selecionada { color: #F69D39; }

        /* Avaliações */
        .card-aval {
            background: #fff8ee;
            border: 1px solid #E0C375;
            border-radius: 10px;
            padding: 12px 16px;
            margin-bottom: 10px;
        }
        .card-aval .nota { color: #F69D39; font-weight: bold; }
        .card-aval .autor { color: #aaa; font-size: 0.8rem; }

        .btn-voltar {
            border: 2px solid #D92243;
            background: transparent;
            color: #D92243;
            border-radius: 20px;
            padding: 6px 18px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s;
        }
        .btn-voltar:hover { background: #D92243; color: white; }

        .btn-salvar {
            background: linear-gradient(135deg, #D92243, #b91a35);
            border: none;
            color: white;
            border-radius: 10px;
            padding: 10px 24px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.2s;
        }
        .btn-salvar:hover { background: linear-gradient(135deg, #F69D39, #D92243); }
        .btn-salvar:disabled { background: #d4a5a5; cursor: not-allowed; }

        .spinner-border { color: #D92243 !important; }
    </style>
</head>
<body>

<!-- Navbar Modularizada -->
<?php include __DIR__ . '/components/navbar.php'; ?>


<!-- Conteúdo -->
<div class="container mt-4 mb-5">

    <!-- Loading -->
    <div id="loading" class="text-center py-5">
        <div class="spinner-border"></div>
        <p class="mt-2 text-muted">Carregando prato...</p>
    </div>

    <!-- Detalhes (preenchido pelo JS) -->
    <div id="conteudo" style="display:none">

        <div class="mb-3">
            <button class="btn-voltar" onclick="history.back()">← Voltar</button>
        </div>

        <div class="row g-4">

            <!-- Coluna esquerda: imagem + info básica -->
            <div class="col-lg-5">
                <div id="areaImagem" class="mb-3"></div>

                <div class="card-det p-3">
                    <h1 id="nomePrato"></h1>
                    <div class="d-flex gap-2 flex-wrap mb-3" id="badges"></div>
                    <p id="descricaoPrato" class="text-muted"></p>

                    <div class="d-flex align-items-center gap-3 mb-3">
                        <span class="estrela-media" id="mediaEstrelas"></span>
                        <span class="text-muted small" id="totalAvaliacoes"></span>
                    </div>

                    <!-- Ações (só logado) -->
                    <div id="areaAcoes" class="d-flex gap-2 flex-wrap" style="display:none!important"></div>
                </div>

                <!-- Resumo por IA (Gemini) -->
                <div id="areaIA" class="card-det p-3 mt-3" style="display:none; background: #fffcf5; border-color: #F69D39;">
                    <h5 style="color: #D92243;">✨ Resumo das Avaliações (IA)</h5>
                    <p id="descricaoIA" class="mb-0 text-muted small" style="line-height: 1.6;"></p>
                </div>

                <!-- Tabela de Valores Nutricionais -->
                <div class="card-det p-3 mt-3" id="areaNutricionalCard">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <h5 class="mb-0">🥗 Valores Nutricionais</h5>
                        <button type="button" class="btn btn-sm btn-outline-danger d-none" id="btnEditarNutricao" onclick="abrirModalNutricional()">
                            ✏️ Editar
                        </button>
                    </div>

                    <div id="conteudoNutricional">
                        <div class="text-center py-2 text-muted small">
                            <div class="spinner-border spinner-border-sm text-danger" role="status"></div>
                            Carregando tabela nutricional...
                        </div>
                    </div>
                </div>

                <!-- Nota técnica (só funcionário) -->
                <div id="areaNota" class="card-det p-3 mt-3" style="display:none">
                    <h5>📋 Nota Técnica</h5>
                    <p id="notaTecnica" class="mb-0 text-muted"></p>
                </div>
            </div>

            <!-- Coluna direita: avaliações + form avaliar -->
            <div class="col-lg-7">

                <!-- Formulário de avaliação (só logado e não avaliou) -->
                <div id="formAvaliar" class="card-det p-3 mb-4" style="display:none">
                    <h5>👍 Avaliar este prato</h5>
                    <p class="mb-1 fw-semibold small">Nota:</p>
                    <div class="d-flex gap-1 mb-3" id="estrelasFom">
                        <span class="estrela-form" data-v="1">★</span>
                        <span class="estrela-form" data-v="2">★</span>
                        <span class="estrela-form" data-v="3">★</span>
                        <span class="estrela-form" data-v="4">★</span>
                        <span class="estrela-form" data-v="5">★</span>
                    </div>
                    <input type="hidden" id="notaSel" value="0">
                    <textarea id="comentario" class="form-control mb-2" rows="2" placeholder="Comentário (opcional)"></textarea>
                    <p id="msgAvaliar" class="text-danger small mb-1"></p>
                    <button class="btn-salvar" id="btnSalvarAval">Salvar avaliação</button>
                </div>

                <!-- Lista de avaliações -->
                <h5>💬 Avaliações</h5>
                <div id="listaAvaliacoes">
                    <p class="text-muted">Nenhuma avaliação ainda.</p>
                </div>

            </div>
        </div>
    </div>

    <!-- Modal Gerenciar Valores Nutricionais (Funcionário) -->
    <div class="modal fade" id="modalNutricional" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">🥗 Tabela Nutricional do Prato</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <form id="formNutricional">
                        <input type="hidden" id="nutricaoId">
                        
                        <div class="mb-3">
                            <label class="form-label fw-bold small">Porção / Medida de Referência</label>
                            <input type="text" class="form-control" id="nutricaoMedida" placeholder="Ex: 100g, 1 concha, 1 unidade..." required>
                        </div>

                        <div class="row g-2 mb-3">
                            <div class="col-6">
                                <label class="form-label fw-bold small">Calorias (kcal)</label>
                                <input type="number" step="0.1" class="form-control" id="nutricaoKcal" placeholder="Ex: 180" required>
                            </div>
                            <div class="col-6">
                                <label class="form-label fw-bold small">Carboidratos (g)</label>
                                <input type="number" step="0.1" class="form-control" id="nutricaoCarboidratos" placeholder="Ex: 25.5" required>
                            </div>
                        </div>

                        <div class="row g-2 mb-3">
                            <div class="col-6">
                                <label class="form-label fw-bold small">Proteínas (g)</label>
                                <input type="number" step="0.1" class="form-control" id="nutricaoProteinas" placeholder="Ex: 14.2" required>
                            </div>
                            <div class="col-6">
                                <label class="form-label fw-bold small">Lipídios / Gorduras (g)</label>
                                <input type="number" step="0.1" class="form-control" id="nutricaoLipidios" placeholder="Ex: 4.8" required>
                            </div>
                        </div>

                        <p id="msgNutricional" class="text-danger small mb-0"></p>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-danger fw-bold" id="btnSalvarNutricao" onclick="salvarValoresNutricionais()">
                        💾 Salvar Tabela Nutricional
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Erro -->
    <div id="areaErro" style="display:none" class="text-center py-5">
        <h4 style="color:#D92243">😕 Prato não encontrado</h4>
        <button class="btn-voltar mt-3" onclick="history.back()">← Voltar</button>
    </div>

</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
<script src="../JS/config.js"></script>
<script src="../JS/pratoDet.js"></script>
</body>
</html>
