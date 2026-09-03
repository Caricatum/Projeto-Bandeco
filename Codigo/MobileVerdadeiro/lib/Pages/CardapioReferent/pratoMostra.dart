import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../../Class/avaliacaoClass.dart';
import '../../Class/pratosClass.dart';
import '../../Class/usuarioClass.dart';

class PratoPage extends StatefulWidget {
  final int pratoId;
  final Usuario usuario;

  const PratoPage({super.key, required this.pratoId, required this.usuario});

  @override
  State<PratoPage> createState() => _PratoPageState();
}

class _PratoPageState extends State<PratoPage> {
  Pratos? prato;

  List<Avaliacao> avaliacoes = [];

  Avaliacao? minhaAvaliacao;

  bool favorito = false;
  int? idFavorito;

  bool carregando = true;
  bool enviandoAvaliacao = false;

  @override
  void initState() {
    super.initState();
    carregarDados();
  }

  // ============================================================
  // CARREGAMENTO INICIAL
  // ============================================================

  Future<void> carregarDados() async {
    await Future.wait([buscarPrato(), buscarAvaliacoes(), verificarFavorito()]);

    if (mounted) {
      setState(() {
        carregando = false;
      });
    }
  }

  // ============================================================
  // BUSCAR PRATO
  // ============================================================

  Future<void> buscarPrato() async {
    final url = Uri.parse('http://localhost:8080/pratos/id/${widget.pratoId}');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        if (mounted) {
          setState(() {
            prato = Pratos.fromJson(jsonDecode(response.body));
          });
        }
      } else {
        debugPrint('Erro ao buscar prato. Código: ${response.statusCode}');
      }
    } catch (e) {
      debugPrint('Erro ao buscar prato: $e');
    }
  }

  // ============================================================
  // BUSCAR AVALIAÇÕES DO PRATO
  // ============================================================

  Future<void> buscarAvaliacoes() async {
    final url = Uri.parse('http://localhost:8080/avaliacoes/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        final List<Avaliacao> lista = dados
            .map((json) => Avaliacao.fromJson(json))
            .where((avaliacao) => avaliacao.pratoId == widget.pratoId)
            .toList();

        Avaliacao? avaliacaoDoUsuario;

        for (final avaliacao in lista) {
          if (avaliacao.usuarioId == widget.usuario.id) {
            avaliacaoDoUsuario = avaliacao;
            break;
          }
        }

        if (mounted) {
          setState(() {
            avaliacoes = lista;
            minhaAvaliacao = avaliacaoDoUsuario;
          });
        }
      } else {
        debugPrint('Erro ao buscar avaliações. Código: ${response.statusCode}');
      }
    } catch (e) {
      debugPrint('Erro ao buscar avaliações: $e');
    }
  }

  // ============================================================
  // VERIFICAR FAVORITO
  // ============================================================

  Future<void> verificarFavorito() async {
    final url = Uri.parse('http://localhost:8080/pratosFavoritos/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        for (final item in dados) {
          final int pratoId = item['prato']['id'];
          final int usuarioId = item['user']['id'];

          if (pratoId == widget.pratoId && usuarioId == widget.usuario.id) {
            if (mounted) {
              setState(() {
                favorito = true;
                idFavorito = item['id'];
              });
            }

            break;
          }
        }
      }
    } catch (e) {
      debugPrint('Erro ao verificar favorito: $e');
    }
  }

  // ============================================================
  // ADICIONAR FAVORITO
  // ============================================================

  Future<void> adicionarFavorito() async {
    final url = Uri.parse('http://localhost:8080/pratosFavoritos/cadastrar');

    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'user': {'id': widget.usuario.id},
          'prato': {'id': widget.pratoId},
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        final dados = jsonDecode(response.body);

        if (mounted) {
          setState(() {
            favorito = true;
            idFavorito = dados['id'];
          });

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Prato adicionado aos favoritos!')),
          );
        }
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Não foi possível adicionar aos favoritos.'),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao adicionar favorito: $e');
    }
  }

  // ============================================================
  // REMOVER FAVORITO
  // ============================================================

  Future<void> removerFavorito() async {
    if (idFavorito == null) {
      return;
    }

    final url = Uri.parse(
      'http://localhost:8080/pratosFavoritos/deletar/$idFavorito',
    );

    try {
      final response = await http.delete(url);

      if (response.statusCode == 200 || response.statusCode == 204) {
        if (mounted) {
          setState(() {
            favorito = false;
            idFavorito = null;
          });

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Prato removido dos favoritos.')),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao remover favorito: $e');
    }
  }

  // ============================================================
  // ALTERNAR FAVORITO
  // ============================================================

  Future<void> alternarFavorito() async {
    if (favorito) {
      await removerFavorito();
    } else {
      await adicionarFavorito();
    }
  }

  // ============================================================
  // CADASTRAR AVALIAÇÃO
  // ============================================================

  Future<void> cadastrarAvaliacao(int nota, String comentario) async {
    final url = Uri.parse('http://localhost:8080/avaliacoes/cadastrar');

    try {
      setState(() {
        enviandoAvaliacao = true;
      });

      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'nota': nota,
          'avaliacao': comentario.trim().isEmpty ? null : comentario.trim(),
          'user': {'id': widget.usuario.id},
          'prato': {'id': widget.pratoId},
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Avaliação enviada com sucesso!')),
          );
        }

        await buscarAvaliacoes();
        await buscarPrato();
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Não foi possível enviar a avaliação. '
                'Código: ${response.statusCode}',
              ),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao cadastrar avaliação: $e');

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Erro de conexão com o servidor.')),
        );
      }
    } finally {
      if (mounted) {
        setState(() {
          enviandoAvaliacao = false;
        });
      }
    }
  }

  // ============================================================
  // ATUALIZAR AVALIAÇÃO
  // ============================================================

  Future<void> atualizarAvaliacao(
    Avaliacao avaliacao,
    int nota,
    String comentario,
  ) async {
    final url = Uri.parse('http://localhost:8080/avaliacoes/atualizar');

    try {
      setState(() {
        enviandoAvaliacao = true;
      });

      final response = await http.put(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'id': avaliacao.id,
          'nota': nota,
          'avaliacao': comentario.trim().isEmpty ? null : comentario.trim(),
          'user': {'id': widget.usuario.id},
          'prato': {'id': widget.pratoId},
        }),
      );

      if (response.statusCode == 200) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Avaliação atualizada com sucesso!')),
          );
        }

        await buscarAvaliacoes();
        await buscarPrato();
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Não foi possível atualizar a avaliação. '
                'Código: ${response.statusCode}',
              ),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao atualizar avaliação: $e');
    } finally {
      if (mounted) {
        setState(() {
          enviandoAvaliacao = false;
        });
      }
    }
  }

  // ============================================================
  // EXCLUIR AVALIAÇÃO
  // ============================================================

  Future<void> excluirAvaliacao(Avaliacao avaliacao) async {
    final url = Uri.parse(
      'http://localhost:8080/avaliacoes/deletar/${avaliacao.id}',
    );

    try {
      final response = await http.delete(url);

      if (response.statusCode == 200 || response.statusCode == 204) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Avaliação excluída com sucesso.')),
          );
        }

        await buscarAvaliacoes();
        await buscarPrato();
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Não foi possível excluir a avaliação. '
                'Código: ${response.statusCode}',
              ),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao excluir avaliação: $e');
    }
  }

  // ============================================================
  // CONFIRMAR EXCLUSÃO
  // ============================================================

  void confirmarExclusao(Avaliacao avaliacao) {
    showDialog(
      context: context,
      builder: (dialogContext) {
        return AlertDialog(
          title: const Text('Excluir avaliação'),
          content: const Text('Tem certeza que deseja excluir sua avaliação?'),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.pop(dialogContext);
              },
              child: const Text('Cancelar'),
            ),
            ElevatedButton(
              onPressed: () async {
                Navigator.pop(dialogContext);

                await excluirAvaliacao(avaliacao);
              },
              child: const Text('Excluir'),
            ),
          ],
        );
      },
    );
  }

  // ============================================================
  // ABRIR FORMULÁRIO DE AVALIAÇÃO
  // ============================================================

  void abrirDialogAvaliacao({Avaliacao? avaliacaoExistente}) {
    int nota = avaliacaoExistente?.nota ?? 5;

    final comentarioController = TextEditingController(
      text: avaliacaoExistente?.avaliacao ?? '',
    );

    final bool editando = avaliacaoExistente != null;

    showDialog(
      context: context,
      builder: (dialogContext) {
        return StatefulBuilder(
          builder: (context, setDialogState) {
            return AlertDialog(
              title: Text(editando ? 'Editar avaliação' : 'Avaliar prato'),
              content: SingleChildScrollView(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Text('Escolha sua nota:'),

                    const SizedBox(height: 8),

                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: List.generate(5, (index) {
                        final valor = index + 1;

                        return IconButton(
                          onPressed: () {
                            setDialogState(() {
                              nota = valor;
                            });
                          },
                          icon: Icon(
                            valor <= nota ? Icons.star : Icons.star_border,
                            color: Colors.amber,
                            size: 32,
                          ),
                        );
                      }),
                    ),

                    const SizedBox(height: 10),

                    TextField(
                      controller: comentarioController,
                      maxLines: 4,
                      decoration: const InputDecoration(
                        labelText: 'Comentário',
                        hintText: 'O que você achou do prato?',
                        border: OutlineInputBorder(),
                      ),
                    ),
                  ],
                ),
              ),
              actions: [
                TextButton(
                  onPressed: () {
                    Navigator.pop(dialogContext);
                  },
                  child: const Text('Cancelar'),
                ),

                ElevatedButton(
                  onPressed: () async {
                    Navigator.pop(dialogContext);

                    if (editando) {
                      await atualizarAvaliacao(
                        avaliacaoExistente,
                        nota,
                        comentarioController.text,
                      );
                    } else {
                      await cadastrarAvaliacao(nota, comentarioController.text);
                    }
                  },
                  child: Text(editando ? 'Salvar' : 'Enviar'),
                ),
              ],
            );
          },
        );
      },
    ).then((_) {
      comentarioController.dispose();
    });
  }

  // ============================================================
  // CALCULAR MÉDIA
  // ============================================================

  double calcularMedia() {
    if (avaliacoes.isEmpty) {
      return 0;
    }

    int soma = 0;

    for (final avaliacao in avaliacoes) {
      soma += avaliacao.nota;
    }

    return soma / avaliacoes.length;
  }

  // ============================================================
  // BUILD
  // ============================================================

  @override
  Widget build(BuildContext context) {
    if (carregando) {
      return const Scaffold(body: Center(child: CircularProgressIndicator()));
    }

    if (prato == null) {
      return Scaffold(
        appBar: AppBar(title: const Text('Detalhes do prato')),
        body: const Center(child: Text('Não foi possível carregar o prato.')),
      );
    }

    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context);
          },
        ),
        title: const Text('Detalhes do prato'),
      ),
      body: _buildBody(),
    );
  }

  // ============================================================
  // BODY
  // ============================================================

  Widget _buildBody() {
    return RefreshIndicator(
      onRefresh: carregarDados,
      child: SingleChildScrollView(
        physics: const AlwaysScrollableScrollPhysics(),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildImagem(),

            Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildTitulo(),

                  const SizedBox(height: 10),

                  _buildDescricao(),

                  const SizedBox(height: 16),

                  _buildDescricaoIA(),

                  const SizedBox(height: 16),

                  _buildNota(),

                  const SizedBox(height: 16),

                  _buildFavorito(),

                  const SizedBox(height: 24),

                  _buildAvaliacaoUsuario(),

                  const SizedBox(height: 24),

                  _buildComentarios(),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // IMAGEM
  // ============================================================

  Widget _buildImagem() {
    if (prato!.imagem == null || prato!.imagem!.isEmpty) {
      return Container(
        height: 250,
        width: double.infinity,
        color: Colors.grey[300],
        child: const Icon(Icons.restaurant, size: 80),
      );
    }

    return Image.network(
      prato!.imagem!,
      height: 250,
      width: double.infinity,
      fit: BoxFit.cover,
      errorBuilder: (context, error, stackTrace) {
        return Container(
          height: 250,
          width: double.infinity,
          color: Colors.grey[300],
          child: const Icon(Icons.broken_image, size: 80),
        );
      },
    );
  }

  // ============================================================
  // TÍTULO
  // ============================================================

  Widget _buildTitulo() {
    return Row(
      children: [
        Expanded(
          child: Text(
            prato!.nome,
            style: const TextStyle(fontSize: 26, fontWeight: FontWeight.bold),
          ),
        ),

        if (prato!.vegano)
          const Tooltip(
            message: 'Prato vegano',
            child: Icon(Icons.eco, color: Colors.green, size: 28),
          ),
      ],
    );
  }

  // ============================================================
  // DESCRIÇÃO
  // ============================================================

  Widget _buildDescricao() {
    return Text(prato!.descricao, style: const TextStyle(fontSize: 16));
  }

  // ============================================================
  // DESCRIÇÃO DA IA
  // ============================================================

  Widget _buildDescricaoIA() {
    if (prato!.descricaoIA == null || prato!.descricaoIA!.trim().isEmpty) {
      return const SizedBox.shrink();
    }

    return Card(
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: const [
                Icon(Icons.auto_awesome),
                SizedBox(width: 8),
                Text(
                  'Opinião geral',
                  style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
                ),
              ],
            ),

            const SizedBox(height: 10),

            Text(prato!.descricaoIA!, style: const TextStyle(fontSize: 16)),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // NOTA
  // ============================================================

  Widget _buildNota() {
    final media = calcularMedia();

    return Row(
      children: [
        const Icon(Icons.star, color: Colors.amber, size: 30),

        const SizedBox(width: 8),

        Text(
          media.toStringAsFixed(1),
          style: const TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
        ),

        const SizedBox(width: 8),

        Text(
          '(${avaliacoes.length} avaliações)',
          style: TextStyle(fontSize: 15, color: Colors.grey[600]),
        ),
      ],
    );
  }

  // ============================================================
  // FAVORITO
  // ============================================================

  Widget _buildFavorito() {
    return SizedBox(
      width: double.infinity,
      child: OutlinedButton.icon(
        onPressed: alternarFavorito,
        icon: Icon(
          favorito ? Icons.favorite : Icons.favorite_border,
          color: favorito ? Colors.red : null,
        ),
        label: Text(
          favorito ? 'Remover dos favoritos' : 'Adicionar aos favoritos',
        ),
      ),
    );
  }

  // ============================================================
  // MINHA AVALIAÇÃO / BOTÃO AVALIAR
  // ============================================================

  Widget _buildAvaliacaoUsuario() {
    if (minhaAvaliacao == null) {
      return SizedBox(
        width: double.infinity,
        child: ElevatedButton.icon(
          onPressed: enviandoAvaliacao
              ? null
              : () {
                  abrirDialogAvaliacao();
                },
          icon: const Icon(Icons.star),
          label: Text(enviandoAvaliacao ? 'Enviando...' : 'Avaliar prato'),
        ),
      );
    }

    return _buildMinhaAvaliacao();
  }

  // ============================================================
  // MINHA AVALIAÇÃO
  // ============================================================

  Widget _buildMinhaAvaliacao() {
    final avaliacao = minhaAvaliacao!;

    return Card(
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Sua avaliação',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 8),

            Row(
              children: List.generate(5, (index) {
                return Icon(
                  index < avaliacao.nota ? Icons.star : Icons.star_border,
                  color: Colors.amber,
                );
              }),
            ),

            if (avaliacao.avaliacao != null &&
                avaliacao.avaliacao!.trim().isNotEmpty)
              Padding(
                padding: const EdgeInsets.only(top: 10),
                child: Text(
                  avaliacao.avaliacao!,
                  style: const TextStyle(fontSize: 15),
                ),
              ),

            const SizedBox(height: 12),

            Row(
              children: [
                Expanded(
                  child: OutlinedButton.icon(
                    onPressed: enviandoAvaliacao
                        ? null
                        : () {
                            abrirDialogAvaliacao(avaliacaoExistente: avaliacao);
                          },
                    icon: const Icon(Icons.edit),
                    label: const Text('Editar'),
                  ),
                ),

                const SizedBox(width: 10),

                Expanded(
                  child: OutlinedButton.icon(
                    onPressed: enviandoAvaliacao
                        ? null
                        : () {
                            confirmarExclusao(avaliacao);
                          },
                    icon: const Icon(Icons.delete_outline),
                    label: const Text('Excluir'),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // COMENTÁRIOS
  // ============================================================

  Widget _buildComentarios() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'Comentários',
          style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
        ),

        const SizedBox(height: 12),

        if (avaliacoes.isEmpty)
          const Text('Ainda não existem avaliações para este prato.'),

        ...avaliacoes.map((avaliacao) => _buildComentario(avaliacao)),
      ],
    );
  }

  // ============================================================
  // COMENTÁRIO INDIVIDUAL
  // ============================================================

  Widget _buildComentario(Avaliacao avaliacao) {
    final bool ehMinha = avaliacao.usuarioId == widget.usuario.id;

    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      child: Padding(
        padding: const EdgeInsets.all(14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                const CircleAvatar(child: Icon(Icons.person)),

                const SizedBox(width: 10),

                Expanded(
                  child: Text(
                    ehMinha ? 'Você' : avaliacao.nomeUsuario,
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 8),

            Row(
              children: List.generate(5, (index) {
                return Icon(
                  index < avaliacao.nota ? Icons.star : Icons.star_border,
                  size: 18,
                  color: Colors.amber,
                );
              }),
            ),

            if (avaliacao.avaliacao != null &&
                avaliacao.avaliacao!.trim().isNotEmpty)
              Padding(
                padding: const EdgeInsets.only(top: 10),
                child: Text(
                  avaliacao.avaliacao!,
                  style: const TextStyle(fontSize: 15),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
