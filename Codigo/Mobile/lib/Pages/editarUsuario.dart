import 'dart:convert';
import 'dart:async';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:tcc_flutter/Pages/cadastro.dart';

import '../Class/usuarioClass.dart';

class EditarUsuario extends StatefulWidget {
  final Usuario usuario;

  const EditarUsuario({super.key, required this.usuario});

  @override
  State<EditarUsuario> createState() => _EditarUsuarioState();
}

class _EditarUsuarioState extends State<EditarUsuario> {
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

      final response = await http.put(
        Uri.parse('http://localhost:8080/user/atualizar'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'id': widget.usuario.id,
          'nome': nomeController.text,
          'login': loginController.text,

          // Necessário para passar pela validação do Java.
          'senhaHash': usuarioCompleto['senhaHash'],
        }),
      );

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
    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context);
          },
        ),
        title: const Text('Editar usuário'),
      ),

      body: Padding(
        padding: const EdgeInsets.all(16),

        child: SingleChildScrollView(
          child: Column(
            children: [
              TextField(
                controller: nomeController,
                decoration: const InputDecoration(labelText: 'Nome'),
              ),

              const SizedBox(height: 16),

              TextField(
                controller: loginController,
                decoration: const InputDecoration(labelText: 'E-mail'),
              ),

              const SizedBox(height: 30),

              const Align(
                alignment: Alignment.centerLeft,
                child: Text(
                  'Alterar senha',
                  style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                ),
              ),

              const SizedBox(height: 16),

              TextField(
                controller: senhaController,
                obscureText: !senhaVisivel,
                decoration: InputDecoration(
                  labelText: 'Nova senha',
                  suffixIcon: IconButton(
                    icon: Icon(
                      senhaVisivel ? Icons.visibility_off : Icons.visibility,
                    ),
                    onPressed: () {
                      setState(() {
                        senhaVisivel = !senhaVisivel;
                      });
                    },
                  ),
                ),
              ),

              const SizedBox(height: 16),

              TextField(
                controller: confirmarSenhaController,
                obscureText: !confirmarSenhaVisivel,
                decoration: InputDecoration(
                  labelText: 'Confirmar nova senha',
                  suffixIcon: IconButton(
                    icon: Icon(
                      confirmarSenhaVisivel
                          ? Icons.visibility_off
                          : Icons.visibility,
                    ),
                    onPressed: () {
                      setState(() {
                        confirmarSenhaVisivel = !confirmarSenhaVisivel;
                      });
                    },
                  ),
                ),
              ),

              const SizedBox(height: 16),

              TextField(
                controller: codigoController,
                keyboardType: TextInputType.number,
                decoration: const InputDecoration(
                  labelText: 'Código de confirmação',
                ),
              ),
              TextButton(
                onPressed: (secRestantes > 0 || enviandoCodigo)
                    ? null
                    : reenviarCodigo,
                child: enviandoCodigo
                    ? const SizedBox(
                        width: 20,
                        height: 20,
                        child: CircularProgressIndicator(),
                      )
                    : Text(
                        secRestantes > 0
                            ? 'Reenviar código em '
                                  '$secRestantes s'
                            : 'Reenviar código',
                      ),
              ),

              const SizedBox(height: 10),

              Align(
                alignment: Alignment.centerLeft,
                child: TextButton(
                  onPressed: enviandoCodigo ? null : enviarCodigo,

                  child: enviandoCodigo
                      ? const SizedBox(
                          width: 20,
                          height: 20,
                          child: CircularProgressIndicator(),
                        )
                      : const Text('Enviar código para o e-mail'),
                ),
              ),

              const SizedBox(height: 30),

              ElevatedButton(
                onPressed: carregando ? null : salvar,

                child: carregando
                    ? const CircularProgressIndicator()
                    : const Text('Salvar'),
              ),

              const SizedBox(height: 30),

              TextButton(
                onPressed: carregando ? null : confirmarExclusao,
                child: const Text('Excluir conta'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
