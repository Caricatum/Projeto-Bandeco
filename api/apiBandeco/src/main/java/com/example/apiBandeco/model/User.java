package com.example.apiBandeco.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.time.LocalDateTime;

@Entity
@Table(name="user")
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    @Email(message = "Email inválido")
    @NotBlank(message = "O Login não pode estar vazio")
    @Column(unique = true, nullable = false)
    private String login;
    @NotBlank(message = "O nome não pode estar vazio")
    @Column
    private String nome;
    @NotBlank(message = "A senha não pode estar vazia")
    @Column (name = "senha_hash")
    private String senhaHash;
    @Column
    private boolean funcionario;
    @Column
    boolean emailConfirmado;
    @Column
    private String codigoConfirmacao;
    @Column
    private LocalDateTime expiracaoConfirmacao;
    @Column
    private String codigoResetSenha;
    @Column
    private LocalDateTime expiracaoResetSenha;

    public String getCodigoConfirmacao() {
        return codigoConfirmacao;
    }

    public void setCodigoConfirmacao(String codigoConfirmacao) {
        this.codigoConfirmacao = codigoConfirmacao;
    }

    public LocalDateTime getExpiracaoConfirmacao() {
        return expiracaoConfirmacao;
    }

    public void setExpiracaoConfirmacao(LocalDateTime expiracaoConfirmacao) {
        this.expiracaoConfirmacao = expiracaoConfirmacao;
    }

    public String getCodigoResetSenha() {
        return codigoResetSenha;
    }

    public void setCodigoResetSenha(String codigoResetSenha) {
        this.codigoResetSenha = codigoResetSenha;
    }

    public LocalDateTime getExpiracaoResetSenha() {
        return expiracaoResetSenha;
    }

    public void setExpiracaoResetSenha(LocalDateTime expiracaoResetSenha) {
        this.expiracaoResetSenha = expiracaoResetSenha;
    }

    public boolean isEmailConfirmado() {
        return emailConfirmado;
    }

    public void setEmailConfirmado(boolean emailConfirmado) {
        this.emailConfirmado = emailConfirmado;
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
