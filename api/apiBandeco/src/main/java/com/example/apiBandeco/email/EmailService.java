package com.example.apiBandeco.email;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.stereotype.Service;

@Service
public class EmailService {

    @Autowired
    private JavaMailSender mailSender;

    public void enviarCodigo(String destino, String codigo){

        SimpleMailMessage mensagem = new SimpleMailMessage();

        mensagem.setTo(destino);
        mensagem.setSubject("Código por email");
        mensagem.setText("Seu código é: " + codigo);

        mailSender.send(mensagem);
    }
}
