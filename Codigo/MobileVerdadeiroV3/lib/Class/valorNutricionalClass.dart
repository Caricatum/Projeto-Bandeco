class ValorNutricional {
  int id;
  double kcal;
  double carboidratos;
  double proteinas;
  double lipidios;
  String medida;
  int? pratoId;

  ValorNutricional({
    this.id = 0,
    this.kcal = 0.0,
    this.carboidratos = 0.0,
    this.proteinas = 0.0,
    this.lipidios = 0.0,
    this.medida = '',
    this.pratoId,
  });

  factory ValorNutricional.fromJson(Map<String, dynamic> json) {
    return ValorNutricional(
      id: json['id'] ?? 0,
      kcal: (json['kcal'] as num?)?.toDouble() ?? 0.0,
      carboidratos: (json['carboidratos'] as num?)?.toDouble() ?? 0.0,
      proteinas: (json['proteinas'] as num?)?.toDouble() ?? 0.0,
      lipidios: (json['lipidios'] as num?)?.toDouble() ?? 0.0,
      medida: json['medida']?.toString() ?? '',
      pratoId: json['prato'] is Map ? json['prato']['id'] : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'kcal': kcal,
      'carboidratos': carboidratos,
      'proteinas': proteinas,
      'lipidios': lipidios,
      'medida': medida,
      if (pratoId != null) 'prato': {'id': pratoId},
    };
  }
}

