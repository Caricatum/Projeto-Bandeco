package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Avisos;
import com.example.apiBandeco.repository.AvisosRepository;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping({"/avisos", "/api/v1/avisos"})
public class AvisosController {
    @Autowired
    UserRepository userRepository;
    @Autowired
    AvisosRepository avisosRepository;

    // Busca aviso por ID (REST: GET /avisos/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public Avisos buscarPorId(@PathVariable("id") int id){
        return avisosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Aviso não encontrado"
                ));
    }

    // Listagem de avisos (REST: GET /avisos)
    @GetMapping
    public List<Avisos> buscarTodosAvisos(){
        return avisosRepository.findAll();
    }

    // Cadastro de aviso (REST: POST /avisos)
    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public Avisos cadastroAvisos (@RequestBody @Valid Avisos aviso){
        Integer userId = aviso.getUser() != null
                ? aviso.getUser().getId() : null;
        if (userId != null){
            var user = userRepository.findById(userId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "User não encontrado"));
            aviso.setUser(user);
        }

        return avisosRepository.save(aviso);
    }

    // Atualização de aviso (REST: PUT /avisos/{id})
    @PutMapping({"/{id}", "/atualizar", ""})
    public Avisos atualizaAvisos (@PathVariable(value = "id", required = false) Integer pathId,
                                  @RequestBody @Valid Avisos aviso){
        int id = pathId != null ? pathId : aviso.getId();
        avisosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Aviso não encontrado"));

        Integer userId = aviso.getUser() != null
                ? aviso.getUser().getId() : null;
        if (userId != null){
            var user = userRepository.findById(userId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "User não encontrado"));
            aviso.setUser(user);
        }
        aviso.setId(id);
        return avisosRepository.save(aviso);
    }

    // Exclusão de aviso (REST: DELETE /avisos/{id})
    @DeleteMapping
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarAvisos(@PathVariable(value = "id") int id){
        var aviso = avisosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Aviso não encontrado"));

        avisosRepository.delete(aviso);
    }
}
