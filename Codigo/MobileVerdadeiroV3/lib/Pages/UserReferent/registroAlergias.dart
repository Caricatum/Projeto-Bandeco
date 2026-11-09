import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:http_parser/http_parser.dart';

import '../../Class/usuarioClass.dart';

class RegistroAlergias extends StatefulWidget {
  final Usuario usuario;

  const RegistroAlergias({super.key, required this.usuario});

  @override
  State<RegistroAlergias> createState() => _RegistroAlergiasState();
}

class _RegistroAlergiasState extends State<RegistroAlergias> {
  late bool alergiaLeite;
  late bool alergiaOvo;
  late bool alergiaAmendoim;
  late bool alergiaCastanhas;
  late bool alergiaFrutosDoMar;
  late bool alergiaPeixes;
  late bool alergiaTrigo;
  late bool alergiaSoja;
  late bool alergiaGergilim;

  bool salvando = false;

  @override
  void initState() {
    super.initState();
    final u = widget.usuario;
    alergiaLeite = u.alergiaLeite;
    alergiaOvo = u.alergiaOvo;
    alergiaAmendoim = u.alergiaAmendoim;
    alergiaCastanhas = u.alergiaCastanhas;
    alergiaFrutosDoMar = u.alergiaFrutosDoMar;
    alergiaPeixes = u.alergiaPeixes;
    alergiaTrigo = u.alergiaTrigo;
    alergiaSoja = u.alergiaSoja;
    alergiaGergilim = u.alergiaGergilim;
  }

  void alterarTodos(bool valor) {
    setState(() {
      alergiaLeite = valor;
      alergiaOvo = valor;
      alergiaAmendoim = valor;
      alergiaCastanhas = valor;
      alergiaFrutosDoMar = valor;
      alergiaPeixes = valor;
      alergiaTrigo = valor;
      alergiaSoja = valor;
      alergiaGergilim = valor;
    });
  }

  Future<void> salvarAlergias() async {
    setState(() {
      salvando = true;
    });

    try {
      // Busca dados completos do usuário para recuperar o senhaHash
      final usuarioResponse = await http.get(
        Uri.parse('http://localhost:8080/user/${widget.usuario.id}'),
      );

      if (usuarioResponse.statusCode != 200) {
        throw Exception('Não foi possível obter dados atuais do usuário.');
      }

      final usuarioCompleto = jsonDecode(usuarioResponse.body);

      final userPayload = {
        'id': widget.usuario.id,
        'nome': widget.usuario.nome,
        'login': widget.usuario.login,
        'senhaHash': usuarioCompleto['senhaHash'] ?? widget.usuario.senhaHash,
        'funcionario': widget.usuario.funcionario,
        'alergiaLeite': alergiaLeite,
        'alergiaOvo': alergiaOvo,
        'alergiaAmendoim': alergiaAmendoim,
        'alergiaCastanhas': alergiaCastanhas,
        'alergiaFrutosDoMar': alergiaFrutosDoMar,
        'alergiaPeixes': alergiaPeixes,
        'alergiaTrigo': alergiaTrigo,
        'alergiaSoja': alergiaSoja,
        'alergiaGergilim': alergiaGergilim,
      };

      final uri = Uri.parse('http://localhost:8080/user/${widget.usuario.id}');
      final request = http.MultipartRequest('PUT', uri);

      request.files.add(
        http.MultipartFile.fromString(
          'user',
          jsonEncode(userPayload),
          contentType: MediaType('application', 'json'),
        ),
      );

      final streamed = await request.send();
      final response = await http.Response.fromStream(streamed);

      if (!mounted) return;

      if (response.statusCode == 200) {
        final Map<String, dynamic> dados = jsonDecode(response.body);
        final Usuario usuarioAtualizado = Usuario.fromJson(dados);

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Alergias atualizadas com sucesso!'),
            backgroundColor: Color(0xFF2E7D32),
          ),
        );

        widget.usuario
          ..alergiaLeite = usuarioAtualizado.alergiaLeite
          ..alergiaOvo = usuarioAtualizado.alergiaOvo
          ..alergiaAmendoim = usuarioAtualizado.alergiaAmendoim
          ..alergiaCastanhas = usuarioAtualizado.alergiaCastanhas
          ..alergiaFrutosDoMar = usuarioAtualizado.alergiaFrutosDoMar
          ..alergiaPeixes = usuarioAtualizado.alergiaPeixes
          ..alergiaTrigo = usuarioAtualizado.alergiaTrigo
          ..alergiaSoja = usuarioAtualizado.alergiaSoja
          ..alergiaGergilim = usuarioAtualizado.alergiaGergilim;

        Navigator.pop(context, widget.usuario);
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Erro ao salvar alergias: ${response.body}'),
            backgroundColor: Colors.red,
          ),
        );
      }
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Erro de conexão ao salvar: $e'),
          backgroundColor: Colors.red,
        ),
      );
    } finally {
      if (mounted) {
        setState(() {
          salvando = false;
        });
      }
    }
  }

  Widget _buildAlergiaTile({
    required String titulo,
    required String emoji,
    required String descricao,
    required bool valor,
    required ValueChanged<bool> onChanged,
  }) {
    return Container(
      margin: const EdgeInsets.symmetric(vertical: 5),
      decoration: BoxDecoration(
        color: valor ? const Color(0xFFFFF1EE) : Colors.white,
        borderRadius: BorderRadius.circular(14),
        border: Border.all(
          color: valor ? const Color(0xFFE76F51) : Colors.grey.shade300,
          width: valor ? 1.8 : 1.0,
        ),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.03),
            blurRadius: 4,
            offset: const Offset(0, 2),
          ),
        ],
      ),
      child: SwitchListTile(
        activeThumbColor: const Color(0xFFE76F51),
        activeTrackColor: const Color(0xFFE76F51).withValues(alpha: 0.4),
        contentPadding: const EdgeInsets.symmetric(horizontal: 14, vertical: 4),
        secondary: Container(
          width: 44,
          height: 44,
          alignment: Alignment.center,
          decoration: BoxDecoration(
            color: valor
                ? const Color(0xFFE76F51).withValues(alpha: 0.15)
                : Colors.grey.shade100,
            borderRadius: BorderRadius.circular(10),
          ),
          child: Text(
            emoji,
            style: const TextStyle(fontSize: 24),
          ),
        ),
        title: Text(
          titulo,
          style: TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 16,
            color: valor ? const Color(0xFF9C3D2A) : const Color(0xFF264653),
          ),
        ),
        subtitle: Text(
          descricao,
          style: TextStyle(
            fontSize: 12,
            color: Colors.grey.shade600,
          ),
        ),
        value: valor,
        onChanged: onChanged,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final int totalMarcadas = [
      alergiaLeite,
      alergiaOvo,
      alergiaAmendoim,
      alergiaCastanhas,
      alergiaFrutosDoMar,
      alergiaPeixes,
      alergiaTrigo,
      alergiaSoja,
      alergiaGergilim,
    ].where((a) => a).length;

    return Scaffold(
      backgroundColor: const Color(0xFFF9F9FB),
      appBar: AppBar(
        title: const Text(
          'Minhas Alergias',
          style: TextStyle(fontWeight: FontWeight.bold),
        ),
        centerTitle: true,
        backgroundColor: const Color(0xFFE76F51),
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SafeArea(
        child: Column(
          children: [
            // Banner explicativo
            Container(
              padding: const EdgeInsets.all(16),
              margin: const EdgeInsets.all(16),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(16),
                border: Border.all(
                  color: const Color(0xFFE76F51).withValues(alpha: 0.3),
                ),
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withValues(alpha: 0.04),
                    blurRadius: 8,
                    offset: const Offset(0, 3),
                  ),
                ],
              ),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      const Icon(
                        Icons.shield_outlined,
                        color: Color(0xFFE76F51),
                        size: 26,
                      ),
                      const SizedBox(width: 8),
                      const Expanded(
                        child: Text(
                          'Proteção e Alertas no Cardápio',
                          style: TextStyle(
                            fontSize: 16,
                            fontWeight: FontWeight.bold,
                            color: Color(0xFF264653),
                          ),
                        ),
                      ),
                      Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 8,
                          vertical: 3,
                        ),
                        decoration: BoxDecoration(
                          color: totalMarcadas > 0
                              ? const Color(0xFFE76F51)
                              : Colors.grey.shade400,
                          borderRadius: BorderRadius.circular(12),
                        ),
                        child: Text(
                          '$totalMarcadas selecionada${totalMarcadas != 1 ? 's' : ''}',
                          style: const TextStyle(
                            color: Colors.white,
                            fontSize: 12,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 8),
                  Text(
                    'Selecione os alimentos aos quais você possui alergia ou intolerância alimentar. '
                    'O aplicativo emitirá avisos visuais automáticos nos pratos e no cardápio diário.',
                    style: TextStyle(
                      fontSize: 13,
                      color: Colors.grey.shade700,
                      height: 1.3,
                    ),
                  ),
                  const SizedBox(height: 12),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      TextButton.icon(
                        icon: const Icon(Icons.clear_all, size: 18),
                        label: const Text('Limpar todas'),
                        style: TextButton.styleFrom(
                          foregroundColor: Colors.grey.shade700,
                        ),
                        onPressed: () => alterarTodos(false),
                      ),
                      const SizedBox(width: 8),
                      TextButton.icon(
                        icon: const Icon(Icons.select_all, size: 18),
                        label: const Text('Marcar todas'),
                        style: TextButton.styleFrom(
                          foregroundColor: const Color(0xFFE76F51),
                        ),
                        onPressed: () => alterarTodos(true),
                      ),
                    ],
                  ),
                ],
              ),
            ),

            // Lista de alergênicos
            Expanded(
              child: ListView(
                padding: const EdgeInsets.symmetric(horizontal: 16),
                children: [
                  _buildAlergiaTile(
                    titulo: 'Leite / Lactose',
                    emoji: '🥛',
                    descricao: 'Leite animal, queijos, manteiga, iogurtes e derivados',
                    valor: alergiaLeite,
                    onChanged: (v) => setState(() => alergiaLeite = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Ovo',
                    emoji: '🥚',
                    descricao: 'Ovos inteiros, clara, gema e preparações contendo ovo',
                    valor: alergiaOvo,
                    onChanged: (v) => setState(() => alergiaOvo = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Amendoim',
                    emoji: '🥜',
                    descricao: 'Amendoim in natura, pasta de amendoim, paçoca e doces',
                    valor: alergiaAmendoim,
                    onChanged: (v) => setState(() => alergiaAmendoim = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Castanhas e Nozes',
                    emoji: '🌰',
                    descricao: 'Castanha-de-caju, castanha-do-Pará, nozes, avelãs e amêndoas',
                    valor: alergiaCastanhas,
                    onChanged: (v) => setState(() => alergiaCastanhas = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Frutos do Mar',
                    emoji: '🦐',
                    descricao: 'Camarão, lagosta, caranguejo, lula, polvo e mariscos',
                    valor: alergiaFrutosDoMar,
                    onChanged: (v) => setState(() => alergiaFrutosDoMar = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Peixes',
                    emoji: '🐟',
                    descricao: 'Peixes frescos, congelados, enlatados e derivados',
                    valor: alergiaPeixes,
                    onChanged: (v) => setState(() => alergiaPeixes = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Trigo / Glúten',
                    emoji: '🌾',
                    descricao: 'Farinha de trigo, pães, massas e alimentos com glúten',
                    valor: alergiaTrigo,
                    onChanged: (v) => setState(() => alergiaTrigo = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Soja',
                    emoji: '🌱',
                    descricao: 'Grãos de soja, tofu, leite de soja, shoyu e proteína de soja',
                    valor: alergiaSoja,
                    onChanged: (v) => setState(() => alergiaSoja = v),
                  ),
                  _buildAlergiaTile(
                    titulo: 'Gergelim',
                    emoji: '⚪',
                    descricao: 'Sementes de gergelim, óleo de gergelim, tahine e pães especiais',
                    valor: alergiaGergilim,
                    onChanged: (v) => setState(() => alergiaGergilim = v),
                  ),
                  const SizedBox(height: 20),
                ],
              ),
            ),

            // Botão Salvar
            Container(
              padding: const EdgeInsets.all(16),
              decoration: BoxDecoration(
                color: Colors.white,
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withValues(alpha: 0.05),
                    offset: const Offset(0, -2),
                    blurRadius: 6,
                  ),
                ],
              ),
              child: SizedBox(
                width: double.infinity,
                height: 50,
                child: ElevatedButton.icon(
                  onPressed: salvando ? null : salvarAlergias,
                  icon: salvando
                      ? const SizedBox(
                          width: 20,
                          height: 20,
                          child: CircularProgressIndicator(
                            color: Colors.white,
                            strokeWidth: 2,
                          ),
                        )
                      : const Icon(Icons.check, color: Colors.white),
                  label: Text(
                    salvando ? 'Salvando alterações...' : 'Salvar Alergias',
                    style: const TextStyle(
                      fontSize: 16,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFE76F51),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
