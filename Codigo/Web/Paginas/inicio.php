<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Painel - Bandeco</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/navbar.css">
    <link rel="stylesheet" href="../CSS/inicio.css">
    <script>
        // VERIFICAÇÃO DE RESTRIÇÃO
        if (sessionStorage.getItem('logado') !== 'true') {
            window.location.href = 'login.php';
        }
    </script>
</head>
<body class="bg-light">

    <!-- Navbar Modularizada -->
    <?php include __DIR__ . '/components/navbar.php'; ?>

    <main class="main-content">
        <div class="panel-container">
            <h1>Bem-vindo ao Bandeco!</h1>

            <div class="button-group">
                <button onclick="window.location.href='dadosperfil.php'">
                    👤 Dados do Perfil
                </button>

                <button onclick="window.location.href='buscaPratos.php'">
                    🍽️ Buscar Pratos
                </button>

                <button onclick="window.location.href='mural.php'">
                    📌 Mural e Cardápio do dia
                </button>

                <button onclick="window.location.href='meusFavoritos.php'">
                    ⭐ Meus Favoritos
                </button>

                <button onclick="logout()">
                    🚪 Sair
                </button>

                <p id="message" class="text-info mt-2"></p>
            </div>
        </div>
    </main>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../JS/config.js"></script>
    <script>
        const voltou = sessionStorage.getItem("volta");
        const message = document.getElementById("message");

        if (voltou === "true") {
            message.innerText = "Voltou do cadastro!";
            sessionStorage.removeItem("volta");
        }
    </script>
</body>
</html>