package com.example.apiBandeco;

import java.io.File;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.security.servlet.SecurityAutoConfiguration;
import org.springframework.context.annotation.Bean;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.security.crypto.bcrypt.BCrypt;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;

//AO COMMITAR, REMOVA A CHAVE!!!
//FICA EM GEMINI CONFIG

@EnableAsync
@SpringBootApplication(exclude = {SecurityAutoConfiguration.class})
public class ApiBandecoApplication {

	public static void main(String[] args) {
		String tmp = System.getProperty("java.io.tmpdir");
		if (tmp == null || tmp.toLowerCase().startsWith("c:\\windows")) {
			String userTemp = System.getenv("LOCALAPPDATA");
			File tempFolder;
			if (userTemp != null && !userTemp.isBlank()) {
				tempFolder = new File(userTemp, "Temp");
			} else {
				tempFolder = new File(System.getProperty("user.home"), "AppData/Local/Temp");
			}
			if (!tempFolder.exists()) {
				tempFolder.mkdirs();
			}
			System.setProperty("java.io.tmpdir", tempFolder.getAbsolutePath());
		}
		SpringApplication.run(ApiBandecoApplication.class, args);
	}

	@Bean
	public PasswordEncoder getPasswordEnconder(){
		BCryptPasswordEncoder encoder = new BCryptPasswordEncoder();
		return encoder;
	}
}
