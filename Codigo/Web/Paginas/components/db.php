<?php
/**
 * Conexão Centralizada com Banco de Dados via PDO
 * Projeto Bandeco — Camada Web
 *
 * Utiliza Prepared Statements nativos para segurança contra SQL Injection
 * e configura o modo de erro seguro sem expor credenciais.
 */

function obterConexaoPDO() {
    static $pdo = null;

    if ($pdo !== null) {
        return $pdo;
    }

    $host     = '143.106.241.4';
    $porta    = '3306';
    $banco    = 'cl203271';
    $usuario  = 'cl203271';
    $senha    = 'cl*23042007';
    $charset  = 'utf8mb4';

    $dsn = "mysql:host={$host};port={$porta};dbname={$banco};charset={$charset}";

    $opcoes = [
        PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::ATTR_EMULATE_PREPARES   => false, // Utiliza prepared statements reais do banco
        PDO::ATTR_PERSISTENT         => false,
        PDO::ATTR_TIMEOUT            => 5,
    ];

    try {
        $pdo = new PDO($dsn, $usuario, $senha, $opcoes);
        return $pdo;
    } catch (PDOException $e) {
        error_log("[Bandeco PDO Error] " . $e->getMessage());
        return null;
    }
}
