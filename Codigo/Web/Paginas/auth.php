<?php
/**
 * Endpoint de Autenticação Segura via PDO e Sessão PHP
 * Projeto Bandeco — Camada Web
 *
 * Gerencia a sessão segura no servidor, inicializando $_SESSION com flags
 * protegidas (HttpOnly, SameSite) e suporte a Prepared Statements via PDO.
 */

header('Content-Type: application/json; charset=utf-8');

require_once __DIR__ . '/components/db.php';
require_once __DIR__ . '/components/auth_session.php';

$action = $_GET['action'] ?? $_POST['action'] ?? 'login';

// ── LOGOUT ─────────────────────────────────────────────────────────
if ($action === 'logout') {
    destruirSessaoSegura();
    echo json_encode([
        'sucesso' => true,
        'mensagem' => 'Sessão encerrada com sucesso.'
    ]);
    exit;
}

// ── CONSULTAR SESSÃO ATUAL (ME) ──────────────────────────────────
if ($action === 'me') {
    $usuario = obterUsuarioSessao();
    if ($usuario) {
        echo json_encode([
            'autenticado' => true,
            'usuario' => $usuario
        ]);
    } else {
        http_response_code(401);
        echo json_encode([
            'autenticado' => false,
            'mensagem' => 'Não autenticado.'
        ]);
    }
    exit;
}

// ── REGISTRO / CRIAÇÃO DA SESSÃO SEGURA (LOGIN) ──────────────────
if ($action === 'login') {
    $dados = json_decode(file_get_contents('php://input'), true) ?: $_POST;

    $login = trim($dados['login'] ?? $dados['username'] ?? '');
    $id = isset($dados['id']) ? (int)$dados['id'] : null;
    $nome = trim($dados['nome'] ?? '');
    $funcionario = !empty($dados['funcionario']);

    if (empty($login)) {
        http_response_code(400);
        echo json_encode([
            'sucesso' => false,
            'erro' => 'Identificador do usuário é obrigatório.'
        ]);
        exit;
    }

    // Se o ID ou nome não tiverem sido enviados, consulta via PDO com Prepared Statement
    if (empty($id) || empty($nome)) {
        $pdo = obterConexaoPDO();
        if ($pdo) {
            try {
                $stmt = $pdo->prepare('SELECT id, nome, login, funcionario FROM user WHERE login = :login LIMIT 1');
                $stmt->execute([':login' => $login]);
                $usuarioBanco = $stmt->fetch();

                if ($usuarioBanco) {
                    $id = (int)$usuarioBanco['id'];
                    $nome = $usuarioBanco['nome'];
                    $funcionario = (bool)$usuarioBanco['funcionario'];
                }
            } catch (PDOException $e) {
                error_log("[Auth PDO] Erro na consulta do usuário: " . $e->getMessage());
            }
        }
    }

    if (empty($id)) {
        http_response_code(401);
        echo json_encode([
            'sucesso' => false,
            'erro' => 'Usuário não localizado para iniciar sessão.'
        ]);
        exit;
    }

    // Inicia e regenera a sessão com proteção contra Session Fixation
    iniciarSessaoSegura();
    session_regenerate_id(true);

    $_SESSION['usuario_id'] = $id;
    $_SESSION['usuario_nome'] = $nome;
    $_SESSION['usuario_login'] = $login;
    $_SESSION['usuario_funcionario'] = $funcionario;
    $_SESSION['ultimo_acesso'] = time();

    echo json_encode([
        'sucesso' => true,
        'usuario' => [
            'id' => $id,
            'nome' => $nome,
            'login' => $login,
            'funcionario' => $funcionario
        ]
    ]);
    exit;
}

// Ação desconhecida
http_response_code(404);
echo json_encode([
    'sucesso' => false,
    'erro' => 'Ação não reconhecida.'
]);
