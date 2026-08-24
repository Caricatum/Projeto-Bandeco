package com.example.apiBandeco.email;

import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.model.User;
import jakarta.mail.internet.MimeMessage;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ClassPathResource;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.List;

@Service
public class EmailService {

    @Autowired
    private JavaMailSender mailSender;

    public void enviarCodigo(User user, String codigo) {

        try {
            MimeMessage message = mailSender.createMimeMessage();
            MimeMessageHelper helper = new MimeMessageHelper(message, true, "UTF-8");
            helper.setSubject("Código de verificação");
            helper.setTo(user.getLogin());

            String template  = carregaTemplateEmail();

            template = template.replace("#{nome}", user.getNome());
            template = template.replace("#{codigo}", codigo);
            helper.setText(template, true);
            mailSender.send(message);
        } catch (Exception exception) {
            System.out.println("Falha ao enviar o email");
        }
    }

    public String carregaTemplateEmail() throws IOException {
        ClassPathResource resource = new ClassPathResource("template-email.html");
        return new String(resource.getInputStream().readAllBytes(), StandardCharsets.UTF_8);
    }

    public String carregaPratosFavoritos() throws IOException {
        ClassPathResource resource = new ClassPathResource("notificacao-pratos.html");
        return new String(resource.getInputStream().readAllBytes(), StandardCharsets.UTF_8);
    }

    public void enviarNotificacaoPratosFavoritos(
            User usuario,
            List<Pratos> pratos,
            String horario) {

        try {
            MimeMessage message = mailSender.createMimeMessage();

            MimeMessageHelper helper =
                    new MimeMessageHelper(message, true, "UTF-8");

            helper.setSubject("Seus pratos favoritos estão no cardápio de hoje!");
            helper.setTo(usuario.getLogin());

            String template = carregaPratosFavoritos();

            StringBuilder listaPratos = new StringBuilder();

            for (Pratos prato : pratos) {
                listaPratos
                        .append("• ")
                        .append(prato.getNome())
                        .append("<br>");
            }

            template = template.replace("#{nome}", usuario.getNome());
            template = template.replace("#{horario}", horario);
            template = template.replace("#{pratos}", listaPratos.toString());

            helper.setText(template, true);

            mailSender.send(message);

        } catch (Exception exception) {
            System.out.println("Falha ao enviar o email");
            exception.printStackTrace();
        }
    }
}