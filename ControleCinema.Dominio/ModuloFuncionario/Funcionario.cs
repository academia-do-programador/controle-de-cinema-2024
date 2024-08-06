using ControleCinema.Dominio.Compartilhado;

namespace ControleCinema.Dominio.ModuloFuncionario;

public class Funcionario : EntidadeBase
{
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    
    public Funcionario() { }

    public Funcionario(string login, string senha, string nome)
    {
        Login = login;
        Senha = senha;
        Nome = nome;
    }
}