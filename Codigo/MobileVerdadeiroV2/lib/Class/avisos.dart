class Avisos {
  int _id;
  String _titulo;
  String? _descricao;
  String _model;
  DateTime _dataInicial;
  DateTime _dataFinal;
  int? _usuarioId;

  Avisos({
    required int id,
    required String titulo,
    String? descricao,
    required String model,
    required DateTime dataInicial,
    required DateTime dataFinal,
    int? usuarioId,
  }) : _id = id,
       _titulo = titulo,
       _descricao = descricao,
       _model = model,
       _dataInicial = dataInicial,
       _dataFinal = dataFinal,
       _usuarioId = usuarioId;

  int get id => _id;
  String get titulo => _titulo;
  String? get descricao => _descricao;
  String get model => _model;
  DateTime get dataInicial => _dataInicial;
  DateTime get dataFinal => _dataFinal;
  int? get usuarioId => _usuarioId;

  set id(int valor) {
    _id = valor;
  }

  set titulo(String valor) {
    _titulo = valor;
  }

  set descricao(String? valor) {
    _descricao = valor;
  }

  set model(String valor) {
    _model = valor;
  }

  set dataInicial(DateTime valor) {
    _dataInicial = valor;
  }

  set dataFinal(DateTime valor) {
    _dataFinal = valor;
  }

  set usuarioId(int? valor) {
    _usuarioId = valor;
  }

  factory Avisos.fromJson(Map<String, dynamic> json) {
    return Avisos(
      id: json['id'] ?? 0,

      titulo: json['titulo'] ?? '',

      descricao: json['descricao'],

      model: json['model'] ?? '',

      dataInicial: DateTime.parse(json['data_inicial']),

      dataFinal: DateTime.parse(json['data_final']),

      usuarioId: json['user'] != null ? json['user']['id'] : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': _id,
      'titulo': _titulo,
      'descricao': _descricao,
      'model': _model,
      'data_inicial': _dataInicial.toIso8601String().split('T')[0],
      'data_final': _dataFinal.toIso8601String().split('T')[0],
      'user': _usuarioId != null ? {'id': _usuarioId} : null,
    };
  }
}
