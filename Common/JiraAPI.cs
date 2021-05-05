using Atlassian.Jira;
using TechTalk.SpecFlow;

namespace SpecFlowFramework.Common
{
    class JiraAPI
    {

        /*
         * Método principal responsável pela criação de uma issue no JIRA
         * Cria a issue com o título, descrição e screenshot do passo de um cenário
         */
        public static void CreateIssue(ScenarioContext scenarioContext, string featureTitle, string screenshot)
        {
            string summary = GenerateIssueSummary(scenarioContext, featureTitle);
            string description = GenerateIssueDescription(scenarioContext);
            PostIssue(summary, description, screenshot);
        }

        /*
         * Retorna o título da issue de acordo com o contexto do cenário
         */
        private static string GenerateIssueSummary(ScenarioContext scenarioContext, string featureTitle)
        {
            string scenarioTitle = scenarioContext.ScenarioInfo.Title;
            string stepText = scenarioContext.StepContext.StepInfo.Text;
            string summary = featureTitle + ": " + scenarioTitle + " - " + stepText;
            return summary;
        }

        /*
         * Retorna a descrição da issue de acordo com o passo dentro do contexto do cenário
         */
        private static string GenerateIssueDescription(ScenarioContext scenarioContext)
        {
            string exceptionMessage = scenarioContext.TestError.Message;
            string exceptionStackTrace = scenarioContext.TestError.StackTrace;
            string description = "\nMensagem:" + exceptionMessage + "\nStack trace: \n" + exceptionStackTrace;
            return description;
        }

        /*
         * Realiza o registro da issue no JIRA de acordo com as parametrizações no arquivo .runsettings
         */
        public static void PostIssue(string summary, string description, string screenshot)
        {
            var jira = Jira.CreateRestClient(Parameters.JiraUrl, Parameters.JiraUser, Parameters.JiraToken);

            CreateIssueFields cif = new CreateIssueFields(Parameters.JiraProject)
            {
                ParentIssueKey = Parameters.JiraParentIssue,
                TimeTrackingData = new IssueTimeTrackingData("0")
            };

            var issue = jira.CreateIssue(cif);

            issue.Type = "10101";
            issue.Summary = summary;
            issue.Description = description;
            issue.Environment = "Url do Ambiente: " + Parameters.Url + "\n Navegador: " + Parameters.BrowserType;

            issue.SaveChanges();

            if (!string.IsNullOrEmpty(screenshot))
            {
                string[] attachments = { screenshot };
                issue.AddAttachment(attachments);
            }
        }
    }
}
