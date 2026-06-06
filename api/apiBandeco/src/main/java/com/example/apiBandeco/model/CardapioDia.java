package com.example.apiBandeco.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotNull;
import java.time.LocalDate;

@Entity
@Table(
        name = "cardapio_dia",
        uniqueConstraints = {@UniqueConstraint(columnNames = "data")}
)
public class CardapioDia {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @NotNull(message = "Informe a data")
    @Column
    private LocalDate data;
    @ManyToOne
    @JoinColumn(name = "id_padrao_almoco")
    private Cardapio padraoAlmoco;
    @ManyToOne
    @JoinColumn(name = "id_vegano_almoco")
    private Cardapio veganoAlmoco;
    @ManyToOne
    @JoinColumn(name = "id_padrao_jantar")
    private Cardapio padraoJantar;
    @ManyToOne
    @JoinColumn(name = "id_vegano_jantar")
    private Cardapio veganoJantar;
    @ManyToOne
    @NotNull(message = "Informe o Usuário")
    @JoinColumn(name = "id_user")
    private User user;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public LocalDate getData() {
        return data;
    }

    public void setData(LocalDate data) {
        this.data = data;
    }

    public Cardapio getPadraoAlmoco() {
        return padraoAlmoco;
    }

    public void setPadraoAlmoco(Cardapio padraoAlmoco) {
        this.padraoAlmoco = padraoAlmoco;
    }

    public Cardapio getVeganoAlmoco() {
        return veganoAlmoco;
    }

    public void setVeganoAlmoco(Cardapio veganoAlmoco) {
        this.veganoAlmoco = veganoAlmoco;
    }

    public Cardapio getPadraoJantar() {
        return padraoJantar;
    }

    public void setPadraoJantar(Cardapio padraoJantar) {
        this.padraoJantar = padraoJantar;
    }

    public Cardapio getVeganoJantar() {
        return veganoJantar;
    }

    public void setVeganoJantar(Cardapio veganoJantar) {
        this.veganoJantar = veganoJantar;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }


}
