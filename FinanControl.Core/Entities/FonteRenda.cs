using FinanControl.Core.Enums;

namespace FinanControl.Core.Entities
{
    public class FonteRenda : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public TipoRenda Tipo { get; set; }
        public decimal ValorEstimado { get; set; }
        public FrequenciaRenda Frequencia { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        // Relacionamentos
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
