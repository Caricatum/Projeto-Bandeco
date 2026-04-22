package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.repository.CategoriaRepository;
import com.example.apiBandeco.repository.PratosRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/pratos")
public class PratosControler {

    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    CategoriaRepository categoriaRepo;

    @GetMapping("/id/{id}")//busca pratos pelo id
    public Pratos buscarPorId(@PathVariable("id") int id){
        return pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato não encontrado"
                ));
    }

    @GetMapping("/all")//busca todos os pratos
    public List<Pratos> buscarTodosPratos(){return pratosRepository.findAll();}

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

    @DeleteMapping("/deletar/{id}") //deleta prato pelo id
    public void deletarPrato(@PathVariable(value = "id") int id){
        pratosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato não encontrado"));
        pratosRepository.deleteById(id);
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

}
