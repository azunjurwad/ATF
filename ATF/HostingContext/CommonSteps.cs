using ATF.APITestContext;
using ATF.WebTestsContext;
using BoDi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using TechTalk.SpecFlow;

namespace ATF.HostingContext
{
    /// <summary>
    /// This class holds methods written as a part of Framework and are common to use in any scripting project    
    /// </summary>
    [Binding]
    public class CommonSteps
    {
        /// <summary>
        /// Can be initiated in constructor only
        /// </summary>
        private readonly IObjectContainer objectContainer = null;

        /// <summary>
        /// Constructor with object container
        /// </summary>
        /// <param name="objectContainer">object container having </param>
        public CommonSteps(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommonSteps()
        {
        }

        /// <summary>
        /// Resolve Hooks from object container
        /// </summary>
        /// <returns>Hooks instance</returns>
        public Hooks GetScenarioHooks()
        {
            if (objectContainer != null)
            {
                var sfHooks = objectContainer.Resolve<Hooks>();
                return sfHooks;
            }
            else
                return null;
        }

        /// <summary>
        /// Resolve web driver instance from object container
        /// </summary>
        /// <returns>Web driver instance</returns>
        public IWebDriver GetDriverInstanace()
        {
            if (objectContainer != null)
            {
                var driver = objectContainer.Resolve<IWebDriver>();
                return driver;
            }
            else
                return null;
        }

        /// <summary>
        /// Resolve Hooks from object container
        /// </summary>
        /// <returns>Webclient instance from hooks instance</returns>
        public WebClient GetScenarioWebClient()
        {
            if (objectContainer != null)
            {
                var webClient = objectContainer.Resolve<WebClient>();
                return webClient;
            }
            else
                return null;
        }

        /// <summary>
        /// Resolve Hooks from object container
        /// </summary>
        public HttpClientService GetHttpClientInstance()
        {
            if (objectContainer != null)
            {
                var httpClientServiceInstance = objectContainer.Resolve<HttpClientService>();
                return httpClientServiceInstance;
            }
            else
                return null;
        }

        /// <summary>
        /// Resolve Hooks and WebClient. Find which webdriver instance is not null and use it navigate
        /// </summary>
        /// <param name="url"></param>
        public void GoToURL(string url)
        {
            IWebDriver webDriver = null;

            webDriver = GetDriverInstanace();
            if (webDriver != null)
            {
                webDriver.Navigate().GoToUrl(url);
            }
        }

        public void WaitUntilElementVisible(IWebDriver driver, string elementXpath)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.IgnoreExceptionTypes(typeof(ElementNotVisibleException), typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(elementXpath)));
        }

        /// <summary>
        /// Use ExpectedConditions ElementIsVisible which poles on availablity 500ms by default. This method tries for given time only not default implicit time.
        /// This method does not wait for given maximum time provided. It returns as soon as condition is met.
        /// </summary>
        /// <param name="driver">web driver instance</param>
        /// <param name="elementXpath">element xpath to be checked if visible</param>
        /// <param name="timeInSeconds">maximum time for which method waits for element to be available</param>
        /// <returns>Result of ExpectedConditions ElementIsVisible</returns>
        public bool IsElementVisible(IWebDriver driver, string elementXpath, int timeInSeconds)
        {
            var originalImplicitTimeOut = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeInSeconds);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(elementXpath)));
            }
            catch (Exception ex)
            {
                ex.GetType();
                driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
                return false;
            }

            driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
            return true;
        }

        public bool IsElementNotVisible(IWebDriver driver, string elementXpath, int timeInSeconds)
        {
            var originalImplicitTimeOut = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeInSeconds);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            try
            {
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(elementXpath)));
            }
            catch (Exception ex)
            {
                ex.GetType();
                driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
                return false;
            }

            driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
            return true;
        }

        /// <summary>
        /// Use ExpectedConditions FrameToBeAvailableAndSwitchToIt which poles on availablity to switch into frame every 500ms. This method tries for given time only not default implicit time.
        /// This method does not wait for given maximum time provided. It returns as soon as condition is met.
        /// </summary>
        /// <param name="driver">web driver instance</param>
        /// <param name="frameLocator">frame xpath to be checked if available for switch</param>
        /// <param name="timeInSeconds">maximum time for which method waits for element to be available</param>
        /// <returns>Result of ExpectedConditions FrameToBeAvailableAndSwitchToIt</returns>
        public bool IsFrameToBeAvailableAndSwitchToIt(IWebDriver driver, string frameLocator, int timeInSeconds)
        {
            var originalImplicitTimeOut = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeInSeconds);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            try
            {
                wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(frameLocator)));
            }
            catch (Exception ex)
            {
                var exception = ex.GetType();
                driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
                return false;
            }

            driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
            return true;
        }

        public bool IsElementClickable(IWebDriver driver, string elementXpath, int timeInSeconds)
        {
            var originalImplicitTimeOut = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeInSeconds);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(elementXpath)));
            }
            catch (Exception ex)
            {
                ex.GetType();
                driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
                return false;
            }

            driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
            return true;
        }

        public void WaitUntilElementInvisible(IWebDriver driver, string elementXpath)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            var element = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(elementXpath)));
        }

        public void WaitUntilElementExists(IWebDriver driver, string elementXpath)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            var element = wait.Until(ExpectedConditions.ElementExists(By.XPath(elementXpath)));
        }

        public void WaitUntilPageLoaded(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));
            wait.Until(
                d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public int GetDefaultElementFrame(IWebDriver driver, string elementXpath)
        {
            int frameNo = 0;
            int count = driver.FindElements(By.TagName("iframe")).Count;
            for (frameNo = 0; frameNo < count; frameNo++)
            {
                driver.SwitchTo().Frame(frameNo);
                int elementCnt = driver.FindElements(By.XPath(elementXpath)).Count;
                if (elementCnt > 0)
                {
                    break;
                }
            }

            driver.SwitchTo().DefaultContent();
            return frameNo;
        }

        /// <summary>
        /// Get web element in frame. This method waits on frame, elements to be loaded and interactable
        /// </summary>
        /// <param name="driver">web driver instance</param>
        /// <param name="elementXpath">element to be found in given frame</param>
        /// <param name="iframeXpath">frame xpath locator</param>
        /// <returns>web element in the given frame</returns>
        public IWebElement GetElementInFrameWithReload(IWebDriver driver, string elementXpath, string iframeXpath, int maxFrameWaitTimeInSeconds, int maxImplicitWait)
        {
            CommonSteps commonSteps = new CommonSteps();
            IWebElement webElement = null;

            driver.SwitchTo().DefaultContent();
            var isElementVisible = IsElementVisible(driver, iframeXpath, maxFrameWaitTimeInSeconds);
            var isElementClickable = IsElementClickable(driver, iframeXpath, maxFrameWaitTimeInSeconds);

            if (isElementVisible == false || isElementClickable == false)
            {
                PageReload(driver);
                webElement = GetElementInFrame(driver, elementXpath, iframeXpath, maxFrameWaitTimeInSeconds, maxImplicitWait);
            }
            else if (isElementVisible == true && isElementClickable == true)
            {
                webElement = GetElementInFrame(driver, elementXpath, iframeXpath, maxFrameWaitTimeInSeconds, maxImplicitWait);
            }

            return webElement;
        }

        /// <summary>
        /// Used to quickly locate element in the given frame. If more than one frames available then uses FindElements
        /// </summary>
        /// <param name="driver">web driver instance</param>
        /// <param name="elementXpath">element to be found in given frame</param>
        /// <param name="iframeXpath">frame xpath locator</param>
        /// <returns>web element in the given frame</returns>
        public IWebElement GetElementInFrame(IWebDriver driver, string elementXpath, string iframeXpath, int maxFrameWaitTimeInSeconds, int maxImplicitWait)
        {
            IWebElement element = null;
            var originalImplicitTimeOut = driver.Manage().Timeouts().ImplicitWait;
            Console.WriteLine("Original implicit timeout in GetElementInFrame method: {0}", originalImplicitTimeOut);
            var isFrameToBeAvailableAndSwitchToIt = IsFrameToBeAvailableAndSwitchToIt(driver, iframeXpath, maxFrameWaitTimeInSeconds);
            if (isFrameToBeAvailableAndSwitchToIt == true)
            {
                Console.WriteLine("Frame detected for switching into it");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(maxImplicitWait);
                Console.WriteLine("Implicit timeout in GetElementInFrame method set to: {0}", driver.Manage().Timeouts().ImplicitWait);
                var frames = driver.FindElements(By.XPath(iframeXpath));
                driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
                Console.WriteLine("Implicit timeout in GetElementInFrame method set back to: {0}", driver.Manage().Timeouts().ImplicitWait);
                Console.WriteLine("{0} frames detected for switching into it", frames.Count);
                if (frames.Count == 0)
                {
                    try
                    {
                        var isElementVisible = IsElementVisible(driver, elementXpath, 10);
                        var isElementClickable = IsElementClickable(driver, elementXpath, 10);
                        if (isElementVisible == true && isElementClickable == true)
                        {
                            Console.WriteLine("Element is visible and clickable");
                            element = driver.FindElement(By.XPath(elementXpath));
                        }
                    }
                    catch (Exception ex)
                    {
                        var exception = ex.GetType();
                        throw ex;
                    }
                }
                else if (frames.Count == 1)
                {
                    try
                    {
                        driver.SwitchTo().Frame(frames[0]);
                        var isElementVisible = IsElementVisible(driver, elementXpath, 10);
                        var isElementClickable = IsElementClickable(driver, elementXpath, 10);
                        if (isElementVisible == true && isElementClickable == true)
                        {
                            element = driver.FindElement(By.XPath(elementXpath));
                        }
                    }
                    catch (Exception ex)
                    {
                        var exception = ex.GetType();
                        throw ex;
                    }
                }
                else if (frames.Count > 1)
                {
                    foreach (var frame in frames)
                    {
                        driver.SwitchTo().Frame(frame);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(maxImplicitWait);
                        int elementCnt = driver.FindElements(By.XPath(elementXpath)).Count;
                        driver.Manage().Timeouts().ImplicitWait = originalImplicitTimeOut;
                        if (elementCnt > 0)
                        {
                            element = driver.FindElement(By.XPath(elementXpath));
                            break;
                        }
                    }
                }
            }
            
            return element;
        }

        /// <summary>
        /// Reload web page
        /// </summary>
        /// <param name="driver">web driver instance</param>
        private void PageReload(IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }

        /// <summary>
        /// Use ExpectedConditions FrameToBeAvailableAndSwitchToIt which poles on availablity 500ms by default
        /// </summary>
        /// <param name="driver">web driver instance</param>
        public void WaitTilliFrameLoaded(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(".//iframe")));
        }

        public void JavaScriptExecuteClick(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("arguments[0].click()", element);
        }

        public object JavaScriptGetFrameElements(IWebDriver driver)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            var iFrameList = javaScriptExecutor.ExecuteScript("return document.getElementsByTagName('iframe')");
            return iFrameList;
        }

        public string JavaScriptExecuteGetVal(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            var elementVal = javaScriptExecutor.ExecuteScript("retun arguments[0].value", element);
            return elementVal.ToString();
        }

        public bool JavaScriptExecuteGetChecked(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            var elementChecked = (bool)javaScriptExecutor.ExecuteScript("return arguments[0].checked", element);
            return elementChecked;
        }

        public bool JavaScriptIsElementVisible(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            bool elementVisible = (bool)javaScriptExecutor.ExecuteScript("return arguments[0].hidden", element);
            return elementVisible;
        }

        public bool JavaScriptIsElementVisible(IWebDriver driver, string jquery)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            var elementVisible = (bool)javaScriptExecutor.ExecuteScript(jquery);
            return elementVisible;
        }

        public void ValidateJsonResponseRecord(string expected, object actual, string key)
        {
            if (!string.Equals(expected, "ignore", StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(expected))
                {
                    Assert.IsNull(actual, "Field: " + key + " Expected:" + expected + " Actual:" + actual);
                }
                else
                {
                    Assert.IsNotNull(actual, "Field: " + key + " Expected:" + expected + " Actual:" + actual);
                    Assert.IsTrue(expected.Equals(actual.ToString(), StringComparison.CurrentCultureIgnoreCase), "Field: " + key + " Expected: " + expected + " Actual: " + actual);
                }
            }
            else
            {
                Console.WriteLine("validation ignored for Field: " + key);
            }
        }

        public bool RetryElementSearch(IWebDriver driver, string elementXpath)
        {
            bool available = false;
            for (int i = 0; i < 5; i++)
            {
                var isPresent = IsElementVisible(driver, elementXpath, 5);
                if (isPresent == true)
                {
                    WaitUntilElementVisible(driver, elementXpath);
                    available = true;
                    break;
                }
            }

            return available;
        }

        public bool RetryElementSearchPostReload(IWebDriver driver, string elementXpath)
        {
            bool available = false;
            for (int i = 0; i < 10; i++)
            {
                var isPresent = IsElementVisible(driver, elementXpath, 5);
                if (isPresent == true)
                {
                    WaitUntilElementVisible(driver, elementXpath);
                    available = true;
                    break;
                }

                driver.Navigate().Refresh();
            }

            return available;
        }

        public bool RetryElementSearchPostClick(IWebDriver driver, string parentElementXpath, string childElementXpath)
        {
            bool available = false;
            bool parentElementAvailable = RetryElementSearch(driver, parentElementXpath);

            if (parentElementAvailable == true)
            {
                var parentElement = driver.FindElement(By.XPath(parentElementXpath));
                for (int i = 0; i < 5; i++)
                {
                    parentElement.Click();
                    var isPresent = IsElementVisible(driver, childElementXpath, 5);
                    if (isPresent == true)
                    {
                        WaitUntilElementVisible(driver, childElementXpath);
                        available = true;
                        break;
                    }
                }
            }

            return available;
        }

        public void TakeScreenshot(IWebDriver driver, IObjectContainer objectContainer)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var screenshotTitle = DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");
            var scenarioInfo = objectContainer.Resolve<ScenarioInfo>();
            var scenarioTitle = scenarioInfo.Title;
            string screenshotfilename = scenarioTitle + "_" + screenshotTitle + ".jpg";
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string specificFolder = Path.Combine(folder, "Temp", "AutomatedTests", "Screenshots");
            if (!Directory.Exists(specificFolder))
            {
                Directory.CreateDirectory(specificFolder);
            }

            string filePath = Path.Combine(specificFolder, screenshotfilename);
            screenshot.SaveAsFile(filePath, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
        }

        public void UITestStabilizerWait(int sec)
        {
            int milisec = (int)TimeSpan.FromSeconds(sec).TotalMilliseconds;
            System.Threading.Thread.Sleep(milisec);
        }

        public IReadOnlyCollection<IWebElement> GetElementsInGivenTime(IWebDriver driver, string path, TimeSpan maxTimeSeconds)
        {
            var originalImplicitTime = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = maxTimeSeconds;
            var elements = driver.FindElements(By.XPath(path));
            driver.Manage().Timeouts().ImplicitWait = originalImplicitTime;
            return elements;
        }
    }
}
