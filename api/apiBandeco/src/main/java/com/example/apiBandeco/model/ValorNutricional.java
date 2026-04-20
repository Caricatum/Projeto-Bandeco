package com.example.apiBandeco.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

@Entity
@Table(name="valor_nutricional")
public class ValorNutricional {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @NotNull(message = "Preencha o campo kcal")
    @Column
    private float kcal;
    @NotNull(message = "Preencha o campo carboidratos")
    @Column
    private float carboidratos;
    @NotNull(message = "Preencha o campo proteinas")
    @Column
    private float proteinas;
    @NotNull(message = "Preencha o campo lipidios")
    @Column
    private float lipidios;
    @NotBlank(message = "Preencha o campo medida")
    @Column
    private String medida;

    public Pratos getPrato() {
        return prato;
    }

    public void setPrato(Pratos prato) {
        this.prato = prato;
    }

    public String getMedida() {
        return medida;
    }

    public void setMedida(String medida) {
        this.medida = medida;
    }

    public float getLipidios() {
        return lipidios;
    }

    public void setLipidios(float lipidios) {
        this.lipidios = lipidios;
    }

    public float getProteinas() {
        return proteinas;
    }

    public void setProteinas(float proteinas) {
        this.proteinas = proteinas;
    }

    public float getCarboidratos() {
        return carboidratos;
    }

    public void setCarboidratos(float carboidratos) {
        this.carboidratos = carboidratos;
    }

    public float getKcal() {
        return kcal;
    }

    public void setKcal(float kcal) {
        this.kcal = kcal;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @OneToOne
    @JoinColumn(name = "id_prato", nullable = false)
    @NotNull(message = "Prato deve ser informado")
    private Pratos prato;
}
