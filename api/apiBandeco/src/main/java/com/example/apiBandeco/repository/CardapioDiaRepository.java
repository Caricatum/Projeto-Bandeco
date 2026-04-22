package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.CardapioDia;
import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDate;

public interface CardapioDiaRepository extends JpaRepository<CardapioDia, Integer> {
    boolean existsByData(LocalDate data);
    boolean existsByDataAndIdNot(LocalDate data, int id);
}
