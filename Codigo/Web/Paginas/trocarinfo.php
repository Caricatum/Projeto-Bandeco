<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Trocar Informações</title>
  <link rel="stylesheet" href="../CSS/trocarinf.css">

  <link rel="stylesheet" href="../../bootstrap-5.3.8-dist/css/bootstrap.min.css">
  <link rel="stylesheet" href="../CSS/navbar.css">
  <script src="../../bootstrap-5.3.8-dist/js/bootstrap.bundle.min.js"></script>
</head>

<body>
  <!-- Navbar -->
  <nav class="navbar navbar-expand-lg">
    <div class="container">
      <img src="../Assets/Images/logo_unicamp.jpg" alt="Logo Unicamp" width="40" height="40"
        class="d-inline-block align-text-top">
      <a class="navbar-brand ms-3" href="inicio.php">Bandeco</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav ms-auto gap-2">
 
          <li class="nav-item">
            <button class="btn-nav" onclick="window.location.href='inicio.php'">🏠 Início</button>
          </li>
 
          <li class="nav-item">
            <button class="btn-nav" onclick="window.location.href='mural.php'">📌 Mural</button>
          </li>
 
          <li class="nav-item">
            <button class="btn-nav" onclick="window.location.href='buscaPratos.php'">🍽️ Buscar Pratos</button>
          </li>
 
          <li class="nav-item">
            <button class="btn-nav" onclick="window.location.href='sobrenos.php'">ℹ️ Sobre nós</button>
          </li>
 
        </ul>
      </div>
    </div>
  </nav>


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

      <div id="div-senha">
        <div class="input-group" id="input-senha">
          <label>Senha:</label>
          <input type="password" id="senha" placeholder="Digite sua senha para trocar as informações" required>
        </div>
      </div>



      <button type="button" class="button" id="trocarinfo">Trocar informações</button>

      <button type="button" class="button" id="voltar" onclick="window.location.href='dadosperfil.php'">
        Voltar
      </button>

      <p id="message"></p>


    </form>
  </div>
  <script type="module" src="../JS/trocarinfo.js"></script>

</body>

</html>