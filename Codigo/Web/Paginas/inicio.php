<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <title>Painel</title>
    <link rel="stylesheet" href="../CSS/inicio.css">
    <script>
        // VERIFICAÇÃO DE RESTRIÇÃO
        if (sessionStorage.getItem('logado') !== 'true') {
            window.location.href = 'login.php'; // Volta pro login se não logado
        }
    </script>
</head>
<body>
    <div class="panel-container">
        <h1>Olá</h1>

        <div class="button-group">
            <button onclick="window.location.href='dadosperfil.php'">
                Dados do Perfil
            </button>

            <button onclick="window.location.href='mural.php'">
                Mural e Cardapio do dia
            </button>

            <button onclick="logout()">
                Sair
            </button>

            <p id="message"></p>
        </div>
    </div>
    <script>
        voltou = sessionStorage.getItem("volta");
        message = document.getElementById("message");

        function logout() {
            sessionStorage.setItem('logado', 'false');
            window.location.href = 'login.php';
        }

        if(voltou == "true"){
        message.innerText = "Voltou do cadastro!";
        }

    </script>
</body>

</html>