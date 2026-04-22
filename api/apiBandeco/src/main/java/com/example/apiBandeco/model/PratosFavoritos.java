package com.example.apiBandeco.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotNull;

@Entity
@Table(
        name = "pratos_favoritados",
        uniqueConstraints = {
        @UniqueConstraint(columnNames = {"id_user", "id_prato"})
    }
)
public class PratosFavoritos {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @ManyToOne
    @JoinColumn(name = "id_user", nullable = false)
    @NotNull(message = "Usuário deve ser informado")
    private User user;
    @ManyToOne
    @JoinColumn(name = "id_pratos", nullable = false)
    @NotNull(message = "Prato deve ser informado")
    private Pratos prato;

    private int id;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public Pratos getPrato() {
        return prato;
    }

    public void setPrato(Pratos prato) {
        this.prato = prato;
    }


}
