import 'pratosClass.dart';

class Usuario {
  int _id;
  String _nome;
  String _senhaHash;
  bool _funcionario;
  String _login;
  bool _emailConfirmado;
  String? _codigoConfirmacao;
  DateTime? _expiracaoConfirmacao;
  String? _codigoResetSenha;
  DateTime? _expiracaoResetSenha;
  bool _alergiaLeite;
  bool _alergiaOvo;
  bool _alergiaAmendoim;
  bool _alergiaCastanhas;
  bool _alergiaFrutosDoMar;
  bool _alergiaPeixes;
  bool _alergiaTrigo;
  bool _alergiaSoja;
  bool _alergiaGergilim;
  String? _imagem;

  Usuario(
    this._id,
    this._nome,
    this._senhaHash,
    this._funcionario,
    this._login,
    this._emailConfirmado, {
    bool alergiaLeite = false,
    bool alergiaOvo = false,
    bool alergiaAmendoim = false,
    bool alergiaCastanhas = false,
    bool alergiaFrutosDoMar = false,
    bool alergiaPeixes = false,
    bool alergiaTrigo = false,
    bool alergiaSoja = false,
    bool alergiaGergilim = false,
    String? imagem,
    String? codigoConfirmacao,
    DateTime? expiracaoConfirmacao,
    String? codigoResetSenha,
    DateTime? expiracaoResetSenha,
  })  : _alergiaLeite = alergiaLeite,
        _alergiaOvo = alergiaOvo,
        _alergiaAmendoim = alergiaAmendoim,
        _alergiaCastanhas = alergiaCastanhas,
        _alergiaFrutosDoMar = alergiaFrutosDoMar,
        _alergiaPeixes = alergiaPeixes,
        _alergiaTrigo = alergiaTrigo,
        _alergiaSoja = alergiaSoja,
        _alergiaGergilim = alergiaGergilim,
        _imagem = imagem,
        _codigoConfirmacao = codigoConfirmacao,
        _expiracaoConfirmacao = expiracaoConfirmacao,
        _codigoResetSenha = codigoResetSenha,
        _expiracaoResetSenha = expiracaoResetSenha;

  factory Usuario.fromJson(Map<String, dynamic> json) {
    return Usuario(
      json['id'] is int
          ? json['id']
          : int.tryParse(json['id']?.toString() ?? '0') ?? 0,
      json['nome']?.toString() ?? '',
      json['senhaHash']?.toString() ?? '',
      json['funcionario'] == true,
      json['login']?.toString() ?? '',
      json['emailConfirmado'] == true,
      alergiaLeite: json['alergiaLeite'] == true,
      alergiaOvo: json['alergiaOvo'] == true,
      alergiaAmendoim: json['alergiaAmendoim'] == true,
      alergiaCastanhas: json['alergiaCastanhas'] == true,
      alergiaFrutosDoMar: json['alergiaFrutosDoMar'] == true,
      alergiaPeixes: json['alergiaPeixes'] == true,
      alergiaTrigo: json['alergiaTrigo'] == true,
      alergiaSoja: json['alergiaSoja'] == true,
      alergiaGergilim: json['alergiaGergilim'] == true,
      imagem: json['imagem']?.toString(),
      codigoConfirmacao: json['codigoConfirmacao']?.toString(),
      expiracaoConfirmacao: json['expiracaoConfirmacao'] != null
          ? DateTime.tryParse(json['expiracaoConfirmacao'].toString())
          : null,
      codigoResetSenha: json['codigoResetSenha']?.toString(),
      expiracaoResetSenha: json['expiracaoResetSenha'] != null
          ? DateTime.tryParse(json['expiracaoResetSenha'].toString())
          : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'nome': nome,
      'senhaHash': _senhaHash,
      'funcionario': funcionario,
      'login': login,
      'emailConfirmado': emailConfirmado,
      'alergiaLeite': alergiaLeite,
      'alergiaOvo': alergiaOvo,
      'alergiaAmendoim': alergiaAmendoim,
      'alergiaCastanhas': alergiaCastanhas,
      'alergiaFrutosDoMar': alergiaFrutosDoMar,
      'alergiaPeixes': alergiaPeixes,
      'alergiaTrigo': alergiaTrigo,
      'alergiaSoja': alergiaSoja,
      'alergiaGergilim': alergiaGergilim,
      if (imagem != null) 'imagem': imagem,
    };
  }

  // Getters e Setters
  int get id => _id;
  set id(int value) => _id = value;

  String get nome => _nome;
  set nome(String value) => _nome = value;

  String get senhaHash => _senhaHash;
  set senhaHash(String value) => _senhaHash = value;

  bool get funcionario => _funcionario;
  set funcionario(bool value) => _funcionario = value;

  String get login => _login;
  set login(String value) => _login = value;

  bool get emailConfirmado => _emailConfirmado;
  set emailConfirmado(bool value) => _emailConfirmado = value;

  String? get codigoConfirmacao => _codigoConfirmacao;
  set codigoConfirmacao(String? value) => _codigoConfirmacao = value;

  DateTime? get expiracaoConfirmacao => _expiracaoConfirmacao;
  set expiracaoConfirmacao(DateTime? value) => _expiracaoConfirmacao = value;

  String? get codigoResetSenha => _codigoResetSenha;
  set codigoResetSenha(String? value) => _codigoResetSenha = value;

  DateTime? get expiracaoResetSenha => _expiracaoResetSenha;
  set expiracaoResetSenha(DateTime? value) => _expiracaoResetSenha = value;

  bool get alergiaLeite => _alergiaLeite;
  set alergiaLeite(bool value) => _alergiaLeite = value;

  bool get alergiaOvo => _alergiaOvo;
  set alergiaOvo(bool value) => _alergiaOvo = value;

  bool get alergiaAmendoim => _alergiaAmendoim;
  set alergiaAmendoim(bool value) => _alergiaAmendoim = value;

  bool get alergiaCastanhas => _alergiaCastanhas;
  set alergiaCastanhas(bool value) => _alergiaCastanhas = value;

  bool get alergiaFrutosDoMar => _alergiaFrutosDoMar;
  set alergiaFrutosDoMar(bool value) => _alergiaFrutosDoMar = value;

  bool get alergiaPeixes => _alergiaPeixes;
  set alergiaPeixes(bool value) => _alergiaPeixes = value;

  bool get alergiaTrigo => _alergiaTrigo;
  set alergiaTrigo(bool value) => _alergiaTrigo = value;

  bool get alergiaSoja => _alergiaSoja;
  set alergiaSoja(bool value) => _alergiaSoja = value;

  bool get alergiaGergilim => _alergiaGergilim;
  set alergiaGergilim(bool value) => _alergiaGergilim = value;

  String? get imagem => _imagem;
  set imagem(String? value) => _imagem = value;

  // Funções utilitárias de Alergias e Restrições Alimentares

  /// Retorna `true` se o usuário tiver ao menos uma alergia marcada.
  bool get temAlergias =>
      _alergiaLeite ||
      _alergiaOvo ||
      _alergiaAmendoim ||
      _alergiaCastanhas ||
      _alergiaFrutosDoMar ||
      _alergiaPeixes ||
      _alergiaTrigo ||
      _alergiaSoja ||
      _alergiaGergilim;

  /// Retorna os nomes amigáveis de todas as alergias cadastradas.
  List<String> getListaAlergias() {
    final List<String> lista = [];
    if (_alergiaLeite) lista.add('Leite / Lactose');
    if (_alergiaOvo) lista.add('Ovo');
    if (_alergiaAmendoim) lista.add('Amendoim');
    if (_alergiaCastanhas) lista.add('Castanhas');
    if (_alergiaFrutosDoMar) lista.add('Frutos do Mar');
    if (_alergiaPeixes) lista.add('Peixes');
    if (_alergiaTrigo) lista.add('Trigo / Glúten');
    if (_alergiaSoja) lista.add('Soja');
    if (_alergiaGergilim) lista.add('Gergelim');
    return lista;
  }

  /// Retorna um mapa com o identificador de cada alergia e seu status.
  Map<String, bool> get mapaAlergias => {
        'alergiaLeite': _alergiaLeite,
        'alergiaOvo': _alergiaOvo,
        'alergiaAmendoim': _alergiaAmendoim,
        'alergiaCastanhas': _alergiaCastanhas,
        'alergiaFrutosDoMar': _alergiaFrutosDoMar,
        'alergiaPeixes': _alergiaPeixes,
        'alergiaTrigo': _alergiaTrigo,
        'alergiaSoja': _alergiaSoja,
        'alergiaGergilim': _alergiaGergilim,
      };

  /// Compara as alergias do usuário com os alérgenos de um prato.
  /// Retorna a lista de alérgenos conflitantes.
  List<String> verificarConflitoAlergias(Pratos prato) {
    final List<String> conflitos = [];
    if (_alergiaLeite && prato.leite) conflitos.add('Leite / Lactose');
    if (_alergiaOvo && prato.ovo) conflitos.add('Ovo');
    if (_alergiaAmendoim && prato.amendoim) conflitos.add('Amendoim');
    if (_alergiaCastanhas && prato.castanhas) conflitos.add('Castanhas');
    if (_alergiaFrutosDoMar && prato.frutosDoMar) conflitos.add('Frutos do Mar');
    if (_alergiaPeixes && prato.peixes) conflitos.add('Peixes');
    if (_alergiaTrigo && prato.trigo) conflitos.add('Trigo / Glúten');
    if (_alergiaSoja && prato.soja) conflitos.add('Soja');
    if (_alergiaGergilim && prato.gergilim) conflitos.add('Gergelim');
    return conflitos;
  }

  /// Cria uma cópia da instância atual substituindo os campos desejados.
  Usuario copyWith({
    int? id,
    String? nome,
    String? senhaHash,
    bool? funcionario,
    String? login,
    bool? emailConfirmado,
    bool? alergiaLeite,
    bool? alergiaOvo,
    bool? alergiaAmendoim,
    bool? alergiaCastanhas,
    bool? alergiaFrutosDoMar,
    bool? alergiaPeixes,
    bool? alergiaTrigo,
    bool? alergiaSoja,
    bool? alergiaGergilim,
    String? imagem,
    String? codigoConfirmacao,
    DateTime? expiracaoConfirmacao,
    String? codigoResetSenha,
    DateTime? expiracaoResetSenha,
  }) {
    return Usuario(
      id ?? _id,
      nome ?? _nome,
      senhaHash ?? _senhaHash,
      funcionario ?? _funcionario,
      login ?? _login,
      emailConfirmado ?? _emailConfirmado,
      alergiaLeite: alergiaLeite ?? _alergiaLeite,
      alergiaOvo: alergiaOvo ?? _alergiaOvo,
      alergiaAmendoim: alergiaAmendoim ?? _alergiaAmendoim,
      alergiaCastanhas: alergiaCastanhas ?? _alergiaCastanhas,
      alergiaFrutosDoMar: alergiaFrutosDoMar ?? _alergiaFrutosDoMar,
      alergiaPeixes: alergiaPeixes ?? _alergiaPeixes,
      alergiaTrigo: alergiaTrigo ?? _alergiaTrigo,
      alergiaSoja: alergiaSoja ?? _alergiaSoja,
      alergiaGergilim: alergiaGergilim ?? _alergiaGergilim,
      imagem: imagem ?? _imagem,
      codigoConfirmacao: codigoConfirmacao ?? _codigoConfirmacao,
      expiracaoConfirmacao: expiracaoConfirmacao ?? _expiracaoConfirmacao,
      codigoResetSenha: codigoResetSenha ?? _codigoResetSenha,
      expiracaoResetSenha: expiracaoResetSenha ?? _expiracaoResetSenha,
    );
  }
}
