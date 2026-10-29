import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../../Class/pratosClass.dart';
import '../../Class/usuarioClass.dart';
import '../CardapioReferent/pratoMostra.dart';

class Favoritos extends StatefulWidget {
  final Usuario usuario;

  const Favoritos({
    super.key,
    required this.usuario,
  });

  @override
  State<Favoritos> createState() => _FavoritosPageState();
}

class _FavoritosPageState extends State<Favoritos> {

  // ============================================================
  // VARIÁVEIS
  // ============================================================

  List<Pratos> favoritos = [];

  bool carregando = true;


  // ============================================================
  // FUNÇÕES
  // ============================================================

  @override
  void initState() {
    super.initState();

    buscarFavoritos();
  }


  Future<void> buscarFavoritos() async {

    final url = Uri.parse(
      'http://localhost:8080/pratosFavoritos/all',
    );

    try {

      final response = await http.get(url);

      if (response.statusCode == 200) {

        final List<dynamic> dados =
            jsonDecode(response.body);

        final List<Pratos> lista = [];

        for (final item in dados) {

          final usuarioId =
              item['user']['id'];

          if (usuarioId ==
              widget.usuario.id) {

            final prato =
                Pratos.fromJson(item['prato']);

            lista.add(prato);
          }
        }

        if (mounted) {
          setState(() {

            favoritos = lista;

            carregando = false;

          });
        }

      } else {

        if (mounted) {
          setState(() {
            carregando = false;
          });
        }
      }

    } catch (e) {

      debugPrint(
        'Erro ao buscar favoritos: $e',
      );

      if (mounted) {
        setState(() {
          carregando = false;
        });
      }
    }
  }


  Future<void> removerFavorito(Pratos prato) async {

    final url =
        Uri.parse(
      'http://localhost:8080/pratosFavoritos/all',
    );

    try {

      final response =
          await http.get(url);

      if (response.statusCode != 200) {
        return;
      }

      final List<dynamic> dados =
          jsonDecode(response.body);

      int? idFavorito;

      for (final item in dados) {

        if (item['user']['id'] ==
                widget.usuario.id &&
            item['prato']['id'] ==
                prato.id) {

          idFavorito = item['id'];

          break;
        }
      }

      if (idFavorito == null) {
        return;
      }


      final deleteUrl =
          Uri.parse(
        'http://localhost:8080/pratosFavoritos/deletar/$idFavorito',
      );

      final deleteResponse =
          await http.delete(deleteUrl);

      if (deleteResponse.statusCode == 200 ||
          deleteResponse.statusCode == 204) {

        if (mounted) {

          setState(() {

            favoritos.removeWhere(
              (item) => item.id == prato.id,
            );

          });

          ScaffoldMessenger.of(context)
              .showSnackBar(
            const SnackBar(
              content: Text(
                'Prato removido dos favoritos.',
              ),
            ),
          );
        }
      }

    } catch (e) {

      debugPrint(
        'Erro ao remover favorito: $e',
      );
    }
  }


  void abrirPrato(Pratos prato) {

    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => PratoPage(
          pratoId: prato.id,
          usuario:
              widget.usuario,
        ),
      ),
    ).then((_) {

      buscarFavoritos();

    });
  }


  // ============================================================
  // BODY
  // ============================================================

  @override
  Widget build(BuildContext context) {

    return Scaffold(

      appBar: AppBar(
        title: const Text(
          'Favoritos',
        ),
      ),

      body: _buildBody(),
    );
  }


  Widget _buildBody() {

    if (carregando) {

      return const Center(
        child: CircularProgressIndicator(),
      );
    }


    if (favoritos.isEmpty) {

      return _buildVazio();
    }


    return RefreshIndicator(

      onRefresh: buscarFavoritos,

      child: ListView.builder(

        padding:
            const EdgeInsets.all(12),

        itemCount:
            favoritos.length,

        itemBuilder:
            (context, index) {

          final prato =
              favoritos[index];

          return _buildPrato(prato);
        },
      ),
    );
  }


  Widget _buildVazio() {

    return Center(

      child: Padding(

        padding:
            const EdgeInsets.all(24),

        child: Column(

          mainAxisAlignment:
              MainAxisAlignment.center,

          children: [

            Icon(

              Icons.favorite_border,

              size: 80,

              color: Colors.grey[400],
            ),

            const SizedBox(
              height: 16,
            ),

            const Text(

              'Você ainda não possui pratos favoritos.',

              textAlign:
                  TextAlign.center,

              style: TextStyle(

                fontSize: 18,

                fontWeight:
                    FontWeight.bold,
              ),
            ),

            const SizedBox(
              height: 8,
            ),

            Text(

              'Adicione pratos aos favoritos para encontrá-los aqui.',

              textAlign:
                  TextAlign.center,

              style: TextStyle(

                fontSize: 15,

                color: Colors.grey[600],
              ),
            ),
          ],
        ),
      ),
    );
  }


  Widget _buildPrato(Pratos prato) {

    return Card(

      margin:
          const EdgeInsets.only(
        bottom: 12,
      ),

      clipBehavior:
          Clip.antiAlias,

      child: InkWell(

        onTap: () {
          abrirPrato(prato);
        },

        child: Row(

          crossAxisAlignment:
              CrossAxisAlignment.start,

          children: [

            _buildImagem(prato),

            Expanded(

              child: Padding(

                padding:
                    const EdgeInsets.all(12),

                child: Column(

                  crossAxisAlignment:
                      CrossAxisAlignment.start,

                  children: [

                    Row(

                      children: [

                        Expanded(

                          child: Text(

                            prato.nome,

                            style:
                                const TextStyle(

                              fontSize: 18,

                              fontWeight:
                                  FontWeight.bold,
                            ),
                          ),
                        ),

                        IconButton(

                          onPressed: () {
                            removerFavorito(
                              prato,
                            );
                          },

                          icon: const Icon(
                            Icons.favorite,
                            color: Colors.red,
                          ),

                          tooltip:
                              'Remover dos favoritos',
                        ),
                      ],
                    ),

                    const SizedBox(
                      height: 6,
                    ),

                    Text(

                      prato.descricao,

                      maxLines: 2,

                      overflow:
                          TextOverflow.ellipsis,

                      style: TextStyle(

                        fontSize: 14,

                        color:
                            Colors.grey[700],
                      ),
                    ),

                    const SizedBox(
                      height: 8,
                    ),

                    Wrap(
                      spacing: 6,
                      runSpacing: 4,
                      children: [
                        if (prato.vegano)
                          Container(
                            padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 2),
                            decoration: BoxDecoration(
                              color: Colors.green.shade100,
                              borderRadius: BorderRadius.circular(8),
                            ),
                            child: Row(
                              mainAxisSize: MainAxisSize.min,
                              children: const [
                                Icon(Icons.eco, size: 14, color: Colors.green),
                                SizedBox(width: 4),
                                Text('Vegano', style: TextStyle(fontSize: 11, color: Colors.green, fontWeight: FontWeight.bold)),
                              ],
                            ),
                          ),
                        if (prato.leite)
                          _chipBadge('🥛 Leite', const Color(0xFFE0F2FE), const Color(0xFF0369A1)),
                        if (prato.ovo)
                          _chipBadge('🥚 Ovo', const Color(0xFFFEF3C7), const Color(0xFFB45309)),
                        if (prato.amendoim)
                          _chipBadge('🥜 Amendoim', const Color(0xFFFEF3C7), const Color(0xFF92400E)),
                        if (prato.castanhas)
                          _chipBadge('🌰 Castanhas', const Color(0xFFFFEDD5), const Color(0xFFC2410C)),
                        if (prato.frutosDoMar)
                          _chipBadge('🦐 Frutos Mar', const Color(0xFFCFFAFE), const Color(0xFF0E7490)),
                        if (prato.peixes)
                          _chipBadge('🐟 Peixes', const Color(0xFFE0F2FE), const Color(0xFF0284C7)),
                        if (prato.trigo)
                          _chipBadge('🌾 Glúten', const Color(0xFFFEF9C3), const Color(0xFF854D0E)),
                        if (prato.soja)
                          _chipBadge('🌱 Soja', const Color(0xFFDCFCE7), const Color(0xFF15803D)),
                        if (prato.gergilim)
                          _chipBadge('⚪ Gergelim', const Color(0xFFF3F4F6), const Color(0xFF374151)),
                        if (prato.altoAcucarAdicionado)
                          _chipBadge('⚠️ Açúcar', const Color(0xFF18181B), const Color(0xFFFACC15)),
                        if (prato.altoGorduraSaturada)
                          _chipBadge('⚠️ Gord. Sat.', const Color(0xFF18181B), const Color(0xFFFACC15)),
                        if (prato.altoSodio)
                          _chipBadge('⚠️ Sódio', const Color(0xFF18181B), const Color(0xFFFACC15)),
                      ],
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _chipBadge(String text, Color bg, Color textCol) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 2),
      decoration: BoxDecoration(
        color: bg,
        borderRadius: BorderRadius.circular(8),
      ),
      child: Text(
        text,
        style: TextStyle(fontSize: 10, color: textCol, fontWeight: FontWeight.bold),
      ),
    );
  }


  Widget _buildImagem(Pratos prato) {

    if (prato.imagem == null ||
        prato.imagem!.isEmpty) {

      return Container(

        width: 120,

        height: 130,

        color: Colors.grey[300],

        child: const Icon(

          Icons.restaurant,

          size: 45,
        ),
      );
    }


    return Image.network(

      prato.imagem!,

      width: 120,

      height: 130,

      fit: BoxFit.cover,

      errorBuilder:
          (context, error, stackTrace) {

        return Container(

          width: 120,

          height: 130,

          color: Colors.grey[300],

          child: const Icon(

            Icons.broken_image,

            size: 45,
          ),
        );
      },
    );
  }
}