package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Avisos;
import com.example.apiBandeco.model.Notificacoes;
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
@RequestMapping("/avisos")
public class AvisosController {
    @Autowired
    UserRepository userRepository;
    @Autowired
    AvisosRepository avisosRepository;

    @GetMapping("/id/{id}")//busca avisos pelo id
    public Avisos buscarPorId(@PathVariable("id") int id){
        return avisosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Aviso não encontrado"
                ));
    }

    @GetMapping("/all")//busca todos os avisos
    public List<Avisos> buscarTodosAvisos(){return avisosRepository.findAll();}

    @PostMapping("/cadastrar")//cadastra um aviso
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

    @PutMapping("/atualizar") //Atualiza Avisos
    public Avisos atualizaAvisos (@RequestBody @Valid Avisos aviso){

        avisosRepository.findById(aviso.getId())
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

        return avisosRepository.save(aviso);
    }

    @DeleteMapping("/deletar/{id}") //deleta avisos pelo id
    public void deletarAvisos(@PathVariable(value = "id") int id){
        var aviso = avisosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Aviso não encontrado"));

        avisosRepository.delete(aviso);
    }
}
