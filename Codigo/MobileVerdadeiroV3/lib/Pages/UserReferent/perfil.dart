import 'package:flutter/material.dart';

import '../../Class/usuarioClass.dart';
import '../cadastro.dart';
import '../UserReferent/editarUsuario.dart';
import '../UserReferent/registroAlergias.dart';
import 'package:url_launcher/url_launcher.dart';

class Perfil extends StatefulWidget {
  final Usuario usuario;

  const Perfil({super.key, required this.usuario});

  @override
  State<Perfil> createState() => _PerfilState();
}

class _PerfilState extends State<Perfil> {
  late Usuario usuario;

  @override
  void initState() {
    super.initState();
    usuario = widget.usuario;
  }

  Future<void> editarUsuario() async {
    final resultado = await Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => EditarUsuario(usuario: usuario)),
    );

    if (resultado != null && resultado is Usuario) {
      setState(() {
        usuario = resultado;
      });
    }
  }

  Future<void> abrirRegistroAlergias() async {
    final resultado = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => RegistroAlergias(usuario: usuario),
      ),
    );

    if (resultado != null && resultado is Usuario) {
      setState(() {
        usuario = resultado;
      });
    }
  }

  Widget _buildAvatar() {
    if (usuario.imagem != null && usuario.imagem!.trim().isNotEmpty) {
      return CircleAvatar(
        radius: 46,
        backgroundColor: const Color(0xFFE76F51).withValues(alpha: 0.2),
        backgroundImage: NetworkImage(usuario.imagem!),
        onBackgroundImageError: (_, __) {},
      );
    }

    final initial = usuario.nome.isNotEmpty
        ? usuario.nome.trim()[0].toUpperCase()
        : 'U';

    return CircleAvatar(
      radius: 46,
      backgroundColor: const Color(0xFFE76F51),
      child: Text(
        initial,
        style: const TextStyle(
          fontSize: 36,
          fontWeight: FontWeight.bold,
          color: Colors.white,
        ),
      ),
    );
  }

  Widget _buildAlergiasCard() {
    final alergias = usuario.getListaAlergias();

    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
        side: BorderSide(
          color: alergias.isNotEmpty
              ? const Color(0xFFE76F51).withValues(alpha: 0.3)
              : Colors.grey.shade200,
        ),
      ),
      elevation: 2,
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Row(
                  children: [
                    Icon(
                      alergias.isNotEmpty
                          ? Icons.warning_amber_rounded
                          : Icons.verified_user_outlined,
                      color: alergias.isNotEmpty
                          ? const Color(0xFFE76F51)
                          : const Color(0xFF2E7D32),
                      size: 22,
                    ),
                    const SizedBox(width: 8),
                    const Text(
                      'Restrições Alimentares',
                      style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ],
                ),
                TextButton(
                  onPressed: abrirRegistroAlergias,
                  style: TextButton.styleFrom(
                    padding: const EdgeInsets.symmetric(horizontal: 8),
                    visualDensity: VisualDensity.compact,
                  ),
                  child: const Text(
                    'Gerenciar',
                    style: TextStyle(
                      color: Color(0xFFE76F51),
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 8),
            if (alergias.isEmpty)
              Row(
                children: [
                  Icon(
                    Icons.check_circle_outline,
                    color: Colors.green.shade600,
                    size: 18,
                  ),
                  const SizedBox(width: 8),
                  Text(
                    'Nenhuma alergia cadastrada',
                    style: TextStyle(color: Colors.grey.shade700, fontSize: 14),
                  ),
                ],
              )
            else
              Wrap(
                spacing: 8,
                runSpacing: 6,
                children: alergias.map((alergia) {
                  return Container(
                    padding: const EdgeInsets.symmetric(
                      horizontal: 10,
                      vertical: 5,
                    ),
                    decoration: BoxDecoration(
                      color: const Color(0xFFFFF1EE),
                      borderRadius: BorderRadius.circular(20),
                      border: Border.all(
                        color: const Color(0xFFE76F51).withValues(alpha: 0.5),
                      ),
                    ),
                    child: Text(
                      alergia,
                      style: const TextStyle(
                        color: Color(0xFF9C3D2A),
                        fontSize: 12,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  );
                }).toList(),
              ),
          ],
        ),
      ),
    );
  }

  Widget _buildOpcaoCard({
    required IconData icone,
    required String titulo,
    required String descricao,
    required VoidCallback onTap,
    Color cor = const Color(0xFFE76F51),
    bool destrutiva = false,
  }) {
    final corFinal = destrutiva ? Colors.red.shade700 : cor;

    return Card(
      margin: const EdgeInsets.only(bottom: 10),
      elevation: 0,
      color: Colors.white,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(18),
        side: BorderSide(color: Colors.grey.shade200),
      ),
      child: ListTile(
        contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 6),
        leading: Container(
          width: 44,
          height: 44,
          decoration: BoxDecoration(
            color: corFinal.withValues(alpha: 0.1),
            borderRadius: BorderRadius.circular(14),
          ),
          child: Icon(icone, color: corFinal),
        ),
        title: Text(titulo, style: const TextStyle(fontWeight: FontWeight.w700)),
        subtitle: Text(descricao),
        trailing: Icon(Icons.arrow_forward_ios, size: 15, color: corFinal),
        onTap: onTap,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context, usuario);
          },
        ),
        centerTitle: true,
        title: const Text("Perfil"),
      ),
      backgroundColor: const Color(0xFFFFF9F5),
      body: SingleChildScrollView(
        padding: const EdgeInsets.fromLTRB(18, 12, 18, 28),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Container(
              padding: const EdgeInsets.fromLTRB(22, 24, 22, 22),
              decoration: BoxDecoration(
                gradient: const LinearGradient(
                  colors: [Color(0xFFE76F51), Color(0xFFB94736)],
                  begin: Alignment.topLeft,
                  end: Alignment.bottomRight,
                ),
                borderRadius: BorderRadius.circular(26),
                boxShadow: [
                  BoxShadow(
                    color: const Color(0xFFE76F51).withValues(alpha: 0.22),
                    blurRadius: 18,
                    offset: const Offset(0, 8),
                  ),
                ],
              ),
              child: Column(
                children: [
                  Container(
                    padding: const EdgeInsets.all(5),
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      border: Border.all(
                        color: Colors.white.withValues(alpha: 0.7),
                        width: 2,
                      ),
                    ),
                    child: _buildAvatar(),
                  ),
                  const SizedBox(height: 14),
                  Text(
                    usuario.nome,
                    textAlign: TextAlign.center,
                    style: const TextStyle(
                      color: Colors.white,
                      fontSize: 23,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 5),
                  Text(
                    usuario.login,
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      color: Colors.white.withValues(alpha: 0.86),
                      fontSize: 14,
                    ),
                  ),
                  const SizedBox(height: 12),
                  Container(
                    padding: const EdgeInsets.symmetric(
                      horizontal: 13,
                      vertical: 6,
                    ),
                    decoration: BoxDecoration(
                      color: Colors.white.withValues(alpha: 0.18),
                      borderRadius: BorderRadius.circular(20),
                    ),
                    child: Text(
                      usuario.funcionario ? "👔 Funcionário" : "🎓 Aluno",
                      style: const TextStyle(
                        color: Colors.white,
                        fontSize: 12,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            const SizedBox(height: 22),
            InkWell(
              onTap: () async {
                final Uri url = Uri.parse(
                  'https://play.google.com/store/apps/details?id=com.inovatecnologia.carteirinha&hl=pt_BR',
                );

                if (await canLaunchUrl(url)) {
                  await launchUrl(url, mode: LaunchMode.externalApplication);
                }
              },
              borderRadius: BorderRadius.circular(18),
              child: Card(
                elevation: 0,
                color: const Color(0xFFFFF0E8),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(18),
                  side: BorderSide(
                    color: const Color(0xFFE76F51).withValues(alpha: 0.18),
                  ),
                ),
                child: const Padding(
                  padding: EdgeInsets.all(17),
                  child: Row(
                    children: [
                      Icon(
                        Icons.account_balance_wallet_outlined,
                        color: Color(0xFFE76F51),
                        size: 30,
                      ),
                      SizedBox(width: 14),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              'Recarregar saldo',
                              style: TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            SizedBox(height: 4),
                            Text(
                              'Acesse o aplicativo de recarga',
                              style: TextStyle(
                                fontSize: 12,
                                color: Colors.black54,
                              ),
                            ),
                          ],
                        ),
                      ),
                      Icon(Icons.open_in_new, color: Color(0xFFE76F51)),
                    ],
                  ),
                ),
              ),
            ),
            const SizedBox(height: 18),
            _buildAlergiasCard(),
            const SizedBox(height: 20),
            const Padding(
              padding: EdgeInsets.only(left: 4, bottom: 10),
              child: Text(
                'Minha conta',
                style: TextStyle(fontSize: 17, fontWeight: FontWeight.bold),
              ),
            ),
            _buildOpcaoCard(
              icone: Icons.shield_outlined,
              titulo: 'Minhas alergias e restrições',
              descricao: usuario.temAlergias
                  ? '${usuario.getListaAlergias().length} alergia(s) configurada(s)'
                  : 'Nenhuma restrição cadastrada',
              onTap: abrirRegistroAlergias,
            ),
            _buildOpcaoCard(
              icone: Icons.manage_accounts_outlined,
              titulo: 'Editar perfil',
              descricao: 'Atualize seus dados e sua senha',
              onTap: editarUsuario,
            ),
            _buildOpcaoCard(
              icone: Icons.logout,
              titulo: 'Sair',
              descricao: 'Encerrar esta sessão',
              cor: Colors.red,
              destrutiva: true,
              onTap: () {
                Navigator.of(context).pushAndRemoveUntil(
                  MaterialPageRoute(builder: (_) => const Cadastro()),
                  (route) => false,
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}
