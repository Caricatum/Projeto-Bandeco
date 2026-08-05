package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Cardapio;
import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.repository.CardapioRepository;
import com.example.apiBandeco.repository.CategoriaRepository;
import com.example.apiBandeco.repository.PratosRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
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
import java.util.Map;
import java.util.UUID;

@CrossOrigin
@RestController
@RequestMapping("/pratos")
public class PratosController {

    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    CategoriaRepository categoriaRepo;
    @Autowired
    CardapioRepository cardapioRepository;

    @GetMapping("/id/{id}")//busca pratos pelo id
    public Pratos buscarPorId(@PathVariable("id") int id){
        return pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato não encontrado"
                ));
    }

    @GetMapping("/nome")//Busca o prato através do nome
    public List<Pratos> buscarPorNome(@RequestParam String nome) {
        return pratosRepository.findByNomeContainingIgnoreCase(nome);
    }

    @GetMapping("/all")//busca todos os pratos
    public List<Pratos> buscarTodosPratos(){return pratosRepository.findAll();}

    @GetMapping("/veganos")//busca todos os pratos veganos
    public List<Pratos> buscarTodosPratosVeganos(){
        return pratosRepository.findByVeganoTrue();
    }

    @PostMapping("/cadastrar")//cadastra um prato
    public Pratos cadastroPratos (@RequestBody @Valid Pratos prato){
        Integer categoriaId = prato.getCategoria() != null
                ? prato.getCategoria().getId() : null;

        if (categoriaId != null){
            var categoria = categoriaRepo.findById(categoriaId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Categoria não encontrada"));
            prato.setCategoria(categoria);
        }
        return pratosRepository.save(prato);
    }

    @DeleteMapping("/deletar/{id}")
    public void deletarPrato(@PathVariable int id) throws IOException {

        Pratos prato = pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Prato não encontrado"));

        List<Cardapio> cardapios = cardapioRepository.findAll();

        for (Cardapio c : cardapios) {

            if (c.getAcompanhamento() != null &&
                    c.getAcompanhamento().getId() == id) {
                c.setAcompanhamento(null);
            }

            if (c.getPrato_principal() != null &&
                    c.getPrato_principal().getId() == id) {
                c.setPrato_principal(null);
            }

            if (c.getGuarnicao() != null &&
                    c.getGuarnicao().getId() == id) {
                c.setGuarnicao(null);
            }

            if (c.getSalada() != null &&
                    c.getSalada().getId() == id) {
                c.setSalada(null);
            }

            if (c.getSobremesa() != null &&
                    c.getSobremesa().getId() == id) {
                c.setSobremesa(null);
            }

            if (c.getRefresco() != null &&
                    c.getRefresco().getId() == id) {
                c.setRefresco(null);
            }
        }

        cardapioRepository.saveAll(cardapios);

        if (prato.getImagem() != null && !prato.getImagem().isBlank()) {

            String nomeArquivo = prato.getImagem()
                    .substring(prato.getImagem().lastIndexOf("/") + 1);

            Path caminho = Paths.get("uploads/pratos", nomeArquivo);

            Files.deleteIfExists(caminho);
        }

        pratosRepository.delete(prato);
    }

    @PutMapping("/atualizar")
    public Pratos atualizaPratos (@RequestBody @Valid Pratos prato){
        pratosRepository.findById(prato.getId())
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

        return pratosRepository.save(prato);
    }


    @PostMapping("/upload") //envia a imagem e retorna seu link
    public Map<String, String> uploadImagem(
            @RequestParam("imagem") MultipartFile arquivo)
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

        String nomeArquivo = UUID.randomUUID() + "." + extensao;

        List<String> permitidas = List.of("jpg", "jpeg", "png", "webp");

        if (!permitidas.contains(extensao.toLowerCase())) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Formato de imagem inválido");
        }

        if (extensao.isBlank()) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Arquivo sem extensão");
        }

        Path destino = Paths.get("uploads/pratos", nomeArquivo);

        Files.createDirectories(destino.getParent());

        Files.copy(
                arquivo.getInputStream(),
                destino,
                StandardCopyOption.REPLACE_EXISTING
        );

        String url = ServletUriComponentsBuilder
                .fromCurrentContextPath()
                .path("/uploads/pratos/")
                .path(nomeArquivo)
                .toUriString();
        return Map.of("url", url);
    }

    @DeleteMapping("/imagem/{id}") // Deleta a imagem do prato
    public void deletarImagem(@PathVariable Integer id) throws IOException {

        Pratos prato = pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Prato não encontrado"));

        if (prato.getImagem() == null || prato.getImagem().isBlank()) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "O prato não possui imagem");
        }

        String nomeArquivo = prato.getImagem()
                .substring(prato.getImagem().lastIndexOf("/") + 1);

        Path caminho = Paths.get("uploads/pratos", nomeArquivo);

        Files.deleteIfExists(caminho);

        Files.deleteIfExists(caminho);

        prato.setImagem(null);

        pratosRepository.save(prato);
    }

}
