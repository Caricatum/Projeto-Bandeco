package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.PratosFavoritos;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PratosFavoritosRepository extends JpaRepository <PratosFavoritos, Integer> {
    boolean existsByUserIdAndPratoId(int userId, int pratoId);
}
