package com.example.apiBandeco.controler;


import com.example.apiBandeco.model.PratosFavoritos;
import com.example.apiBandeco.repository.PratosFavoritosRepository;
import com.example.apiBandeco.repository.PratosRepository;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/pratosFavoritos")
public class PratosFavoritosController {
    @Autowired
    PratosFavoritosRepository pratosFavoritosRepository;
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    UserRepository userRepository;

    @GetMapping("/id/{id}")//busca prato favorito pelo id
    public PratosFavoritos buscarPorId(@PathVariable("id") int id){
        return pratosFavoritosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato favorito não encontrado"
                ));
    }

    @GetMapping("/all")//busca todos os pratos favoritos
    public List<PratosFavoritos> buscarTodosPratosFavoritos(){return pratosFavoritosRepository.findAll();}

    @PostMapping("/cadastrar")//cadastra um prato favorito
    public PratosFavoritos cadastroPratosFavoritos (@RequestBody @Valid PratosFavoritos pratoFavoritos){
        Integer pratoId = pratoFavoritos.getPrato() != null
                ? pratoFavoritos.getPrato().getId() : null;
        Integer userId = pratoFavoritos.getUser() != null
                ? pratoFavoritos.getUser().getId() : null;

        if (pratoId != null){
            var prato = pratosRepository.findById(pratoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Prato não encontrado"));
            pratoFavoritos.setPrato(prato);
        }if (userId != null){
            var user = userRepository.findById(userId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "User não encontrado"));
            pratoFavoritos.setUser(user);
        }

        boolean jaExiste = pratosFavoritosRepository
                .existsByUserIdAndPratoId(userId, pratoId);

        if (jaExiste) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST, "Prato já favoritado");
        }

        return pratosFavoritosRepository.save(pratoFavoritos);
    }

    @DeleteMapping("/deletar/{id}") //deleta prato favorito pelo id
    public void deletarPratoFavorito(@PathVariable(value = "id") int id){
        pratosFavoritosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato favorito não encontrado"));
        pratosFavoritosRepository.deleteById(id);
    }

}
