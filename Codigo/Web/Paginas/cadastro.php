<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cadastro - Bandeco</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../CSS/style.css">
</head>
<body id="bodyCadastro" class="bg-custom">

    <div class="auth-card">
        <div class="auth-header">
            <h2>Criar Conta</h2>
            <p class="auth-subtitle">Cadastre-se para acessar o Bandeco</p>
        </div>

        <form id="cadastroForm">
            <div class="input-group-custom">
                <label for="name">👤 Nome Completo</label>
                <input type="text" id="name" class="input-custom" placeholder="Seu nome completo" required>
            </div>

            <div class="input-group-custom">
                <label for="username">📧 E-mail Institucional</label>
                <input type="email" id="username" class="input-custom" placeholder="exemplo@unicamp.br" required>
            </div>

            <div class="input-group-custom">
                <label for="password">🔒 Senha</label>
                <div class="password-wrapper">
                    <input type="password" id="password" class="input-custom" placeholder="Crie uma senha forte" required>
                    <button type="button" class="btn-toggle-pass" id="btnToggleSenha" title="Mostrar/ocultar senha">👁️</button>
                </div>
                <div class="password-strength-container">
                    <div class="password-strength-bar" id="forcaBarra"></div>
                    <span class="password-strength-label" id="forcaTexto"></span>
                </div>
                <ul class="password-requirements" id="listaRequisitos">
                    <li class="req-item" id="reqTam"><span class="req-icon"></span> Mínimo de 8 caracteres</li>
                    <li class="req-item" id="reqMaius"><span class="req-icon"></span> Pelo menos uma letra maiúscula (A-Z)</li>
                    <li class="req-item" id="reqMinus"><span class="req-icon"></span> Pelo menos uma letra minúscula (a-z)</li>
                    <li class="req-item" id="reqNum"><span class="req-icon"></span> Pelo menos um número (0-9)</li>
                </ul>
            </div>

            <div class="input-group-custom">
                <label for="confirmPassword">🔒 Confirmar Senha</label>
                <div class="password-wrapper">
                    <input type="password" id="confirmPassword" class="input-custom" placeholder="Repita sua senha" required>
                    <button type="button" class="btn-toggle-pass" id="btnToggleConfirmar" title="Mostrar/ocultar senha">👁️</button>
                </div>
            </div>
            
            <button type="submit" class="btn-auth-primary" id="btnCadastrar">
                Cadastrar
            </button>

            <button type="button" class="btn-auth-secondary" onclick="window.location.href='login.php'">
                ← Já possui uma conta? Faça Login
            </button>

            <p id="message" class="auth-message"></p>
        </form>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../JS/config.js"></script>
    <script src="../JS/cadastro.js"></script>
</body>
</html>