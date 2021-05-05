using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;

/*
 * Possibilita a execução dos testes em paralelo
 * 
 * Utiliza o número de threads padrão permitido (4)
 */
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace SpecFlowFramework.Common
{
    /*
     * Classe responsável pela execução de métodos adicionais globais entre 
     * Features, Scenarios e/ou Steps
     */
    [Binding]
    class Hooks : IDisposable
    {

        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver = null;

        public Hooks(IObjectContainer objectContainer)
        {
            this._objectContainer = objectContainer;
        }

        #region Test Run Hooks

        /*
         * Método executado antes da inicialização da suíte de testes
         * 
         * Responsável pela inicialização do relatório de acordo com a parametrização
         * no arquivo .runsettings
         */
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            if (Parameters.GenerateReport)
            {
                ExtentReports.InitializeExtentReport();
            }
        }

        /*
         * Método executado após o término da execução da suíte de testes
         * 
         * Responsável por finalizar o relatório conforme a parametrização
         * no arquivo .runsettings
         */
        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (Parameters.GenerateReport)
            {
                ExtentReports.FlushReport();
            }
        }

        #endregion

        #region Feature Hooks

        /*
         * Método executado antes da execução de um método de Feature
         * 
         * Responsável por chamar o médodo de criação de Feature do relatório de acordo
         * com a parametrização no arquivo .runsettings
         */
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            if (Parameters.GenerateReport)
            {
                ExtentReports.CreateFeature(featureContext);
            }
        }

        #endregion

        #region Scenario Hooks

        /*
         * Método executado antes de cada Scenario de uma Feature
         * 
         * Realiza a instanciação de um novo WebDriver e o respectivo WebDriverWait
         * e disponibiliza ambos no ScenarioContext
         * 
         * Acessa o endereço principal de um website parametrizado pelo arquivo .runsettings
         */
        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _driver = WebDriverFactory.CreateDriverInstance(Parameters.BrowserType);
            _objectContainer.RegisterInstanceAs(_driver, typeof(IWebDriver));

            WebDriverWait wait = WebDriverFactory.SetWaitHelper(_driver);
            _objectContainer.RegisterInstanceAs(wait, typeof(WebDriverWait));

            if (Parameters.GenerateReport)
            {
                ExtentReports.CreateScenario(scenarioContext);
            }

            _driver.Navigate().GoToUrl(Parameters.Url);
        }

        #endregion


        #region Step Hooks

        /*
         * Método executado após a execução de um Step
         * 
         * Responsável por chamar o método de criação de Step no relatório de acordo
         * com a parametrização no arquivo .runsettings
         * 
         * Responsável pela abertura de apontamentos no JIRA de acordo com a parametrização
         * no arquivo .runsettings
         */
        [AfterStep]
        public void AfterStep(IWebDriver driver, FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            ScenarioExecutionStatus executionStatus = scenarioContext.ScenarioExecutionStatus;
            string screenshot = null;

            if (Parameters.ScreenshotType == ScreenshotType.AllTests)
            {
                screenshot = WebDriverFactory.TakeScreenshot(driver);
            }
            else if (executionStatus == ScenarioExecutionStatus.TestError && Parameters.ScreenshotType == ScreenshotType.OnlyErrors)
            {
                screenshot = WebDriverFactory.TakeScreenshot(driver);
            }

            if (Parameters.GenerateReport)
            {
                ExtentReports.CreateStep(scenarioContext, screenshot);
            }

            if (Parameters.JiraIntegration && executionStatus == ScenarioExecutionStatus.TestError)
            {
                string featureTitle = featureContext.FeatureInfo.Title;
                JiraAPI.CreateIssue(scenarioContext, featureTitle, screenshot);
            }
        }

        #endregion

        /**
         * Método obrigatório implementado da interface IDisposable
         * 
         * Responsável pela finalização do driver do contexto atual
         */
        public void Dispose()
        {
            _driver?.Quit();
        }
    }
}
