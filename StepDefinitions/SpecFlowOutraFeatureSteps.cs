using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlowFramework.DataModels;
using TechTalk.SpecFlow;

namespace SpecFlowFramework.Steps
{
    [Binding]
    public class SpecFlowOutraFeatureSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;

        public SpecFlowOutraFeatureSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
        }

        [Given(@"que eu tenha esse")]
        public void DadoQueEuTenhaEsse()
        {
            _driver.Navigate().GoToUrl("http://gitlab.luxfacta.com.br/");
        }

        [Given(@"que eu tenha um model de usuários")]
        public void DadoQueEuTenhaUmModelDeUsuarios(Usuario usuario)
        {
            _scenarioContext.Set<Usuario>(usuario, "usuario");
            Assert.AreEqual("Alessandro Corbanesi", usuario.Nome);
        }

        [When(@"eu fizer isso")]
        public void QuandoEuFizerIsso()
        {
            _driver.Navigate().GoToUrl("https://portal.luxfacta.com.br/login");
        }

        [Then(@"vai aparecer isso")]
        public void EntaoVaiAparecerIsso()
        {
            _driver.Navigate().GoToUrl("http://duckduckgo.com");
        }

        [Then(@"eu terei isso")]
        public void EntaoEuTereiIsso()
        {
            Usuario usuario = _scenarioContext.Get<Usuario>("usuario");
            Assert.AreEqual("acorbanesi@luxfacta.com", usuario.Email);
        }

    }
}
