package com.example.apiBandeco.repository;

import com.example.apiBandeco.model.ValorNutricional;
import org.aspectj.apache.bcel.util.Repository;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ValorNutricionalRepository extends JpaRepository <ValorNutricional, Integer> {
}
