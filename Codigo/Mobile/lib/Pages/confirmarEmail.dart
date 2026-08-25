import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'cadastro.dart';
import 'login.dart';

class ConfirmarEmail extends StatefulWidget {
  final String email;

  const ConfirmarEmail({super.key, required this.email});

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

  @override
  void dispose() {
    codigoController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Confirmar e-mail')),
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Icon(Icons.email, size: 80),

            const SizedBox(height: 20),

            const Text(
              'Verifique seu e-mail',
              style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 10),

            Text(
              'Enviamos um código de confirmação para:',
              textAlign: TextAlign.center,
            ),

            const SizedBox(height: 5),

            Text(
              widget.email,
              style: const TextStyle(fontWeight: FontWeight.bold),
            ),

            const SizedBox(height: 30),

            TextField(
              controller: codigoController,
              keyboardType: TextInputType.number,
              maxLength: 6,
              textAlign: TextAlign.center,
              decoration: const InputDecoration(
                labelText: 'Código de confirmação',
                border: OutlineInputBorder(),
              ),
            ),

            const SizedBox(height: 20),

            ElevatedButton(
              onPressed: carregando ? null : confirmarEmail,
              child: carregando
                  ? const CircularProgressIndicator()
                  : const Text('Confirmar e-mail'),
            ),
          ],
        ),
      ),
    );
  }
}
