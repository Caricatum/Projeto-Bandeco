package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.Avaliacoes;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface AvaliacoesRepository extends JpaRepository <Avaliacoes, Integer> {
    boolean existsByUserIdAndPratoId(int userId, int pratoId);

    List<Avaliacoes> findByPratoId(Integer pratoId);

    boolean existsByUserIdAndPratoIdAndIdNot(int userId, int pratoId, int id);
}
