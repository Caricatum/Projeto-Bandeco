<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login - Bandeco</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/style.css">
</head>
<body class="bg-custom">

    <div class="auth-card">
        <div class="auth-header">
            <h2>Entrar no Bandeco</h2>
            <p class="auth-subtitle">Acesse o cardápio e seus pratos favoritos</p>
        </div>

        <form id="loginForm">
            <div class="input-group-custom">
                <label for="username">📧 E-mail / Login</label>
                <input type="text" id="username" class="input-custom" placeholder="Digite seu e-mail" required autocomplete="username">
            </div>

            <div class="input-group-custom">
                <label for="password">🔒 Senha</label>
                <input type="password" id="password" class="input-custom" placeholder="Digite sua senha" required autocomplete="current-password">
            </div>

            <button type="submit" class="btn-auth-primary" id="btnEntrar">
                Entrar
            </button>
            
            <button type="button" class="btn-auth-secondary" onclick="window.location.href='cadastro.php'">
                Não tem conta? Cadastre-se aqui
            </button>

            <button type="button" class="btn-auth-link" onclick="window.location.href='resetSenha.php'">
                Esqueceu sua senha?
            </button>

            <p id="message" class="auth-message"></p>
        </form>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../JS/config.js"></script>
    <script src="../JS/login.js"></script>
</body>
</html>