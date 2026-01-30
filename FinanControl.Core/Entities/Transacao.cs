using FinanControl.Core.Enums;

namespace FinanControl.Core.Entities
{
    public class Transacao : BaseEntity
    {
        public string Descricao { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public DateTime Data { get; set; }
        public bool Efetivada { get; set; } = true; // Se já foi realizada
        public bool Recorrente { get; set; } = false;
        public int? RecorrenciaId { get; set; } // Para transações recorrentes
        public DateTime DataCriacao { get; set; }

        // Relacionamentos
        public int ContaId { get; set; }
        public Conta Conta { get; set; } = null!;

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
