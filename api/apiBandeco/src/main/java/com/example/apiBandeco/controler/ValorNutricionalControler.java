package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.model.ValorNutricional;
import com.example.apiBandeco.repository.PratosRepository;
import com.example.apiBandeco.repository.ValorNutricionalRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@CrossOrigin
@RestController
@RequestMapping("/valorNutricional")
public class ValorNutricionalControler {
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    ValorNutricionalRepository valorNutricionalRepository;

    @GetMapping("/id/{id}")//busca valor nutricional pelo id
    public Optional<ValorNutricional> buscarPorId(@PathVariable("id") int id){
        return valorNutricionalRepository.findById(id);
    }

    @GetMapping("/all")//busca todos os valores nutricionais
    public List<ValorNutricional> buscarTodosValores(){return valorNutricionalRepository.findAll();}

    @PostMapping("/cadastrarValores")//cadastra um valor nutricional
    public void cadastroValoresNutricionais (@RequestBody @Valid ValorNutricional valorNutricional){
        int pratoId = valorNutricional.getPrato().getId();

        pratosRepository.findById(pratoId)
                .orElseThrow(() -> new RuntimeException("Prato não existe"));
        valorNutricionalRepository.save(valorNutricional);
    }

    @DeleteMapping("/deletarValor/{id}") //deleta valor nutricional pelo id
    public void deletarValor(@PathVariable(value = "id") int id){
        valorNutricionalRepository.deleteById(id);
    }

    @PutMapping("/atualizarValor")
    public void atualizaValor (@RequestBody @Valid ValorNutricional valorNutricional){

        int pratoId = valorNutricional.getPrato().getId();

        var prato = pratosRepository.findById(pratoId)
                .orElseThrow(() -> new RuntimeException("Prato não existe"));

        valorNutricional.setPrato(prato);

        valorNutricionalRepository.save(valorNutricional);
    }
}
