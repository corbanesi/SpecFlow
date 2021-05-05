using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace SpecFlowFramework.Steps
{
    [Binding]
    public class SpecFlowFeatureSteps
    {
        private readonly IWebDriver _driver;
        readonly ScenarioContext _scenarioContext;

        public SpecFlowFeatureSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
        }

        [Given(@"que meu primeiro número seja (.*)")]
        public void DadoQueMeuPrimeiroNumeroSeja(int number)
        {
            _driver.Navigate().GoToUrl("http://duckduckgo.com");

            _scenarioContext.Set<int>(number, "Number1");
        }

        [Given(@"meu segundo número seja (.*)")]
        public void DadoMeuSegundoNumeroSeja(int number)
        {
            _driver.Navigate().GoToUrl("https://bootstrap-vue.js.org/");

            _scenarioContext.Set<int>(number, "Number2");
        }

        [When(@"somo os dois números")]
        public void QuandoSomoOsDoisNumeros()
        {
            int n1 = _scenarioContext.Get<int>("Number1");
            int n2 = _scenarioContext.Get<int>("Number2");

            _scenarioContext.Set<int>(n1 + n2, "Result");
        }

        [Then(@"o resultado deve ser (.*)")]
        public void EntaoOResultadoDeveSer(int expected)
        {
            _driver.Navigate().GoToUrl("https://specflow.org/");

            int actual = _scenarioContext.Get<int>("Result");

            Assert.AreEqual(expected, actual);
        }
    }
}
