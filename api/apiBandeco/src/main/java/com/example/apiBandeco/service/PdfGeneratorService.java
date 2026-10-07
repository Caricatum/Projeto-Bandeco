package com.example.apiBandeco.service;

import com.example.apiBandeco.model.Cardapio;
import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.model.ValorNutricional;
import jakarta.servlet.http.HttpServletResponse;
import org.openpdf.text.*;
import org.openpdf.text.pdf.PdfPCell;
import org.openpdf.text.pdf.PdfPTable;
import org.openpdf.text.pdf.PdfWriter;
import org.springframework.stereotype.Service;

import java.awt.Color;
import java.io.IOException;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.time.format.TextStyle;
import java.util.Locale;

@Service
public class PdfGeneratorService {

    // ===== PALETA DE CORES QUENTES DO BANDECO =====
    private static final Color COLOR_PRIMARY = new Color(217, 34, 67);      // #D92243 Vermelho Bandeco
    private static final Color COLOR_PRIMARY_DARK = new Color(122, 23, 40); // #7A1728 Bordô / Vinho
    private static final Color COLOR_GOLD = new Color(224, 195, 117);       // #E0C375 Dourado
    private static final Color COLOR_CREAM = new Color(255, 245, 229);      // #FFF5E5 Creme do Bandeco
    private static final Color COLOR_CREAM_ALT = new Color(255, 250, 242);  // #FFFAF2 Creme sutil para zebrado

    // ===== CORES DO CARDÁPIO VEGANO =====
    private static final Color COLOR_VEGAN = new Color(25, 135, 84);        // #198754 Verde vegano
    private static final Color COLOR_VEGAN_LIGHT = new Color(244, 255, 247);// #F4FFF7 Fundo suave vegano

    // ===== CORES DE TEXTO E BORDAS =====
    private static final Color COLOR_TEXT_DARK = new Color(44, 62, 80);     // #2C3E50 Texto escuro
    private static final Color COLOR_TEXT_MUTED = new Color(115, 115, 115); // Texto cinza secundário
    private static final Color COLOR_BORDER = new Color(230, 230, 230);     // Divisórias sutis

    // ===== TIPOGRAFIA =====
    private final Font fontBannerTitle = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 17, Color.WHITE);
    private final Font fontBannerSub = FontFactory.getFont(FontFactory.HELVETICA, 9.5f, new Color(255, 245, 229));
    private final Font fontBannerDateLabel = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 9, new Color(255, 245, 229));
    private final Font fontBannerDate = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 13, Color.WHITE);

    private final Font fontSecaoPadrao = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 11.5f, COLOR_PRIMARY_DARK);
    private final Font fontSecaoVegano = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 11.5f, COLOR_VEGAN);

    private final Font fontHeaderTabela = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 8.5f, Color.WHITE);
    private final Font fontCategoria = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 8.5f, COLOR_TEXT_DARK);
    private final Font fontPrato = FontFactory.getFont(FontFactory.HELVETICA, 8.5f, COLOR_TEXT_DARK);
    private final Font fontMedida = FontFactory.getFont(FontFactory.HELVETICA, 8f, COLOR_TEXT_MUTED);
    private final Font fontNutriente = FontFactory.getFont(FontFactory.HELVETICA, 8.5f, COLOR_TEXT_DARK);

    private final Font fontTotalLabelPadrao = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 8.5f, COLOR_PRIMARY_DARK);
    private final Font fontTotalValorPadrao = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 8.5f, COLOR_PRIMARY_DARK);
    private final Font fontTotalLabelVegano = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 8.5f, COLOR_VEGAN);
    private final Font fontTotalValorVegano = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 8.5f, COLOR_VEGAN);

    private final Font fontRodape = FontFactory.getFont(FontFactory.HELVETICA_OBLIQUE, 7.5f, COLOR_TEXT_MUTED);

    public void export(
            HttpServletResponse response,
            Cardapio cardapioNormal,
            Cardapio cardapioVegano,
            LocalDate data
    ) throws IOException {
        Document document = new Document(PageSize.A4, 28, 28, 24, 24);
        PdfWriter.getInstance(document, response.getOutputStream());

        document.open();

        // 1. Cabeçalho com banner institucional Bandeco
        document.add(criarBannerCabecalho(data));

        // 2. Cardápio Padrão
        document.add(criarTituloSecao("CARDÁPIO PADRÃO", false));
        document.add(criarTabela(cardapioNormal, false));

        // Espaçamento entre os dois cardápios
        Paragraph espaco = new Paragraph(" ");
        espaco.setSpacingBefore(4f);
        espaco.setSpacingAfter(4f);
        document.add(espaco);

        // 3. Cardápio Vegano
        document.add(criarTituloSecao("CARDÁPIO VEGANO", true));
        document.add(criarTabela(cardapioVegano, true));

        // 4. Rodapé institucional
        document.add(criarRodape());

        document.close();
    }

    private PdfPTable criarBannerCabecalho(LocalDate data) {
        PdfPTable headerTable = new PdfPTable(2);
        headerTable.setWidthPercentage(100);
        try {
            headerTable.setWidths(new float[]{65f, 35f});
        } catch (DocumentException ignored) {}
        headerTable.setSpacingAfter(10f);

        // Lado esquerdo: título do sistema e do relatório
        PdfPCell cellLeft = new PdfPCell();
        cellLeft.setBackgroundColor(COLOR_PRIMARY);
        cellLeft.setPadding(10f);
        cellLeft.setPaddingLeft(14f);
        cellLeft.setBorder(Rectangle.BOTTOM);
        cellLeft.setBorderColorBottom(COLOR_GOLD);
        cellLeft.setBorderWidthBottom(3f);

        Paragraph pSub = new Paragraph("RESTAURANTE UNIVERSITÁRIO", fontBannerSub);
        Paragraph pTitle = new Paragraph("Cardápio & Informações Nutricionais", fontBannerTitle);
        cellLeft.addElement(pSub);
        cellLeft.addElement(pTitle);

        // Lado direito: dia da semana e data formatada
        PdfPCell cellRight = new PdfPCell();
        cellRight.setBackgroundColor(COLOR_PRIMARY);
        cellRight.setPadding(10f);
        cellRight.setPaddingRight(14f);
        cellRight.setHorizontalAlignment(Element.ALIGN_RIGHT);
        cellRight.setBorder(Rectangle.BOTTOM);
        cellRight.setBorderColorBottom(COLOR_GOLD);
        cellRight.setBorderWidthBottom(3f);

        String diaSemana = data.getDayOfWeek().getDisplayName(TextStyle.FULL, new Locale("pt", "BR"));
        String diaSemanaFormatado = Character.toUpperCase(diaSemana.charAt(0)) + diaSemana.substring(1);

        Paragraph pDataLabel = new Paragraph(diaSemanaFormatado, fontBannerDateLabel);
        pDataLabel.setAlignment(Element.ALIGN_RIGHT);
        Paragraph pData = new Paragraph(data.format(DateTimeFormatter.ofPattern("dd/MM/yyyy")), fontBannerDate);
        pData.setAlignment(Element.ALIGN_RIGHT);
        cellRight.addElement(pDataLabel);
        cellRight.addElement(pData);

        headerTable.addCell(cellLeft);
        headerTable.addCell(cellRight);
        return headerTable;
    }

    private PdfPTable criarTituloSecao(String titulo, boolean isVegano) {
        PdfPTable table = new PdfPTable(1);
        table.setWidthPercentage(100);
        table.setSpacingBefore(4f);
        table.setSpacingAfter(4f);

        PdfPCell cell = new PdfPCell();
        cell.setBackgroundColor(isVegano ? COLOR_VEGAN_LIGHT : COLOR_CREAM);
        cell.setBorder(Rectangle.LEFT);
        cell.setBorderColorLeft(isVegano ? COLOR_VEGAN : COLOR_PRIMARY);
        cell.setBorderWidthLeft(4f);
        cell.setPaddingTop(5f);
        cell.setPaddingBottom(5f);
        cell.setPaddingLeft(8f);

        Paragraph p = new Paragraph(titulo, isVegano ? fontSecaoVegano : fontSecaoPadrao);
        cell.addElement(p);

        table.addCell(cell);
        return table;
    }

    private PdfPTable criarTabela(Cardapio cardapio, boolean isVegano) {
        PdfPTable tabela = new PdfPTable(7);
        tabela.setWidthPercentage(100);

        try {
            tabela.setWidths(new float[]{17f, 29f, 12f, 10.5f, 10.5f, 10.5f, 10.5f});
        } catch (DocumentException ignored) {}

        // Cabeçalhos das colunas
        Color headerBg = isVegano ? COLOR_VEGAN : COLOR_PRIMARY_DARK;
        tabela.addCell(criarCelulaCabecalho("Categoria", headerBg, Element.ALIGN_LEFT));
        tabela.addCell(criarCelulaCabecalho("Prato", headerBg, Element.ALIGN_LEFT));
        tabela.addCell(criarCelulaCabecalho("Porção", headerBg, Element.ALIGN_CENTER));
        tabela.addCell(criarCelulaCabecalho("Kcal", headerBg, Element.ALIGN_RIGHT));
        tabela.addCell(criarCelulaCabecalho("Carbo (g)", headerBg, Element.ALIGN_RIGHT));
        tabela.addCell(criarCelulaCabecalho("Proteína (g)", headerBg, Element.ALIGN_RIGHT));
        tabela.addCell(criarCelulaCabecalho("Lipídios (g)", headerBg, Element.ALIGN_RIGHT));

        float[] totais = new float[4]; // [0] = Kcal, [1] = Carboidratos, [2] = Proteínas, [3] = Lipídios
        int[] rowCounter = new int[]{0};

        if (cardapio != null) {
            adicionarLinhaPrato(tabela, "Acompanhamento", cardapio.getAcompanhamento(), isVegano, rowCounter, totais);
            adicionarLinhaPrato(tabela, "Prato Principal", cardapio.getPrato_principal(), isVegano, rowCounter, totais);
            adicionarLinhaPrato(tabela, "Guarnição", cardapio.getGuarnicao(), isVegano, rowCounter, totais);
            adicionarLinhaPrato(tabela, "Salada", cardapio.getSalada(), isVegano, rowCounter, totais);
            adicionarLinhaPrato(tabela, "Sobremesa", cardapio.getSobremesa(), isVegano, rowCounter, totais);
            adicionarLinhaPrato(tabela, "Refresco", cardapio.getRefresco(), isVegano, rowCounter, totais);
        }

        // Se nenhum prato foi inserido
        if (rowCounter[0] == 0) {
            PdfPCell cellVazio = new PdfPCell(new Phrase("Nenhum prato cadastrado para esta refeição.", fontMedida));
            cellVazio.setColspan(7);
            cellVazio.setPadding(8f);
            cellVazio.setHorizontalAlignment(Element.ALIGN_CENTER);
            cellVazio.setBorder(Rectangle.BOX);
            cellVazio.setBorderColor(COLOR_BORDER);
            tabela.addCell(cellVazio);
        } else {
            // Linha de total estimado da refeição
            adicionarLinhaTotal(tabela, isVegano, totais);
        }

        return tabela;
    }

    private PdfPCell criarCelulaCabecalho(String texto, Color corFundo, int alinhamento) {
        PdfPCell cell = new PdfPCell(new Phrase(texto, fontHeaderTabela));
        cell.setBackgroundColor(corFundo);
        cell.setHorizontalAlignment(alinhamento);
        cell.setVerticalAlignment(Element.ALIGN_MIDDLE);
        cell.setPaddingTop(5f);
        cell.setPaddingBottom(5f);
        cell.setPaddingLeft(5f);
        cell.setPaddingRight(5f);
        cell.setBorder(Rectangle.NO_BORDER);
        return cell;
    }

    private void adicionarLinhaPrato(
            PdfPTable tabela,
            String categoria,
            Pratos prato,
            boolean isVegano,
            int[] rowCounter,
            float[] totais
    ) {
        if (prato == null) {
            return;
        }

        Color bgLinha = (rowCounter[0] % 2 == 0)
                ? Color.WHITE
                : (isVegano ? COLOR_VEGAN_LIGHT : COLOR_CREAM_ALT);

        rowCounter[0]++;

        ValorNutricional valor = prato.getValorNutricional();

        tabela.addCell(criarCelulaDado(categoria, fontCategoria, bgLinha, Element.ALIGN_LEFT));
        tabela.addCell(criarCelulaDado(prato.getNome(), fontPrato, bgLinha, Element.ALIGN_LEFT));

        if (valor != null) {
            tabela.addCell(criarCelulaDado(valor.getMedida() != null ? valor.getMedida() : "-", fontMedida, bgLinha, Element.ALIGN_CENTER));
            tabela.addCell(criarCelulaDado(formatarNutriente(valor.getKcal()), fontNutriente, bgLinha, Element.ALIGN_RIGHT));
            tabela.addCell(criarCelulaDado(formatarNutriente(valor.getCarboidratos()), fontNutriente, bgLinha, Element.ALIGN_RIGHT));
            tabela.addCell(criarCelulaDado(formatarNutriente(valor.getProteinas()), fontNutriente, bgLinha, Element.ALIGN_RIGHT));
            tabela.addCell(criarCelulaDado(formatarNutriente(valor.getLipidios()), fontNutriente, bgLinha, Element.ALIGN_RIGHT));

            totais[0] += valor.getKcal();
            totais[1] += valor.getCarboidratos();
            totais[2] += valor.getProteinas();
            totais[3] += valor.getLipidios();
        } else {
            tabela.addCell(criarCelulaDado("-", fontMedida, bgLinha, Element.ALIGN_CENTER));
            tabela.addCell(criarCelulaDado("-", fontNutriente, bgLinha, Element.ALIGN_RIGHT));
            tabela.addCell(criarCelulaDado("-", fontNutriente, bgLinha, Element.ALIGN_RIGHT));
            tabela.addCell(criarCelulaDado("-", fontNutriente, bgLinha, Element.ALIGN_RIGHT));
            tabela.addCell(criarCelulaDado("-", fontNutriente, bgLinha, Element.ALIGN_RIGHT));
        }
    }

    private PdfPCell criarCelulaDado(String texto, Font fonte, Color bgCor, int alinhamento) {
        PdfPCell cell = new PdfPCell(new Phrase(texto, fonte));
        cell.setBackgroundColor(bgCor);
        cell.setHorizontalAlignment(alinhamento);
        cell.setVerticalAlignment(Element.ALIGN_MIDDLE);
        cell.setPaddingTop(4.5f);
        cell.setPaddingBottom(4.5f);
        cell.setPaddingLeft(5f);
        cell.setPaddingRight(5f);
        cell.setBorder(Rectangle.BOTTOM);
        cell.setBorderColorBottom(COLOR_BORDER);
        cell.setBorderWidthBottom(0.5f);
        return cell;
    }

    private void adicionarLinhaTotal(PdfPTable tabela, boolean isVegano, float[] totais) {
        Color bgTotal = isVegano ? new Color(230, 247, 234) : new Color(254, 240, 215);
        Color borderColor = isVegano ? COLOR_VEGAN : COLOR_GOLD;
        Font fontLabel = isVegano ? fontTotalLabelVegano : fontTotalLabelPadrao;
        Font fontValor = isVegano ? fontTotalValorVegano : fontTotalValorPadrao;

        PdfPCell cellLabel = new PdfPCell(new Phrase("Total Estimado da Refeição:", fontLabel));
        cellLabel.setColspan(3);
        cellLabel.setHorizontalAlignment(Element.ALIGN_RIGHT);
        cellLabel.setVerticalAlignment(Element.ALIGN_MIDDLE);
        cellLabel.setPaddingTop(5f);
        cellLabel.setPaddingBottom(5f);
        cellLabel.setPaddingRight(8f);
        cellLabel.setBackgroundColor(bgTotal);
        cellLabel.setBorder(Rectangle.TOP | Rectangle.BOTTOM);
        cellLabel.setBorderColorTop(borderColor);
        cellLabel.setBorderWidthTop(1.2f);
        cellLabel.setBorderColorBottom(borderColor);
        cellLabel.setBorderWidthBottom(1.2f);
        tabela.addCell(cellLabel);

        for (int i = 0; i < 4; i++) {
            PdfPCell cellValor = new PdfPCell(new Phrase(formatarNutriente(totais[i]), fontValor));
            cellValor.setHorizontalAlignment(Element.ALIGN_RIGHT);
            cellValor.setVerticalAlignment(Element.ALIGN_MIDDLE);
            cellValor.setPaddingTop(5f);
            cellValor.setPaddingBottom(5f);
            cellValor.setPaddingRight(5f);
            cellValor.setBackgroundColor(bgTotal);
            cellValor.setBorder(Rectangle.TOP | Rectangle.BOTTOM);
            cellValor.setBorderColorTop(borderColor);
            cellValor.setBorderWidthTop(1.2f);
            cellValor.setBorderColorBottom(borderColor);
            cellValor.setBorderWidthBottom(1.2f);
            tabela.addCell(cellValor);
        }
    }

    private PdfPTable criarRodape() {
        PdfPTable rodapeTable = new PdfPTable(1);
        rodapeTable.setWidthPercentage(100);
        rodapeTable.setSpacingBefore(10f);

        PdfPCell cell = new PdfPCell();
        cell.setBorder(Rectangle.TOP);
        cell.setBorderColorTop(new Color(220, 220, 220));
        cell.setBorderWidthTop(0.5f);
        cell.setPaddingTop(5f);
        cell.setHorizontalAlignment(Element.ALIGN_CENTER);

        Paragraph p1 = new Paragraph(
                "* Os valores nutricionais apresentados são estimativas calculadas com base nas porções de referência e podem variar conforme o preparo diário.",
                fontRodape
        );
        p1.setAlignment(Element.ALIGN_CENTER);

        Paragraph p2 = new Paragraph(
                "Documento emitido eletronicamente pelo Sistema Bandeco RU",
                fontRodape
        );
        p2.setAlignment(Element.ALIGN_CENTER);

        cell.addElement(p1);
        cell.addElement(p2);
        rodapeTable.addCell(cell);

        return rodapeTable;
    }

    private String formatarNutriente(float valor) {
        if (valor == (long) valor) {
            return String.format(Locale.ROOT, "%d", (long) valor);
        }
        return String.format(Locale.ROOT, "%.1f", valor);
    }
}
