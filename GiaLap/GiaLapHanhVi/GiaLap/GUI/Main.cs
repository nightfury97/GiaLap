using MetroFramework.Forms;
using OpenQA.Selenium;
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
            driver = new FirefoxDriver();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string originalWindow = driver.CurrentWindowHandle;
            ClickLinkNewTab(driver, originalWindow);
        }
    }
}
