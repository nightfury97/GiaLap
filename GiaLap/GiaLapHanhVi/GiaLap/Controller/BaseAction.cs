using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using GiaLap.Modal;
using OpenQA.Selenium.Firefox;
using SeleniumUndetectedChromeDriver;

namespace GiaLap.Controller
{
    public class BaseAction
    {
        public int Sleep()
        {
            Random rnd = new Random();
            int num = rnd.Next(1000, 5000);
            Thread.Sleep(num);
            return num;
        }
        public int Sleep(int num)
        {
            Thread.Sleep(num);
            return num;
        }

        public string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            if (File.Exists(tempDirectory))
            {
                return GetTemporaryDirectory();
            }
            else
            {
                Directory.CreateDirectory(tempDirectory);
                return tempDirectory;
            }
        }
        public static IWebDriver LoadProfileChrome(IWebDriver driver,string ID)
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var ProfileFolderPath = Path.GetFullPath(Path.Combine(outPutDirectory, @"..\..\bin\Debug\Profile"));
            if (driver != null)
            {
                try
                {
                    driver.Close();
                    driver.Quit();
                }
                catch (Exception ex)
                {
                }
            }
            ChromeOptions options = new ChromeOptions();
            if (!Directory.Exists(ProfileFolderPath))
            {
                Directory.CreateDirectory(ProfileFolderPath);
            }
            if (Directory.Exists(ProfileFolderPath))
            {
                options.AddArguments("user-data-dir=" + ProfileFolderPath + "\\" + ID);
            }
            driver = new ChromeDriver(options);
            return driver;
        }
        public static IWebDriver ChromeWebdriver(ProxyAuthent proxyAuth)
        {
            string ip = proxyAuth.ip + proxyAuth.port.ToString();

            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            var ExtentionFolderPath = Path.GetFullPath(Path.Combine(outPutDirectory, @"..\..\bin\Debug\Extention"));
            
            var chromeDriverPath = Path.GetFullPath(Path.Combine(outPutDirectory, @"..\..\bin\Debug\chromedriver_win32\1.exe"));
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");
            if (!string.IsNullOrEmpty(ip))
            {
                if (!string.IsNullOrEmpty(proxyAuth.username) && !string.IsNullOrEmpty(proxyAuth.pass))
                {
                    options.AddExtension(ExtentionFolderPath + "\\Proxy-Auto-Auth.crx");
                }
                options.AddArgument(string.Format("--proxy-server={0}", ip));
            }
            var driver = UndetectedChromeDriver.Create(options,
            driverExecutablePath: chromeDriverPath,
            // hide selenium command prompt window  
            hideCommandPromptWindow: true);

            //var service = ChromeDriverService.CreateDefaultService(chromeDriverPath);
            //service.HideCommandPromptWindow = true;
            //var driver = new ChromeDriver(service,options);

            if (!string.IsNullOrEmpty(ip))
            {
                if (!string.IsNullOrEmpty(proxyAuth.username) && !string.IsNullOrEmpty(proxyAuth.pass))
                {
                    driver.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                    driver.Navigate();

                    driver.FindElement(By.Id("login")).SendKeys(proxyAuth.username);
                    driver.FindElement(By.Id("password")).SendKeys(proxyAuth.pass);
                    driver.FindElement(By.Id("retry")).Clear();
                    driver.FindElement(By.Id("retry")).SendKeys("2");

                    driver.FindElement(By.Id("save")).Click();
                }
            }
            return driver;
        }
        
        public static void Hover(IWebDriver driver, IWebElement element)
        {
            Actions action = new Actions(driver);

            try
            {
                action.MoveToElement(element);
            }
            catch
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, " + (element.Location.Y - 150).ToString() + ")");
                action.MoveToElement(element);
            }
        }
        public static void Move(IWebDriver driver, int X, int Y)
        {
            Actions action = new Actions(driver);

            try
            {
                action.MoveByOffset(X, Y);
            }
            catch
            {

            }
        }
        public static void ScrollToTheBottom(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }
        public static void ScrollToPoint(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("window.scrollTo({\r\n  top: 100,\r\n  left: 100,\r\n  behavior: \"smooth\",\r\n});");
        }
        public static string GetCookiesAsString(IWebDriver driver)
        {
            var cookies = driver.Manage().Cookies.AllCookies;
            return System.Text.Json.JsonSerializer.Serialize(cookies, new JsonSerializerOptions { WriteIndented = true });
        }
        public static void SetCookies(IWebDriver driver, string json)
        {
            var cookies = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            foreach (var c in cookies)
            {
                string name = c.Name;
                string value = c.Value;
                string domain = c.Domain;
                string path = c.Path;
                DateTime? expiry = c.Expiry;
                bool secure = c.Secure;
                bool isHttpOnly = c.IsHttpOnly;
                string sameSite = c.SameSite;

                var cookie = new OpenQA.Selenium.Cookie(name, value, domain, path, expiry, secure, isHttpOnly, sameSite);

                driver.Manage().Cookies.AddCookie(cookie);
            }
        }
        public static bool MatchStringsPartial(string string1, string string2)
        {
            return string2.ToLower().Trim().Contains(string1.ToLower().Trim());
        }
        public static bool MatchStringsFull(string string1, string string2)
        {
            return string2.ToLower().Trim().Equals(string1.ToLower().Trim());
        }
        public static bool CheckHeight(IWebDriver driver, string oldHeight)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var result = js.ExecuteScript("return document.body.scrollHeight");
            string newHeight = result.ToString();
            return newHeight != oldHeight;
        }
        public static void Scroll(int total_scrolls, IWebDriver driver, int scroll_time = 10, int scroll_delay = 1)
        {
            string old_height;
            int current_scrolls = 0;

            while (true)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                try
                {
                    if (current_scrolls == total_scrolls) { break; }
                    old_height = js.ExecuteScript("return document.body.scrollHeight").ToString();
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                    wait.Until(x => CheckHeight(driver, old_height));
                }
                catch { }
            }
        }
        public static void NewTab(IWebDriver driver)
        {
            driver.SwitchTo().NewWindow(WindowType.Tab);
        }
        public static void NewWindows(IWebDriver driver)
        {
            driver.SwitchTo().NewWindow(WindowType.Window);
        }
        public static void SwitchToTab(IWebDriver driver, string originalWindow)
        {
            List<string> tabs = driver.WindowHandles.ToList();
            foreach (string window in driver.WindowHandles)
            {
                if (originalWindow != window)
                {
                    driver.SwitchTo().Window(window);
                    break;
                }
            }
        }
        public static void CloseTab(IWebDriver driver, string originalWindow)
        {
            driver.Close();
            //Switch back to the old tab or window
            driver.SwitchTo().Window(originalWindow);
        }
        public static IWebElement FindElementByID(IWebDriver driver, string ID)
        {
            try
            {
                return driver.FindElement(By.Id(ID));
            }
            catch
            {
                return null;
            }
        }
        public static IWebElement FindElementByXpath(IWebDriver driver, string xpath)
        {
            try
            {
                return driver.FindElement(By.XPath(xpath));
            }
            catch
            {
                return null;
            }
        }
        public static void PressEnter(IWebElement target)
        {
            target.SendKeys(OpenQA.Selenium.Keys.Enter);
        }
        public static void PressSpace(IWebElement target)
        {
            target.SendKeys(OpenQA.Selenium.Keys.Space);
        }

    }
}
