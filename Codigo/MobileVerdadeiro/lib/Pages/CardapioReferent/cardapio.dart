import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:tcc_flutter/Class/cardapioClass.dart';
import 'package:tcc_flutter/Class/pratosClass.dart';

import '../../Class/cardapioDiaClass.dart';
import '../../Class/usuarioClass.dart';

class CardapioPage extends StatefulWidget {
  const CardapioPage({super.key, required this.usuario});

  final Usuario usuario;

  @override
  State<CardapioPage> createState() => _CardapioPageState();
}

class _CardapioPageState extends State<CardapioPage> {
  DateTime? dataSelecionada;

  Cardapio? padraoAlmocoSelecionado;
  Cardapio? veganoAlmocoSelecionado;
  Cardapio? padraoJantarSelecionado;
  Cardapio? veganoJantarSelecionado;

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
      final response = await http.get(
        Uri.parse('http://localhost:8080/cardapio/all'),
      );

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

  Future<void> abrirCadastroCardapioDia() async {
    dataSelecionada = null;

    padraoAlmocoSelecionado = null;
    veganoAlmocoSelecionado = null;
    padraoJantarSelecionado = null;
    veganoJantarSelecionado = null;

    await showDialog(
      context: context,
      builder: (context) {
        return StatefulBuilder(
          builder: (context, setDialogState) {
            return AlertDialog(
              title: const Text('Cadastrar Cardápio do Dia'),
              content: SingleChildScrollView(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    // DATA
                    ListTile(
                      leading: const Icon(Icons.calendar_month),
                      title: Text(
                        dataSelecionada == null
                            ? 'Selecionar data'
                            : '${dataSelecionada!.day.toString().padLeft(2, '0')}/'
                                  '${dataSelecionada!.month.toString().padLeft(2, '0')}/'
                                  '${dataSelecionada!.year}',
                      ),
                      onTap: () async {
                        final data = await showDatePicker(
                          context: context,
                          initialDate: DateTime.now(),
                          firstDate: DateTime(2020),
                          lastDate: DateTime(2100),
                        );

                        if (data != null) {
                          setDialogState(() {
                            dataSelecionada = data;
                          });
                        }
                      },
                    ),

                    const SizedBox(height: 10),

                    // ALMOÇO PADRÃO
                    DropdownButtonFormField<Cardapio>(
                      decoration: const InputDecoration(
                        labelText: 'Almoço padrão',
                        border: OutlineInputBorder(),
                      ),
                      value: padraoAlmocoSelecionado,
                      items: cardapios
                          .where((cardapio) => !cardapio.vegano)
                          .map((cardapio) {
                            return DropdownMenuItem<Cardapio>(
                              value: cardapio,
                              child: Text('Cardápio ${cardapio.id}'),
                            );
                          })
                          .toList(),
                      onChanged: (value) {
                        setDialogState(() {
                          padraoAlmocoSelecionado = value;
                        });
                      },
                    ),

                    const SizedBox(height: 12),

                    // ALMOÇO VEGANO
                    DropdownButtonFormField<Cardapio>(
                      decoration: const InputDecoration(
                        labelText: 'Almoço vegano',
                        border: OutlineInputBorder(),
                      ),
                      value: veganoAlmocoSelecionado,
                      items: cardapios.where((cardapio) => cardapio.vegano).map(
                        (cardapio) {
                          return DropdownMenuItem<Cardapio>(
                            value: cardapio,
                            child: Text('Cardápio ${cardapio.id}'),
                          );
                        },
                      ).toList(),
                      onChanged: (value) {
                        setDialogState(() {
                          veganoAlmocoSelecionado = value;
                        });
                      },
                    ),

                    const SizedBox(height: 12),

                    // JANTAR PADRÃO
                    DropdownButtonFormField<Cardapio>(
                      decoration: const InputDecoration(
                        labelText: 'Jantar padrão',
                        border: OutlineInputBorder(),
                      ),
                      value: padraoJantarSelecionado,
                      items: cardapios
                          .where((cardapio) => !cardapio.vegano)
                          .map((cardapio) {
                            return DropdownMenuItem<Cardapio>(
                              value: cardapio,
                              child: Text('Cardápio ${cardapio.id}'),
                            );
                          })
                          .toList(),
                      onChanged: (value) {
                        setDialogState(() {
                          padraoJantarSelecionado = value;
                        });
                      },
                    ),

                    const SizedBox(height: 12),

                    // JANTAR VEGANO
                    DropdownButtonFormField<Cardapio>(
                      decoration: const InputDecoration(
                        labelText: 'Jantar vegano',
                        border: OutlineInputBorder(),
                      ),
                      value: veganoJantarSelecionado,
                      items: cardapios.where((cardapio) => cardapio.vegano).map(
                        (cardapio) {
                          return DropdownMenuItem<Cardapio>(
                            value: cardapio,
                            child: Text('Cardápio ${cardapio.id}'),
                          );
                        },
                      ).toList(),
                      onChanged: (value) {
                        setDialogState(() {
                          veganoJantarSelecionado = value;
                        });
                      },
                    ),
                  ],
                ),
              ),

              actions: [
                TextButton(
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                  child: const Text('Cancelar'),
                ),

                ElevatedButton(
                  onPressed: () async {
                    await cadastrarCardapioDia();
                  },
                  child: const Text('Cadastrar'),
                ),
              ],
            );
          },
        );
      },
    );
  }

  Future<void> cadastrarCardapioDia() async {
    if (!widget.usuario.funcionario) {
      return;
    }

    if (dataSelecionada == null) {
      mostrarMensagem('Selecione uma data.');
      return;
    }

    if (padraoAlmocoSelecionado == null &&
        veganoAlmocoSelecionado == null &&
        padraoJantarSelecionado == null &&
        veganoJantarSelecionado == null) {
      mostrarMensagem('Selecione pelo menos um cardápio.');
      return;
    }

    // Troque pelo ID do usuário logado
    const int usuarioId = 60;

    final cardapioDia = CardapioDia(
      data: dataSelecionada!,
      padraoAlmoco: padraoAlmocoSelecionado,
      veganoAlmoco: veganoAlmocoSelecionado,
      padraoJantar: padraoJantarSelecionado,
      veganoJantar: veganoJantarSelecionado,
      userId: usuarioId,
    );

    try {
      final response = await http.post(
        Uri.parse('http://localhost:8080/cardapioDia/cadastrar'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode(cardapioDia.toJson()),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        if (mounted) {
          Navigator.of(context).pop();
        }

        mostrarMensagem('Cardápio do dia cadastrado com sucesso!');
      } else {
        mostrarMensagem('Erro ao cadastrar cardápio do dia: ${response.body}');
      }
    } catch (e) {
      mostrarMensagem('Erro de conexão com a API: $e');
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

                if (widget.usuario.funcionario)
                  IconButton(
                    onPressed: deletando
                        ? null
                        : () => deletarCardapio(cardapio),
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

      floatingActionButton: widget.usuario.funcionario
          ? FloatingActionButton.extended(
              onPressed: abrirCadastroCardapioDia,
              icon: const Icon(Icons.add),
              label: const Text('Cardápio do Dia'),
            )
          : null,
    );
  }
}
