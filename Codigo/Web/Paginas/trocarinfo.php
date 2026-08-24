<!DOCTYPE html>
<html lang="pt-BR">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Trocar Informações - Bandeco</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <link rel="stylesheet" href="../CSS/navbar.css">
  <link rel="stylesheet" href="../CSS/trocarinf.css">
</head>

<body>
  <!-- Navbar Modularizada -->
  <?php include __DIR__ . '/components/navbar.php'; ?>


  <!--dados-->
  <div id="dadosContainer" class="dados-container">
    <form id="dadosForm">
      <h2>Trocar Informações</h2>
      <div class="input-group " id="div-usuario">
        <label>Usuário</label>
        <input type="text" id="username" required placeholder="Digite seu Usuário">
      </div>

      <div id="div-nome">
        <div class="input-group" id="input-nome">
          <label>Nome</label>
          <input type="text" id="name" placeholder="Digite seu nome">
        </div>
      </div>


      <section class="section" id="sectionTipodeUsuario">
        <div class="row content-align-center">Tipo de pessoa</div>

        <div class="radio-option">
          <input type="radio" name="tipoDeUsuario" id="aluno" value="false" required checked>
          <label for="aluno">Aluno</label>
        </div>

        <div class="radio-option">
          <input type="radio" name="tipoDeUsuario" id="func" value="true" required>
          <label for="func">Funcionário</label>
        </div>
      </section>

      <button type="button" class="button" id="trocarinfo">Trocar informações</button>

      <button type="button" class="button" id="voltar" onclick="window.location.href='dadosperfil.php'">
        Voltar
      </button>

      <p id="message"></p>


    </form>
  </div>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
  <script src="../JS/config.js"></script>
  <script type="module" src="../JS/trocarinfo.js"></script>
</body>
</html>