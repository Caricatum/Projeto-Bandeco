<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Confirmar E-mail - Bandeco</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/style.css">
</head>
<body class="bg-custom">

    <div class="auth-card">
        <div class="auth-header">
            <h2>Confirme seu E-mail</h2>
            <p class="auth-subtitle">
                Código de 6 dígitos enviado para <strong id="emailMostrado" class="text-dark">seu e-mail</strong>
            </p>
        </div>

        <form id="confirmarForm">
            <div class="input-group-custom">
                <label for="email">📧 E-mail</label>
                <input type="email" id="email" class="input-custom" required>
            </div>

            <div class="input-group-custom">
                <label for="codigo">🔢 Código de Confirmação</label>
                <input type="text" id="codigo" class="input-custom input-codigo" maxlength="6" placeholder="000000" required autocomplete="one-time-code">
            </div>

            <div class="timer-container" id="timerContainer">
                <span class="timer-icon">⏱️</span>
                <span class="timer-text" id="timerText">Código válido por 10:00</span>
            </div>

            <button type="submit" class="btn-auth-primary" id="btnConfirmar">
                Confirmar e Ativar Conta
            </button>

            <button type="button" class="btn-resend" id="btnReenviar">
                🔄 Reenviar Código de Confirmação
            </button>

            <button type="button" class="btn-auth-secondary" onclick="window.location.href='login.php'">
                ← Voltar ao Login
            </button>

            <p id="message" class="auth-message"></p>
        </form>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../JS/config.js"></script>
    <script src="../JS/confirmarEmail.js"></script>
</body>
</html>
