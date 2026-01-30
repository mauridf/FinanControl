namespace FinanControl.Core.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public TipoTransacao Tipo { get; set; }
        public string? Cor { get; set; } // Para UI (ex: "#FF0000")
        public string? Icone { get; set; } // Para UI (ex: "food")
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }

        // Relacionamentos
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
