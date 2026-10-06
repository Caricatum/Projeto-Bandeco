package com.example.apiBandeco.service;

import com.example.apiBandeco.model.Cardapio;
import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.model.ValorNutricional;
import jakarta.servlet.http.HttpServletResponse;
import org.openpdf.text.*;
import org.openpdf.text.pdf.PdfPTable;
import org.openpdf.text.pdf.PdfWriter;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;

@Service
public class PdfGeneratorService {
    public void export (
            HttpServletResponse response,
            Cardapio cardapioNormal,
            Cardapio cardapioVegano,
            LocalDate data
    ) throws IOException {
        Document document = new Document(PageSize.A4);
        PdfWriter.getInstance(document, response.getOutputStream());

        document.open();
        Font fontTitle = FontFactory.getFont(FontFactory.HELVETICA_BOLD);
        fontTitle.setSize(18);

        Paragraph titulo = new Paragraph(
                "Cardápio - Restaurante Universitário",
                fontTitle
        );

        titulo.setAlignment(Paragraph.ALIGN_CENTER);

        document.add(titulo);

        Font fontData = FontFactory.getFont(FontFactory.HELVETICA);
        fontData.setSize(12);

        DateTimeFormatter formatter =
                DateTimeFormatter.ofPattern("dd/MM/yyyy");

        Paragraph dataParagrafo = new Paragraph(
                "Data: " + data.format(formatter),
                fontData
        );

        dataParagrafo.setAlignment(Paragraph.ALIGN_CENTER);

        document.add(dataParagrafo);

        Font fontSubtitulo =
                FontFactory.getFont(FontFactory.HELVETICA_BOLD);

        fontSubtitulo.setSize(15);

        Paragraph normal =
                new Paragraph("Cardápio Normal", fontSubtitulo);
        normal.setSpacingAfter(10);

        document.add(normal);

        PdfPTable tabelaNormal =
                criarTabela(cardapioNormal);

        document.add(tabelaNormal);

        Paragraph vegano =
                new Paragraph("Cardápio Vegano", fontSubtitulo);
        vegano.setSpacingAfter(10);

        document.add(vegano);

        PdfPTable tabelaVegano =
                criarTabela(cardapioVegano);

        document.add(tabelaVegano);


        document.close();
    }

    private PdfPTable criarTabela(
            Cardapio cardapio
    ) {
        PdfPTable tabela = new PdfPTable(7);

        tabela.addCell("Categoria");
        tabela.addCell("Prato");
        tabela.addCell("Medida");
        tabela.addCell("Kcal");
        tabela.addCell("Carboidratos");
        tabela.addCell("Proteínas");
        tabela.addCell("Lipídios");

        adicionarPrato(
                tabela,
                "Acompanhamento",
                cardapio.getAcompanhamento()
        );

        adicionarPrato(
                tabela,
                "Prato principal",
                cardapio.getPrato_principal()
        );

        adicionarPrato(
                tabela,
                "Guarnição",
                cardapio.getGuarnicao()
        );

        adicionarPrato(
                tabela,
                "Salada",
                cardapio.getSalada()
        );

        adicionarPrato(
                tabela,
                "Sobremesa",
                cardapio.getSobremesa()
        );

        adicionarPrato(
                tabela,
                "Refresco",
                cardapio.getRefresco()
        );

        return tabela;
    }

    private void adicionarPrato(
            PdfPTable tabela,
            String categoria,
            Pratos prato
    ) {
        if (prato == null) {
            return;
        }

        ValorNutricional valor = prato.getValorNutricional();

        tabela.addCell(categoria);
        tabela.addCell(prato.getNome());

        if (valor != null) {
            tabela.addCell(valor.getMedida());
            tabela.addCell(String.valueOf(valor.getKcal()));
            tabela.addCell(String.valueOf(valor.getCarboidratos()));
            tabela.addCell(String.valueOf(valor.getProteinas()));
            tabela.addCell(String.valueOf(valor.getLipidios()));
        } else {
            tabela.addCell("-");
            tabela.addCell("-");
            tabela.addCell("-");
            tabela.addCell("-");
            tabela.addCell("-");
        }
    }
}
