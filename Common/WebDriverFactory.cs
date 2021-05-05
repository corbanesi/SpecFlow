using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace SpecFlowFramework.Common
{
    /*
     * Classe responsável por criar as instâncias de WebDriver
     */
    class WebDriverFactory
    {

        /*
         * Cria a retorna uma instância do WebDriver baseado no BrowserType fornecido
         * como parâmetro.
         * 
         * Também estipula o ImplicityWait do respectivo WebDriver com o valor do parâmetro 
         * no arquivo .runsettings
         */
        public static IWebDriver CreateDriverInstance(BrowserType browserType)
        {
            IWebDriver driver = browserType switch
            {
                BrowserType.Chrome => GetChrome(),
                BrowserType.ChromeHeadless => GetChromeHeadless(),
                BrowserType.RemoteChrome => GetRemoteChrome(),
                BrowserType.RemoteChromeHeadless => GetRemoteChromeHeadless(),
                _ => throw new ArgumentNullException("BrowserType não especificado!"),
            };

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Parameters.ImplicitWait);

            return driver;
        }

        /*
         * Retorna uma nova instância do WebDriverWait baseado no WebDriver fornecido
         * 
         * O valor do ExplicityWait da instância é correspondente ao presente no arquivo .runsettings
         */
        public static WebDriverWait SetWaitHelper(IWebDriver driver)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(Parameters.ExplicitWait));
        }

        /*
         * Método responsável por tirar evidência de tela do driver correspondente do cenário
         * 
         * Retorna o caminho para o arquivo salvo
         */
        public static string TakeScreenshot(IWebDriver driver)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            string saveAs = Parameters.ReportFolder + fileName;

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(saveAs);

            return saveAs;
        }

        /*
         * Retorna uma instância do ChromeDriver
         */
        private static IWebDriver GetChrome()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-extensions");

            return new ChromeDriver(options);
        }

        /*
         * Retorna uma instância do ChromeDriver no modo Headless
         */
        private static IWebDriver GetChromeHeadless()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1200,1100");
            options.AddArgument("--start-maximized");

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-dev-shm-usage");

            return new ChromeDriver(options);
        }

        /*
        * Retorna uma instância remota do Chrome
        */
        private static IWebDriver GetRemoteChrome()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-extensions");

            return new RemoteWebDriver(new Uri(Parameters.HubUrl), options);
        }

        /*
        * Retorna uma instância remota do Chrome Headless
        */
        private static IWebDriver GetRemoteChromeHeadless()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1200,1100");
            options.AddArgument("--start-maximized");

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-dev-shm-usage");

            return new RemoteWebDriver(new Uri(Parameters.HubUrl), options);
        }

    }
}
