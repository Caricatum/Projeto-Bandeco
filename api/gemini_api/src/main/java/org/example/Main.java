package org.example;

import com.google.genai.Client;
import com.google.genai.types.GenerateContentResponse;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class Main {
    public static void main(String[] args) {
        Client client = new Client();
        GenerateContentResponse response = client.models.generateContent("gemini-3.6-flash", "Hello, Gemini!", null);
        System.out.println(response.text());
    }
}