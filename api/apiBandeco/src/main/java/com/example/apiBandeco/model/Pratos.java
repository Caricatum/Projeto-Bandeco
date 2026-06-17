package com.example.apiBandeco.model;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.util.List;

@Entity
@Table(name="pratos")
public class Pratos {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @NotBlank(message = "O nome não pode estar vazio")
    @Column
    private String nome;
    @NotBlank(message = "A descricao não pode estar vazia")
    @Column
    private String descricao;
    @NotNull(message = "Vegano deve ser informado")
    @Column
    private boolean vegano;
    @Column
    private String imagem;
    @Column
    private String notaTecnica;
    @JsonIgnore
    @OneToMany(mappedBy = "prato",
            cascade = CascadeType.ALL,
            orphanRemoval = true)
    private List<Avaliacoes> avaliacoes;
    @JsonIgnore
    @OneToMany(mappedBy = "prato",
            cascade = CascadeType.ALL,
            orphanRemoval = true)
    private List<PratosFavoritos> favoritos;
    @JsonIgnore
    @OneToOne(mappedBy = "prato",
            cascade = CascadeType.ALL,
            orphanRemoval = true)
    private ValorNutricional valorNutricional;

    public ValorNutricional getValorNutricional() {
        return valorNutricional;
    }

    public void setValorNutricional(ValorNutricional valorNutricional) {
        this.valorNutricional = valorNutricional;
    }

    public List<Avaliacoes> getAvaliacoes() {
        return avaliacoes;
    }

    public void setAvaliacoes(List<Avaliacoes> avaliacoes) {
        this.avaliacoes = avaliacoes;
    }

    public List<PratosFavoritos> getFavoritos() {
        return favoritos;
    }

    public void setFavoritos(List<PratosFavoritos> favoritos) {
        this.favoritos = favoritos;
    }

    public String getNotaTecnica() {
        return notaTecnica;
    }

    public void setNotaTecnica(String notaTecnica) {
        this.notaTecnica = notaTecnica;
    }

    public Categoria getCategoria() {
        return categoria;
    }

    public void setCategoria(Categoria categoria) {
        this.categoria = categoria;
    }

    public String getImagem() {
        return imagem;
    }

    public void setImagem(String imagem) {
        this.imagem = imagem;
    }

    public boolean isVegano() {
        return vegano;
    }

    public void setVegano(boolean vegano) {
        this.vegano = vegano;
    }

    public String getDescricao() {
        return descricao;
    }

    public void setDescricao(String descricao) {
        this.descricao = descricao;
    }

    public String getNome() {
        return nome;
    }

    public void setNome(String nome) {
        this.nome = nome;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @ManyToOne
    @JoinColumn(name = "id_categoria", nullable = false)
    @NotNull(message = "Categoria deve ser informada")
    private Categoria categoria;

}
