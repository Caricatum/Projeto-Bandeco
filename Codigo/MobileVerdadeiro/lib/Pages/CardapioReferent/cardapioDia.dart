import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:tcc_flutter/Class/cardapioClass.dart';
import 'package:tcc_flutter/Class/cardapioDiaClass.dart';
import 'package:tcc_flutter/Pages/menuNavegacao.dart';
import 'package:tcc_flutter/Class/usuarioClass.dart';

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

  bool carregandoHoje = true;
  bool carregandoSemana = true;

  String? erroHoje;
  String? erroSemana;

  @override
  void initState() {
    super.initState();

    carregarHoje();
    carregarSemana();
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

  Widget mostrarCardapio(String titulo, Cardapio? cardapio) {
    if (cardapio == null) {
      return const SizedBox.shrink();
    }

    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              titulo,
              style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 10),

            if (cardapio.pratoPrincipal != null)
              Text(
                'Prato principal: '
                '${cardapio.pratoPrincipal!.nome}',
              ),

            if (cardapio.acompanhamento != null)
              Text(
                'Acompanhamento: '
                '${cardapio.acompanhamento!.nome}',
              ),

            if (cardapio.guarnicao != null)
              Text(
                'Guarnição: '
                '${cardapio.guarnicao!.nome}',
              ),

            if (cardapio.salada != null)
              Text(
                'Salada: '
                '${cardapio.salada!.nome}',
              ),

            if (cardapio.sobremesa != null)
              Text(
                'Sobremesa: '
                '${cardapio.sobremesa!.nome}',
              ),

            if (cardapio.refresco != null)
              Text(
                'Refresco: '
                '${cardapio.refresco!.nome}',
              ),
          ],
        ),
      ),
    );
  }

  Widget mostrarDia(CardapioDia dia) {
    return Card(
      margin: const EdgeInsets.only(bottom: 20),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              nomeDia(dia.data),
              style: const TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
            ),

            Text(
              formatarData(dia.data),
              style: const TextStyle(color: Colors.grey),
            ),

            const SizedBox(height: 15),

            const Text(
              'Almoço',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 8),

            mostrarCardapio('Padrão', dia.padraoAlmoco),

            mostrarCardapio('Vegano', dia.veganoAlmoco),

            const SizedBox(height: 10),

            const Text(
              'Jantar',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
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
      appBar: AppBar(title: const Text('Cardápio')),

      drawer: MenuNavegacao(usuario: widget.usuario),

      body: RefreshIndicator(
        onRefresh: () async {
          await carregarHoje();
          await carregarSemana();
        },

        child: ListView(
          padding: const EdgeInsets.all(16),
          children: [
            const Text(
              'Cardápio de Hoje',
              style: TextStyle(fontSize: 26, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 15),

            if (carregandoHoje)
              const Center(child: CircularProgressIndicator())
            else if (erroHoje != null)
              Card(
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Text(erroHoje!),
                ),
              )
            else if (cardapioHoje != null)
              mostrarDia(cardapioHoje!),

            const SizedBox(height: 20),

            const Divider(),

            const SizedBox(height: 20),

            const Text(
              'Cardápio da Semana',
              style: TextStyle(fontSize: 26, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 15),

            if (carregandoSemana)
              const Center(child: CircularProgressIndicator())
            else if (erroSemana != null)
              Card(
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Text(erroSemana!),
                ),
              )
            else if (cardapiosSemana.isEmpty)
              const Card(
                child: Padding(
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
