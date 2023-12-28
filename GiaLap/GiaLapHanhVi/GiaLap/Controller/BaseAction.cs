using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuetBaiDang.Controller
{
    public class BaseAction
    {
        
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
        public static bool CheckHeight(IWebDriver driver, int oldHeight)
        {
            int newHeight = (int)driver.ExecuteScript("return document.body.scrollHeight");
            return newHeight != oldHeight;
        }
        public static void Scroll(int total_scrolls, IWebDriver driver, int scroll_time= 10, int scroll_delay= 1)
        {
            int old_height = 0;
            int current_scrolls = 0;
            var cookies = driver.Manage().Cookies.AllCookies;
            
        }
    }
}
