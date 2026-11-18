import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../../Class/pratosClass.dart';
import '../../Class/usuarioClass.dart';
import '../../Widgets/imagemPrato.dart';
import '../CardapioReferent/pratoMostra.dart';
import '../../Services/notificacaoService.dart';

class Favoritos extends StatefulWidget {
  final Usuario usuario;

  const Favoritos({super.key, required this.usuario});

  @override
  State<Favoritos> createState() => _FavoritosPageState();
}

class _FavoritosPageState extends State<Favoritos> {
  // ============================================================
  // VARIÁVEIS
  // ============================================================

  List<Pratos> favoritos = [];
  Set<int> idsNoCardapioHoje = {};

  bool carregando = true;

  // ============================================================
  // FUNÇÕES
  // ============================================================

  @override
  void initState() {
    super.initState();

    buscarFavoritos();
    verificarCardapioHoje();
  }

  Future<void> verificarCardapioHoje() async {
    final matches = await NotificacaoService.instance
        .verificarCardapioHojeEFavoritos(usuarioId: widget.usuario.id);
    if (mounted) {
      final Set<int> ids = {};
      for (final m in matches) {
        final p = m['prato'] as Pratos;
        ids.add(p.id);
      }
      setState(() {
        idsNoCardapioHoje = ids;
      });
    }
  }

  Future<void> buscarFavoritos() async {
    final url = Uri.parse('http://localhost:8080/pratosFavoritos');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        final List<Pratos> lista = [];

        for (final item in dados) {
          final usuarioId = item['user']['id'];

          if (usuarioId == widget.usuario.id) {
            final prato = Pratos.fromJson(item['prato']);

            lista.add(prato);
          }
        }

        if (mounted) {
          setState(() {
            favoritos = lista;

            carregando = false;
          });

          verificarCardapioHoje();
        }
      } else {
        if (mounted) {
          setState(() {
            carregando = false;
          });
        }
      }
    } catch (e) {
      debugPrint('Erro ao buscar favoritos: $e');

      if (mounted) {
        setState(() {
          carregando = false;
        });
      }
    }
  }

  Future<void> removerFavorito(Pratos prato) async {
    final url = Uri.parse('http://localhost:8080/pratosFavoritos');

    try {
      final response = await http.get(url);

      if (response.statusCode != 200) {
        return;
      }

      final List<dynamic> dados = jsonDecode(response.body);

      int? idFavorito;

      for (final item in dados) {
        if (item['user']['id'] == widget.usuario.id &&
            item['prato']['id'] == prato.id) {
          idFavorito = item['id'];

          break;
        }
      }

      if (idFavorito == null) {
        return;
      }

      final deleteUrl = Uri.parse(
        'http://localhost:8080/pratosFavoritos/$idFavorito',
      );

      final deleteResponse = await http.delete(deleteUrl);

      if (deleteResponse.statusCode == 200 ||
          deleteResponse.statusCode == 204) {
        if (mounted) {
          setState(() {
            favoritos.removeWhere((item) => item.id == prato.id);
          });

          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: const Text('Prato removido dos favoritos.'),
              backgroundColor: const Color(0xFFD84315),
              behavior: SnackBarBehavior.floating,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(10),
              ),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao remover favorito: $e');
    }
  }

  void abrirPrato(Pratos prato) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) =>
            PratoPage(pratoId: prato.id, usuario: widget.usuario),
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
      backgroundColor: const Color(0xFFFFF9F5),
      appBar: AppBar(
        title: const Text(
          'Favoritos',
          style: TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.bold,
          ),
        ),
        centerTitle: true,
        flexibleSpace: Container(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              colors: [Color(0xFFD84315), Color(0xFFFF6F00)],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
        ),
        iconTheme: const IconThemeData(color: Colors.white),
        elevation: 2,
        shadowColor: const Color(0x40D84315),
      ),
      body: _buildBody(),
    );
  }

  Widget _buildBody() {
    if (carregando) {
      return const Center(
        child: CircularProgressIndicator(
          color: Color(0xFFD84315),
        ),
      );
    }

    if (favoritos.isEmpty) {
      return _buildVazio();
    }

    return RefreshIndicator(
      color: const Color(0xFFD84315),
      backgroundColor: Colors.white,
      onRefresh: buscarFavoritos,
      child: ListView.builder(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 16),
        itemCount: favoritos.length,
        itemBuilder: (context, index) {
          final prato = favoritos[index];

          return _buildPrato(prato);
        },
      ),
    );
  }

  Widget _buildVazio() {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              padding: const EdgeInsets.all(24),
              decoration: const BoxDecoration(
                color: Color(0xFFFFEBE5),
                shape: BoxShape.circle,
                boxShadow: [
                  BoxShadow(
                    color: Color(0x20D84315),
                    blurRadius: 16,
                    offset: Offset(0, 4),
                  ),
                ],
              ),
              child: const Icon(
                Icons.favorite_rounded,
                size: 64,
                color: Color(0xFFFF7043),
              ),
            ),
            const SizedBox(height: 24),
            const Text(
              'Você ainda não possui pratos favoritos.',
              textAlign: TextAlign.center,
              style: TextStyle(
                fontSize: 18,
                fontWeight: FontWeight.bold,
                color: Color(0xFF3E2723),
              ),
            ),
            const SizedBox(height: 8),
            const Text(
              'Adicione pratos aos favoritos para encontrá-los aqui.',
              textAlign: TextAlign.center,
              style: TextStyle(
                fontSize: 15,
                color: Color(0xFF795548),
                height: 1.4,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPrato(Pratos prato) {
    return Card(
      margin: const EdgeInsets.only(bottom: 14),
      elevation: 2,
      shadowColor: const Color(0x26D84315),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
        side: const BorderSide(color: Color(0xFFFFE0B2), width: 0.8),
      ),
      clipBehavior: Clip.antiAlias,
      child: InkWell(
        onTap: () {
          abrirPrato(prato);
        },
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildImagem(prato),
            Expanded(
              child: Padding(
                padding: const EdgeInsets.all(12),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Expanded(
                          child: Text(
                            prato.nome,
                            style: const TextStyle(
                              fontSize: 17,
                              fontWeight: FontWeight.bold,
                              color: Color(0xFF3E2723),
                            ),
                          ),
                        ),
                        Material(
                          color: const Color(0xFFFFEBEE),
                          shape: const CircleBorder(),
                          child: InkWell(
                            customBorder: const CircleBorder(),
                            onTap: () {
                              removerFavorito(prato);
                            },
                            child: const Padding(
                              padding: EdgeInsets.all(6),
                              child: Icon(
                                Icons.favorite,
                                color: Color(0xFFE53935),
                                size: 20,
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 6),
                    Text(
                      prato.descricao,
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
                      style: const TextStyle(
                        fontSize: 13,
                        color: Color(0xFF5D4037),
                        height: 1.3,
                      ),
                    ),
                    const SizedBox(height: 10),
                    Wrap(
                      spacing: 6,
                      runSpacing: 4,
                      children: [
                        if (idsNoCardapioHoje.contains(prato.id))
                          Container(
                            padding: const EdgeInsets.symmetric(
                              horizontal: 8,
                              vertical: 3,
                            ),
                            decoration: BoxDecoration(
                              color: const Color(0xFFFFF3E0),
                              borderRadius: BorderRadius.circular(8),
                              border: Border.all(
                                color: const Color(0xFFFFB74D),
                              ),
                            ),
                            child: const Row(
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                Icon(
                                  Icons.restaurant,
                                  size: 13,
                                  color: Color(0xFFE65100),
                                ),
                                SizedBox(width: 4),
                                Text(
                                  'No cardápio de hoje!',
                                  style: TextStyle(
                                    fontSize: 11,
                                    color: Color(0xFFE65100),
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        if (prato.vegano)
                          Container(
                            padding: const EdgeInsets.symmetric(
                              horizontal: 6,
                              vertical: 2,
                            ),
                            decoration: BoxDecoration(
                              color: Colors.green.shade100,
                              borderRadius: BorderRadius.circular(8),
                            ),
                            child: Row(
                              mainAxisSize: MainAxisSize.min,
                              children: const [
                                Icon(Icons.eco, size: 14, color: Colors.green),
                                SizedBox(width: 4),
                                Text(
                                  'Vegano',
                                  style: TextStyle(
                                    fontSize: 11,
                                    color: Colors.green,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        if (prato.leite)
                          _chipBadge(
                            '🥛 Leite',
                            const Color(0xFFE0F2FE),
                            const Color(0xFF0369A1),
                          ),
                        if (prato.ovo)
                          _chipBadge(
                            '🥚 Ovo',
                            const Color(0xFFFEF3C7),
                            const Color(0xFFB45309),
                          ),
                        if (prato.amendoim)
                          _chipBadge(
                            '🥜 Amendoim',
                            const Color(0xFFFEF3C7),
                            const Color(0xFF92400E),
                          ),
                        if (prato.castanhas)
                          _chipBadge(
                            '🌰 Castanhas',
                            const Color(0xFFFFEDD5),
                            const Color(0xFFC2410C),
                          ),
                        if (prato.frutosDoMar)
                          _chipBadge(
                            '🦐 Frutos Mar',
                            const Color(0xFFCFFAFE),
                            const Color(0xFF0E7490),
                          ),
                        if (prato.peixes)
                          _chipBadge(
                            '🐟 Peixes',
                            const Color(0xFFE0F2FE),
                            const Color(0xFF0284C7),
                          ),
                        if (prato.trigo)
                          _chipBadge(
                            '🌾 Glúten',
                            const Color(0xFFFEF9C3),
                            const Color(0xFF854D0E),
                          ),
                        if (prato.soja)
                          _chipBadge(
                            '🌱 Soja',
                            const Color(0xFFDCFCE7),
                            const Color(0xFF15803D),
                          ),
                        if (prato.gergilim)
                          _chipBadge(
                            '⚪ Gergelim',
                            const Color(0xFFF3F4F6),
                            const Color(0xFF374151),
                          ),
                        if (prato.altoAcucarAdicionado)
                          _chipBadge(
                            '⚠️ Açúcar',
                            const Color(0xFF18181B),
                            const Color(0xFFFACC15),
                          ),
                        if (prato.altoGorduraSaturada)
                          _chipBadge(
                            '⚠️ Gord. Sat.',
                            const Color(0xFF18181B),
                            const Color(0xFFFACC15),
                          ),
                        if (prato.altoSodio)
                          _chipBadge(
                            '⚠️ Sódio',
                            const Color(0xFF18181B),
                            const Color(0xFFFACC15),
                          ),
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
        style: TextStyle(
          fontSize: 10,
          color: textCol,
          fontWeight: FontWeight.bold,
        ),
      ),
    );
  }

  Widget _buildImagem(Pratos prato) {
    return ImagemPrato(
      imagem: prato.imagem,
      width: 120,
      height: 130,
      placeholderColor: const Color(0xFFFFF3E0),
      placeholderIcon: Icons.restaurant,
      placeholderIconColor: const Color(0xFFFF8A65),
      placeholderIconSize: 45,
    );
  }
}
