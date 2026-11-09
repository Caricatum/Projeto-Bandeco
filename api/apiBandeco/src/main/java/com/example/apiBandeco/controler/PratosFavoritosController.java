package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.PratosFavoritos;
import com.example.apiBandeco.repository.PratosFavoritosRepository;
import com.example.apiBandeco.repository.PratosRepository;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;
import java.util.Map;

@CrossOrigin
@RestController
@RequestMapping({"/pratosFavoritos", "/pratos-favoritos", "/api/v1/pratos-favoritos"})
public class PratosFavoritosController {
    @Autowired
    PratosFavoritosRepository pratosFavoritosRepository;
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    UserRepository userRepository;
    @Autowired
    NotificacoesService notificacoesService;

    // Busca prato favorito por ID (REST: GET /pratos-favoritos/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public PratosFavoritos buscarPorId(@PathVariable("id") int id){
        return pratosFavoritosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato favorito não encontrado"
                ));
    }

    // Listagem com suporte a filtro por usuário (REST: GET /pratos-favoritos?userId=...)
    @GetMapping
    public List<PratosFavoritos> buscarTodosPratosFavoritos(@RequestParam(required = false) Integer userId){
        if (userId != null) {
            return pratosFavoritosRepository.findByUserId(userId);
        }
        return pratosFavoritosRepository.findAll();
    }

    // Cadastro de favorito (REST: POST /pratos-favoritos)
    @PostMapping({"", "/cadastrar"})
    @ResponseStatus(HttpStatus.CREATED)
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
        }
        if (userId != null){
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

    // Exclusão de favorito (REST: DELETE /pratos-favoritos/{id})
    @DeleteMapping({"/{id}", "/deletar/{id}"})
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarPratoFavorito(@PathVariable(value = "id") int id){
        pratosFavoritosRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato favorito não encontrado"));
        pratosFavoritosRepository.deleteById(id);
    }

    // Disparo de notificações de favoritos (REST: POST /pratos-favoritos/notificacoes)
    @PostMapping({"/notificar", "/notificacoes"})
    public ResponseEntity<Map<String, String>> enviarNotificacoes() {
        notificacoesService.enviarNotificacoesPratosFavoritos();
        return ResponseEntity.ok(Map.of("mensagem", "Notificações enviadas com sucesso"));
    }
}
