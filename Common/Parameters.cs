using NUnit.Framework;
using System;
using System.IO;

namespace SpecFlowFramework.Common
{

    /*
     * Classe responsável pelo resgate de parâmetros para utilização nos cenários de testes
     *
     * Os dados são provindos do arquivo .runsettings utilizado na execução
    */
    class Parameters
    {

        // URL principal do website a ser acessada logo após a instanciação do WebDriver
        public static string Url => TestContext.Parameters["URL"];

        // Valor de espera para seleção de elementos e ações do Selenium
        public static int ImplicitWait => int.Parse(TestContext.Parameters["IMPLICIT_WAIT"]);

        // Valor de espera para ExpectedConditions do WebDriverWait
        public static int ExplicitWait => int.Parse(TestContext.Parameters["EXPLICIT_WAIT"]);

        // Parâmetro referente a geração de relatório da execução dos testes
        public static bool GenerateReport => bool.Parse(TestContext.Parameters["GENERATE_REPORT"]);

        // Parâmetro referente ao diretório em que será criado o relatório
        public static string ReportFolder
        {
            get
            {
                string folder = TestContext.Parameters["REPORT_FOLDER"];
                if (string.IsNullOrEmpty(folder))
                {
                    return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
                }
                return folder + Path.DirectorySeparatorChar;
            }
        }

        // URL do servidor remoto do selenium grid que contém os nodes com WebDriver
        public static string HubUrl => TestContext.Parameters["HUB_URL"];

        // Parâmetro referente a integração com o JIRA ara abertura de apontamentos
        public static bool JiraIntegration => bool.Parse(TestContext.Parameters["JIRA_INTEGRATION"]);

        // Valor referente a url do servidor do JIRA
        public static string JiraUrl => TestContext.Parameters["JIRA_URL"];

        // Parâmetro referente ao usuário de conexão com o JIRA
        public static string JiraUser => TestContext.Parameters["JIRA_USER"];

        // Parâmetro referente ao token do usuário de conexão com o JIRA
        public static string JiraToken => TestContext.Parameters["JIRA_TOKEN"];

        // Parâmetro referente ao projeto que será usado na abertura de um apontamento
        public static string JiraProject => TestContext.Parameters["JIRA_PROJECT"];

        // Parâmetro referente a uma sub-tarefa de um projeto
        public static string JiraParentIssue => TestContext.Parameters["JIRA_PARENT_ISSUE"];

        // Tipo de navegador a ser executado os cenários de testes
        public static BrowserType BrowserType
        {
            get
            {
                Enum.TryParse(TestContext.Parameters["BROWSER_TYPE"], out BrowserType browserType);
                return browserType;
            }
        }

        // Tipo de screenshot a ser tirada nos cenários de testes
        public static ScreenshotType ScreenshotType
        {
            get
            {
                Enum.TryParse(TestContext.Parameters["TAKE_SCREENSHOT"], out ScreenshotType screenshotType);
                return screenshotType;
            }
        }

    }
}
