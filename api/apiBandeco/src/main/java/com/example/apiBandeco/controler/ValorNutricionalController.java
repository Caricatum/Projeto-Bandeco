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
@RequestMapping({"/valorNutricional", "/valores-nutricionais", "/api/v1/valores-nutricionais"})
public class ValorNutricionalController {
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    ValorNutricionalRepository valorNutricionalRepository;

    // Busca valor nutricional por ID (REST: GET /valores-nutricionais/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public ValorNutricional buscarPorId(@PathVariable("id") int id){
        return valorNutricionalRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Valor Nutricional não encontrado"
                ));
    }

    // Listagem geral (REST: GET /valores-nutricionais)
    @GetMapping
    public List<ValorNutricional> buscarTodosValores(){
        return valorNutricionalRepository.findAll();
    }

    // Cadastro de valor nutricional (REST: POST /valores-nutricionais)
    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
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

    // Atualização de valor nutricional (REST: PUT /valores-nutricionais/{id})
    @PutMapping({"/{id}", "/atualizar", ""})
    public ValorNutricional atualizaValor (@PathVariable(value = "id", required = false) Integer pathId,
                                           @RequestBody @Valid ValorNutricional valorNutricional){
        int id = pathId != null ? pathId : valorNutricional.getId();
        valorNutricionalRepository.findById(id)
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
        valorNutricional.setId(id);
        return valorNutricionalRepository.save(valorNutricional);
    }

    // Exclusão de valor nutricional (REST: DELETE /valores-nutricionais/{id})
    @DeleteMapping
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarValor(@PathVariable(value = "id") int id){
        valorNutricionalRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Valor Nutricional não encontrado"));

        valorNutricionalRepository.deleteById(id);
    }
}
