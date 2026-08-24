<!DOCTYPE html>
<html lang="pt-BR">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Dados do perfil</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <link rel="stylesheet" href="../CSS/navbar.css">
  <link rel="stylesheet" href="../CSS/dadosperfil.css">
</head>

<body>

  <!-- Navbar Modularizada -->
  <?php include __DIR__ . '/components/navbar.php'; ?>



  <!-- Formulário de busca -->
  <div id="dadosContainer" class="dados-container">
    <form id="dadosForm">
      <h2>Qual o seu Usuário?</h2>

      <div class="input-group" id="div-usuario">
        <label>Usuário</label>
        <input type="text" id="username" required placeholder="Digite seu Usuário">
      </div>

      <div id="div-nome" style="display:none">
        <div class="input-group" id="input-nome">
          <label>Nome</label>
          <input type="text" id="name" placeholder="Nome do usuário" readonly>
        </div>
      </div>

      <section class="section" id="sectionTipodeUsuario" style="display:none">
        <label>Tipo de Pessoa</label>
        <div class="radio-option">
          <label for="aluno">Aluno</label>
          <input type="radio" name="tipoDeUsuario" id="aluno" value="false" checked disabled>
        </div>
        <div class="radio-option">
          <label for="func">Funcionário</label>
          <input type="radio" name="tipoDeUsuario" id="func" value="true" disabled>
        </div>
      </section>

      <button type="submit" class="button">Buscar</button>

      <button type="button" class="button" id="trocarinfo" style="display:none">
        ✏️ Trocar Informações
      </button>

      <button type="button" class="button btn-deletar" id="deletar" style="display:none">
        🗑️ Deletar Usuário
      </button>

      <button type="button" class="button" id="voltar" style="display:none"
        onclick="window.location.href='inicio.php'">
        Voltar
      </button>

      <p id="message"></p>
    </form>
  </div>

  <script>
    // =============================================
    // LOGOUT
    // =============================================
    function logout() {
      sessionStorage.setItem('logado', 'false');
      localStorage.clear();
      window.location.href = 'login.php';
    }
  </script>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
  <script src="../JS/config.js"></script>
  <script src="../JS/dadosperfil.js"></script>
</body>

</html>