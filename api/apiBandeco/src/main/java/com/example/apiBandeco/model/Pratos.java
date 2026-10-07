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
    private boolean leite;
    @Column
    private boolean ovo;
    @Column
    private boolean amendoim;
    @Column
    private boolean castanhas;
    @Column
    private boolean frutosDoMar;
    @Column
    private boolean peixes;
    @Column
    private boolean trigo;
    @Column
    private boolean soja;
    @Column
    private boolean gergilim;
    @Column
    private boolean altoAcucarAdicionado;
    @Column
    private boolean altoGorduraSaturada;
    @Column
    private boolean altoSodio;
    @Column
    private String imagem;
    @Column
    private String notaTecnica;
    @Column(columnDefinition = "TEXT")
    private String descricaoIA;
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

    public boolean isLeite() {
        return leite;
    }

    public void setLeite(boolean leite) {
        this.leite = leite;
    }

    public boolean isOvo() {
        return ovo;
    }

    public void setOvo(boolean ovo) {
        this.ovo = ovo;
    }

    public boolean isAmendoim() {
        return amendoim;
    }

    public void setAmendoim(boolean amendoim) {
        this.amendoim = amendoim;
    }

    public boolean isCastanhas() {
        return castanhas;
    }

    public void setCastanhas(boolean castanhas) {
        this.castanhas = castanhas;
    }

    public boolean isFrutosDoMar() {
        return frutosDoMar;
    }

    public void setFrutosDoMar(boolean frutosDoMar) {
        this.frutosDoMar = frutosDoMar;
    }

    public boolean isPeixes() {
        return peixes;
    }

    public void setPeixes(boolean peixes) {
        this.peixes = peixes;
    }

    public boolean isTrigo() {
        return trigo;
    }

    public void setTrigo(boolean trigo) {
        this.trigo = trigo;
    }

    public boolean isSoja() {
        return soja;
    }

    public void setSoja(boolean soja) {
        this.soja = soja;
    }

    public boolean isGergilim() {
        return gergilim;
    }

    public void setGergilim(boolean gergilim) {
        this.gergilim = gergilim;
    }

    public boolean isAltoAcucarAdicionado() {
        return altoAcucarAdicionado;
    }

    public void setAltoAcucarAdicionado(boolean altoAcucarAdicionado) {
        this.altoAcucarAdicionado = altoAcucarAdicionado;
    }

    public boolean isAltoGorduraSaturada() {
        return altoGorduraSaturada;
    }

    public void setAltoGorduraSaturada(boolean altoGorduraSaturada) {
        this.altoGorduraSaturada = altoGorduraSaturada;
    }

    public boolean isAltoSodio() {
        return altoSodio;
    }

    public void setAltoSodio(boolean altoSodio) {
        this.altoSodio = altoSodio;
    }


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

    public String getDescricaoIA() {
        return descricaoIA;
    }

    public void setDescricaoIA(String descricaoIA) {
        this.descricaoIA = descricaoIA;
    }


    @ManyToOne
    @JoinColumn(name = "id_categoria", nullable = false)
    @NotNull(message = "Categoria deve ser informada")
    private Categoria categoria;

}
