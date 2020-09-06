using ATF.APITestContext;
using ATF.WebTestsContext;
using BoDi;
using OpenQA.Selenium;

namespace ATF.HostingContext
{
    public sealed class Hooks
    {
        private readonly IObjectContainer objectContainer;

        public HttpClientService httpClientServiceInstance = null;

        public WebClient webClient;

        public Hooks(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        public void ChromeDriver()
        {
            var webDriverContext = new WebDriverContext();
            
            webClient = webDriverContext.CreateChromeDriver();
            objectContainer.RegisterInstanceAs<WebClient>(webClient);
            objectContainer.RegisterInstanceAs<IWebDriver>(webClient.chromeDriver);
        }

        public void FirefoxDriver()
        {
            var webDriverContext = new WebDriverContext();

            webClient = webDriverContext.CreateFirefixDriver();
            objectContainer.RegisterInstanceAs<WebClient>(webClient);
            objectContainer.RegisterInstanceAs<IWebDriver>(webClient.firefoxDriver);
        }

        public void IEDriver()
        {
            var webDriverContext = new WebDriverContext();

            webClient = webDriverContext.CreateIEDriver();
            objectContainer.RegisterInstanceAs<WebClient>(webClient);
            objectContainer.RegisterInstanceAs<IWebDriver>(webClient.internetExplorerDriver);
        }

        public void InitHttpClientService()
        {
            httpClientServiceInstance = HttpClientService.Instance;
            objectContainer.RegisterInstanceAs<HttpClientService>(httpClientServiceInstance);
        }
    }
}
