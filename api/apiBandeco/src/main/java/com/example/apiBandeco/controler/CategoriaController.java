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
@RequestMapping("/categoria")
public class CategoriaController {
    @Autowired
    CategoriaRepository categoriaRepo;

    @GetMapping("/id/{id}")//busca uma categoria pelo id
    public Categoria buscarPorId(@PathVariable("id") int id){
        return categoriaRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Categoria não encontrada"
                ));
    }

    @GetMapping("/all")//busca todas as categorias
    public List<Categoria> buscarTodasCategorias(){return categoriaRepo.findAll();}

    @PostMapping("/cadastrar")//cadastra uma categoria
    public void cadastroCategoria (@RequestBody @Valid Categoria cate){
        categoriaRepo.save(cate);
    }

    @DeleteMapping("/deletar/{id}") //deleta categoria pelo id
    public void deletarCategoria(@PathVariable(value = "id") int id){
        categoriaRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Categoria não encontrada"));
        categoriaRepo.deleteById(id);
    }

    @PutMapping("/atualizar") //Atualiza Categoria
    public void atualizaCategoria (@RequestBody @Valid Categoria categoria){
        categoriaRepo.findById(categoria.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Categoria não encontrada"));
        categoriaRepo.save(categoria);
    }

}
