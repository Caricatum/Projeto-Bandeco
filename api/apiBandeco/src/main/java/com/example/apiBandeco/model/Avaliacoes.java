package com.example.apiBandeco.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

@Entity
@Table(
        name = "avaliacoes",
        uniqueConstraints = {
                @UniqueConstraint(columnNames = {"id_user", "id_prato"})
        }
)
public class Avaliacoes {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @Column
    private String avaliacao;
    @NotNull(message = "A nota não pode ser nula")
    @Column
    private int nota;

    public Pratos getPrato() {
        return prato;
    }

    public void setPrato(Pratos prato) {
        this.prato = prato;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public int getNota() {
        return nota;
    }

    public void setNota(int nota) {
        this.nota = nota;
    }

    public String getAvaliacao() {
        return avaliacao;
    }

    public void setAvaliacao(String avaliacao) {
        this.avaliacao = avaliacao;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @ManyToOne
    @JoinColumn(name = "id_user", nullable = false)
    @NotNull(message = "Usuário deve ser informado")
    private User user;
    @ManyToOne
    @JoinColumn(name = "id_pratos", nullable = false)
    @NotNull(message = "Prato deve ser informado")
    private Pratos prato;
}
