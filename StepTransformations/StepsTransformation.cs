using SpecFlowFramework.DataModels;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowFramework.StepTransformations
{

    [Binding]
    class StepsTransformation
    {

        [StepArgumentTransformation("que eu tenha um model de usuários")]
        public Usuario CriaUsuario(Table table)
        {
            return table.CreateInstance<Usuario>();
        }

    }

}
