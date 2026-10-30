import 'dart:convert';
import 'dart:async';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:http_parser/http_parser.dart';
import 'package:tcc_flutter/Pages/cadastro.dart';

import '../../Class/usuarioClass.dart';
import 'registroAlergias.dart';

class EditarUsuario extends StatefulWidget {
  final Usuario usuario;

  const EditarUsuario({super.key, required this.usuario});

  @override
  State<EditarUsuario> createState() => _EditarUsuarioState();
}

class _EditarUsuarioState extends State<EditarUsuario> {
  late Usuario usuarioAtual;
  late TextEditingController nomeController;
  late TextEditingController loginController;
  late TextEditingController senhaController;
  late TextEditingController confirmarSenhaController;
  late TextEditingController codigoController;

  bool carregando = false;
  bool enviandoCodigo = false;

  bool senhaVisivel = false;
  bool confirmarSenhaVisivel = false;

  int secRestantes = 0;
  Timer? timer;
  bool reenviado = false;

  @override
  void initState() {
    super.initState();

    usuarioAtual = widget.usuario;

    nomeController = TextEditingController(text: widget.usuario.nome);

    loginController = TextEditingController(text: widget.usuario.login);

    senhaController = TextEditingController();

    confirmarSenhaController = TextEditingController();

    codigoController = TextEditingController();
  }

  @override
  void dispose() {
    nomeController.dispose();
    loginController.dispose();
    senhaController.dispose();
    confirmarSenhaController.dispose();
    codigoController.dispose();

    super.dispose();
  }

  Future<void> enviarCodigo() async {
    setState(() {
      enviandoCodigo = true;
    });

    try {
      final response = await http.post(
        Uri.parse(
          'http://localhost:8080/user/solicitarResetSenha'
          '?login=${Uri.encodeComponent(loginController.text)}',
        ),
      );

      if (response.statusCode == 200) {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Código enviado para o seu e-mail.')),
        );
      } else {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Erro ao enviar código: ${response.body}')),
        );
      }
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    } finally {
      if (mounted) {
        setState(() {
          enviandoCodigo = false;
        });
      }
    }
  }

  Future<void> salvar() async {
    // Verifica se as senhas foram preenchidas
    // apenas se o usuário quiser alterar a senha.
    if (senhaController.text.isNotEmpty ||
        confirmarSenhaController.text.isNotEmpty ||
        codigoController.text.isNotEmpty) {
      if (senhaController.text.isEmpty) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(const SnackBar(content: Text('Digite a nova senha.')));
        return;
      }

      if (confirmarSenhaController.text.isEmpty) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(const SnackBar(content: Text('Confirme a nova senha.')));
        return;
      }

      if (senhaController.text != confirmarSenhaController.text) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('As senhas não coincidem.')),
        );
        return;
      }

      if (codigoController.text.isEmpty) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Digite o código enviado para seu e-mail.'),
          ),
        );
        return;
      }
    }

    setState(() {
      carregando = true;
    });

    try {
      // Busca os dados completos do usuário.
      final usuarioResponse = await http.get(
        Uri.parse('http://localhost:8080/user/id/${widget.usuario.id}'),
      );

      if (usuarioResponse.statusCode != 200) {
        throw Exception('Não foi possível buscar os dados do usuário.');
      }

      final usuarioCompleto = jsonDecode(usuarioResponse.body);

      final userPayload = {
        'id': widget.usuario.id,
        'nome': nomeController.text.trim(),
        'login': loginController.text.trim(),
        // Necessário para passar pela validação do Java.
        'senhaHash': usuarioCompleto['senhaHash'] ?? widget.usuario.senhaHash,
        'funcionario': widget.usuario.funcionario,
        'alergiaLeite': usuarioAtual.alergiaLeite,
        'alergiaOvo': usuarioAtual.alergiaOvo,
        'alergiaAmendoim': usuarioAtual.alergiaAmendoim,
        'alergiaCastanhas': usuarioAtual.alergiaCastanhas,
        'alergiaFrutosDoMar': usuarioAtual.alergiaFrutosDoMar,
        'alergiaPeixes': usuarioAtual.alergiaPeixes,
        'alergiaTrigo': usuarioAtual.alergiaTrigo,
        'alergiaSoja': usuarioAtual.alergiaSoja,
        'alergiaGergilim': usuarioAtual.alergiaGergilim,
      };

      final uri = Uri.parse('http://localhost:8080/user/atualizar');
      final request = http.MultipartRequest('PUT', uri);
      request.files.add(
        http.MultipartFile.fromString(
          'user',
          jsonEncode(userPayload),
          contentType: MediaType('application', 'json'),
        ),
      );

      final streamedResponse = await request.send();
      final response = await http.Response.fromStream(streamedResponse);

      if (response.statusCode != 200) {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Erro ao atualizar usuário: ${response.body}'),
          ),
        );

        return;
      }
      //Altera senha apenas se o usuário tiver preenchido o campo de nova senha.
      if (senhaController.text.isNotEmpty) {
        final resetResponse = await http.put(
          Uri.parse(
            'http://localhost:8080/user/resetSenha'
            '?login=${Uri.encodeComponent(loginController.text)}'
            '&codigo=${Uri.encodeComponent(codigoController.text)}'
            '&novaSenha=${Uri.encodeComponent(senhaController.text)}',
          ),
        );

        if (resetResponse.statusCode != 200) {
          if (!mounted) return;

          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Nome/e-mail foram atualizados, '
                'mas houve um erro ao alterar a senha: '
                '${resetResponse.body}',
              ),
            ),
          );

          return;
        }
      }

      final dados = jsonDecode(response.body);

      Usuario usuarioAtualizado = Usuario.fromJson(dados);

      if (!mounted) return;

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Usuário atualizado com sucesso!')),
      );

      Navigator.pop(context, usuarioAtualizado);
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    } finally {
      if (mounted) {
        setState(() {
          carregando = false;
        });
      }
    }
  }

  // Inicia a contagem regressiva para o reenvio do código.
  void iniciarContagem() {
    timer?.cancel();

    setState(() {
      secRestantes = 30;
    });

    timer = Timer.periodic(const Duration(seconds: 1), (timer) {
      if (secRestantes <= 1) {
        timer.cancel();

        if (mounted) {
          setState(() {
            secRestantes = 0;
          });
        }
      } else {
        if (mounted) {
          setState(() {
            secRestantes--;
          });
        }
      }
    });
  }

  // Reenvia o código de confirmação para o e-mail do usuário.
  Future<void> reenviarCodigo() async {
    if (secRestantes > 0 || reenviado) {
      return;
    }

    setState(() {
      reenviado = true;
    });

    final url = Uri.parse(
      'http://localhost:8080/user/reenviarCodigoCadastro'
      '?id=${widget.usuario.id}',
    );

    try {
      final response = await http.put(url);

      if (response.statusCode == 200) {
        iniciarContagem();

        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Um novo código foi enviado para seu e-mail.'),
          ),
        );
      } else {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              response.body.isNotEmpty
                  ? response.body
                  : 'Não foi possível reenviar o código.',
            ),
          ),
        );
      }
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    } finally {
      if (mounted) {
        setState(() {
          reenviado = false;
        });
      }
    }
  }

  // Confirma a exclusão da conta do usuário.
  Future<void> confirmarExclusao() async {
    final senhaController = TextEditingController();
    bool senhaVisivel = false;

    final confirmar = await showDialog<bool>(
      context: context,
      builder: (context) {
        return StatefulBuilder(
          builder: (context, setDialogState) {
            return AlertDialog(
              title: const Text('Excluir conta'),
              content: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Text(
                    'Essa ação não pode ser desfeita.\n\n'
                    'Digite sua senha atual para confirmar a exclusão da conta.',
                  ),
                  const SizedBox(height: 16),
                  TextField(
                    controller: senhaController,
                    obscureText: !senhaVisivel,
                    decoration: InputDecoration(
                      labelText: 'Senha atual',
                      suffixIcon: IconButton(
                        icon: Icon(
                          senhaVisivel
                              ? Icons.visibility_off
                              : Icons.visibility,
                        ),
                        onPressed: () {
                          setDialogState(() {
                            senhaVisivel = !senhaVisivel;
                          });
                        },
                      ),
                    ),
                  ),
                ],
              ),
              actions: [
                TextButton(
                  onPressed: () {
                    Navigator.pop(context, false);
                  },
                  child: const Text('Cancelar'),
                ),
                ElevatedButton(
                  onPressed: () {
                    if (senhaController.text.isEmpty) {
                      ScaffoldMessenger.of(context).showSnackBar(
                        const SnackBar(content: Text('Digite sua senha.')),
                      );
                      return;
                    }

                    Navigator.pop(context, true);
                  },
                  child: const Text('Excluir'),
                ),
              ],
            );
          },
        );
      },
    );

    if (confirmar != true) {
      senhaController.dispose();
      return;
    }

    final senha = senhaController.text;
    senhaController.dispose();

    await deletarConta(senha);
  }

  // Função para deletar a conta do usuário.
  Future<void> deletarConta(String senha) async {
    setState(() {
      carregando = true;
    });

    try {
      // Primeiro verifica se a senha está correta.
      final validarResponse = await http.get(
        Uri.parse(
          'http://localhost:8080/user/validar'
          '?login=${Uri.encodeComponent(loginController.text)}'
          '&senhaHash=${Uri.encodeComponent(senha)}',
        ),
      );

      if (validarResponse.statusCode == 200 ||
          validarResponse.statusCode == 204) {
        // Senha correta: agora exclui a conta.
        final response = await http.delete(
          Uri.parse('http://localhost:8080/user/deletar/${widget.usuario.id}'),
        );

        if (!mounted) return;

        if (response.statusCode == 200 || response.statusCode == 204) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Conta excluída com sucesso.')),
          );

          Navigator.pushAndRemoveUntil(
            context,
            MaterialPageRoute(builder: (context) => const Cadastro()),
            (route) => false,
          );
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                response.body.isNotEmpty
                    ? response.body
                    : 'Não foi possível excluir a conta.',
              ),
            ),
          );
        }
      } else {
        if (!mounted) return;

        ScaffoldMessenger.of(
          context,
        ).showSnackBar(const SnackBar(content: Text('Senha incorreta.')));
      }
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    } finally {
      if (mounted) {
        setState(() {
          carregando = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context);
          },
        ),
        centerTitle: true,
        title: const Text('Editar usuário'),
      ),
      backgroundColor: const Color(0xFFFFF8F3),
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.fromLTRB(18, 18, 18, 30),
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 560),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  Center(
                    child: Container(
                      width: 72,
                      height: 72,
                      decoration: BoxDecoration(
                        color: const Color(0xFFFFEDE5),
                        borderRadius: BorderRadius.circular(23),
                      ),
                      child: const Icon(
                        Icons.manage_accounts_outlined,
                        color: Color(0xFFE76F51),
                        size: 39,
                      ),
                    ),
                  ),
                  const SizedBox(height: 14),
                  Text(
                    'Seus dados',
                    textAlign: TextAlign.center,
                    style: theme.textTheme.headlineSmall?.copyWith(
                      fontWeight: FontWeight.bold,
                      color: const Color(0xFF3D2925),
                    ),
                  ),
                  const SizedBox(height: 6),
                  Text(
                    'Mantenha suas informações sempre atualizadas.',
                    textAlign: TextAlign.center,
                    style: TextStyle(color: Colors.grey.shade700),
                  ),
                  const SizedBox(height: 24),
                  Card(
                    margin: EdgeInsets.zero,
                    elevation: 0,
                    color: Colors.white,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(20),
                      side: BorderSide(color: Colors.grey.shade200),
                    ),
                    child: Padding(
                      padding: const EdgeInsets.all(18),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: [
                          const Text(
                            'Informações pessoais',
                            style: TextStyle(
                              fontSize: 17,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          const SizedBox(height: 16),
                          TextField(
                            controller: nomeController,
                            decoration: InputDecoration(
                              labelText: 'Nome',
                              prefixIcon: const Icon(Icons.person_outline),
                              filled: true,
                              fillColor: const Color(0xFFFFFAF7),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                              ),
                              enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                                borderSide: BorderSide(
                                  color: Colors.grey.shade300,
                                ),
                              ),
                            ),
                          ),
                          const SizedBox(height: 14),
                          TextField(
                            controller: loginController,
                            decoration: InputDecoration(
                              labelText: 'E-mail',
                              prefixIcon: const Icon(Icons.mail_outline),
                              filled: true,
                              fillColor: const Color(0xFFFFFAF7),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                              ),
                              enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                                borderSide: BorderSide(
                                  color: Colors.grey.shade300,
                                ),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 14),
                  Card(
                    margin: EdgeInsets.zero,
                    elevation: 0,
                    color: Colors.white,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(18),
                      side: BorderSide(color: Colors.grey.shade200),
                    ),
                    child: ListTile(
                      contentPadding: const EdgeInsets.symmetric(
                        horizontal: 17,
                        vertical: 6,
                      ),
                      leading: Container(
                        width: 44,
                        height: 44,
                        decoration: BoxDecoration(
                          color: const Color(0xFFE76F51).withValues(alpha: 0.1),
                          borderRadius: BorderRadius.circular(14),
                        ),
                        child: const Icon(
                          Icons.shield_outlined,
                          color: Color(0xFFE76F51),
                        ),
                      ),
                      title: const Text(
                        'Restrições e alergias',
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                      subtitle: Text(
                        usuarioAtual.temAlergias
                            ? '${usuarioAtual.getListaAlergias().length} alergia(s) configurada(s)'
                            : 'Nenhuma alergia configurada',
                        style: const TextStyle(fontSize: 12),
                      ),
                      trailing: const Icon(
                        Icons.arrow_forward_ios,
                        size: 15,
                        color: Color(0xFFE76F51),
                      ),
                      onTap: () async {
                        final resultado = await Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (_) =>
                                RegistroAlergias(usuario: usuarioAtual),
                          ),
                        );
                        if (resultado != null && resultado is Usuario) {
                          setState(() {
                            usuarioAtual = resultado;
                          });
                        }
                      },
                    ),
                  ),
                  const SizedBox(height: 22),
                  Card(
                    margin: EdgeInsets.zero,
                    elevation: 0,
                    color: Colors.white,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(20),
                      side: BorderSide(color: Colors.grey.shade200),
                    ),
                    child: Padding(
                      padding: const EdgeInsets.all(18),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: [
                          const Text(
                            'Alterar senha',
                            style: TextStyle(
                              fontSize: 17,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          const SizedBox(height: 5),
                          Text(
                            'Deixe estes campos vazios para manter a senha atual.',
                            style: TextStyle(
                              fontSize: 13,
                              color: Colors.grey.shade700,
                            ),
                          ),
                          const SizedBox(height: 16),
                          TextField(
                            controller: senhaController,
                            obscureText: !senhaVisivel,
                            decoration: InputDecoration(
                              labelText: 'Nova senha',
                              prefixIcon: const Icon(Icons.lock_outline),
                              filled: true,
                              fillColor: const Color(0xFFFFFAF7),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                              ),
                              enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                                borderSide: BorderSide(
                                  color: Colors.grey.shade300,
                                ),
                              ),
                              suffixIcon: IconButton(
                                icon: Icon(
                                  senhaVisivel
                                      ? Icons.visibility_off
                                      : Icons.visibility,
                                ),
                                onPressed: () {
                                  setState(() {
                                    senhaVisivel = !senhaVisivel;
                                  });
                                },
                              ),
                            ),
                          ),
                          const SizedBox(height: 14),
                          TextField(
                            controller: confirmarSenhaController,
                            obscureText: !confirmarSenhaVisivel,
                            decoration: InputDecoration(
                              labelText: 'Confirmar nova senha',
                              prefixIcon: const Icon(Icons.lock_outline),
                              filled: true,
                              fillColor: const Color(0xFFFFFAF7),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                              ),
                              enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                                borderSide: BorderSide(
                                  color: Colors.grey.shade300,
                                ),
                              ),
                              suffixIcon: IconButton(
                                icon: Icon(
                                  confirmarSenhaVisivel
                                      ? Icons.visibility_off
                                      : Icons.visibility,
                                ),
                                onPressed: () {
                                  setState(() {
                                    confirmarSenhaVisivel =
                                        !confirmarSenhaVisivel;
                                  });
                                },
                              ),
                            ),
                          ),
                          const SizedBox(height: 14),
                          TextField(
                            controller: codigoController,
                            keyboardType: TextInputType.number,
                            decoration: InputDecoration(
                              labelText: 'Código de confirmação',
                              prefixIcon: const Icon(Icons.pin_outlined),
                              filled: true,
                              fillColor: const Color(0xFFFFFAF7),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                              ),
                              enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(14),
                                borderSide: BorderSide(
                                  color: Colors.grey.shade300,
                                ),
                              ),
                            ),
                          ),
                          const SizedBox(height: 6),
                          TextButton.icon(
                            onPressed: (secRestantes > 0 || enviandoCodigo)
                                ? null
                                : reenviarCodigo,
                            icon: enviandoCodigo
                                ? const SizedBox(
                                    width: 18,
                                    height: 18,
                                    child: CircularProgressIndicator(
                                      strokeWidth: 2,
                                    ),
                                  )
                                : const Icon(Icons.refresh_rounded),
                            label: Text(
                              secRestantes > 0
                                  ? 'Reenviar código em ${secRestantes}s'
                                  : 'Reenviar código',
                            ),
                          ),
                          Align(
                            alignment: Alignment.centerLeft,
                            child: TextButton.icon(
                              onPressed: enviandoCodigo ? null : enviarCodigo,
                              icon: const Icon(
                                Icons.mark_email_unread_outlined,
                              ),
                              label: const Text('Enviar código para o e-mail'),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 20),
                  SizedBox(
                    height: 52,
                    child: ElevatedButton(
                      onPressed: carregando ? null : salvar,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: const Color(0xFFE76F51),
                        foregroundColor: Colors.white,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(15),
                        ),
                      ),
                      child: carregando
                          ? const SizedBox(
                              width: 22,
                              height: 22,
                              child: CircularProgressIndicator(
                                strokeWidth: 2.5,
                                color: Colors.white,
                              ),
                            )
                          : const Text(
                              'Salvar alterações',
                              style: TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                    ),
                  ),
                  const SizedBox(height: 10),
                  TextButton.icon(
                    onPressed: carregando ? null : confirmarExclusao,
                    icon: const Icon(Icons.delete_outline),
                    label: const Text('Excluir conta'),
                    style: TextButton.styleFrom(
                      foregroundColor: Colors.red.shade700,
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
