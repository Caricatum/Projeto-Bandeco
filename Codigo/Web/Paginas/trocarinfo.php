<?php
require_once __DIR__ . '/components/auth_session.php';
verificarAutenticacao();
?>
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

  <main class="main-container">
    <div class="edit-card">
      
      <!-- Cabeçalho -->
      <div class="edit-header">
        <div class="edit-avatar-wrapper">
          <div class="edit-avatar" id="avatarIcon">✏️</div>
          <img id="avatarFoto" class="edit-foto d-none" alt="Foto do Usuário" onerror="mostrarIconePadraoAvatar()">
        </div>
        <div class="mb-3">
          <label for="fotoPerfil" class="btn btn-sm btn-outline-danger">📷 Trocar Foto</label>
          <input type="file" id="fotoPerfil" accept="image/png, image/jpeg, image/jpg, image/webp" class="d-none">
          <button type="button" id="btnRemoverFoto" class="btn btn-sm btn-outline-secondary ms-1 d-none">Remover Foto</button>
          <small class="text-muted d-block mt-1" style="font-size:0.75rem;">Formatos aceitos: JPG, PNG, WEBP</small>
        </div>
        <h2 class="edit-title" id="tituloEdicao">Editar Informações</h2>
        <p class="edit-subtitle" id="subtituloEdicao">Atualize os dados de cadastro</p>
      </div>

      <form id="dadosForm">
        
        <!-- Campo Nome Completo -->
        <div class="form-group-custom">
          <label class="form-label-custom" for="name">👤 Nome Completo</label>
          <input type="text" id="name" class="input-custom" placeholder="Digite seu nome completo" required>
        </div>

        <!-- Campo Usuário / E-mail -->
        <div class="form-group-custom">
          <label class="form-label-custom" for="username">🔑 Login / E-mail</label>
          <input type="text" id="username" class="input-custom" placeholder="Digite seu login ou e-mail" required>
        </div>

        <!-- Restrições Alimentares / Alergias -->
        <div class="form-group-custom">
          <label class="form-label-custom mb-1">⚠️ Restrições Alimentares / Alergias</label>
          <small class="text-muted d-block mb-2">Marque ou desmarque conforme suas necessidades:</small>
          <div class="alergias-grid">
            <label class="alergia-chip"><input type="checkbox" id="alergiaLeite"> 🥛 Leite</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaOvo"> 🥚 Ovo</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaAmendoim"> 🥜 Amendoim</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaCastanhas"> 🌰 Castanhas</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaFrutosDoMar"> 🦐 Frutos Mar</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaPeixes"> 🐟 Peixes</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaTrigo"> 🌾 Trigo/Glúten</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaSoja"> 🌱 Soja</label>
            <label class="alergia-chip"><input type="checkbox" id="alergiaGergilim"> ⚪ Gergelim</label>
          </div>
        </div>

        <!-- Seção: Tipo de Usuário (Visível e Editável apenas para Funcionários) -->
        <div id="secaoTipoFuncionario" class="tipo-selector-box d-none">
          <label class="form-label-custom mb-1">👔 Nível de Acesso (Tipo de Pessoa)</label>
          <small class="text-muted d-block mb-2">Selecione o papel do usuário no sistema:</small>

          <div class="tipo-options-container">
            <div class="tipo-card-option" id="cardAluno" data-value="false">
              🎓 Aluno
            </div>
            <div class="tipo-card-option" id="cardFunc" data-value="true">
              👔 Funcionário
            </div>
          </div>
          <input type="hidden" id="tipoUsuarioValor" value="false">
        </div>

        <!-- Aviso para Alunos (Não podem mudar tipo de conta) -->
        <div id="avisoTipoAluno" class="aviso-aluno-box d-none">
          <span style="font-size:1.8rem">🎓</span>
          <div>
            <strong class="d-block text-dark" style="font-size:0.95rem">Conta de Aluno</strong>
            <small class="text-muted">Alterações no nível de acesso só podem ser efetuadas pela equipe de administração.</small>
          </div>
        </div>

        <!-- Mensagens -->
        <div id="message" class="text-center my-3 fw-semibold"></div>

        <!-- Botões -->
        <div class="mt-4">
          <button type="button" class="btn-salvar-info" id="trocarinfo">
            💾 Salvar Alterações
          </button>
          
          <button type="button" class="btn-cancelar-info mt-2" onclick="window.location.href='dadosperfil.php'">
            ← Cancelar e Voltar
          </button>
        </div>

      </form>

    </div>
  </main>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
  <script src="../JS/config.js"></script>
  <script src="../JS/trocarinfo.js"></script>
</body>
</html>
