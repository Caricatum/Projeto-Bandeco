<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Recuperar Senha - Bandeco</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/style.css">
</head>
<body class="bg-custom">

    <div class="auth-card">

        <!-- ETAPA 1: Solicitar código -->
        <div id="etapa1">
            <div class="auth-header">
                <h2>Recuperar Senha</h2>
                <p class="auth-subtitle">Digite seu e-mail para receber o código de verificação</p>
            </div>

            <form id="formSolicitar">
                <div class="input-group-custom">
                    <label for="emailSolicitar">📧 E-mail Institucional</label>
                    <input type="email" id="emailSolicitar" class="input-custom" required placeholder="seu@email.com">
                </div>

                <button type="submit" class="btn-auth-primary" id="btnEnviarCodigo">
                    Enviar Código de Recuperação
                </button>

                <button type="button" class="btn-auth-secondary" onclick="window.location.href='login.php'">
                    ← Voltar ao Login
                </button>

                <p id="msgEtapa1" class="auth-message"></p>
            </form>
        </div>

        <!-- ETAPA 2: Inserir código + nova senha -->
        <div id="etapa2" style="display:none">
            <div class="auth-header">
                <h2>Definir Nova Senha</h2>
                <p class="auth-subtitle">
                    Código enviado para <strong id="emailMostrado" class="text-dark"></strong>
                </p>
            </div>

            <form id="formRedefinir">
                <div class="input-group-custom">
                    <label for="codigoReset">🔢 Código de 6 Dígitos</label>
                    <input type="text" id="codigoReset" class="input-custom input-codigo" maxlength="6" placeholder="000000" required autocomplete="one-time-code">
                </div>

                <div class="timer-container" id="timerContainer">
                    <span class="timer-icon">⏱️</span>
                    <span class="timer-text" id="timerText">Código válido por 10:00</span>
                </div>

                <div class="input-group-custom">
                    <label for="novaSenha">🔒 Nova Senha</label>
                    <input type="password" id="novaSenha" class="input-custom" placeholder="Mínimo 6 caracteres" required>
                </div>

                <div class="input-group-custom">
                    <label for="confirmarSenha">🔒 Confirmar Nova Senha</label>
                    <input type="password" id="confirmarSenha" class="input-custom" placeholder="Repita a nova senha" required>
                </div>

                <button type="submit" class="btn-auth-primary" id="btnSalvarNovaSenha">
                    Salvar Nova Senha
                </button>

                <button type="button" class="btn-resend" id="btnReenviarReset">
                    🔄 Reenviar Código de Recuperação
                </button>

                <button type="button" class="btn-auth-secondary" onclick="voltarEtapa1()">
                    ← Trocar e-mail
                </button>

                <p id="msgEtapa2" class="auth-message"></p>
            </form>
        </div>

    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../JS/config.js"></script>
    <script src="../JS/resetSenha.js"></script>
</body>
</html>
