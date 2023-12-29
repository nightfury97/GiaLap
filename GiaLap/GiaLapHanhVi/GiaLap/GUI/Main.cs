using MetroFramework.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GiaLap.Controller.BaseAction;
using SeleniumProxyAuth;
using SeleniumProxyAuth.Models;
using System.IO;
using System.Reflection;
using GiaLap.Modal;

namespace GiaLap.GUI
{
    public partial class Main : MetroForm
    {
        public Main()
        {
            InitializeComponent();
        }
        IWebDriver driver = null;
        private void button1_Click(object sender, EventArgs e)
        {
            //driver = new FirefoxDriver();
        }
        private void TestSeleniumProxyServer(SeleniumProxyServer proxyServer, ProxyAuth auth)
        {
            //// Add a new local proxy server endpoint
            //var localPort = proxyServer.AddEndpoint(auth);

            //var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
           
            //ChromeOptions options = new ChromeOptions();
            ////options1.AddArguments("headless");
            //options.AddArgument("--no-sandbox");
            //options.AddArgument("--disable-gpu");
            //options.AddArgument("--disable-notifications");
            //options.AddArgument("--disable-infobars");
            //options.AddArgument("--disable-extensions");
            //options.AddArgument("--mute-audio");
            ////options.AddExtension("E:/Git/GiaLap/GiaLap/GiaLapHanhVi/GiaLap/bin/Debug/extention.crx");
            //// Configure the driver's proxy server to the local endpoint port
            //options.AddArgument($"--proxy-server=198.46.246.89:6713");
            //options.AddArgument("ignore-certificate-errors");
            //// Optional
            //var service = ChromeDriverService.CreateDefaultService(chromeDriverPath);
            //service.HideCommandPromptWindow = true;

            //// Create the driver
            //var driver = new ChromeDriver(service, options);

            //// Test if the driver is working correctly
            //driver.Navigate().GoToUrl("https://www.myip.com/");

            ////driver.Navigate().GoToUrl("https://amibehindaproxy.com/");
            

            // Dispose the driver
            //driver.Dispose();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string ip = "198.46.246.89:6713";
            string username = "xxmlerfa";
            string pass = "wjxj72ylljeo";
            ProxyAuthent proxyAuthent = new ProxyAuthent("198.46.246.89",6713, "xxmlerfa", "wjxj72ylljeo");


            driver = ChromeWebdriver(proxyAuthent);
            driver.Navigate().GoToUrl("https://nowsecure.nl");
        }
    }
    
}
