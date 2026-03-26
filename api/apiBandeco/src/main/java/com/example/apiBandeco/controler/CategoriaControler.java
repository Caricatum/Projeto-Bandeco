package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Categoria;
import com.example.apiBandeco.model.User;
import com.example.apiBandeco.repository.CategoriaRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@CrossOrigin
@RestController
@RequestMapping("/categoria")
public class CategoriaControler {
    @Autowired
    CategoriaRepository categoriaRepo;

    @GetMapping("/id/{id}")//busca uma categoria pelo id
    public Optional<Categoria> buscarPorId(@PathVariable("id") int id){
        return categoriaRepo.findById(id);
    }

    @GetMapping("/all")//busca todas as categorias
    public List<Categoria> buscarTodasCategorias(){return categoriaRepo.findAll();}

    @PostMapping("/cadastrarCategoria")//cadastra uma categoria
    public void cadastroCategoria (@RequestBody @Valid Categoria cate){
        categoriaRepo.save(cate);
    }

    @DeleteMapping("/deletarCategoria/{id}") //deleta categoria pelo id
    public void deletarCategoria(@PathVariable(value = "id") int id){
        categoriaRepo.deleteById(id);
    }

    @PutMapping("/atualizarCategoria") //Atualiza Categoria
    public void atualizaCategoria (@RequestBody @Valid Categoria categoria){
        categoriaRepo.save(categoria);
    }

}
