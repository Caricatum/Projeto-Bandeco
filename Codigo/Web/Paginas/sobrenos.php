<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Sobre Nós - Bandeco Unicamp</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/navbar.css">
    <link rel="stylesheet" href="../CSS/inicio.css">
</head>
<body class="bg-light">

    <!-- Navbar Parcial -->
    <?php include __DIR__ . '/components/navbar.php'; ?>

    <main class="container my-5">
        <div class="row justify-content-center">
            <div class="col-lg-8">
                <div class="card shadow-sm border-0 p-4 rounded-3">
                    <h1 class="text-primary text-center mb-4">ℹ️ Sobre o Projeto Bandeco</h1>
                    
                    <p class="lead text-secondary text-center">
                        Sistema desenvolvido para facilitação e acompanhamento do Cardápio do Restaurante Universitário (Bandeco) do COTIL / UNICAMP.
                    </p>

                    <hr class="my-4">

                    <h4 class="text-dark mb-3">📌 Objetivos do Sistema</h4>
                    <ul class="list-group list-group-flush mb-4">
                        <li class="list-group-item bg-transparent">🍽️ <strong>Consulta de Cardápio:</strong> Acompanhamento rápido das refeições diárias e opções veganas.</li>
                        <li class="list-group-item bg-transparent">⭐ <strong>Avaliações e Favoritos:</strong> Espaço para alunos e funcionários favoritarem e avaliarem pratos.</li>
                        <li class="list-group-item bg-transparent">📌 <strong>Mural de Avisos:</strong> Comunicação eficiente sobre funcionamento e cardápios especiais.</li>
                    </ul>

                    <h4 class="text-dark mb-3">💻 Tecnologia</h4>
                    <p class="text-muted">
                        O projeto é composto por um frontend interativo em HTML5, CSS3, JavaScript e PHP, integrado a uma API RESTful em Java (Spring Boot) e banco de dados MySQL.
                    </p>

                    <div class="text-center mt-4">
                        <button class="btn btn-outline-primary" onclick="window.location.href='inicio.php'">
                            ← Voltar ao Início
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
