using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace ATF.WebTestsContext
{
    public class WebClient
    {
        public FirefoxDriver firefoxDriver;

        public InternetExplorerDriver internetExplorerDriver;

        public ChromeDriver chromeDriver;

        private string uri;

        public string Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
            }
        }
    }
}
