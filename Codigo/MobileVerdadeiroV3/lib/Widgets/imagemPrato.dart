import 'package:flutter/material.dart';

class ImagemPrato extends StatelessWidget {
  const ImagemPrato({
    super.key,
    required this.imagem,
    required this.width,
    required this.height,
    this.fit = BoxFit.cover,
    this.placeholderColor = const Color(0xFFFFE0B2),
    this.placeholderIcon = Icons.restaurant,
    this.placeholderIconColor = const Color(0xFFE76F51),
    this.placeholderIconSize = 48,
  });

  final String? imagem;
  final double width;
  final double height;
  final BoxFit fit;
  final Color placeholderColor;
  final IconData placeholderIcon;
  final Color placeholderIconColor;
  final double placeholderIconSize;

  String? _urlDaImagem() {
    final valor = imagem?.trim();
    if (valor == null || valor.isEmpty) return null;

    final uri = Uri.tryParse(valor);
    if (uri == null) return null;
    if (uri.hasScheme) return uri.toString();

    return Uri.parse('http://localhost:8080/').resolve(valor).toString();
  }

  @override
  Widget build(BuildContext context) {
    final url = _urlDaImagem();
    if (url == null) return _placeholder();

    return Image.network(
      url,
      width: width,
      height: height,
      fit: fit,
      gaplessPlayback: true,
      webHtmlElementStrategy: WebHtmlElementStrategy.fallback,
      errorBuilder: (context, error, stackTrace) => _placeholder(),
    );
  }

  Widget _placeholder() {
    return Container(
      width: width,
      height: height,
      color: placeholderColor,
      child: Center(
        child: Icon(
          placeholderIcon,
          size: placeholderIconSize,
          color: placeholderIconColor,
        ),
      ),
    );
  }
}
