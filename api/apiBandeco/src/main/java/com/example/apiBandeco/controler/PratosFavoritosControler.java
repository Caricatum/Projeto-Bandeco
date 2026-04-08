package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Pratos;
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
import java.util.Optional;

@CrossOrigin
@RestController
@RequestMapping("/pratosFavoritos")
public class PratosFavoritosControler {
    @Autowired
    PratosFavoritosRepository pratosFavoritosRepository;
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    UserRepository userRepository;

    @GetMapping("/id/{id}")//busca prato favorito pelo id
    public Optional<PratosFavoritos> buscarPorId(@PathVariable("id") int id){
        return pratosFavoritosRepository.findById(id);
    }

    @GetMapping("/all")//busca todos os pratos favoritos
    public List<PratosFavoritos> buscarTodosPratosFavoritos(){return pratosFavoritosRepository.findAll();}

    @PostMapping("/cadastrarPratosFavoritos")//cadastra um prato favorito
    public PratosFavoritos cadastroPratosFavoritos (@RequestBody @Valid PratosFavoritos pratoFavoritos){
        int pratoId = pratoFavoritos.getPrato().getId();
        int userId = pratoFavoritos.getUser().getId();

        var prato = pratosRepository.findById(pratoId)
                .orElseThrow(() -> new ResponseStatusException(
                HttpStatus.NOT_FOUND, "Prato não existe"));

        var user = userRepository.findById(userId)
                .orElseThrow(() -> new ResponseStatusException(
                HttpStatus.NOT_FOUND, "User não existe"));

        pratoFavoritos.setPrato(prato);
        pratoFavoritos.setUser(user);

        boolean jaExiste = pratosFavoritosRepository
                .existsByUserIdAndPratoId(userId, pratoId);

        if (jaExiste) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST, "Prato já favoritado");
        }

        return pratosFavoritosRepository.save(pratoFavoritos);
    }

    @DeleteMapping("/deletarPratosFavoritos/{id}") //deleta prato favorito pelo id
    public void deletarPratoFavorito(@PathVariable(value = "id") int id){
        pratosFavoritosRepository.deleteById(id);
    }

}
