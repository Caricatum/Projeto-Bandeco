import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:tcc_flutter/Pages/CardapioReferent/pratoMostra.dart';
import 'package:tcc_flutter/Widgets/imagemPrato.dart';
import 'atualizaPrato.dart';
import 'package:tcc_flutter/Class/categoriaClass.dart';
import 'cadastroPrato.dart';
import '../menuNavegacao.dart';
import '../../Class/usuarioClass.dart';

class CardapioPratos extends StatefulWidget {
  const CardapioPratos({super.key, required this.usuario});

  final Usuario usuario;

  @override
  State<CardapioPratos> createState() => _CardapioPratosState();
}

class _CardapioPratosState extends State<CardapioPratos> {
  late Future<List<dynamic>> pratosFuture;

  // Mostra tudo
  Future<List<dynamic>> listarPratos() async {
    final url = Uri.parse('http://localhost:8080/pratos/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      } else {
        throw Exception('Falha ao carregar os pratos');
      }
    } catch (e) {
      throw Exception('Erro de conexão: $e');
    }
  }

  // Deletar prato
  Future<void> deletarPrato(int id) async {
    final response = await http.delete(
      Uri.parse("http://localhost:8080/pratos/deletar/$id"),
    );

    if (response.statusCode != 200) {
      throw Exception("Erro ao excluir prato.");
    }
  }

  // Confirma pra deletar o prato
  Future<void> confirmarExclusao(int id) async {
    final bool? confirmar = await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Excluir prato"),
          content: const Text("Tem certeza que deseja excluir este prato?"),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.pop(context, false);
              },
              child: const Text("Cancelar"),
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
              onPressed: () {
                Navigator.pop(context, true);
              },
              child: const Text("Excluir"),
            ),
          ],
        );
      },
    );

    if (confirmar == true) {
      try {
        await deletarPrato(id);

        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Prato removido com sucesso!")),
        );

        setState(() {
          pratosFuture = listarPratos();
        });
      } catch (e) {
        if (!mounted) return;
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text(e.toString())));
      }
    }
  }

  final TextEditingController nomeController = TextEditingController();

  bool somenteVeganos = false;
  bool somenteSemLactose = false;
  bool somenteSemGluten = false;

  Categoria? categoriaSelecionada;

  @override
  void initState() {
    super.initState();
    pratosFuture = listarPratos();
  }

  @override
  void dispose() {
    nomeController.dispose();
    super.dispose();
  }

  Widget _tagChip(String texto, Color bg, Color textColor) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
      decoration: BoxDecoration(
        color: bg,
        borderRadius: BorderRadius.circular(10),
      ),
      child: Text(
        texto,
        style: TextStyle(
          color: textColor,
          fontWeight: FontWeight.bold,
          fontSize: 11,
        ),
      ),
    );
  }

  Widget _buildTagsPrato(dynamic prato) {
    if (prato is! Map) return const SizedBox.shrink();

    final List<Widget> tags = [];

    // Alérgenos
    if (prato['leite'] == true) {
      tags.add(
        _tagChip('🥛 Leite', const Color(0xFFE0F2FE), const Color(0xFF0369A1)),
      );
    }
    if (prato['ovo'] == true) {
      tags.add(
        _tagChip('🥚 Ovo', const Color(0xFFFEF3C7), const Color(0xFFB45309)),
      );
    }
    if (prato['amendoim'] == true) {
      tags.add(
        _tagChip(
          '🥜 Amendoim',
          const Color(0xFFFEF3C7),
          const Color(0xFF92400E),
        ),
      );
    }
    if (prato['castanhas'] == true) {
      tags.add(
        _tagChip(
          '🌰 Castanhas',
          const Color(0xFFFFEDD5),
          const Color(0xFFC2410C),
        ),
      );
    }
    if (prato['frutosDoMar'] == true) {
      tags.add(
        _tagChip(
          '🦐 Frutos Mar',
          const Color(0xFFCFFAFE),
          const Color(0xFF0E7490),
        ),
      );
    }
    if (prato['peixes'] == true) {
      tags.add(
        _tagChip('🐟 Peixes', const Color(0xFFE0F2FE), const Color(0xFF0284C7)),
      );
    }
    if (prato['trigo'] == true) {
      tags.add(
        _tagChip('🌾 Glúten', const Color(0xFFFEF9C3), const Color(0xFF854D0E)),
      );
    }
    if (prato['soja'] == true) {
      tags.add(
        _tagChip('🌱 Soja', const Color(0xFFDCFCE7), const Color(0xFF15803D)),
      );
    }
    if (prato['gergilim'] == true) {
      tags.add(
        _tagChip(
          '⚪ Gergelim',
          const Color(0xFFF3F4F6),
          const Color(0xFF374151),
        ),
      );
    }

    // Rótulos Nutricionais
    if (prato['altoAcucarAdicionado'] == true) {
      tags.add(
        _tagChip('⚠️ Açúcar', const Color(0xFF18181B), const Color(0xFFFACC15)),
      );
    }
    if (prato['altoGorduraSaturada'] == true) {
      tags.add(
        _tagChip(
          '⚠️ Gord. Sat.',
          const Color(0xFF18181B),
          const Color(0xFFFACC15),
        ),
      );
    }
    if (prato['altoSodio'] == true) {
      tags.add(
        _tagChip('⚠️ Sódio', const Color(0xFF18181B), const Color(0xFFFACC15)),
      );
    }

    if (tags.isEmpty) return const SizedBox.shrink();

    return Padding(
      padding: const EdgeInsets.only(top: 8),
      child: Wrap(spacing: 6, runSpacing: 4, children: tags),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFFFF8F1),
      appBar: AppBar(
        backgroundColor: const Color(0xFFE76F51),
        elevation: 0,
        centerTitle: true,
        title: const Text(
          '🍽️ Pratos',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
        actions: widget.usuario.funcionario
            ? [
                IconButton(
                  icon: const Icon(
                    Icons.add_circle_outline,
                    color: Colors.white,
                  ),
                  tooltip: "Cadastrar prato",
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (_) => Cadastroprato(usuario: widget.usuario),
                      ),
                    ).then((_) {
                      setState(() {});
                    });
                  },
                ),
              ]
            : null,
      ),
      drawer: MenuNavegacao(usuario: widget.usuario),
      body: Column(
        children: [
          Container(
            margin: const EdgeInsets.all(16),
            padding: const EdgeInsets.all(16),
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(18),
              boxShadow: [
                BoxShadow(
                  color: Colors.orange.withOpacity(0.15),
                  blurRadius: 12,
                  offset: const Offset(0, 5),
                ),
              ],
            ),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: nomeController,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                      filled: true,
                      fillColor: const Color(0xFFFFF3E6),
                      prefixIcon: const Icon(
                        Icons.search,
                        color: Color(0xFFE76F51),
                      ),
                      labelText: "Buscar prato",
                      labelStyle: const TextStyle(color: Color(0xFFE76F51)),
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(15),
                        borderSide: BorderSide.none,
                      ),
                    ),
                    onChanged: (_) => setState(() {}),
                  ),
                ),
                const SizedBox(width: 12),
                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFE76F51),
                    foregroundColor: Colors.white,
                    padding: const EdgeInsets.all(16),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(14),
                    ),
                  ),
                  onPressed: () {
                    FocusScope.of(context).unfocus();
                    setState(() {});
                  },
                  child: const Icon(Icons.search),
                ),
              ],
            ),
          ),

          // FILTROS
          SingleChildScrollView(
            scrollDirection: Axis.horizontal,
            padding: const EdgeInsets.symmetric(horizontal: 16),
            child: Row(
              children: [
                FilterChip(
                  label: const Text("🌱 Veganos"),
                  selected: somenteVeganos,
                  selectedColor: Colors.green.shade100,
                  onSelected: (value) {
                    setState(() {
                      somenteVeganos = value;
                    });
                  },
                ),
                const SizedBox(width: 8),
                FilterChip(
                  label: const Text("🥛 Sem Lactose"),
                  selected: somenteSemLactose,
                  selectedColor: Colors.blue.shade100,
                  onSelected: (value) {
                    setState(() {
                      somenteSemLactose = value;
                    });
                  },
                ),
                const SizedBox(width: 8),
                FilterChip(
                  label: const Text("🌾 Sem Glúten"),
                  selected: somenteSemGluten,
                  selectedColor: Colors.amber.shade100,
                  onSelected: (value) {
                    setState(() {
                      somenteSemGluten = value;
                    });
                  },
                ),
                const SizedBox(width: 8),
                DropdownButton<Categoria>(
                  value: categoriaSelecionada,
                  hint: const Text("Categoria"),
                  items: categorias.map((categoria) {
                    return DropdownMenuItem(
                      value: categoria,
                      child: Text(categoria.descricao),
                    );
                  }).toList(),
                  onChanged: (Categoria? value) {
                    setState(() {
                      categoriaSelecionada = value;
                    });
                  },
                ),
                if (categoriaSelecionada != null)
                  IconButton(
                    icon: const Icon(Icons.clear, size: 18),
                    tooltip: "Limpar categoria",
                    onPressed: () {
                      setState(() {
                        categoriaSelecionada = null;
                      });
                    },
                  ),
              ],
            ),
          ),

          const SizedBox(height: 10),

          // LISTAGEM GERAL DE PRATOS
          Expanded(
            child: FutureBuilder<List<dynamic>>(
              future: pratosFuture,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(
                    child: CircularProgressIndicator(color: Color(0xFFE76F51)),
                  );
                }

                if (snapshot.hasError) {
                  return Center(
                    child: Text(
                      "Erro: ${snapshot.error}",
                      style: const TextStyle(color: Colors.red),
                    ),
                  );
                }

                if (!snapshot.hasData || snapshot.data!.isEmpty) {
                  return const Center(
                    child: Text(
                      "Nenhum prato cadastrado.",
                      style: TextStyle(fontSize: 18),
                    ),
                  );
                }

                List<dynamic> pratos = snapshot.data!;

                final busca = nomeController.text.trim().toLowerCase();
                if (busca.isNotEmpty) {
                  pratos = pratos.where((prato) {
                    final nome = prato['nome']?.toString().toLowerCase() ?? '';
                    return nome.contains(busca);
                  }).toList();
                }

                if (somenteVeganos) {
                  pratos = pratos
                      .where((prato) => prato['vegano'] == true)
                      .toList();
                }

                if (somenteSemLactose) {
                  pratos = pratos
                      .where((prato) => prato['leite'] != true)
                      .toList();
                }

                if (somenteSemGluten) {
                  pratos = pratos
                      .where((prato) => prato['trigo'] != true)
                      .toList();
                }

                if (categoriaSelecionada != null) {
                  pratos = pratos.where((prato) {
                    return prato['categoria'] != null &&
                        prato['categoria']['id'] == categoriaSelecionada!.id;
                  }).toList();
                }

                if (pratos.isEmpty) {
                  return Center(
                    child: Text(
                      busca.isEmpty
                          ? "Nenhum prato corresponde aos filtros."
                          : "Nenhum prato encontrado.",
                      style: TextStyle(color: Colors.grey.shade700),
                    ),
                  );
                }

                return ListView.builder(
                  padding: const EdgeInsets.all(16),
                  itemCount: pratos.length,
                  itemBuilder: (context, index) {
                    final prato = pratos[index];

                    return Card(
                      elevation: 8,
                      color: Colors.white,
                      margin: const EdgeInsets.only(bottom: 18),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(22),
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Stack(
                            children: [
                              ClipRRect(
                                borderRadius: const BorderRadius.vertical(
                                  top: Radius.circular(22),
                                ),
                                child: ImagemPrato(
                                  imagem: prato['imagem']?.toString(),
                                  height: 220,
                                  width: double.infinity,
                                  placeholderColor: Colors.orange.shade100,
                                  placeholderIcon: Icons.restaurant,
                                  placeholderIconColor: Colors.orange,
                                  placeholderIconSize: 70,
                                ),
                              ),

                              if (prato['nota'] != null)
                                Positioned(
                                  top: 12,
                                  right: 12,
                                  child: Container(
                                    padding: const EdgeInsets.symmetric(
                                      horizontal: 10,
                                      vertical: 6,
                                    ),
                                    decoration: BoxDecoration(
                                      color: Colors.white,
                                      borderRadius: BorderRadius.circular(20),
                                    ),
                                    child: Row(
                                      children: [
                                        const Icon(
                                          Icons.star,
                                          color: Colors.amber,
                                          size: 18,
                                        ),
                                        const SizedBox(width: 4),
                                        Text(
                                          prato['nota'].toString(),
                                          style: const TextStyle(
                                            fontWeight: FontWeight.bold,
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                ),
                            ],
                          ),

                          Padding(
                            padding: const EdgeInsets.all(18),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  prato['nome'] ?? "Sem nome",
                                  style: const TextStyle(
                                    fontSize: 22,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0xFF9C3D2A),
                                  ),
                                ),
                                const SizedBox(height: 10),

                                Text(
                                  prato['descricao'] ?? "Sem descrição.",
                                  style: TextStyle(
                                    color: Colors.grey.shade700,
                                    height: 1.5,
                                  ),
                                ),

                                const SizedBox(height: 12),

                                Row(
                                  children: [
                                    if (prato['categoria'] != null)
                                      Container(
                                        padding: const EdgeInsets.symmetric(
                                          horizontal: 12,
                                          vertical: 7,
                                        ),
                                        decoration: BoxDecoration(
                                          color: Colors.blue.shade50,
                                          borderRadius: BorderRadius.circular(
                                            20,
                                          ),
                                        ),
                                        child: Row(
                                          mainAxisSize: MainAxisSize.min,
                                          children: [
                                            const Icon(
                                              Icons.restaurant_menu,
                                              size: 18,
                                              color: Colors.blue,
                                            ),
                                            const SizedBox(width: 5),
                                            Text(
                                              prato['categoria']['descricao'] ??
                                                  "",
                                              style: const TextStyle(
                                                color: Colors.blue,
                                                fontWeight: FontWeight.bold,
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),

                                    const SizedBox(width: 8),

                                    if (prato['vegano'] == true)
                                      Container(
                                        padding: const EdgeInsets.symmetric(
                                          horizontal: 12,
                                          vertical: 7,
                                        ),
                                        decoration: BoxDecoration(
                                          color: Colors.green.shade100,
                                          borderRadius: BorderRadius.circular(
                                            20,
                                          ),
                                        ),
                                        child: Row(
                                          children: const [
                                            Icon(
                                              Icons.eco,
                                              color: Colors.green,
                                              size: 18,
                                            ),
                                            SizedBox(width: 5),
                                            Text(
                                              "Vegano",
                                              style: TextStyle(
                                                color: Colors.green,
                                                fontWeight: FontWeight.bold,
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),

                                    const Spacer(),

                                    Container(
                                      padding: const EdgeInsets.symmetric(
                                        horizontal: 12,
                                        vertical: 7,
                                      ),
                                      decoration: BoxDecoration(
                                        color: const Color(0xFFFFE8D6),
                                        borderRadius: BorderRadius.circular(20),
                                      ),
                                      child: Text(
                                        "ID ${prato['id']}",
                                        style: const TextStyle(
                                          color: Color(0xFFE76F51),
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ),
                                  ],
                                ),

                                // Alérgenos e rótulos
                                _buildTagsPrato(prato),

                                const SizedBox(height: 16),

                                Row(
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: [
                                    IconButton(
                                      onPressed: () {
                                        Navigator.push(
                                          context,
                                          MaterialPageRoute(
                                            builder: (context) => PratoPage(
                                              pratoId: prato['id'],
                                              usuario: widget.usuario,
                                            ),
                                          ),
                                        );
                                      },
                                      icon: const Icon(Icons.arrow_forward),
                                    ),

                                    if (widget.usuario.funcionario) ...[
                                      OutlinedButton.icon(
                                        icon: const Icon(Icons.edit),
                                        label: const Text("Editar"),
                                        onPressed: () async {
                                          final atualizado =
                                              await Navigator.push(
                                                context,
                                                MaterialPageRoute(
                                                  builder: (_) => AtualizaPrato(
                                                    prato: prato,
                                                    usuario: widget.usuario,
                                                  ),
                                                ),
                                              );
                                          if (atualizado == true && mounted) {
                                            setState(() {
                                              pratosFuture = listarPratos();
                                            });
                                          }
                                        },
                                      ),

                                      const SizedBox(width: 10),

                                      ElevatedButton.icon(
                                        style: ElevatedButton.styleFrom(
                                          backgroundColor: Colors.red,
                                          foregroundColor: Colors.white,
                                        ),
                                        icon: const Icon(Icons.delete),
                                        label: const Text("Excluir"),
                                        onPressed: () async {
                                          await confirmarExclusao(prato['id']);
                                        },
                                      ),
                                    ],
                                  ],
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                    );
                  },
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}
