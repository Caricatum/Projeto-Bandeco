<!DOCTYPE html>
<html lang="pt-BR">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Dados do perfil</title>
  <link rel="stylesheet" href="../CSS/navbar.css">
  <link rel="stylesheet" href="../CSS/dadosperfil.css">
  <link rel="stylesheet" href="../../bootstrap-5.3.8-dist/css/bootstrap.min.css">
  <script src="../../bootstrap-5.3.8-dist/js/bootstrap.bundle.min.js"></script>
</head>

<body>

  <!-- Navbar -->
  <nav class="navbar navbar-expand-lg">
    <div class="container">
      <img src="../Assets/Images/logo_unicamp.jpg" alt="Logo Unicamp" width="40" height="40"
        class="d-inline-block align-text-top">
      <a class="navbar-brand ms-3" href="inicio.php">Bandeco</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-label="Abrir menu">
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
          <li class="nav-item">
                    <button class="btn-nav btn-sair" onclick="logout()">Sair</button>
                </li>
        </ul>
      </div>
    </div>
  </nav>


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


  <!-- ===== MODAL: CONFIRMAR SENHA ===== -->
  <div class="modal fade" id="modalSenha" tabindex="-1">
    <div class="modal-dialog">
      <div class="modal-content">

        <div class="modal-header" style="background:#FFF5E5; border-bottom:1px solid #E0C375;">
          <h5 class="modal-title" style="color:#D92243;" id="modalSenhaTitulo">Confirmar ação</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>

        <div class="modal-body">
          <p id="modalSenhaDescricao" class="text-muted mb-3"></p>
          <label style="color:#7a1728; font-weight:600;">Digite sua senha para confirmar:</label>
          <input type="password" id="inputSenhaModal" class="form-control mt-1"
            placeholder="Sua senha atual" autocomplete="current-password">
          <p id="msgSenhaModal" class="text-danger mt-2 mb-0" style="font-size:14px;"></p>
        </div>

        <div class="modal-footer" style="border-top:1px solid #E0C375;">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
          <button type="button" class="button" id="btnConfirmarSenha" style="width:auto; padding:8px 20px; margin:0;">
            Confirmar
          </button>
        </div>

      </div>
    </div>
  </div>


  <script src="../JS/dadosperfil.js"></script>
</body>

</html>