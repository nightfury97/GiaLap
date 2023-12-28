using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumProxyAuth.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SeleniumProxyAuth;

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
        public string BackgroundJs(string host, string port, string username, string password)
        {
            string data = File.ReadAllText("test.txt");

            string tempDirectory = GetTemporaryDirectory();
            string BackgroundJs = data.Replace("%host", host).Replace("%port", port).Replace("%username", username).Replace("%password", password);
            File.WriteAllText(tempDirectory+"test.txt", BackgroundJs);
            return BackgroundJs;
        }
        //public static string BackgroundJs(string host, string port, string username, string password)
        //{
        //    string data = @"
        //    var config = {            
        //        mode: 'fixed_servers',
        //        rules:
        //        {
        //        singleProxy:
        //            {
        //            scheme: 'http',
        //            host: '%host',
        //            port: %port
        //        },
        //        bypassList:['localhost']
        //        }
        //    };
        //    chrome.proxy.settings.set({value: config, scope: 'regular'}, function() { });
        //    function callbackFn(details)
        //    {
        //        return {
        //        authCredentials:{
        //                username: '%username',
        //                password: '%password'
        //            }
        //        };
        //    }
        //    chrome.webRequest.onAuthRequired.addListener(
        //                callbackFn,
        //                {urls: ['<all_urls>']
        //    }, ['blocking']);
        //    ";
        //    string BackgroundJs = data.Replace("%host", host).Replace("%port", port).Replace("%username", username).Replace("%password", password);
        //    return BackgroundJs;
        //}
        //public static void CreateJsFile(string fileName, string content)
        //{
        //    // Tạo một đối tượng `FileStream` để ghi vào file.
        //    FileStream fs = new FileStream(fileName, FileMode.Create);

        //    // Ghi nội dung vào file.
        //    fs.Write(Encoding.Default.GetBytes(content), 0, content.Length);

        //    // Đóng file.
        //    fs.Close();
        //}
        public void Test()
        {
            // Create a local proxy server
            var proxyServer = new SeleniumProxyServer();

            // Don't await, have multiple drivers at once using the local proxy server
            TestSeleniumProxyServer(proxyServer, new ProxyAuth("123.123.123.123", 80, "prox-username1", "proxy-password1"));
            TestSeleniumProxyServer(proxyServer, new ProxyAuth("11.22.33.44", 80, "prox-username2", "proxy-password2"));
            TestSeleniumProxyServer(proxyServer, new ProxyAuth("111.222.222.111", 80, "prox-username3", "proxy-password3"));

            while (true) { }
        }
        public static IWebDriver ChromeWebdriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--mute-audio");
            options.AddArgument(
                "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 Edg/92.0.902.62");


            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            options.AddArgument("ignore-certificate-errors");
            IWebDriver driver = new ChromeDriver(service,options);
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
