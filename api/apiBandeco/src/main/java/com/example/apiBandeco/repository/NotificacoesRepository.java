package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.Notificacoes;
import org.springframework.data.jpa.repository.JpaRepository;

public interface NotificacoesRepository extends JpaRepository <Notificacoes, Integer> {
}
