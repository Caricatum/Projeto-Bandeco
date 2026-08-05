<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Confirmar E-mail</title>
    <link rel="stylesheet" href="../CSS/style.css">

    <link rel="stylesheet" href="../../bootstrap-5.3.8-dist/css/bootstrap.min.css">
    <script src="../../bootstrap-5.3.8-dist/js/bootstrap.bundle.min.js"></script>
</head>
<body class="bg-custom">

    <div id="loginContainer" class="login-container p-4">
        <form id="confirmarForm">
            <h2>Confirme seu e-mail</h2>

            <p style="text-align:center; color:#7a1728; font-size:14px; margin-bottom:20px;">
                Enviamos um código de 6 dígitos para <strong id="emailMostrado"></strong>.<br>
                Digite o código abaixo para ativar sua conta.
            </p>

            <div class="input-group">
                <label>E-mail</label>
                <input type="text" id="email" required>
            </div>

            <div class="input-group">
                <label>Código de confirmação</label>
                <input type="text" id="codigo" maxlength="6" placeholder="000000" required>
            </div>

            <button type="submit" class="button">Confirmar</button>

            <button type="button" class="button" id="voltarLogin" onclick="window.location.href='login.php'">
                Voltar ao login
            </button>

            <p id="message"></p>
        </form>
    </div>

    <script src="../JS/confirmarEmail.js"></script>
</body>
</html>
