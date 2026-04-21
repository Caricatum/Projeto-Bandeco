package com.example.apiBandeco.model;


import jakarta.persistence.*;
import jakarta.validation.constraints.NotNull;

@Entity
@Table(name = "cardapio")
public class Cardapio {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @ManyToOne
    @JoinColumn(name = "id_acompanhamento")
    private Pratos acompanhamento;
    @ManyToOne
    @JoinColumn(name = "id_prato_principal")
    private Pratos prato_principal;
    @ManyToOne
    @JoinColumn(name = "id_guarnicao")
    private Pratos guarnicao;
    @ManyToOne
    @JoinColumn(name = "id_salada")
    private Pratos salada;
    @ManyToOne
    @JoinColumn(name = "id_sobremesa")
    private Pratos sobremesa;
    @ManyToOne
    @JoinColumn(name = "id_refresco")
    private Pratos refresco;
    @NotNull(message = "informe se o prato é vegano")
    @Column
    private boolean vegano;

    public boolean isVegano() {
        return vegano;
    }

    public void setVegano(boolean vegano) {
        this.vegano = vegano;
    }

    public Pratos getRefresco() {
        return refresco;
    }

    public void setRefresco(Pratos refresco) {
        this.refresco = refresco;
    }

    public Pratos getSobremesa() {
        return sobremesa;
    }

    public void setSobremesa(Pratos sobremesa) {
        this.sobremesa = sobremesa;
    }

    public Pratos getSalada() {
        return salada;
    }

    public void setSalada(Pratos salada) {
        this.salada = salada;
    }

    public Pratos getGuarnicao() {
        return guarnicao;
    }

    public void setGuarnicao(Pratos guarnicao) {
        this.guarnicao = guarnicao;
    }

    public Pratos getPrato_principal() {
        return prato_principal;
    }

    public void setPrato_principal(Pratos prato_principal) {
        this.prato_principal = prato_principal;
    }

    public Pratos getAcompanhamento() {
        return acompanhamento;
    }

    public void setAcompanhamento(Pratos acompanhamento) {
        this.acompanhamento = acompanhamento;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }
}
