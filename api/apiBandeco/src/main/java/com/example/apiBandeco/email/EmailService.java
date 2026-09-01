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
            List<Pratos> pratosAlmoco,
            List<Pratos> pratosJantar) {

        try {
            MimeMessage message = mailSender.createMimeMessage();

            MimeMessageHelper helper =
                    new MimeMessageHelper(message, true, "UTF-8");

            helper.setSubject(
                    "Seus pratos favoritos estão no cardápio de hoje!"
            );

            helper.setTo(usuario.getLogin());

            String template = carregaPratosFavoritos();

            StringBuilder almoco = new StringBuilder();
            StringBuilder jantar = new StringBuilder();

            if (!pratosAlmoco.isEmpty()) {

                almoco.append("""
        <table width="100%" cellpadding="0" cellspacing="0" border="0"
               style="background:#f5f5f5; border-radius:10px; margin-bottom:15px;">
            <tr>
                <td style="
                    padding:18px 20px;
                    font-size:16px;
                    line-height:1.8;
                    color:#2d2d2d;
                ">
                    <strong>🍛 Almoço</strong>
                    <br><br>
    """);

                for (Pratos prato : pratosAlmoco) {
                    almoco.append("• ")
                            .append(prato.getNome())
                            .append("<br>");
                }

                almoco.append("""
                </td>
            </tr>
        </table>
    """);
            }

            if (!pratosJantar.isEmpty()) {

                jantar.append("""
        <table width="100%" cellpadding="0" cellspacing="0" border="0"
               style="background:#f5f5f5; border-radius:10px; margin-bottom:15px;">
            <tr>
                <td style="
                    padding:18px 20px;
                    font-size:16px;
                    line-height:1.8;
                    color:#2d2d2d;
                ">
                    <strong>🌙 Jantar</strong>
                    <br><br>
    """);

                for (Pratos prato : pratosJantar) {
                    jantar.append("• ")
                            .append(prato.getNome())
                            .append("<br>");
                }

                jantar.append("""
                </td>
            </tr>
        </table>
    """);
            }

            template = template.replace(
                    "#{nome}",
                    usuario.getNome()
            );

            template = template.replace(
                    "#{almoco}",
                    almoco.toString()
            );

            template = template.replace(
                    "#{jantar}",
                    jantar.toString()
            );

            helper.setText(template, true);

            mailSender.send(message);

        } catch (Exception exception) {

            System.out.println("Falha ao enviar o email");
            exception.printStackTrace();
        }
    }
}