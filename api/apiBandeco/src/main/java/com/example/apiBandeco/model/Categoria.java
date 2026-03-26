package com.example.apiBandeco.model;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;

@Entity
@Table(name="categoria")
public class Categoria {

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getDescricao() {
        return descricao;
    }

    public void setDescricao(String descricao) {
        this.descricao = descricao;
    }

    @Id
    private int id;
    @NotBlank(message = "A descrição não pode estar vazia")
    @Size(min = 3, max = 100, message = "A descrição deve ter entre 3 e 45 caracteres")
    @Column
    private String descricao;
}
