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


    @PostMapping(value = "/cadastrar",consumes = MediaType.MULTIPART_FORM_DATA_VALUE)//cadastra um prato
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

        deletarImagem(prato.getId());

        pratosRepository.delete(prato);
    }

    @DeleteMapping(value = "/excluirImagem/{id}")
    public void excluirImagem(@PathVariable int id) throws IOException{
        Pratos prato = pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Prato não encontrado"));

        deletarImagem(prato.getId());
        prato.setImagem(null);
        pratosRepository.save(prato);
    }

    @PutMapping(value = "/atualizar",consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public Pratos atualizaPratos (@RequestPart("prato") @Valid Pratos prato,
                                  @RequestPart(value = "imagem", required = false) MultipartFile arquivo)
            throws IOException{
        Pratos pratoAtual = pratosRepository.findById(prato.getId())
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
            deletarImagem(prato.getId());
            pratoAtual.setImagem(urlImagem);
        } else {
            prato.setImagem(
                    pratoAtual.getImagem()
            );
        }

        pratoAtual.setNome(prato.getNome());
        pratoAtual.setDescricao(prato.getDescricao());
        pratoAtual.setVegano(prato.isVegano());
        pratoAtual.setCategoria(prato.getCategoria());

        return pratosRepository.save(pratoAtual);
    }


     //envia a imagem e retorna seu link
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
