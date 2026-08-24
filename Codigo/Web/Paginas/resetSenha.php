<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Recuperar Senha</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/style.css">
</head>
<body class="bg-custom">

    <div id="loginContainer" class="login-container p-4">

        <!-- ETAPA 1: Solicitar código -->
        <div id="etapa1">
            <form id="formSolicitar">
                <h2>Recuperar Senha</h2>
                <p style="text-align:center; color:#7a1728; font-size:14px; margin-bottom:20px;">
                    Digite seu e-mail para receber o código de redefinição.
                </p>
                <div class="input-group">
                    <label>E-mail</label>
                    <input type="text" id="emailSolicitar" required placeholder="seu@email.com">
                </div>
                <button type="submit" class="button">Enviar Código</button>
                <button type="button" class="button" onclick="window.location.href='login.php'">
                    Voltar ao Login
                </button>
                <p id="msgEtapa1"></p>
            </form>
        </div>

        <!-- ETAPA 2: Inserir código + nova senha -->
        <div id="etapa2" style="display:none">
            <form id="formRedefinir">
                <h2>Nova Senha</h2>
                <p style="text-align:center; color:#7a1728; font-size:14px; margin-bottom:20px;">
                    Enviamos um código para <strong id="emailMostrado"></strong>.
                </p>
                <div class="input-group">
                    <label>Código de verificação</label>
                    <input type="text" id="codigoReset" maxlength="6" placeholder="000000" required>
                </div>
                <div class="input-group">
                    <label>Nova senha</label>
                    <input type="password" id="novaSenha" placeholder="Mínimo 6 caracteres" required>
                </div>
                <div class="input-group">
                    <label>Confirmar nova senha</label>
                    <input type="password" id="confirmarSenha" placeholder="Repita a senha" required>
                </div>
                <button type="submit" class="button">Redefinir Senha</button>
                <button type="button" class="button" onclick="voltarEtapa1()"
                    style="background:linear-gradient(135deg,#9b1b30,#D92243);">
                    Reenviar código
                </button>
                <p id="msgEtapa2"></p>
            </form>
        </div>

    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../JS/config.js"></script>
    <script src="../JS/resetSenha.js"></script>
</body>
</html>
