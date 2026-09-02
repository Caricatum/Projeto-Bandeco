<?php
/**
 * Proxy de imagens dos pratos.
 * Serve arquivos da pasta api/apiBandeco/uploads/pratos/ diretamente pelo PHP,
 * evitando dependencia do servidor de arquivos estaticos do Spring Boot.
 *
 * Uso: imagem.php?arquivo=nome-do-arquivo.webp
 */

$arquivo = $_GET["arquivo"] ?? "";

// Sanitiza: aceita apenas nome de arquivo, sem barras nem ".."
$arquivo = basename($arquivo);

if (!$arquivo) {
    http_response_code(400);
    exit("Arquivo nao informado.");
}

// Caminho absoluto ate a pasta de uploads
$pastaUploads = __DIR__ . "/../../../api/apiBandeco/uploads/pratos/";
$caminho = realpath($pastaUploads . $arquivo);

// Garante que o arquivo esta dentro da pasta esperada (seguranca)
$pastaNormalizada = realpath($pastaUploads);
if (!$caminho || !$pastaNormalizada || strpos($caminho, $pastaNormalizada) !== 0) {
    http_response_code(404);
    exit("Imagem nao encontrada.");
}

if (!file_exists($caminho)) {
    http_response_code(404);
    exit("Imagem nao encontrada.");
}

// Detecta o tipo MIME pela extensao
$ext = strtolower(pathinfo($caminho, PATHINFO_EXTENSION));
$mimes = [
    "jpg"  => "image/jpeg",
    "jpeg" => "image/jpeg",
    "png"  => "image/png",
    "webp" => "image/webp",
    "gif"  => "image/gif",
];

$contentType = $mimes[$ext] ?? "application/octet-stream";

// Cache de 1 dia no navegador
header("Content-Type: " . $contentType);
header("Content-Length: " . filesize($caminho));
header("Cache-Control: public, max-age=86400");
header("Last-Modified: " . gmdate("D, d M Y H:i:s", filemtime($caminho)) . " GMT");

readfile($caminho);
exit;
