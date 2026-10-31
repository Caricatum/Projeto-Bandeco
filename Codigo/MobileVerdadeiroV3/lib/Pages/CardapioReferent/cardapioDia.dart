import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:tcc_flutter/Class/cardapioClass.dart';
import 'package:tcc_flutter/Class/cardapioDiaClass.dart';
import 'package:tcc_flutter/Class/pratosClass.dart';
import 'package:tcc_flutter/Pages/CardapioReferent/pratoMostra.dart';
import 'package:tcc_flutter/Pages/menuNavegacao.dart';
import 'package:tcc_flutter/Class/usuarioClass.dart';
import 'package:tcc_flutter/Services/notificacaoService.dart';

class CardapioMostra extends StatefulWidget {
  const CardapioMostra({super.key, required this.usuario});

  final Usuario usuario;

  @override
  State<CardapioMostra> createState() => _CardapioAlunoPageState();
}

class _CardapioAlunoPageState extends State<CardapioMostra> {
  final String url = 'http://localhost:8080/cardapioDia';

  CardapioDia? cardapioHoje;
  List<CardapioDia> cardapiosSemana = [];
  Set<int> idsFavoritos = {};

  bool carregandoHoje = true;
  bool carregandoSemana = true;

  String? erroHoje;
  String? erroSemana;

  @override
  void initState() {
    super.initState();

    carregarFavoritos();
    carregarHoje();
    carregarSemana();
  }

  Future<void> carregarFavoritos() async {
    try {
      final response = await http.get(
        Uri.parse('http://localhost:8080/pratosFavoritos/all'),
      );
      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);
        final Set<int> ids = {};
        for (final item in dados) {
          if (item is Map &&
              item['user'] is Map &&
              item['user']['id'] == widget.usuario.id &&
              item['prato'] != null) {
            ids.add(item['prato']['id']);
          }
        }
        if (mounted) {
          setState(() {
            idsFavoritos = ids;
          });
        }
      }
    } catch (_) {}
  }

  Future<void> carregarHoje() async {
    setState(() {
      carregandoHoje = true;
      erroHoje = null;
    });

    final data = DateTime.now().toIso8601String().split('T').first;

    try {
      final response = await http.get(Uri.parse('$url/data/$data'));

      if (response.statusCode == 200) {
        setState(() {
          cardapioHoje = CardapioDia.fromJson(jsonDecode(response.body));

          carregandoHoje = false;
        });

        // Verifica favoritos e alergias no cardápio de hoje.
        await Future.wait([
          NotificacaoService.instance.verificarCardapioHojeEFavoritos(
            usuarioId: widget.usuario.id,
          ),
          NotificacaoService.instance.verificarAlergiasNoCardapio(
            usuario: widget.usuario,
          ),
        ]);
      } else if (response.statusCode == 404) {
        setState(() {
          cardapioHoje = null;
          erroHoje = 'Nenhum cardápio cadastrado para hoje.';
          carregandoHoje = false;
        });
      } else {
        throw Exception('Erro ${response.statusCode}');
      }
    } catch (e) {
      setState(() {
        erroHoje = 'Não foi possível carregar o cardápio.';
        carregandoHoje = false;
      });
    }
  }

  Future<void> carregarSemana() async {
    setState(() {
      carregandoSemana = true;
      erroSemana = null;
    });

    try {
      final response = await http.get(Uri.parse('$url/semana'));

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        setState(() {
          cardapiosSemana = dados
              .map((json) => CardapioDia.fromJson(json))
              .toList();

          cardapiosSemana.sort((a, b) => a.data.compareTo(b.data));

          carregandoSemana = false;
        });
      } else {
        throw Exception('Erro ${response.statusCode}');
      }
    } catch (e) {
      setState(() {
        erroSemana = 'Não foi possível carregar os cardápios da semana.';
        carregandoSemana = false;
      });
    }
  }

  String formatarData(DateTime data) {
    return '${data.day.toString().padLeft(2, '0')}/'
        '${data.month.toString().padLeft(2, '0')}/'
        '${data.year}';
  }

  String nomeDia(DateTime data) {
    const dias = [
      'Segunda-feira',
      'Terça-feira',
      'Quarta-feira',
      'Quinta-feira',
      'Sexta-feira',
      'Sábado',
      'Domingo',
    ];

    return dias[data.weekday - 1];
  }

  void abrirPrato(Pratos prato) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) =>
            PratoPage(pratoId: prato.id, usuario: widget.usuario),
      ),
    );
  }

  Widget mostrarItemPrato(String rotulo, Pratos? prato) {
    if (prato == null) {
      return const SizedBox.shrink();
    }

    final bool ehFavorito = idsFavoritos.contains(prato.id);

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 3),
      child: InkWell(
        onTap: () => abrirPrato(prato),
        borderRadius: BorderRadius.circular(6),
        child: Padding(
          padding: const EdgeInsets.symmetric(vertical: 2, horizontal: 2),
          child: Row(
            children: [
              Expanded(
                child: Text.rich(
                  TextSpan(
                    text: '$rotulo: ',
                    style: const TextStyle(
                      fontWeight: FontWeight.w600,
                      color: Color(0xFF4A3B32),
                    ),
                    children: [
                      TextSpan(
                        text: prato.nome,
                        style: const TextStyle(
                          color: Color(0xFFE76F51),
                          fontWeight: FontWeight.bold,
                          decoration: TextDecoration.underline,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              if (ehFavorito)
                Container(
                  margin: const EdgeInsets.only(left: 6),
                  padding: const EdgeInsets.symmetric(
                    horizontal: 6,
                    vertical: 2,
                  ),
                  decoration: BoxDecoration(
                    color: Colors.red.shade50,
                    borderRadius: BorderRadius.circular(8),
                    border: Border.all(color: Colors.red.shade200),
                  ),
                  child: const Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(Icons.favorite, size: 11, color: Colors.red),
                      SizedBox(width: 3),
                      Text(
                        'Favorito',
                        style: TextStyle(
                          color: Colors.red,
                          fontSize: 10,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ],
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }

  Widget mostrarCardapio(String titulo, Cardapio? cardapio) {
    if (cardapio == null) {
      return const SizedBox.shrink();
    }

    final bool ehVegano = titulo.toLowerCase().contains('vegano');

    return Card(
      elevation: 0,
      color: ehVegano ? const Color(0xFFF1F8E9) : const Color(0xFFFFE8D6),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(14),
        side: BorderSide(
          color: ehVegano
              ? Colors.green.shade200
              : const Color(0xFFF4A261).withValues(alpha: 0.4),
        ),
      ),
      margin: const EdgeInsets.only(bottom: 12),
      child: Padding(
        padding: const EdgeInsets.all(14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(
                  ehVegano ? Icons.eco : Icons.restaurant_menu,
                  size: 18,
                  color: ehVegano ? Colors.green : const Color(0xFFE76F51),
                ),
                const SizedBox(width: 6),
                Text(
                  titulo,
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                    color: ehVegano
                        ? Colors.green.shade800
                        : const Color(0xFF9C3D2A),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 8),

            mostrarItemPrato('Prato principal', cardapio.pratoPrincipal),
            mostrarItemPrato('Acompanhamento', cardapio.acompanhamento),
            mostrarItemPrato('Guarnição', cardapio.guarnicao),
            mostrarItemPrato('Salada', cardapio.salada),
            mostrarItemPrato('Sobremesa', cardapio.sobremesa),
            mostrarItemPrato('Refresco', cardapio.refresco),
          ],
        ),
      ),
    );
  }

  Widget mostrarDia(CardapioDia dia) {
    return Card(
      elevation: 4,
      color: Colors.white,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
      margin: const EdgeInsets.only(bottom: 20),
      child: Padding(
        padding: const EdgeInsets.all(18),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  nomeDia(dia.data),
                  style: const TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF9C3D2A),
                  ),
                ),
                Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: 10,
                    vertical: 4,
                  ),
                  decoration: BoxDecoration(
                    color: const Color(0xFFFFE8D6),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: Text(
                    formatarData(dia.data),
                    style: const TextStyle(
                      color: Color(0xFFE76F51),
                      fontWeight: FontWeight.bold,
                      fontSize: 13,
                    ),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 16),

            Row(
              children: const [
                Icon(Icons.wb_sunny, color: Color(0xFFE76F51), size: 22),
                SizedBox(width: 8),
                Text(
                  'Almoço',
                  style: TextStyle(
                    fontSize: 19,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF9C3D2A),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 8),

            mostrarCardapio('Padrão', dia.padraoAlmoco),
            mostrarCardapio('Vegano', dia.veganoAlmoco),

            const SizedBox(height: 14),

            Row(
              children: const [
                Icon(
                  Icons.nightlight_round,
                  color: Color(0xFFF4A261),
                  size: 22,
                ),
                SizedBox(width: 8),
                Text(
                  'Jantar',
                  style: TextStyle(
                    fontSize: 19,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF9C3D2A),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 8),

            mostrarCardapio('Padrão', dia.padraoJantar),
            mostrarCardapio('Vegano', dia.veganoJantar),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFFFF8F1),
      appBar: AppBar(
        backgroundColor: const Color(0xFFE76F51),
        foregroundColor: Colors.white,
        elevation: 0,
        centerTitle: true,
        title: const Text(
          '📅 Cardápio',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
      ),

      drawer: MenuNavegacao(usuario: widget.usuario),

      body: RefreshIndicator(
        color: const Color(0xFFE76F51),
        onRefresh: () async {
          await carregarHoje();
          await carregarSemana();
        },

        child: ListView(
          padding: const EdgeInsets.all(16),
          children: [
            const Text(
              'Cardápio de Hoje',
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
                color: Color(0xFF9C3D2A),
              ),
            ),

            const SizedBox(height: 15),

            if (carregandoHoje)
              const Center(
                child: Padding(
                  padding: EdgeInsets.all(24),
                  child: CircularProgressIndicator(color: Color(0xFFE76F51)),
                ),
              )
            else if (erroHoje != null)
              Card(
                color: const Color(0xFFFFE8D6),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16),
                ),
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Text(
                    erroHoje!,
                    style: const TextStyle(color: Color(0xFF9C3D2A)),
                  ),
                ),
              )
            else if (cardapioHoje != null)
              mostrarDia(cardapioHoje!),

            const SizedBox(height: 20),

            Divider(color: Colors.orange.shade200, thickness: 1),

            const SizedBox(height: 20),

            const Text(
              'Cardápio da Semana',
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
                color: Color(0xFF9C3D2A),
              ),
            ),

            const SizedBox(height: 15),

            if (carregandoSemana)
              const Center(
                child: Padding(
                  padding: EdgeInsets.all(24),
                  child: CircularProgressIndicator(color: Color(0xFFE76F51)),
                ),
              )
            else if (erroSemana != null)
              Card(
                color: const Color(0xFFFFE8D6),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16),
                ),
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Text(
                    erroSemana!,
                    style: const TextStyle(color: Color(0xFF9C3D2A)),
                  ),
                ),
              )
            else if (cardapiosSemana.isEmpty)
              Card(
                color: Colors.white,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16),
                ),
                child: const Padding(
                  padding: EdgeInsets.all(16),
                  child: Text('Nenhum cardápio cadastrado para esta semana.'),
                ),
              )
            else
              ...cardapiosSemana.map((dia) => mostrarDia(dia)),
          ],
        ),
      ),
    );
  }
}
