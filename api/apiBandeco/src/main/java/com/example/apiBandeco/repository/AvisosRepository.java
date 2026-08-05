package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.Avisos;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AvisosRepository extends JpaRepository <Avisos, Integer> {
}
