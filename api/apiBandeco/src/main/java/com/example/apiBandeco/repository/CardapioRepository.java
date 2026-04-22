package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.Cardapio;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CardapioRepository extends JpaRepository<Cardapio, Integer> {
}
