using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;

namespace SpecFlowFramework.Common
{
    /*
     * Classe responsável pela geração de relatório de acordo com as Features, Scenarios e Steps
     */
    class ExtentReports
    {
        // Variavel estática referente ao ExtentReport
        private static AventStack.ExtentReports.ExtentReports _extentReport;

        // Variável estática por thread referente a Feature sendo executada
        [ThreadStatic]
        private static ExtentTest _feature;

        // Variável estática por thread referente ao Scenario de uma Feature sendo executada
        [ThreadStatic]
        private static ExtentTest _scenario;

        /*
         * Método responsável por instanciar o objeto ExtentReport
         */
        public static void InitializeExtentReport()
        {
            _extentReport = new AventStack.ExtentReports.ExtentReports();
            _extentReport.AttachReporter(new ExtentHtmlReporter(Parameters.ReportFolder));
            _extentReport.AddSystemInfo("OS", Environment.OSVersion.VersionString);
        }

        /*
         * Método responsável por destruir o objeto ExtentReport
         */
        public static void FlushReport()
        {
            _extentReport.Flush();
        }

        /*
         * Método responsável por instanciar o objeto Feature a partir do objeto ExtentReport
         */
        public static void CreateFeature(FeatureContext featureContext)
        {
            _feature = _extentReport.CreateTest<Feature>(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description);
            _feature.AssignDevice(Parameters.BrowserType.ToString());
        }

        /*
         * Método responsável por instanciar o objeto Scenario a partir do objeto Feature
         *
         * Adiciona as tags respectivas do cenário dentro do respectivo objeto
         */
        public static void CreateScenario(ScenarioContext scenarioContext)
        {
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            foreach (string tag in scenarioContext.ScenarioInfo.Tags)
            {
                _scenario.AssignCategory(tag);
            }
        }

        /*
         * Método responsável por criar o objeto Step a partir do ScenarioContext
         * 
         * Para cada Step, é adicionado uma screenshot dependendo da parametrização 
         * no arquivo .runsettings
         */
        public static void CreateStep(ScenarioContext scenarioContext, string screenshot)
        {
            ScenarioExecutionStatus executionStatus = scenarioContext.ScenarioExecutionStatus;
            StepDefinitionType stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType;

            string stepDefBr = StepTranslation(stepType);
            string stepText = stepDefBr + scenarioContext.StepContext.StepInfo.Text;

            MediaEntityModelProvider mediaModel = null;

            if (!string.IsNullOrEmpty(screenshot))
            {
                mediaModel = GetStepScreenshot(screenshot, executionStatus);
            }

            switch (executionStatus)
            {
                case ScenarioExecutionStatus.OK:
                    CreateStepNode(stepType, stepText)
                        .Log(Status.Pass, mediaModel);
                    break;

                case ScenarioExecutionStatus.TestError:
                    Exception error = scenarioContext.TestError;
                    CreateStepNode(stepType, stepText)
                        .Log(Status.Fail, error.Message, mediaModel);
                    break;

                case ScenarioExecutionStatus.Skipped:
                    CreateStepNode(stepType, stepText).Log(Status.Skip);
                    break;

                default: throw new ArgumentNullException("CreateTestStep ExecutionStatus" + executionStatus);
            }

        }

        /*
         * Retorna o media model necessário para anexar no step relatório caso tenha uma screenshot 
         * considerando o ScreenshotType
         */
        private static MediaEntityModelProvider GetStepScreenshot(string screenshot, ScenarioExecutionStatus executionStatus)
        {
            MediaEntityModelProvider mediaModel = null;

            if (Parameters.ScreenshotType == ScreenshotType.AllTests)
            {
                mediaModel = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshot).Build();
            }
            else if (executionStatus.Equals(ScenarioExecutionStatus.TestError) && (Parameters.ScreenshotType == ScreenshotType.OnlyErrors))
            {
                mediaModel = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshot).Build();
            }

            return mediaModel;
        }

        /*
        * Método responsável por criar um Step node com a respectiva descrição e tipo de Step
        */
        private static ExtentTest CreateStepNode(StepDefinitionType StepType, string StepText)
        {
            ExtentTest extentTest = StepType switch
            {
                StepDefinitionType.Given => _scenario.CreateNode<Given>(StepText),
                StepDefinitionType.When => _scenario.CreateNode<When>(StepText),
                StepDefinitionType.Then => _scenario.CreateNode<Then>(StepText),
                _ => throw new ArgumentNullException("CreateStepNode StepType" + StepType),
            };
            return extentTest;
        }

        /*
         * Método workaround responsável por traduzir o StepDefinitionType para português
         */
        private static string StepTranslation(StepDefinitionType StepType)
        {
            return StepType switch
            {
                StepDefinitionType.Given => "Dado ",
                StepDefinitionType.When => "Quando ",
                StepDefinitionType.Then => "Então ",
                _ => "",
            };
        }

    }

}
