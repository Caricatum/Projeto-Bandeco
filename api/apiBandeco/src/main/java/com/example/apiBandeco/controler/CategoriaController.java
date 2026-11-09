package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Categoria;
import com.example.apiBandeco.repository.CategoriaRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping({"/categoria", "/categorias", "/api/v1/categorias"})
public class CategoriaController {
    @Autowired
    CategoriaRepository categoriaRepo;

    // Busca categoria por ID (REST: GET /categorias/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public Categoria buscarPorId(@PathVariable("id") int id){
        return categoriaRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Categoria não encontrada"
                ));
    }

    // Listagem de categorias (REST: GET /categorias)
    @GetMapping
    public List<Categoria> buscarTodasCategorias(){
        return categoriaRepo.findAll();
    }

    // Cadastro de categoria (REST: POST /categorias)
    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public Categoria cadastroCategoria (@RequestBody @Valid Categoria cate){
        return categoriaRepo.save(cate);
    }

    // Atualização de categoria (REST: PUT /categorias/{id})
    @PutMapping({"/{id}", "/atualizar", ""})
    public Categoria atualizaCategoria (@PathVariable(value = "id", required = false) Integer pathId,
                                        @RequestBody @Valid Categoria categoria){
        int id = pathId != null ? pathId : categoria.getId();
        categoriaRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Categoria não encontrada"));
        categoria.setId(id);
        return categoriaRepo.save(categoria);
    }

    // Exclusão de categoria (REST: DELETE /categorias/{id})
    @DeleteMapping
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarCategoria(@PathVariable(value = "id") int id){
        categoriaRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Categoria não encontrada"));
        categoriaRepo.deleteById(id);
    }
}
