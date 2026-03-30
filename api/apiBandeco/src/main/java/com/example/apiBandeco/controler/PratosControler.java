package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.repository.CategoriaRepository;
import com.example.apiBandeco.repository.PratosRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@CrossOrigin
@RestController
@RequestMapping("/pratos")
public class PratosControler {

    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    CategoriaRepository categoriaRepo;

    @GetMapping("/id/{id}")//busca pratos pelo id
    public Optional<Pratos> buscarPorId(@PathVariable("id") int id){
        return pratosRepository.findById(id);
    }

    @GetMapping("/all")//busca todos os pratos
    public List<Pratos> buscarTodosPratos(){return pratosRepository.findAll();}

    @PostMapping("/cadastrarPratos")//cadastra um prato
    public void cadastroPratos (@RequestBody @Valid Pratos prato){
        int categoriaId = prato.getCategoria().getId();

        categoriaRepo.findById(categoriaId)
                .orElseThrow(() -> new RuntimeException("Categoria não existe"));
        pratosRepository.save(prato);
    }

    @DeleteMapping("/deletarPratos/{id}") //deleta prato pelo id
    public void deletarPrato(@PathVariable(value = "id") int id){
        pratosRepository.deleteById(id);
    }

    @PutMapping("/atualizarPratos")
    public void atualizaPratos (@RequestBody @Valid Pratos prato){

        int categoriaId = prato.getCategoria().getId();

        var categoria = categoriaRepo.findById(categoriaId)
                .orElseThrow(() -> new RuntimeException("Categoria não existe"));

        prato.setCategoria(categoria);

        pratosRepository.save(prato);
    }

}
