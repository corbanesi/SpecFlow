using TechTalk.SpecFlow.Assist.Attributes;

namespace SpecFlowFramework.DataModels
{
    public class Usuario
    {
        [TableAliases("Nome")]
        public string Nome { get; set; }

        [TableAliases("E-mail")]
        public string Email { get; set; }

        [TableAliases("Data de nascimento")]
        public string DataNascimento { get; set; }
    }
}
