import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import '../cadastro.dart';
import '../login.dart';
import 'dart:async';

class ConfirmarEmail extends StatefulWidget {
  final String email;
  final int id;

  const ConfirmarEmail({super.key, required this.email, required this.id});

  @override
  State<ConfirmarEmail> createState() => _ConfirmarEmailState();
}

class _ConfirmarEmailState extends State<ConfirmarEmail> {
  final TextEditingController codigoController = TextEditingController();

  bool carregando = false;

  Future<void> confirmarEmail() async {
    if (codigoController.text.length != 6) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Digite o código de 6 dígitos')),
      );
      return;
    }

    setState(() {
      carregando = true;
    });

    final url = Uri.parse(
      'http://localhost:8080/user/confirmarEmail'
      '?email=${Uri.encodeComponent(widget.email)}'
      '&codigo=${Uri.encodeComponent(codigoController.text)}',
    );

    try {
      final response = await http.post(url);

      if (response.statusCode == 200) {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('E-mail confirmado com sucesso!')),
        );

        Navigator.pushAndRemoveUntil(
          context,
          MaterialPageRoute(builder: (_) => Login()),
          (route) => false,
        );
      } else {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              response.body.isNotEmpty
                  ? response.body
                  : 'Código inválido ou expirado',
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
          carregando = false;
        });
      }
    }
  }

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

  Future<void> reenviarCodigo() async {
    if (secRestantes > 0 || reenviado) {
      return;
    }

    setState(() {
      reenviado = true;
    });

    final url = Uri.parse(
      'http://localhost:8080/user/reenviarCodigoCadastro'
      '?id=${widget.id}',
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

  int secRestantes = 0;
  bool reenviado = false;
  Timer? timer;

  @override
  void dispose() {
    codigoController.dispose();
    timer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
      appBar: AppBar(
        centerTitle: true,
        title: const Text('Confirmar e-mail'),
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
                        width: 78,
                        height: 78,
                        decoration: BoxDecoration(
                          color: const Color(0xFFFFF0E8),
                          borderRadius: BorderRadius.circular(25),
                        ),
                        child: const Icon(
                          Icons.mark_email_unread_outlined,
                          size: 42,
                          color: Color(0xFFE76F51),
                        ),
                      ),
                    ),
                    const SizedBox(height: 20),
                    Text(
                      'Verifique seu e-mail',
                      textAlign: TextAlign.center,
                      style: theme.textTheme.headlineSmall?.copyWith(
                        fontWeight: FontWeight.bold,
                        color: const Color(0xFF3D2925),
                      ),
                    ),
                    const SizedBox(height: 9),
                    Text(
                      'Enviamos um código de confirmação para:',
                      textAlign: TextAlign.center,
                      style: TextStyle(color: Colors.grey.shade700),
                    ),
                    const SizedBox(height: 10),
                    Container(
                      padding: const EdgeInsets.symmetric(
                        horizontal: 12,
                        vertical: 11,
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
                      controller: codigoController,
                      keyboardType: TextInputType.number,
                      maxLength: 6,
                      textAlign: TextAlign.center,
                      style: const TextStyle(
                        fontSize: 22,
                        fontWeight: FontWeight.bold,
                        letterSpacing: 10,
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
                    OutlinedButton.icon(
                      onPressed: secRestantes == 0 && !reenviado
                          ? reenviarCodigo
                          : null,
                      icon: reenviado
                          ? const SizedBox(
                              width: 18,
                              height: 18,
                              child: CircularProgressIndicator(strokeWidth: 2),
                            )
                          : const Icon(Icons.refresh_rounded),
                      label: Text(
                        secRestantes > 0
                            ? 'Tente novamente em ${secRestantes}s'
                            : 'Não recebeu? Reenviar código',
                      ),
                      style: OutlinedButton.styleFrom(
                        foregroundColor: const Color(0xFFE76F51),
                        side: BorderSide(
                          color: const Color(0xFFE76F51).withValues(alpha: 0.4),
                        ),
                        padding: const EdgeInsets.symmetric(vertical: 13),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(14),
                        ),
                      ),
                    ),
                    const SizedBox(height: 18),
                    SizedBox(
                      height: 52,
                      child: ElevatedButton(
                        onPressed: carregando ? null : confirmarEmail,
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
                                'Confirmar e-mail',
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
