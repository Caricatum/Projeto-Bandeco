<!DOCTYPE html>
<html lang="pt-BR">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Dados do Perfil - Bandeco</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <link rel="stylesheet" href="../CSS/navbar.css">
  <link rel="stylesheet" href="../CSS/dadosperfil.css">
</head>

<body>

  <!-- Navbar Modularizada -->
  <?php include __DIR__ . '/components/navbar.php'; ?>

  <main class="main-container">
    <div class="perfil-card">
      
      <!-- Cabeçalho do Perfil -->
      <div class="perfil-header">
        <div class="perfil-avatar" id="avatarIcon">👤</div>
        <h2 class="perfil-title" id="tituloPagina">Meu Perfil</h2>
        <span id="badgeTipoUsuario" class="badge-tipo-aluno">Carregando...</span>
      </div>

      <!-- Barra de Busca (Visível apenas para Funcionários) -->
      <div id="boxBuscaFuncionario" class="busca-box d-none">
        <label class="info-label mb-2">🔍 Gerenciar outro usuário (Apenas Funcionário)</label>
        <form id="formBuscaUsuario" class="d-flex gap-2">
          <input type="text" id="inputBuscaUser" class="form-control form-control-sm" placeholder="Digite o login do usuário...">
          <button type="submit" class="btn btn-sm btn-primary px-3 fw-bold" style="background:#D92243; border-color:#D92243;">
            Buscar
          </button>
        </form>
        <div id="btnVerMeuPerfil" class="text-end mt-2 d-none">
          <a href="#" id="linkVoltarMeuPerfil" class="small text-decoration-none" style="color:#7a1728;">← Ver meu próprio perfil</a>
        </div>
      </div>

      <!-- Detalhes do Usuário Carregado -->
      <div id="dadosUsuarioContainer">
        <div class="info-item">
          <div class="info-label">Nome Completo</div>
          <div class="info-value" id="exibeNome">—</div>
        </div>

        <div class="info-item">
          <div class="info-label">Login / E-mail</div>
          <div class="info-value" id="exibeLogin">—</div>
        </div>

        <div class="info-item">
          <div class="info-label">Tipo de Acesso</div>
          <div class="info-value" id="exibeTipo">—</div>
        </div>
      </div>

      <!-- Mensagens de Feedback -->
      <div id="message" class="text-center my-3 fw-semibold"></div>

      <!-- Ações -->
      <div class="mt-4">
        <!-- Botão Trocar Informações (Sempre visível para o próprio usuário e para funcionários) -->
        <button type="button" class="btn-acao-primario" id="btnTrocarInfo">
          ✏️ Editar Informações
        </button>

        <!-- Botão Deletar Usuário (Apenas funcionários visualizando outro usuário ou a si mesmos) -->
        <button type="button" class="btn-acao-deletar d-none" id="btnDeletarUser">
          🗑️ Deletar Usuário
        </button>

        <!-- Botão Voltar ao Início -->
        <button type="button" class="btn-acao-voltar mt-2" onclick="window.location.href='inicio.php'">
          ← Voltar ao Início
        </button>
      </div>

    </div>
  </main>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
  <script src="../JS/config.js"></script>
  <script src="../JS/dadosperfil.js"></script>
</body>

</html>