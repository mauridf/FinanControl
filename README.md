# FinanControl - Sistema de Gestão Financeira Pessoal
<div align="center">
https://img.shields.io/badge/.NET%2520MAUI-512BD4?style=for-the-badge&logo=.net&logoColor=white
https://img.shields.io/badge/C%2523-239120?style=for-the-badge&logo=c-sharp&logoColor=white
https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white
https://img.shields.io/badge/Entity%2520Framework-512BD4?style=for-the-badge&logo=.net&logoColor=white
https://img.shields.io/badge/MVVM-Pattern-9C27B0?style=for-the-badge

Sistema completo de gestão financeira pessoal desenvolvido em .NET MAUI para Windows, Android e iOS

</div>
📋 Índice
📱 Visão Geral

✨ Funcionalidades

🏗️ Arquitetura

🛠️ Tecnologias

🚀 Começando

📁 Estrutura do Projeto

⚙️ Configuração

📦 Build e Deploy

🧪 Testando

🤝 Contribuindo

📄 Licença

📱 Visão Geral
FinanControl é um sistema completo de gestão financeira pessoal que permite aos usuários controlar suas receitas, despesas, contas e categorias financeiras. Desenvolvido com arquitetura em camadas e padrão MVVM, o sistema oferece uma experiência nativa multiplataforma.

Principais Objetivos:

✅ Controle total das finanças pessoais

✅ Interface intuitiva e responsiva

✅ Sincronização offline com SQLite

✅ Multiplataforma: Windows, Android, iOS

✅ Segurança com hash de senhas

✨ Funcionalidades
🔐 Autenticação
Cadastro de Usuário: Nome, email, telefone, data de nascimento, endereço

Login Seguro: Autenticação com hash SHA256

Sessão Persistente: Manter usuário logado entre sessões

📊 Dashboard
Visão Geral: Saldo total, receitas vs despesas do mês

Resumo Financeiro: Gráficos e indicadores visuais

Últimas Transações: Lista das movimentações recentes

Status Financeiro: Indicador de saúde financeira

💰 Gestão Financeira
Contas: Cadastro de contas bancárias e carteiras

Categorias: Classificação personalizada de transações

Fontes de Renda: Controle de múltiplas fontes de receita

Transações: Registro detalhado de entradas e saídas

📈 Recursos Avançados
Dashboard Interativo: Métricas em tempo real

Filtros por Período: Visualização mensal/trimestral/anual

Relatórios: Resumos financeiros detalhados

Notificações: Lembretes de contas a pagar

🏗️ Arquitetura
text
FinanControl/
├── FinanControl.Core/          # Camada de Domínio
│   ├── Entities/              # Entidades de domínio
│   ├── Enums/                 # Enumeradores
│   └── Interfaces/            # Interfaces do domínio
│
├── FinanControl.Infra.Data/   # Camada de Infraestrutura
│   ├── AppDbContext.cs        # Contexto do EF Core
│   ├── Repository.cs          # Repositório genérico
│   └── Interfaces/            # Interfaces de repositório
│
└── FinanControl.App/          # Camada de Apresentação (MAUI)
    ├── Views/                 # Páginas XAML
    ├── ViewModels/            # ViewModels (MVVM)
    ├── Converters/            # Conversores XAML
    ├── Services/              # Serviços de aplicação
    └── Resources/             # Recursos (imagens, fontes)
📐 Padrões de Design
MVVM (Model-View-ViewModel): Separação clara de responsabilidades

Repository Pattern: Abstraction de acesso a dados

Dependency Injection: Injeção de dependências com Microsoft DI

Observable Pattern: Notificações de mudanças com INotifyPropertyChanged

🛠️ Tecnologias
Backend
.NET 8 - Runtime e SDK

.NET MAUI - Framework multiplataforma

Entity Framework Core 8 - ORM

SQLite - Banco de dados local

CommunityToolkit.Maui - Componentes e utilitários

Frontend
XAML - Interface de usuário

C# Markup - Lógica de interface

MAUI Controls - Componentes nativos

Converters - Transformação de dados para UI

Ferramentas
Visual Studio 2022+ - IDE principal

SQLite Browser - Visualização do banco

Git - Controle de versão

NuGet - Gerenciador de pacotes

🚀 Começando
Pré-requisitos
Visual Studio 2022 (versão 17.8 ou superior)

Workload: .NET Multi-platform App UI development

Workload: .NET Desktop Development (para Windows)

.NET 8 SDK (ou superior)

Plataformas Alvo:

Windows 10/11 (versão 19041+)

Android (API 21+)

iOS (15.0+)

macOS (10.15+)

📥 Clonando o Repositório
bash
git clone https://github.com/seu-usuario/FinanControl.git
cd FinanControl
🔧 Configurando o Ambiente
Abra a solução no Visual Studio:

bash
FinanControl.sln
Restaurar pacotes NuGet:

Clique direito na solução → "Restaurar Pacotes NuGet"

Configurar plataforma alvo:

Selecione a plataforma (Windows, Android, etc.)

Certifique-se de que o projeto de inicialização seja FinanControl.App

▶️ Executando o Projeto
Compilar a solução:

text
Build → Rebuild Solution
Executar:

Pressione F5 ou clique em "Start Debugging"

Para Android: Conectar dispositivo ou usar emulador

Para Windows: Executará diretamente

Primeiro acesso:

Tela de login será exibida

Clique em "Criar uma conta" para registrar

Preencha os dados e confirme

📁 Estrutura do Projeto
FinanControl.Core
csharp
// Entidades principais
public class Usuario { ... }          // Dados do usuário
public class Conta { ... }            // Contas bancárias
public class Categoria { ... }        // Categorias de transação
public class FonteRenda { ... }       // Fontes de renda
public class Transacao { ... }        // Transações financeiras

// Enums
public enum TipoTransacao { Receita, Despesa, Transferencia }
public enum TipoConta { ContaCorrente, Poupanca, Investimento }
public enum TipoRenda { Salario, Freelance, Investimento }
FinanControl.Infra.Data
csharp
// Contexto do banco de dados
public class AppDbContext : DbContext { ... }

// Repositório genérico
public class Repository<T> : IRepository<T> where T : BaseEntity { ... }

// Interfaces
public interface IRepository<T> where T : class { ... }
FinanControl.App
text
Views/
├── LoginPage.xaml              # Tela de login
├── RegistroPage.xaml           # Tela de registro
├── DashboardPage.xaml          # Dashboard principal
├── ContasPage.xaml             # Gestão de contas
├── CategoriasPage.xaml         # Gestão de categorias
├── FontesRendaPage.xaml        # Gestão de fontes de renda
└── TransacoesPage.xaml         # Gestão de transações

ViewModels/
├── BaseViewModel.cs            # ViewModel base
├── LoginViewModel.cs           # Lógica de login
├── DashboardViewModel.cs       # Lógica do dashboard
└── ...                         # Outros ViewModels

Converters/
├── NotNullToBoolConverter.cs   # Conversor nulo → booleano
├── TipoTransacaoToColorConverter.cs # Tipo → cor
└── ...                         # Outros conversores

Services/
└── AuthService.cs              # Serviço de autenticação
⚙️ Configuração
Banco de Dados
O sistema utiliza SQLite com Entity Framework Core. O banco é criado automaticamente na primeira execução:

csharp
// Localização do banco:
// Windows: %APPDATA%\FinanControl.App\financontrol.db
// Android/iOS: FileSystem.AppDataDirectory/financontrol.db
Configurações de Build
Para Windows:
xml
<TargetFrameworks>net10.0-windows10.0.19041.0</TargetFrameworks>
<SupportedOSPlatformVersion>10.0.17763.0</SupportedOSPlatformVersion>
Para Android:
xml
<TargetFrameworks>net10.0-android</TargetFrameworks>
<SupportedOSPlatformVersion>21.0</SupportedOSPlatformVersion>
Para iOS:
xml
<TargetFrameworks>net10.0-ios</TargetFrameworks>
<SupportedOSPlatformVersion>15.0</SupportedOSPlatformVersion>
Configuração do MauiProgram.cs
csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseMauiCommunityToolkit()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

    // Configurar banco de dados
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "financontrol.db");
        options.UseSqlite($"Filename={databasePath}");
    });

    // Registrar serviços, ViewModels e Views
    // ...
    
    return builder.Build();
}
📦 Build e Deploy
Build para Desenvolvimento
bash
# Build para Windows
dotnet build -c Debug -f net10.0-windows10.0.19041.0

# Build para Android
dotnet build -c Debug -f net10.0-android

# Build para iOS (requer macOS)
dotnet build -c Debug -f net10.0-ios
Publicação para Produção
Windows (MSIX Package):
bash
dotnet publish -c Release -f net10.0-windows10.0.19041.0 \
  -p:WindowsPackageType=Msix \
  -p:MsixPublisherName="CN=YourPublisher" \
  -p:PackageCertificateKeyFile="your-certificate.pfx"
Android (APK/AAB):
bash
# Gerar APK
dotnet publish -c Release -f net10.0-android \
  -p:AndroidPackageFormat=apk

# Gerar AAB (Google Play)
dotnet publish -c Release -f net10.0-android \
  -p:AndroidPackageFormat=aab \
  -p:AndroidKeyStore=true \
  -p:AndroidSigningKeyStore=keystore.jks
iOS (IPA):
bash
# Requer macOS com Xcode
dotnet publish -c Release -f net10.0-ios \
  -p:BuildIpa=true \
  -p:IpaPackageDir=./output/
Configurações de Assinatura
Android:

xml
<PropertyGroup Condition="$(TargetFramework.Contains('-android'))">
  <AndroidKeyStore>True</AndroidKeyStore>
  <AndroidSigningKeyStore>keystore.jks</AndroidSigningKeyStore>
  <AndroidSigningStorePass>password</AndroidSigningStorePass>
  <AndroidSigningKeyAlias>keyalias</AndroidSigningKeyAlias>
  <AndroidSigningKeyPass>password</AndroidSigningKeyPass>
</PropertyGroup>
Windows:

xml
<PropertyGroup Condition="$(TargetFramework.Contains('-windows'))">
  <PackageCertificateKeyFile>certificate.pfx</PackageCertificateKeyFile>
  <PackageCertificatePassword>password</PackageCertificatePassword>
</PropertyGroup>
🧪 Testando
Testes Manuais
Teste de Fluxo:

Cadastro → Login → Dashboard → Funcionalidades

Testar CRUD completo para cada módulo

Teste de Plataforma:

Verificar layout em diferentes tamanhos de tela

Testar em dispositivos móveis e desktop

Validar responsividade

Teste de Dados:

Persistência após reinício do app

Consistência dos cálculos financeiros

Validações de entrada de dados

Testes Automatizados (Futuro)
csharp
// Exemplo de teste unitário
[Fact]
public void Login_ComCredenciaisValidas_DeveAutenticar()
{
    // Arrange
    var authService = new AuthService();
    var email = "teste@email.com";
    var senha = "123456";
    
    // Act
    var resultado = authService.LoginAsync(email, senha);
    
    // Assert
    Assert.True(resultado);
}
🤝 Contribuindo
Diretrizes de Contribuição
Fork o repositório

Crie uma branch para sua feature:

bash
git checkout -b feature/nova-funcionalidade
Faça commit das mudanças:

bash
git commit -m "feat: adiciona nova funcionalidade"
Push para a branch:

bash
git push origin feature/nova-funcionalidade
Abra um Pull Request

Padrões de Commit
feat: Nova funcionalidade

fix: Correção de bug

docs: Documentação

style: Formatação, pontuação, etc.

refactor: Refatoração de código

test: Adição ou correção de testes

chore: Atualização de build, dependências, etc.

Código de Conduta
Respeite todos os contribuidores

Mantenha discussões construtivas

Seja paciente com novos contribuidores

Reporte problemas de forma educada

📄 Licença
Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para detalhes.

📞 Suporte
Problemas Conhecidos
Emulador Android lento:

Use Android Virtual Device com hardware acceleration

Configure RAM suficiente (mínimo 4GB)

SQLite não criando banco:

Verifique permissões de escrita

Limpe e reconstrua o projeto

Layout não responsivo:

Use Grid com * e Auto para layouts flexíveis

Teste em diferentes densidades de tela

Recursos Úteis
Documentação .NET MAUI

Entity Framework Core

MAUI Community Toolkit

SQLite Documentation

Roadmap
Sincronização em nuvem

Exportação PDF/Excel

Gráficos avançados

Orçamento mensal

Notificações push

Widgets para celular

Autenticação biométrica

Backup automático

<div align="center">
Desenvolvido com ❤️ usando .NET MAUI

Se este projeto te ajudou, considere dar uma ⭐ no repositório!

</div>
