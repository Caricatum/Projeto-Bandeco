package com.example.apiBandeco.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.time.LocalDate;

@Entity
@Table(name = "notificacoes")
public class Notificacoes {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @NotBlank (message = "Titulo não pode ser vazio")
    @Column
    private String titulo;
    @Column
    private String descricao;
    @NotBlank(message = "model não pode ser vazio")
    @Column
    private String model;
    @NotNull(message = "data inicial não pode estar vazia")
    @Column
    private LocalDate data_inicial;
    @NotNull(message = "data inicial não pode estar vazia")
    @Column
    private LocalDate data_final;
    @ManyToOne
    @JoinColumn(name = "id_user", nullable = false)
    @NotNull(message = "Usuário deve ser informado")
    private User user;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getTitulo() {
        return titulo;
    }

    public void setTitulo(String titulo) {
        this.titulo = titulo;
    }

    public String getDescricao() {
        return descricao;
    }

    public void setDescricao(String descricao) {
        this.descricao = descricao;
    }

    public String getModel() {
        return model;
    }

    public void setModel(String model) {
        this.model = model;
    }

    public LocalDate getData_inicial() {
        return data_inicial;
    }

    public void setData_inicial(LocalDate data_inicial) {
        this.data_inicial = data_inicial;
    }

    public LocalDate getData_final() {
        return data_final;
    }

    public void setData_final(LocalDate data_final) {
        this.data_final = data_final;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

}
