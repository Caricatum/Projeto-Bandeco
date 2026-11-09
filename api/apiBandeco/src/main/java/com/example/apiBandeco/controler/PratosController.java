package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Cardapio;
import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.repository.CardapioRepository;
import com.example.apiBandeco.repository.CategoriaRepository;
import com.example.apiBandeco.repository.PratosRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.List;
import java.util.UUID;

@CrossOrigin
@RestController
@RequestMapping({"/pratos", "/api/v1/pratos"})
public class PratosController {

    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    CategoriaRepository categoriaRepo;
    @Autowired
    CardapioRepository cardapioRepository;

    // Busca prato por ID (REST: GET /pratos/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public Pratos buscarPorId(@PathVariable("id") int id){
        return pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato não encontrado"
                ));
    }

    // Listagem com suporte a filtros por query params (REST: GET /pratos?nome=...&vegano=...)
    @GetMapping
    public List<Pratos> listarPratos(
            @RequestParam(required = false) String nome,
            @RequestParam(required = false) Boolean vegano) {

        if (nome != null && !nome.isBlank()) {
            return pratosRepository.findByNomeContainingIgnoreCase(nome);
        }
        if (vegano != null && vegano) {
            return pratosRepository.findByVeganoTrue();
        }
        return pratosRepository.findAll();
    }

    // Rota legada de busca por nome
    @GetMapping("/nome")
    public List<Pratos> buscarPorNome(@RequestParam String nome) {
        return pratosRepository.findByNomeContainingIgnoreCase(nome);
    }

    // Rota legada de busca por veganos
    @GetMapping("/veganos")
    public List<Pratos> buscarTodosPratosVeganos(){
        return pratosRepository.findByVeganoTrue();
    }

    // Cadastro de prato (REST: POST /pratos)
    @PostMapping(consumes = {MediaType.MULTIPART_FORM_DATA_VALUE, MediaType.APPLICATION_JSON_VALUE})
    @ResponseStatus(HttpStatus.CREATED)
    public Pratos cadastroPratos (@RequestPart("prato") @Valid Pratos prato,
                                  @RequestPart(value = "imagem", required = false) MultipartFile arquivo)
            throws IOException{
        Integer categoriaId = prato.getCategoria() != null
                ? prato.getCategoria().getId() : null;

        if (categoriaId != null){
            var categoria = categoriaRepo.findById(categoriaId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Categoria não encontrada"));
            prato.setCategoria(categoria);
        }

        if (arquivo != null && !arquivo.isEmpty()) {
            String urlImagem = uploadImagem(arquivo);
            prato.setImagem(urlImagem);
        } else {
            prato.setImagem(null);
        }

        return pratosRepository.save(prato);
    }

    // Atualização de prato (REST: PUT /pratos/{id})
    @PutMapping(value = {"/{id}", "/atualizar", ""}, consumes = {MediaType.MULTIPART_FORM_DATA_VALUE, MediaType.APPLICATION_JSON_VALUE})
    public Pratos atualizaPratos (@PathVariable(value = "id", required = false) Integer pathId,
                                  @RequestPart("prato") @Valid Pratos prato,
                                  @RequestPart(value = "imagem", required = false) MultipartFile arquivo)
            throws IOException{

        int id = pathId != null ? pathId : prato.getId();
        Pratos pratoAtual = pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato não encontrado"));

        Integer categoriaId = prato.getCategoria() != null
                ? prato.getCategoria().getId() : null;

        if (categoriaId != null){
            var categoria = categoriaRepo.findById(categoriaId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Categoria não encontrada"));
            prato.setCategoria(categoria);
        }

        if (arquivo != null && !arquivo.isEmpty()) {
            String urlImagem = uploadImagem(arquivo);
            deletarImagem(pratoAtual.getId());
            pratoAtual.setImagem(urlImagem);
        } else if (prato.getImagem() != null && !prato.getImagem().isBlank()) {
            pratoAtual.setImagem(prato.getImagem());
        }

        pratoAtual.setNome(prato.getNome());
        pratoAtual.setDescricao(prato.getDescricao());
        pratoAtual.setVegano(prato.isVegano());
        if (prato.getCategoria() != null) {
            pratoAtual.setCategoria(prato.getCategoria());
        }

        pratoAtual.setLeite(prato.isLeite());
        pratoAtual.setOvo(prato.isOvo());
        pratoAtual.setAmendoim(prato.isAmendoim());
        pratoAtual.setCastanhas(prato.isCastanhas());
        pratoAtual.setFrutosDoMar(prato.isFrutosDoMar());
        pratoAtual.setPeixes(prato.isPeixes());
        pratoAtual.setTrigo(prato.isTrigo());
        pratoAtual.setSoja(prato.isSoja());
        pratoAtual.setGergilim(prato.isGergilim());

        pratoAtual.setAltoAcucarAdicionado(prato.isAltoAcucarAdicionado());
        pratoAtual.setAltoGorduraSaturada(prato.isAltoGorduraSaturada());
        pratoAtual.setAltoSodio(prato.isAltoSodio());

        return pratosRepository.save(pratoAtual);
    }

    // Exclusão de prato (REST: DELETE /pratos/{id})
    @DeleteMapping
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarPrato(@PathVariable int id) throws IOException {

        Pratos prato = pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Prato não encontrado"));

        List<Cardapio> cardapios = cardapioRepository.findAll();

        for (Cardapio c : cardapios) {
            if (c.getAcompanhamento() != null && c.getAcompanhamento().getId() == id) {
                c.setAcompanhamento(null);
            }
            if (c.getPrato_principal() != null && c.getPrato_principal().getId() == id) {
                c.setPrato_principal(null);
            }
            if (c.getGuarnicao() != null && c.getGuarnicao().getId() == id) {
                c.setGuarnicao(null);
            }
            if (c.getSalada() != null && c.getSalada().getId() == id) {
                c.setSalada(null);
            }
            if (c.getSobremesa() != null && c.getSobremesa().getId() == id) {
                c.setSobremesa(null);
            }
            if (c.getRefresco() != null && c.getRefresco().getId() == id) {
                c.setRefresco(null);
            }
        }

        cardapioRepository.saveAll(cardapios);
        deletarImagem(prato.getId());
        pratosRepository.delete(prato);
    }

    // Exclusão de imagem do prato (REST: DELETE /pratos/{id}/imagem)
    @DeleteMapping(value = {"/{id}/imagem", "/excluirImagem/{id}"})
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void excluirImagem(@PathVariable int id) throws IOException{
        Pratos prato = pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Prato não encontrado"));

        deletarImagem(prato.getId());
        prato.setImagem(null);
        pratosRepository.save(prato);
    }

    // Envia a imagem e retorna seu link
    private String uploadImagem(MultipartFile arquivo)
            throws IOException {
        if (arquivo.isEmpty()) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Nenhum arquivo enviado");
        }

        if (arquivo.getContentType() == null ||
                !arquivo.getContentType().startsWith("image/")) {

            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "O arquivo deve ser uma imagem");
        }

        String nomeOriginal = arquivo.getOriginalFilename();
        String extensao = "";

        if (nomeOriginal != null && nomeOriginal.contains(".")) {
            extensao = nomeOriginal.substring(nomeOriginal.lastIndexOf(".") + 1);
        }

        extensao = extensao.toLowerCase();

        List<String> permitidas = List.of("jpg", "jpeg", "png", "webp");

        if (extensao.isBlank()) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Arquivo sem extensão");
        }

        if (!permitidas.contains(extensao)) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Formato inválido");
        }

        String nomeArquivo = UUID.randomUUID() + "." + extensao;

        Path destino = Paths.get("uploads/pratos", nomeArquivo);

        Files.createDirectories(destino.getParent());

        Files.copy(
                arquivo.getInputStream(),
                destino,
                StandardCopyOption.REPLACE_EXISTING
        );

        return ServletUriComponentsBuilder
                .fromCurrentContextPath()
                .path("/uploads/pratos/")
                .path(nomeArquivo)
                .toUriString();
    }

    // Deleta a imagem do prato
    private void deletarImagem(Integer id) throws IOException {
        Pratos prato = pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Prato não encontrado"));

        if (prato.getImagem() == null || prato.getImagem().isBlank()) {
            return;
        }

        String nomeArquivo = prato.getImagem()
                .substring(prato.getImagem().lastIndexOf("/") + 1);

        Path caminho = Paths.get("uploads/pratos", nomeArquivo);

        Files.deleteIfExists(caminho);
    }
}
