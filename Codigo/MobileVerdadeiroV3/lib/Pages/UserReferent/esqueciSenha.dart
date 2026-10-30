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
    final theme = Theme.of(context);

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
        centerTitle: true,
        title: const Text('Esqueci Minha Senha'),
      ),
      backgroundColor: const Color(0xFFFFF8F3),
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 480),
              child: Container(
                padding: const EdgeInsets.all(24),
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(26),
                  boxShadow: [
                    BoxShadow(
                      color: const Color(0xFF7A1728).withValues(alpha: 0.08),
                      blurRadius: 24,
                      offset: const Offset(0, 10),
                    ),
                  ],
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    Center(
                      child: Container(
                        width: 76,
                        height: 76,
                        decoration: BoxDecoration(
                          color: const Color(0xFFFFF0E8),
                          borderRadius: BorderRadius.circular(24),
                        ),
                        child: const Icon(
                          Icons.lock_reset_rounded,
                          size: 40,
                          color: Color(0xFFE76F51),
                        ),
                      ),
                    ),
                    const SizedBox(height: 18),
                    Text(
                      'Crie uma nova senha',
                      textAlign: TextAlign.center,
                      style: theme.textTheme.headlineSmall?.copyWith(
                        fontWeight: FontWeight.bold,
                        color: const Color(0xFF3D2925),
                      ),
                    ),
                    const SizedBox(height: 8),
                    Text(
                      'Informe a nova senha e o código enviado para:',
                      textAlign: TextAlign.center,
                      style: TextStyle(color: Colors.grey.shade700),
                    ),
                    const SizedBox(height: 8),
                    Container(
                      padding: const EdgeInsets.symmetric(
                        horizontal: 12,
                        vertical: 10,
                      ),
                      decoration: BoxDecoration(
                        color: const Color(0xFFFFF8F3),
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: Text(
                        widget.email,
                        textAlign: TextAlign.center,
                        style: const TextStyle(
                          fontWeight: FontWeight.w600,
                          color: Color(0xFF7A1728),
                        ),
                      ),
                    ),
                    const SizedBox(height: 24),
                    TextField(
                      controller: senhaController,
                      obscureText: !senhaVisivel,
                      enabled: !carregando,
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
                          borderSide: BorderSide(color: Colors.grey.shade300),
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
                      enabled: !carregando,
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
                          borderSide: BorderSide(color: Colors.grey.shade300),
                        ),
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
                    const SizedBox(height: 14),
                    TextField(
                      controller: codigoController,
                      keyboardType: TextInputType.number,
                      maxLength: 6,
                      enabled: !carregando,
                      textAlign: TextAlign.center,
                      style: const TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.bold,
                        letterSpacing: 8,
                      ),
                      decoration: InputDecoration(
                        labelText: 'Código de confirmação',
                        counterText: '',
                        prefixIcon: const Icon(Icons.pin_outlined),
                        filled: true,
                        fillColor: const Color(0xFFFFFAF7),
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(14),
                        ),
                        enabledBorder: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(14),
                          borderSide: BorderSide(color: Colors.grey.shade300),
                        ),
                      ),
                    ),
                    const SizedBox(height: 8),
                    TextButton.icon(
                      onPressed: (secRestantes > 0 || enviandoCodigo)
                          ? null
                          : reenviarCodigo,
                      icon: enviandoCodigo
                          ? const SizedBox(
                              width: 18,
                              height: 18,
                              child: CircularProgressIndicator(strokeWidth: 2),
                            )
                          : const Icon(Icons.mark_email_unread_outlined),
                      label: Text(
                        secRestantes > 0
                            ? 'Reenviar código em ${secRestantes}s'
                            : 'Reenviar código',
                      ),
                    ),
                    const SizedBox(height: 16),
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
                                'Salvar nova senha',
                                style: TextStyle(
                                  fontSize: 16,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}
