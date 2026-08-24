package com.example.apiBandeco.controler;

import com.example.apiBandeco.email.EmailService;
import com.example.apiBandeco.model.*;
import com.example.apiBandeco.repository.CardapioDiaRepository;
import com.example.apiBandeco.repository.CardapioRepository;
import com.example.apiBandeco.repository.PratosFavoritosRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

import java.time.LocalDate;
import java.util.*;

@Service
public class NotificacoesService {
    @Autowired
    private CardapioRepository cardapioRepository;

    @Autowired
    private PratosFavoritosRepository favoritosRepository;

    @Autowired
    private EmailService emailService;

    @Autowired
    private CardapioDiaRepository cardapioDiaRepository;

    public void enviarNotificacoesPratosFavoritos() {

        LocalDate hoje = LocalDate.now();

        CardapioDia cardapioDia = cardapioDiaRepository.findByData(hoje)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Cardápio de hoje não encontrado"
                ));

        List<Pratos> pratosAlmoco = new ArrayList<>();
        List<Pratos> pratosJantar = new ArrayList<>();

        // ALMOÇO
        pratosAlmoco = listarCardapio(
                cardapioDia.getVeganoAlmoco(),
                pratosAlmoco
        );

        pratosAlmoco = listarCardapio(
                cardapioDia.getPadraoAlmoco(),
                pratosAlmoco
        );

        // JANTAR
        pratosJantar = listarCardapio(
                cardapioDia.getPadraoJantar(),
                pratosJantar
        );

        pratosJantar = listarCardapio(
                cardapioDia.getVeganoJantar(),
                pratosJantar
        );

        // TODOS OS FAVORITOS
        List<PratosFavoritos> favoritos = favoritosRepository.findAll();

        Map<User, List<Pratos>> favoritosAlmoco = new HashMap<>();
        Map<User, List<Pratos>> favoritosJantar = new HashMap<>();

        for (PratosFavoritos favorito : favoritos) {

            User usuario = favorito.getUser();
            Pratos prato = favorito.getPrato();

            if (pratoEstaNoCardapio(pratosAlmoco, prato)) {

                favoritosAlmoco
                        .computeIfAbsent(usuario, k -> new ArrayList<>())
                        .add(prato);
            }

            if (pratoEstaNoCardapio(pratosJantar, prato)) {

                favoritosJantar
                        .computeIfAbsent(usuario, k -> new ArrayList<>())
                        .add(prato);
            }
        }


        for (Map.Entry<User, List<Pratos>> entrada : favoritosAlmoco.entrySet()) {

            emailService.enviarNotificacaoPratosFavoritos(
                    entrada.getKey(),
                    entrada.getValue(),
                    "almoço"
            );
        }


        for (Map.Entry<User, List<Pratos>> entrada : favoritosJantar.entrySet()) {

            emailService.enviarNotificacaoPratosFavoritos(
                    entrada.getKey(),
                    entrada.getValue(),
                    "jantar"
            );
        }
    }

    private List<Pratos> listarCardapio(Cardapio cardapio, List<Pratos> pratos){

        if (cardapio == null) {
            return pratos;
        }

        adicionarPrato(pratos, cardapio.getAcompanhamento());
        adicionarPrato(pratos, cardapio.getPrato_principal());
        adicionarPrato(pratos, cardapio.getGuarnicao());
        adicionarPrato(pratos, cardapio.getSalada());
        adicionarPrato(pratos, cardapio.getSobremesa());
        adicionarPrato(pratos, cardapio.getRefresco());

        return pratos;
    }
    private void adicionarPrato(List<Pratos> pratos, Pratos prato) {

        if (prato == null) {
            return;
        }

        boolean jaExiste = pratos.stream()
                .anyMatch(p -> Objects.equals(p.getId(), prato.getId()));

        if (!jaExiste) {
            pratos.add(prato);
        }
    }

    private boolean pratoEstaNoCardapio(
            List<Pratos> cardapio,
            Pratos prato) {

        return cardapio.stream()
                .anyMatch(p -> Objects.equals(p.getId(), prato.getId()));
    }
}
