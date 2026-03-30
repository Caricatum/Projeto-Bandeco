package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.Pratos;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PratosRepository extends JpaRepository <Pratos, Integer> {
}
