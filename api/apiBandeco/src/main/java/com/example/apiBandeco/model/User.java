package com.example.apiBandeco.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.time.LocalDateTime;

@Entity
@Table(name="user")
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @NotBlank(message = "O Login não pode estar vazio")
    @Column
    private String login;
    @NotBlank(message = "O nome não pode estar vazio")
    @Column
    private String nome;
    @NotBlank(message = "A senha não pode estar vazia")
    @Column (name = "senha_hash")
    private String senhaHash;
    @NotNull(message = "Funcionário não pode ser nullo")
    @Column
    private boolean funcionario;
    @Column
    private String codigo;
    @Column
    private LocalDateTime expiracaoCodigo;

    public LocalDateTime getExpiracaoCodigo() {
        return expiracaoCodigo;
    }

    public void setExpiracaoCodigo(LocalDateTime expiracaoCodigo) {
        this.expiracaoCodigo = expiracaoCodigo;
    }

    public String getCodigo() {
        return codigo;
    }

    public void setCodigo(String codigo) {
        this.codigo = codigo;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getNome() {
        return nome;
    }

    public void setNome(String nome) {
        this.nome = nome;
    }

    public String getSenhaHash() {
        return senhaHash;
    }

    public void setSenhaHash(String senhaHash) {
        this.senhaHash = senhaHash;
    }

    public boolean isFuncionario() {
        return funcionario;
    }

    public void setFuncionario(boolean funcionario) {
        this.funcionario = funcionario;
    }
}
