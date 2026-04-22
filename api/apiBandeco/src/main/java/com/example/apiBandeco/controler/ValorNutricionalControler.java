package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.ValorNutricional;
import com.example.apiBandeco.repository.PratosRepository;
import com.example.apiBandeco.repository.ValorNutricionalRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/valorNutricional")
public class ValorNutricionalControler {
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    ValorNutricionalRepository valorNutricionalRepository;

    @GetMapping("/id/{id}")//busca valor nutricional pelo id
    public ValorNutricional buscarPorId(@PathVariable("id") int id){
        return valorNutricionalRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "valor Nutricional não encontrado"
                ));
    }

    @GetMapping("/all")//busca todos os valores nutricionais
    public List<ValorNutricional> buscarTodosValores(){return valorNutricionalRepository.findAll();}

    @PostMapping("/cadastrar")//cadastra um valor nutricional
    public ValorNutricional cadastroValoresNutricionais (@RequestBody @Valid ValorNutricional valorNutricional){
        Integer pratoId = valorNutricional.getPrato() != null
                ? valorNutricional.getPrato().getId() : null;

        if (pratoId != null){
            var prato = pratosRepository.findById(pratoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Prato não encontrado"));
            valorNutricional.setPrato(prato);
        }
        return valorNutricionalRepository.save(valorNutricional);
    }

    @DeleteMapping("/deletar/{id}") //deleta valor nutricional pelo id
    public void deletarValor(@PathVariable(value = "id") int id){
        valorNutricionalRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Valor Nutricional não encontrado"));

        valorNutricionalRepository.deleteById(id);
    }

    @PutMapping("/atualizar")
    public ValorNutricional atualizaValor (@RequestBody @Valid ValorNutricional valorNutricional){
        valorNutricionalRepository.findById(valorNutricional.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Valor Nutricional não encontrado"));

        Integer pratoId = valorNutricional.getPrato() != null
                ? valorNutricional.getPrato().getId() : null;

        if (pratoId != null){
            var prato = pratosRepository.findById(pratoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Prato não encontrado"));
            valorNutricional.setPrato(prato);
        }

        return valorNutricionalRepository.save(valorNutricional);
    }
}
