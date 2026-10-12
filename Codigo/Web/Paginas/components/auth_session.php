<?php
/**
 * Gerenciador Seguro de Sessão PHP
 * Projeto Bandeco — Camada Web
 *
 * Configura cookies de sessão com HttpOnly, SameSite,
 * regeneração de ID contra Session Fixation e proteção por timeout.
 */

function iniciarSessaoSegura() {
    if (session_status() === PHP_SESSION_NONE) {
        $tempoExpiracao = 7200; // 2 horas de inatividade

        // Parâmetros de segurança para o cookie de sessão
        ini_set('session.use_only_cookies', 1);
        ini_set('session.use_strict_mode', 1);

        session_set_cookie_params([
            'lifetime' => $tempoExpiracao,
            'path'     => '/',
            'domain'   => '',
            'secure'   => isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on',
            'httponly' => true,  // Impede acesso aos cookies via scripts maliciosos (XSS)
            'samesite' => 'Lax'  // Proteção contra CSRF
        ]);

        session_start();

        // Verificação de timeout por inatividade
        if (isset($_SESSION['ultimo_acesso']) && (time() - $_SESSION['ultimo_acesso'] > $tempoExpiracao)) {
            destruirSessaoSegura();
            session_start();
        }
        $_SESSION['ultimo_acesso'] = time();
    }
}

/**
 * Valida a sessão no servidor. Redireciona para login.php se não estiver logado.
 * @param bool $requerFuncionario Se true, restringe a página apenas para funcionários
 */
function verificarAutenticacao($requerFuncionario = false) {
    iniciarSessaoSegura();

    $logado = !empty($_SESSION['usuario_id']);

    if (!$logado) {
        header('Location: login.php');
        exit;
    }

    if ($requerFuncionario && empty($_SESSION['usuario_funcionario'])) {
        header('Location: inicio.php');
        exit;
    }
}

/**
 * Retorna os dados essenciais e seguros do usuário na sessão
 */
function obterUsuarioSessao() {
    iniciarSessaoSegura();
    if (empty($_SESSION['usuario_id'])) {
        return null;
    }

    return [
        'id'          => $_SESSION['usuario_id'],
        'nome'        => $_SESSION['usuario_nome'] ?? '',
        'login'       => $_SESSION['usuario_login'] ?? '',
        'funcionario' => !empty($_SESSION['usuario_funcionario']),
    ];
}

/**
 * Destrói completamente a sessão no servidor e invalida o cookie do cliente
 */
function destruirSessaoSegura() {
    if (session_status() === PHP_SESSION_NONE) {
        session_start();
    }

    $_SESSION = [];

    if (ini_get("session.use_cookies")) {
        $params = session_get_cookie_params();
        setcookie(
            session_name(),
            '',
            time() - 42000,
            $params["path"],
            $params["domain"],
            $params["secure"],
            $params["httponly"]
        );
    }

    session_destroy();
}
