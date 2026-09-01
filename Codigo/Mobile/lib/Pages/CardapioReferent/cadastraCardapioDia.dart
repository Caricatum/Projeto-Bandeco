import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:tcc_flutter/Class/cardapioClass.dart';
import 'package:tcc_flutter/Class/cardapioDiaClass.dart';

import 'package:tcc_flutter/Class/usuarioClass.dart';
import 'package:tcc_flutter/Pages/menuNavegacao.dart';

class CardapioDiaPage extends StatefulWidget {
  final Usuario usuario;

  const CardapioDiaPage({super.key, required this.usuario});

  @override
  State<CardapioDiaPage> createState() => _CardapioDiaPageState();
}

class _CardapioDiaPageState extends State<CardapioDiaPage> {
  CardapioDia? cardapioDia;

  List<Cardapio> cardapios = [];

  Cardapio? padraoAlmoco;
  Cardapio? veganoAlmoco;
  Cardapio? padraoJantar;
  Cardapio? veganoJantar;

  DateTime dataSelecionada = DateTime.now();

  bool carregando = true;

  @override
  void initState() {
    super.initState();

    carregarDados();
  }

  Future<void> carregarDados() async {
    setState(() {
      carregando = true;
    });

    await buscarCardapios();
    await buscarCardapioDia();

    setState(() {
      carregando = false;
    });
  }

  // Função para buscar todos os cardápios disponíveis
  Future<void> buscarCardapios() async {
    final response = await http.get(
      Uri.parse('http://localhost:8080/cardapio/all'),
    );

    if (response.statusCode == 200) {
      final List<dynamic> dados = jsonDecode(response.body);

      setState(() {
        cardapios = dados.map((json) => Cardapio.fromJson(json)).toList();
      });
    }
  }

  // Função para buscar o cardápio do dia selecionado
  Future<void> buscarCardapioDia() async {
    final data = dataSelecionada.toIso8601String().split('T').first;

    final response = await http.get(
      Uri.parse('http://localhost:8080/cardapioDia/data/$data'),
    );

    if (response.statusCode == 200) {
      final dados = jsonDecode(response.body);

      setState(() {
        cardapioDia = CardapioDia.fromJson(dados);

        padraoAlmoco = cardapioDia!.padraoAlmoco;
        veganoAlmoco = cardapioDia!.veganoAlmoco;
        padraoJantar = cardapioDia!.padraoJantar;
        veganoJantar = cardapioDia!.veganoJantar;
      });
    } else if (response.statusCode == 404) {
      setState(() {
        cardapioDia = null;

        padraoAlmoco = null;
        veganoAlmoco = null;
        padraoJantar = null;
        veganoJantar = null;
      });
    }
  }

  // Função para cadastrar, atualizar ou deletar o cardápio do dia
  Future<void> cadastrarCardapioDia() async {
    final novoCardapioDia = CardapioDia(
      data: dataSelecionada,
      padraoAlmoco: padraoAlmoco,
      veganoAlmoco: veganoAlmoco,
      padraoJantar: padraoJantar,
      veganoJantar: veganoJantar,
      user: widget.usuario,
    );

    final response = await http.post(
      Uri.parse('http://localhost:8080/cardapioDia/cadastrar'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode(novoCardapioDia.toJson()),
    );

    if (response.statusCode == 200) {
      await buscarCardapioDia();

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Cardápio cadastrado com sucesso')),
      );
    } else {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao cadastrar: ${response.body}')),
      );
    }
  }

  // Função para atualizar o cardápio do dia
  Future<void> atualizarCardapioDia() async {
    if (cardapioDia == null) return;

    final atualizado = CardapioDia(
      id: cardapioDia!.id,
      data: dataSelecionada,
      padraoAlmoco: padraoAlmoco,
      veganoAlmoco: veganoAlmoco,
      padraoJantar: padraoJantar,
      veganoJantar: veganoJantar,
      user: widget.usuario,
    );

    final response = await http.put(
      Uri.parse('http://localhost:8080/cardapioDia/atualizar'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode(atualizado.toJson()),
    );

    if (response.statusCode == 200) {
      await buscarCardapioDia();

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Cardápio atualizado com sucesso')),
      );
    } else {
      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao atualizar: ${response.body}')),
      );
    }
  }

  // Função para deletar o cardápio do dia
  Future<void> deletarCardapioDia() async {
    if (cardapioDia == null) return;

    final response = await http.delete(
      Uri.parse('http://localhost:8080/cardapioDia/deletar/${cardapioDia!.id}'),
    );

    if (response.statusCode == 200 || response.statusCode == 204) {
      setState(() {
        cardapioDia = null;

        padraoAlmoco = null;
        veganoAlmoco = null;
        padraoJantar = null;
        veganoJantar = null;
      });

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Cardápio deletado com sucesso')),
      );
    }
  }

  // Função para selecionar a data do cardápio
  Future<void> selecionarData() async {
    final data = await showDatePicker(
      context: context,
      initialDate: dataSelecionada,
      firstDate: DateTime(2020),
      lastDate: DateTime(2100),
    );

    if (data != null) {
      setState(() {
        dataSelecionada = data;
      });

      await buscarCardapioDia();
    }
  }

  List<Cardapio> cardapiosPadrao() {
    return cardapios.where((cardapio) => !cardapio.vegano).toList();
  }

  List<Cardapio> cardapiosVeganos() {
    return cardapios.where((cardapio) => cardapio.vegano).toList();
  }

  String nomeCardapio(Cardapio cardapio) {
    final prato = cardapio.pratoPrincipal;

    if (prato != null) {
      return prato.nome;
    }

    return 'Cardápio ${cardapio.id}';
  }

  Widget selecionarCardapio({
    required String titulo,
    required Cardapio? valor,
    required List<Cardapio> opcoes,
    required Function(Cardapio?) onChanged,
  }) {
    return DropdownButtonFormField<Cardapio>(
      value: valor,
      decoration: InputDecoration(
        labelText: titulo,
        border: const OutlineInputBorder(),
      ),
      items: opcoes.map((cardapio) {
        return DropdownMenuItem<Cardapio>(
          value: cardapio,
          child: Text(nomeCardapio(cardapio)),
        );
      }).toList(),
      onChanged: onChanged,
    );
  }

  Widget mostrarCardapio(String titulo, Cardapio? cardapio) {
    if (cardapio == null) {
      return Text('$titulo: não cadastrado');
    }

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          titulo,
          style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),

        if (cardapio.acompanhamento != null)
          Text(
            'Acompanhamento: '
            '${cardapio.acompanhamento!.nome}',
          ),

        if (cardapio.pratoPrincipal != null)
          Text(
            'Prato principal: '
            '${cardapio.pratoPrincipal!.nome}',
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
    );
  }

  @override
  Widget build(BuildContext context) {
    if (carregando) {
      return const Scaffold(body: Center(child: CircularProgressIndicator()));
    }

    final padrao = cardapiosPadrao();
    final vegano = cardapiosVeganos();

    return Scaffold(
      appBar: AppBar(title: const Text('Cardápio do Dia')),

      drawer: MenuNavegacao(usuario: widget.usuario),

      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            ElevatedButton(
              onPressed: selecionarData,
              child: Text(
                'Data: '
                '${dataSelecionada.day.toString().padLeft(2, '0')}/'
                '${dataSelecionada.month.toString().padLeft(2, '0')}/'
                '${dataSelecionada.year}',
              ),
            ),

            const SizedBox(height: 20),

            const Text(
              'ALMOÇO',
              style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 10),

            selecionarCardapio(
              titulo: 'Cardápio padrão',
              valor: padraoAlmoco,
              opcoes: padrao,
              onChanged: (value) {
                setState(() {
                  padraoAlmoco = value;
                });
              },
            ),

            const SizedBox(height: 10),

            selecionarCardapio(
              titulo: 'Cardápio vegano',
              valor: veganoAlmoco,
              opcoes: vegano,
              onChanged: (value) {
                setState(() {
                  veganoAlmoco = value;
                });
              },
            ),

            const SizedBox(height: 30),

            const Text(
              'JANTAR',
              style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 10),

            selecionarCardapio(
              titulo: 'Cardápio padrão',
              valor: padraoJantar,
              opcoes: padrao,
              onChanged: (value) {
                setState(() {
                  padraoJantar = value;
                });
              },
            ),

            const SizedBox(height: 10),

            selecionarCardapio(
              titulo: 'Cardápio vegano',
              valor: veganoJantar,
              opcoes: vegano,
              onChanged: (value) {
                setState(() {
                  veganoJantar = value;
                });
              },
            ),

            const SizedBox(height: 30),

            if (cardapioDia == null) ...[
              ElevatedButton(
                onPressed: cadastrarCardapioDia,
                child: const Text('Cadastrar Cardápio'),
              ),
            ] else ...[
              ElevatedButton(
                onPressed: atualizarCardapioDia,
                child: const Text('Atualizar Cardápio'),
              ),

              const SizedBox(height: 10),

              ElevatedButton(
                onPressed: deletarCardapioDia,
                child: const Text('Deletar Cardápio'),
              ),
            ],

            const SizedBox(height: 30),

            const Divider(),

            const Text(
              'Cardápio cadastrado',
              style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 20),

            mostrarCardapio('Almoço padrão', cardapioDia?.padraoAlmoco),

            const SizedBox(height: 15),

            mostrarCardapio('Almoço vegano', cardapioDia?.veganoAlmoco),

            const SizedBox(height: 15),

            mostrarCardapio('Jantar padrão', cardapioDia?.padraoJantar),

            const SizedBox(height: 15),

            mostrarCardapio('Jantar vegano', cardapioDia?.veganoJantar),
          ],
        ),
      ),
    );
  }
}
