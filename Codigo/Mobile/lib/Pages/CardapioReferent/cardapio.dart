import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:tcc_flutter/Class/cardapioClass.dart';
import 'package:tcc_flutter/Class/pratosClass.dart';

class CardapioPage extends StatefulWidget {
  const CardapioPage({super.key});

  @override
  State<CardapioPage> createState() => _CardapioPageState();
}

class _CardapioPageState extends State<CardapioPage> {

  List<Cardapio> cardapios = [];

  bool carregando = true;
  bool deletando = false;

  @override
  void initState() {
    super.initState();
    carregarCardapios();
  }

  Future<void> carregarCardapios() async {
    setState(() {
      carregando = true;
    });

    try {
      final response = await http.get(Uri.parse('http://localhost:8080/cardapio/all'));

      if (response.statusCode != 200) {
        throw Exception('Erro ao buscar cardápios: ${response.statusCode}');
      }

      final List<dynamic> dados = jsonDecode(response.body);

      setState(() {
        cardapios = dados.map((json) => Cardapio.fromJson(json)).toList();

        carregando = false;
      });
    } catch (e) {
      setState(() {
        carregando = false;
      });

      mostrarMensagem('Erro ao carregar cardápios: $e');
    }
  }

  Future<void> deletarCardapio(Cardapio cardapio) async {
    if (cardapio.id == null || deletando) {
      return;
    }

    final bool? confirmar = await showDialog<bool>(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Excluir cardápio'),
          content: Text('Deseja realmente excluir o cardápio ${cardapio.id}?'),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(false);
              },
              child: const Text('Cancelar'),
            ),
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(true);
              },
              child: const Text('Excluir'),
            ),
          ],
        );
      },
    );

    if (confirmar != true) {
      return;
    }

    setState(() {
      deletando = true;
    });

    try {
      final response = await http.delete(
        Uri.parse('http://localhost:8080/cardapio/deletar/${cardapio.id}'),
      );

      if (response.statusCode == 200 || response.statusCode == 204) {
        setState(() {
          cardapios.removeWhere((item) => item.id == cardapio.id);
        });

        mostrarMensagem('Cardápio ${cardapio.id} excluído com sucesso.');
      } else {
        mostrarMensagem('Erro ao excluir cardápio: ${response.body}');
      }
    } catch (e) {
      mostrarMensagem('Erro de conexão com a API: $e');
    } finally {
      setState(() {
        deletando = false;
      });
    }
  }

  void mostrarMensagem(String mensagem) {
    if (!mounted) {
      return;
    }

    ScaffoldMessenger.of(
      context,
    ).showSnackBar(SnackBar(content: Text(mensagem)));
  }

  Widget mostrarPrato(String titulo, dynamic prato) {
    if (prato == null) {
      return Text('$titulo: Não informado');
    }

    return Text('$titulo: ${prato.nome}');
  }

  Widget cardapioWidget(Cardapio cardapio) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(12),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  'Cardápio ${cardapio.id ?? ''}',
                  style: const TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),

                IconButton(
                  onPressed: deletando ? null : () => deletarCardapio(cardapio),
                  icon: const Icon(Icons.delete),
                ),
              ],
            ),

            const SizedBox(height: 8),

            mostrarPrato('Prato principal', cardapio.pratoPrincipal),

            const SizedBox(height: 4),

            mostrarPrato('Acompanhamento', cardapio.acompanhamento),

            const SizedBox(height: 4),

            mostrarPrato('Guarnição', cardapio.guarnicao),

            const SizedBox(height: 4),

            mostrarPrato('Salada', cardapio.salada),

            const SizedBox(height: 4),

            mostrarPrato('Sobremesa', cardapio.sobremesa),

            const SizedBox(height: 4),

            mostrarPrato('Refresco', cardapio.refresco),

            const SizedBox(height: 8),

            Text('Vegano: ${cardapio.vegano ? 'Sim' : 'Não'}'),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Cardápios'),
        actions: [
          IconButton(
            onPressed: carregarCardapios,
            icon: const Icon(Icons.refresh),
          ),
        ],
      ),
      body: carregando
          ? const Center(child: CircularProgressIndicator())
          : cardapios.isEmpty
          ? const Center(child: Text('Nenhum cardápio cadastrado.'))
          : RefreshIndicator(
              onRefresh: carregarCardapios,
              child: ListView.builder(
                padding: const EdgeInsets.all(12),
                itemCount: cardapios.length,
                itemBuilder: (context, index) {
                  return cardapioWidget(cardapios[index]);
                },
              ),
            ),
    );
  }
}
