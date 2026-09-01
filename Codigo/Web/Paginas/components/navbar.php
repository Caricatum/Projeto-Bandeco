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
// Lógica global para gerenciamento do estado da Navbar
document.addEventListener('DOMContentLoaded', () => {
    const isLogado = sessionStorage.getItem('logado') === 'true';
    const itemLogin = document.getElementById('itemLogin');
    const itemSair = document.getElementById('itemSair');

    if (itemLogin) itemLogin.style.display = isLogado ? 'none' : 'block';
    if (itemSair) itemSair.style.display = isLogado ? 'block' : 'none';
});

function logout() {
    sessionStorage.setItem('logado', 'false');
    localStorage.clear();
    window.location.href = 'login.php';
}
</script>
