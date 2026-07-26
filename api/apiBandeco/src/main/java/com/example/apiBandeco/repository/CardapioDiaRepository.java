package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.CardapioDia;
import com.example.apiBandeco.model.User;
import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

public interface CardapioDiaRepository extends JpaRepository<CardapioDia, Integer> {
    boolean existsByData(LocalDate data);
    boolean existsByDataAndIdNot(LocalDate data, int id);
    public Optional<CardapioDia> findByData(LocalDate data);
    List<CardapioDia> findByDataBetween(LocalDate inicio, LocalDate fim);

}
