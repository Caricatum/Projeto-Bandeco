import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class Esquecisenha extends StatefulWidget {
  final String email;

  const Esquecisenha({super.key, required this.email});

  @override
  State<Esquecisenha> createState() => _EsquecisenhaState();
}

class _EsquecisenhaState extends State<Esquecisenha> {
  late TextEditingController senhaController;
  late TextEditingController confirmarSenhaController;
  late TextEditingController codigoController;

  bool carregando = false;
  bool enviandoCodigo = false;

  bool senhaVisivel = false;
  bool confirmarSenhaVisivel = false;

  int secRestantes = 0;

  @override
  void initState() {
    super.initState();

    senhaController = TextEditingController();
    confirmarSenhaController = TextEditingController();
    codigoController = TextEditingController();

    // Solicita o código automaticamente quando a tela é aberta.
    enviarCodigo();
  }

  @override
  void dispose() {
    senhaController.dispose();
    confirmarSenhaController.dispose();
    codigoController.dispose();

    super.dispose();
  }

  Future<void> enviarCodigo() async {
    if (enviandoCodigo) {
      return;
    }

    setState(() {
      enviandoCodigo = true;
    });

    try {
      final url = Uri.parse(
        'http://localhost:8080/user/solicitarResetSenha'
        '?login=${Uri.encodeQueryComponent(widget.email)}',
      );

      final response = await http.post(url);

      if (!mounted) {
        return;
      }

      if (response.statusCode == 200) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Código enviado para o seu e-mail.')),
        );

        iniciarContagem();
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              response.body.isNotEmpty
                  ? response.body
                  : 'Não foi possível enviar o código.',
            ),
          ),
        );
      }
    } catch (e) {
      if (!mounted) {
        return;
      }

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro de conexão com o servidor.')),
      );
    } finally {
      if (mounted) {
        setState(() {
          enviandoCodigo = false;
        });
      }
    }
  }

  void iniciarContagem() {
    setState(() {
      secRestantes = 30;
    });

    Future.doWhile(() async {
      await Future.delayed(const Duration(seconds: 1));

      if (!mounted) {
        return false;
      }

      if (secRestantes <= 1) {
        setState(() {
          secRestantes = 0;
        });

        return false;
      }

      setState(() {
        secRestantes--;
      });

      return true;
    });
  }

  Future<void> reenviarCodigo() async {
    if (secRestantes > 0 || enviandoCodigo) {
      return;
    }

    await enviarCodigo();
  }

  bool validarCampos() {
    final senha = senhaController.text.trim();
    final confirmarSenha = confirmarSenhaController.text.trim();
    final codigo = codigoController.text.trim();

    if (senha.isEmpty) {
      mostrarMensagem('Digite a nova senha.');
      return false;
    }

    if (confirmarSenha.isEmpty) {
      mostrarMensagem('Confirme a nova senha.');
      return false;
    }

    if (senha != confirmarSenha) {
      mostrarMensagem('As senhas não coincidem.');
      return false;
    }

    if (codigo.isEmpty) {
      mostrarMensagem('Digite o código enviado para seu e-mail.');
      return false;
    }

    return true;
  }

  void mostrarMensagem(String mensagem) {
    if (!mounted) {
      return;
    }

    ScaffoldMessenger.of(
      context,
    ).showSnackBar(SnackBar(content: Text(mensagem)));
  }

  Future<void> salvar() async {
    if (!validarCampos()) {
      return;
    }

    setState(() {
      carregando = true;
    });

    try {
      final login = widget.email.trim();
      final codigo = codigoController.text.trim();
      final novaSenha = senhaController.text;

      final url = Uri.parse(
        'http://localhost:8080/user/resetSenha'
        '?login=${Uri.encodeQueryComponent(widget.email.trim())}'
        '&codigo=${Uri.encodeQueryComponent(codigoController.text.trim())}'
        '&novaSenha=${Uri.encodeQueryComponent(senhaController.text)}',
      );

      final response = await http.post(url);

      if (!mounted) {
        return;
      }

      if (response.statusCode == 200) {
        mostrarMensagem('Senha atualizada com sucesso!');
        Navigator.pop(context, true);
      } else {
        mostrarMensagem(
          response.body.isNotEmpty
              ? response.body
              : 'Não foi possível atualizar a senha.',
        );
      }
    } catch (e) {
      if (!mounted) {
        return;
      }

      print('ERRO: $e');

      mostrarMensagem('Erro de conexão com o servidor.');
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
          onPressed: carregando
              ? null
              : () {
                  Navigator.pop(context);
                },
        ),
        title: const Text('Esqueci Minha Senha'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            children: [
              Text(
                'Digite a nova senha e o código '
                'enviado para o e-mail:\n${widget.email}',
                style: const TextStyle(fontSize: 16),
              ),

              const SizedBox(height: 20),

              TextField(
                controller: senhaController,
                obscureText: !senhaVisivel,
                enabled: !carregando,
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
                enabled: !carregando,
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
                maxLength: 6,
                enabled: !carregando,
                decoration: const InputDecoration(
                  labelText: 'Código de confirmação',
                  counterText: '',
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

              const SizedBox(height: 30),

              ElevatedButton(
                onPressed: carregando ? null : salvar,
                child: carregando
                    ? const SizedBox(
                        width: 20,
                        height: 20,
                        child: CircularProgressIndicator(),
                      )
                    : const Text('Salvar'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
