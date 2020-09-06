using OpenQA.Selenium.Chrome;
using System;

namespace ATF.WebTestsContext
{
    public class WebDriverContext
    {
        private string Uri;

        public WebClient webClient;

        public WebDriverContext(string uri)
        {
            this.Uri = uri;
        }
        public WebDriverContext()
        {
            this.Uri = string.Empty;
        }


        public WebClient CreateChromeDriver()
        {
            webClient = new WebClient();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(" --disable-gpu");
            webClient.chromeDriver = new ChromeDriver(options);
            webClient.chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            webClient.chromeDriver.Manage().Window.Maximize();
            webClient.chromeDriver.Manage().Cookies.DeleteAllCookies();
            webClient.Uri = this.Uri;
            return webClient;
        }

        public WebClient CreateIEDriver()
        {
            webClient = new WebClient();
            webClient.internetExplorerDriver = new OpenQA.Selenium.IE.InternetExplorerDriver();
            webClient.internetExplorerDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            webClient.internetExplorerDriver.Manage().Window.Maximize();
            webClient.internetExplorerDriver.Manage().Cookies.DeleteAllCookies();
            webClient.Uri = this.Uri;
            return webClient;
        }

        public WebClient CreateFirefixDriver()
        {
            webClient = new WebClient();
            webClient.firefoxDriver = new OpenQA.Selenium.Firefox.FirefoxDriver();
            webClient.firefoxDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            webClient.firefoxDriver.Manage().Window.Maximize();
            webClient.firefoxDriver.Manage().Cookies.DeleteAllCookies();
            webClient.Uri = this.Uri;
            return webClient;
        }
    }
}
