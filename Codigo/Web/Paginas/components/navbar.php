<!-- Navbar Compartilhada Bandeco -->
<nav class="navbar navbar-expand-lg">
    <div class="container">
        <img src="../Assets/Images/logo_unicamp.jpg" alt="Logo Unicamp" width="40" height="40" class="d-inline-block align-text-top me-2" onerror="this.style.display='none'">
        <a class="navbar-brand fw-bold" href="inicio.php">Bandeco</a>

        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ms-auto gap-2 align-items-center">
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
                    <button class="btn-nav" onclick="window.location.href='meusFavoritos.php'">⭐ Favoritos</button>
                </li>
                <li class="nav-item">
                    <button class="btn-nav" onclick="window.location.href='sobrenos.php'">ℹ️ Sobre nós</button>
                </li>
                
                <!-- Botão Dados do Perfil: visível se logado -->
                <li class="nav-item" id="itemPerfil" style="display: none;">
                    <button class="btn-nav btn-perfil" onclick="window.location.href='dadosperfil.php'" title="Meu Perfil">
                        <img id="navAvatarFoto" class="nav-avatar-img d-none" alt="Foto de Perfil" onerror="this.classList.add('d-none'); document.getElementById('navAvatarIcon').classList.remove('d-none');">
                        <span id="navAvatarIcon" class="nav-avatar-icon">👤</span>
                        <span id="navNomeUsuario" class="d-none d-sm-inline">Perfil</span>
                    </button>
                </li>

                <!-- Botão Login: visível se não logado -->
                <li class="nav-item" id="itemLogin">
                    <button id="btnLogin" class="btn-nav" onclick="window.location.href='login.php'">Login</button>
                </li>

                <!-- Botão Sair: visível se logado -->
                <li class="nav-item" id="itemSair">
                    <button class="btn-nav btn-sair" onclick="logout()">Sair</button>
                </li>
            </ul>
        </div>
    </div>
</nav>

<script>
// ============================================================================
// BLINDAGEM DE SESSÃO DO SERVIDOR (ANTIFRAUDE DEVTOOLS)
// Injeta a verdade absoluta da sessão PHP e impede adulterações no console
// ============================================================================
(function() {
    const sessaoServidor = <?php 
        require_once __DIR__ . '/auth_session.php';
        iniciarSessaoSegura();
        $u = obterUsuarioSessao();
        echo json_encode($u, JSON_HEX_TAG | JSON_HEX_APOS | JSON_HEX_QUOT | JSON_HEX_AMP);
    ?>;

    if (sessaoServidor && sessaoServidor.id) {
        // Cria objeto congelado (imutável) acessível na janela
        try {
            Object.defineProperty(window, 'USUARIO_ATUAL', {
                value: Object.freeze({
                    id: Number(sessaoServidor.id),
                    nome: String(sessaoServidor.nome || ''),
                    username: String(sessaoServidor.login || ''),
                    tipo: sessaoServidor.funcionario ? 'true' : 'false',
                    funcionario: Boolean(sessaoServidor.funcionario),
                    imagem: sessaoServidor.imagem ? String(sessaoServidor.imagem) : (localStorage.getItem('imagem') || '')
                }),
                writable: false,
                configurable: false
            });
        } catch (e) {
            window.USUARIO_ATUAL = Object.freeze(sessaoServidor);
        }

        // Restaura e sincroniza os armazenamentos locais com a verdade do servidor
        // (descartando qualquer adulteração feita via DevTools no console)
        sessionStorage.setItem('logado', 'true');
        localStorage.setItem('id', String(sessaoServidor.id));
        localStorage.setItem('nome', String(sessaoServidor.nome || ''));
        localStorage.setItem('username', String(sessaoServidor.login || ''));
        localStorage.setItem('tipo', sessaoServidor.funcionario ? 'true' : 'false');
        if (sessaoServidor.imagem) {
            localStorage.setItem('imagem', String(sessaoServidor.imagem));
        }
    } else {
        window.USUARIO_ATUAL = null;
        sessionStorage.setItem('logado', 'false');
        localStorage.removeItem('id');
        localStorage.removeItem('nome');
        localStorage.removeItem('username');
        localStorage.removeItem('tipo');
        localStorage.removeItem('imagem');
    }
})();

function resolverUrlImagemNav(url) {
    if (!url) return '';
    if (url.startsWith('http://') || url.startsWith('https://')) {
        return url;
    }
    const nomeArquivo = url.substring(url.lastIndexOf('/') + 1);
    return `imagem.php?arquivo=${encodeURIComponent(nomeArquivo)}`;
}

// Lógica global para gerenciamento do estado da Navbar
document.addEventListener('DOMContentLoaded', () => {
    const isLogado = !!(window.USUARIO_ATUAL && window.USUARIO_ATUAL.id);
    const itemLogin = document.getElementById('itemLogin');
    const itemSair = document.getElementById('itemSair');
    const itemPerfil = document.getElementById('itemPerfil');

    if (itemLogin) itemLogin.style.display = isLogado ? 'none' : 'block';
    if (itemSair) itemSair.style.display = isLogado ? 'block' : 'none';
    if (itemPerfil) itemPerfil.style.display = isLogado ? 'block' : 'none';

    if (isLogado) {
        const foto = (window.USUARIO_ATUAL && window.USUARIO_ATUAL.imagem) || localStorage.getItem('imagem');
        const nome = (window.USUARIO_ATUAL && window.USUARIO_ATUAL.nome) || localStorage.getItem('nome') || 'Perfil';
        const primeiroNome = nome.split(' ')[0] || 'Perfil';

        const navNome = document.getElementById('navNomeUsuario');
        if (navNome) navNome.textContent = primeiroNome;

        const navAvatarFoto = document.getElementById('navAvatarFoto');
        const navAvatarIcon = document.getElementById('navAvatarIcon');

        if (foto && navAvatarFoto && navAvatarIcon) {
            navAvatarFoto.src = resolverUrlImagemNav(foto);
            navAvatarFoto.classList.remove('d-none');
            navAvatarIcon.classList.add('d-none');
        } else if (navAvatarFoto && navAvatarIcon) {
            navAvatarFoto.classList.add('d-none');
            navAvatarIcon.classList.remove('d-none');
        }
    }
});

function logout() {
    fetch('auth.php?action=logout', { method: 'POST' })
        .finally(() => {
            sessionStorage.setItem('logado', 'false');
            localStorage.clear();
            sessionStorage.clear();
            window.location.href = 'login.php';
        });
}
</script>
