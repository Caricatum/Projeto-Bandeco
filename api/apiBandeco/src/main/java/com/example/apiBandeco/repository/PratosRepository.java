package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.Pratos;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface PratosRepository extends JpaRepository <Pratos, Integer> {
    List<Pratos> findByNomeContainingIgnoreCase(String nome);

}
