package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.Avaliacoes;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AvaliacoesRepository extends JpaRepository <Avaliacoes, Integer> {
    boolean existsByUserIdAndPratoId(int userId, int pratoId);

    boolean existsByUserIdAndPratoIdAndIdNot(int userId, int pratoId, int id);
}
