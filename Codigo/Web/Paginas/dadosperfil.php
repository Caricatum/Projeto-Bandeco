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

      <!-- Barra de Busca e Lista de Usuários (Visível apenas para Funcionários) -->
      <div id="boxBuscaFuncionario" class="busca-box d-none">
        <div class="d-flex justify-content-between align-items-center mb-2">
          <label class="info-label mb-0">👥 Gerenciamento de Usuários</label>
          <span class="badge bg-secondary text-white small" id="contadorUsuarios">0 cadastrados</span>
        </div>

        <!-- Campo de Busca em Tempo Real -->
        <div class="input-group input-group-sm mb-2">
          <span class="input-group-text bg-white">🔍</span>
          <input type="text" id="inputBuscaUser" class="form-control" placeholder="Buscar por nome ou e-mail...">
          <button class="btn btn-outline-secondary" type="button" id="btnLimparBusca">✕</button>
        </div>

        <!-- Filtros Rápidos -->
        <div class="d-flex gap-1 mb-2">
          <button type="button" class="btn btn-sm btn-filtro-user active" data-filtro="todos">Todos</button>
          <button type="button" class="btn btn-sm btn-filtro-user" data-filtro="alunos">🎓 Alunos</button>
          <button type="button" class="btn btn-sm btn-filtro-user" data-filtro="funcionarios">👔 Funcionários</button>
        </div>

        <!-- Lista Rolável de Usuários -->
        <div id="listaUsuariosScroll" class="lista-usuarios-scroll">
          <div class="text-center py-3 text-muted small">
            <div class="spinner-border spinner-border-sm text-danger" role="status"></div>
            Carregando usuários...
          </div>
        </div>

        <div id="btnVerMeuPerfil" class="text-end mt-2 d-none">
          <a href="#" id="linkVoltarMeuPerfil" class="small text-decoration-none fw-bold" style="color:#D92243;">
            👤 Voltar ao meu perfil
          </a>
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