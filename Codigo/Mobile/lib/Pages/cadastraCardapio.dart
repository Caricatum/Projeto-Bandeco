import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:tcc_flutter/Class/cardapioClass.dart';
import 'package:tcc_flutter/Class/pratosClass.dart';
import 'package:tcc_flutter/Class/categoriaClass.dart';

class CadastraCardapio extends StatefulWidget {
  const CadastraCardapio({super.key});

  @override
  State<CadastraCardapio> createState() => _CadastraCardapioState();
}

class _CadastraCardapioState extends State<CadastraCardapio> {

  List<Pratos> pratos = [];

  Cardapio cardapio = Cardapio();

  bool carregando = true;
  bool salvando = false;

  @override
  void initState() {
    super.initState();
    carregarPratos();
  }

  Future<void> carregarPratos() async {
    try {
      final response = await http.get(
        Uri.parse('http://localhost:8080/pratos/all'),
      );

      if (response.statusCode != 200) {
        throw Exception('Erro ao buscar pratos');
      }

      final List<dynamic> dados = jsonDecode(response.body);

      setState(() {
        pratos = dados
            .map((json) => Pratos.fromJson(json))
            .toList();

        carregando = false;
      });
    } catch (e) {
      setState(() {
        carregando = false;
      });

      mostrarMensagem('Erro ao carregar pratos: $e');
    }
  }

  List<Pratos> pratosDaCategoria(int categoriaId) {
    return pratos
        .where((prato) => prato.categoriaId == categoriaId)
        .toList();
  }

  Future<void> salvarCardapio() async {
    if (salvando) {
      return;
    }

    setState(() {
      salvando = true;
    });

    try {
      final response = await http.post(
        Uri.parse('http://localhost:8080/cardapio/cadastrar'),
        headers: {
          'Content-Type': 'application/json',
        },
        body: jsonEncode(cardapio.toJson()),
      );

      if (response.statusCode == 200 ||
          response.statusCode == 201) {
        final dados = jsonDecode(response.body);

        final novoCardapio = Cardapio.fromJson(dados);

        mostrarMensagem(
          'Cardápio cadastrado com sucesso. ID: ${novoCardapio.id}',
        );

        limparFormulario();
      } else {
        mostrarMensagem(
          'Erro ao cadastrar cardápio: ${response.body}',
        );
      }
    } catch (e) {
      mostrarMensagem(
        'Erro de conexão com a API: $e',
      );
    } finally {
      setState(() {
        salvando = false;
      });
    }
  }

  void limparFormulario() {
    setState(() {
      cardapio = Cardapio();
    });
  }

  void mostrarMensagem(String mensagem) {
    if (!mounted) {
      return;
    }

    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(mensagem),
      ),
    );
  }

  Widget seletorPrato({
    required String label,
    required int categoriaId,
    required Pratos? valor,
    required ValueChanged<Pratos?> onChanged,
  }) {
    final opcoes = pratosDaCategoria(categoriaId);

    return DropdownButtonFormField<Pratos>(
      value: valor,
      decoration: InputDecoration(
        labelText: label,
      ),
      items: opcoes.map((prato) {
        return DropdownMenuItem<Pratos>(
          value: prato,
          child: Text(prato.nome),
        );
      }).toList(),
      onChanged: onChanged,
    );
  }

  @override
  Widget build(BuildContext context) {
    if (carregando) {
      return const Scaffold(
        body: Center(
          child: CircularProgressIndicator(),
        ),
      );
    }

    return Scaffold(
      appBar: AppBar(
        title: const Text('Cadastrar Cardápio'),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            seletorPrato(
              label: 'Prato principal',
              categoriaId: 3,
              valor: cardapio.pratoPrincipal,
              onChanged: (Pratos? prato) {
                setState(() {
                  cardapio.pratoPrincipal = prato;
                });
              },
            ),

            const SizedBox(height: 16),

            seletorPrato(
              label: 'Acompanhamento',
              categoriaId: 1,
              valor: cardapio.acompanhamento,
              onChanged: (Pratos? prato) {
                setState(() {
                  cardapio.acompanhamento = prato;
                });
              },
            ),

            const SizedBox(height: 16),

            seletorPrato(
              label: 'Guarnição',
              categoriaId: 2,
              valor: cardapio.guarnicao,
              onChanged: (Pratos? prato) {
                setState(() {
                  cardapio.guarnicao = prato;
                });
              },
            ),

            const SizedBox(height: 16),

            seletorPrato(
              label: 'Salada',
              categoriaId: 5,
              valor: cardapio.salada,
              onChanged: (Pratos? prato) {
                setState(() {
                  cardapio.salada = prato;
                });
              },
            ),

            const SizedBox(height: 16),

            seletorPrato(
              label: 'Sobremesa',
              categoriaId: 6,
              valor: cardapio.sobremesa,
              onChanged: (Pratos? prato) {
                setState(() {
                  cardapio.sobremesa = prato;
                });
              },
            ),

            const SizedBox(height: 16),

            seletorPrato(
              label: 'Refresco',
              categoriaId: 4,
              valor: cardapio.refresco,
              onChanged: (Pratos? prato) {
                setState(() {
                  cardapio.refresco = prato;
                });
              },
            ),

            const SizedBox(height: 16),

            CheckboxListTile(
              title: const Text('Cardápio vegano'),
              value: cardapio.vegano,
              onChanged: (bool? valor) {
                setState(() {
                  cardapio.vegano = valor ?? false;
                });
              },
            ),

            const SizedBox(height: 16),

            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: salvando ? null : salvarCardapio,
                child: salvando
                    ? const CircularProgressIndicator()
                    : const Text('Cadastrar cardápio'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}