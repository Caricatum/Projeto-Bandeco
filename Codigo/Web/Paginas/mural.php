<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Mural de avisos e Cardápio</title>

    <!-- JS -->
    <script src="../JS/mural.js" defer></script>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>

<body class="bg-light">

    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg bg-body-tertiary shadow-sm">
        <div class="container">

            <img src="../Assets/Images/logo_unicamp.jpg" alt="Logo Unicamp" width="40" height="40"
                class="d-inline-block align-text-top">

            <a class="navbar-brand ms-3" href="">
                Inicio
            </a>

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">

                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarNav">

                <ul class="navbar-nav ms-auto">
                    
                    <li class="nav-item">
                        <a class="nav-link" onclick="window.location.href='login.php'">
                            <button id="btnLogin" class="btn btn-dark">
                                Login
                            </button>
                        </a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link" onclick="window.location.href='sobrenos.php'">
                            <button id="btnSobrenos" class="btn btn-dark">
                                Sobre nós
                            </button>
                        </a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link" onclick="window.location.href='projetos.php'">
                            <button id="btnProjetos" class="btn btn-dark">
                                Projetos
                            </button>
                        </a>
                    </li>

                </ul>

            </div>
        </div>
    </nav>


    <!-- CONTAINER -->
    <div class="container mt-5">

        <!-- TOPO -->
        <div class="d-flex justify-content-between align-items-center mb-4">

            <h1>📌 Mural de Avisos</h1>

            <!-- BOTÃO FUNCIONÁRIO -->
            <button id="btnNovoAviso" class="btn btn-primary d-none">

                + Novo Aviso
            </button>

        </div>


        <!-- MURAL -->
        <div id="mural" class="row g-3">
            <!-- Avisos serão inseridos via JS -->

        </div>

    </div>


    <!-- MODAL -->
    <div class="modal fade" id="modalAviso" tabindex="-1">

        <div class="modal-dialog">

            <div class="modal-content">

                <!-- HEADER -->
                <div class="modal-header">

                    <h5 class="modal-title">
                        Criar Aviso
                    </h5>

                    <button type="button" class="btn-close" data-bs-dismiss="modal">
                    </button>

                </div>


                <!-- BODY -->
                <div class="modal-body">

                    <div class="mb-3">

                        <label class="form-label">
                            Título
                        </label>

                        <input type="text" id="tituloAviso" class="form-control">

                    </div>


                    <div class="mb-3">

                        <label class="form-label">
                            Descrição
                        </label>

                        <textarea id="descricaoAviso" class="form-control"></textarea>

                    </div>

                </div>


                <!-- FOOTER -->
                <div class="modal-footer">

                    <button class="btn btn-secondary" data-bs-dismiss="modal">

                        Cancelar
                    </button>

                    <button id="salvarAviso" class="btn btn-success">

                        Salvar
                    </button>

                </div>

            </div>

        </div>

    </div>


    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>


        
    <div class="container mt-4">

    <h1 class="text-center mb-4">Cardápio do Dia</h1>

    <!-- ALMOÇO -->
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="bg-primary text-white p-2 rounded">Almoço</h2>
        </div>

        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header">
                    <h4>Cardápio Padrão</h4>
                </div>
                <div class="card-body">
                    <p><strong>Acompanhamento:</strong> Arroz e Feijão</p>
                    <p><strong>Prato Principal:</strong> Frango Grelhado</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Guarnição:</strong> Batata Sauté</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Salada:</strong> Alface e Tomate</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Sobremesa:</strong> Gelatina</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Refresco:</strong> Suco de Laranja</p> <!-- Preciso adicionar API nisso -->
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header bg-success text-white">
                    <h4>Cardápio Vegano</h4>
                </div>
                <div class="card-body">
                    <p><strong>Acompanhamento:</strong> Arroz Integral</p>
                    <p><strong>Prato Principal:</strong> Hambúrguer de Lentilha</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Guarnição:</strong> Legumes Assados</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Salada:</strong> Mix de Folhas</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Sobremesa:</strong> Fruta</p> <!-- Preciso adicionar API nisso -->
                    <p><strong>Refresco:</strong> Suco de Uva</p> <!-- Preciso adicionar API nisso -->
                </div>
            </div>
        </div>
    </div>

    <!-- JANTA -->
    <div class="row">
        <div class="col-12">
            <h2 class="bg-dark text-white p-2 rounded">Janta</h2>
        </div>

        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header">
                    <h4>Cardápio Padrão</h4>
                </div>
                <div class="card-body">
                    <p><strong>Acompanhamento:</strong> Arroz e Feijão</p>
                    <p><strong>Prato Principal:</strong> Carne Assada</p>
                    <p><strong>Guarnição:</strong> Purê de Batata</p>
                    <p><strong>Salada:</strong> Repolho</p>
                    <p><strong>Sobremesa:</strong> Pudim</p>
                    <p><strong>Refresco:</strong> Suco de Limão</p>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header bg-success text-white">
                    <h4>Cardápio Vegano</h4>
                </div>
                <div class="card-body">
                    <p><strong>Acompanhamento:</strong> Arroz Integral</p>
                    <p><strong>Prato Principal:</strong> Tofu Grelhado</p>
                    <p><strong>Guarnição:</strong> Abobrinha Refogada</p>
                    <p><strong>Salada:</strong> Cenoura Ralada</p>
                    <p><strong>Sobremesa:</strong> Banana</p>
                    <p><strong>Refresco:</strong> Chá Gelado</p>
                </div>
            </div>
        </div>
    </div>

</div>
    

</body>

</html>