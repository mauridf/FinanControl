using System.Security.Cryptography;
using System.Text;
using FinanControl.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanControl.App.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private Usuario _usuarioLogado;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public Usuario UsuarioLogado => _usuarioLogado;

        public async Task<bool> RegistrarAsync(Usuario usuario, string senha)
        {
            try
            {
                // Verificar se email já existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                {
                    return false;
                }

                // Gerar hash da senha
                usuario.SenhaHash = GerarHashSenha(senha);
                usuario.DataCadastro = DateTime.UtcNow;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                _usuarioLogado = usuario;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoginAsync(string email, string senha)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (usuario == null)
                    return false;

                var senhaHash = GerarHashSenha(senha);

                if (usuario.SenhaHash != senhaHash)
                    return false;

                _usuarioLogado = usuario;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Logout()
        {
            _usuarioLogado = null;
        }

        public async Task<Usuario> GetUsuarioLogadoAsync()
        {
            if (_usuarioLogado == null)
                return null;

            // Recarregar do banco para garantir dados atualizados
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == _usuarioLogado.Id);
        }

        private string GerarHashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
