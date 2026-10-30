class Pratos {
  int _id;
  String _nome;
  String _descricao;
  bool _vegano;
  bool _leite;
  bool _ovo;
  bool _amendoim;
  bool _castanhas;
  bool _frutosDoMar;
  bool _peixes;
  bool _trigo;
  bool _soja;
  bool _gergilim;
  bool _altoAcucarAdicionado;
  bool _altoGorduraSaturada;
  bool _altoSodio;
  String? _imagem;
  String? _notaTecnica;
  String? _descricaoIA;
  int _categoriaId;
  String? _categoriaDescricao;

  Pratos({
    int id = 0,
    String nome = '',
    String descricao = '',
    bool vegano = false,
    bool leite = false,
    bool ovo = false,
    bool amendoim = false,
    bool castanhas = false,
    bool frutosDoMar = false,
    bool peixes = false,
    bool trigo = false,
    bool soja = false,
    bool gergilim = false,
    bool altoAcucarAdicionado = false,
    bool altoGorduraSaturada = false,
    bool altoSodio = false,
    String? imagem,
    String? notaTecnica,
    String? descricaoIA,
    int categoriaId = 0,
    String? categoriaDescricao,
  })  : _id = id,
        _nome = nome,
        _descricao = descricao,
        _vegano = vegano,
        _leite = leite,
        _ovo = ovo,
        _amendoim = amendoim,
        _castanhas = castanhas,
        _frutosDoMar = frutosDoMar,
        _peixes = peixes,
        _trigo = trigo,
        _soja = soja,
        _gergilim = gergilim,
        _altoAcucarAdicionado = altoAcucarAdicionado,
        _altoGorduraSaturada = altoGorduraSaturada,
        _altoSodio = altoSodio,
        _imagem = imagem,
        _notaTecnica = notaTecnica,
        _descricaoIA = descricaoIA,
        _categoriaId = categoriaId,
        _categoriaDescricao = categoriaDescricao;

  // Getters e Setters
  int get id => _id;
  set id(int value) => _id = value;

  String get nome => _nome;
  set nome(String value) => _nome = value;

  String get descricao => _descricao;
  set descricao(String value) => _descricao = value;

  bool get vegano => _vegano;
  set vegano(bool value) => _vegano = value;

  bool get leite => _leite;
  set leite(bool value) => _leite = value;

  bool get ovo => _ovo;
  set ovo(bool value) => _ovo = value;

  bool get amendoim => _amendoim;
  set amendoim(bool value) => _amendoim = value;

  bool get castanhas => _castanhas;
  set castanhas(bool value) => _castanhas = value;

  bool get frutosDoMar => _frutosDoMar;
  set frutosDoMar(bool value) => _frutosDoMar = value;

  bool get peixes => _peixes;
  set peixes(bool value) => _peixes = value;

  bool get trigo => _trigo;
  set trigo(bool value) => _trigo = value;

  bool get soja => _soja;
  set soja(bool value) => _soja = value;

  bool get gergilim => _gergilim;
  set gergilim(bool value) => _gergilim = value;

  bool get altoAcucarAdicionado => _altoAcucarAdicionado;
  set altoAcucarAdicionado(bool value) => _altoAcucarAdicionado = value;

  bool get altoGorduraSaturada => _altoGorduraSaturada;
  set altoGorduraSaturada(bool value) => _altoGorduraSaturada = value;

  bool get altoSodio => _altoSodio;
  set altoSodio(bool value) => _altoSodio = value;

  String? get imagem => _imagem;
  set imagem(String? value) => _imagem = value;

  String? get notaTecnica => _notaTecnica;
  set notaTecnica(String? value) => _notaTecnica = value;

  String? get descricaoIA => _descricaoIA;
  set descricaoIA(String? value) => _descricaoIA = value;

  int get categoriaId => _categoriaId;
  set categoriaId(int value) => _categoriaId = value;

  String? get categoriaDescricao => _categoriaDescricao;
  set categoriaDescricao(String? value) => _categoriaDescricao = value;

  // Helpers para verificação de alérgenos e alertas
  bool get temAlergenos =>
      _leite ||
      _ovo ||
      _amendoim ||
      _castanhas ||
      _frutosDoMar ||
      _peixes ||
      _trigo ||
      _soja ||
      _gergilim;

  bool get temAlertasNutricionais =>
      _altoAcucarAdicionado || _altoGorduraSaturada || _altoSodio;

  factory Pratos.fromJson(Map<String, dynamic> json) {
    int catId = 0;
    String? catDesc;
    if (json['categoria'] != null) {
      if (json['categoria'] is Map) {
        catId = json['categoria']['id'] ?? 0;
        catDesc = json['categoria']['descricao'];
      } else if (json['categoria'] is int) {
        catId = json['categoria'];
      }
    } else if (json['categoriaId'] != null) {
      catId = json['categoriaId'];
    }

    return Pratos(
      id: json['id'] ?? 0,
      nome: json['nome'] ?? '',
      descricao: json['descricao'] ?? '',
      vegano: json['vegano'] ?? false,
      leite: json['leite'] ?? false,
      ovo: json['ovo'] ?? false,
      amendoim: json['amendoim'] ?? false,
      castanhas: json['castanhas'] ?? false,
      frutosDoMar: json['frutosDoMar'] ?? false,
      peixes: json['peixes'] ?? false,
      trigo: json['trigo'] ?? false,
      soja: json['soja'] ?? false,
      gergilim: json['gergilim'] ?? false,
      altoAcucarAdicionado: json['altoAcucarAdicionado'] ?? false,
      altoGorduraSaturada: json['altoGorduraSaturada'] ?? false,
      altoSodio: json['altoSodio'] ?? false,
      imagem: json['imagem'],
      notaTecnica: json['notaTecnica'],
      descricaoIA: json['descricaoIA'],
      categoriaId: catId,
      categoriaDescricao: catDesc,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': _id,
      'nome': _nome,
      'descricao': _descricao,
      'vegano': _vegano,
      'leite': _leite,
      'ovo': _ovo,
      'amendoim': _amendoim,
      'castanhas': _castanhas,
      'frutosDoMar': _frutosDoMar,
      'peixes': _peixes,
      'trigo': _trigo,
      'soja': _soja,
      'gergilim': _gergilim,
      'altoAcucarAdicionado': _altoAcucarAdicionado,
      'altoGorduraSaturada': _altoGorduraSaturada,
      'altoSodio': _altoSodio,
      'imagem': _imagem,
      'notaTecnica': _notaTecnica,
      'descricaoIA': _descricaoIA,
      'categoria': {'id': _categoriaId},
    };
  }
}

/// Alias para permitir o uso de Prato no singular
typedef Prato = Pratos;
