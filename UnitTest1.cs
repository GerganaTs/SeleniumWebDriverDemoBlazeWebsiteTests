global using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumWebDriverDemoBlazeWebsiteTests
{
    public class Tests
    {
        private WebDriver driver;
        private const string BaseUrl = "https://www.demoblaze.com/";
        private DateTime randomString = DateTime.Now;
        IWebElement homeLink;
        IWebElement contactLink;
        IWebElement aboutUsLink;
        IWebElement cartLink;
        IWebElement loginLink;
        IWebElement signUpLink;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(BaseUrl);
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            homeLink = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".navbar-nav > .nav-item:nth-child(1) .nav-link")));
            contactLink = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".navbar-nav > .nav-item:nth-child(2) .nav-link")));
            aboutUsLink = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".navbar-nav > .nav-item:nth-child(3) .nav-link")));
            cartLink = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".navbar-nav > .nav-item:nth-child(4) .nav-link")));
            loginLink = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".navbar-nav > .nav-item:nth-child(5) .nav-link")));
            signUpLink = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("signin2")));
        }

        [OneTimeTearDown]
        public void CLoseBrowser()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        public void Test_WebsiteHomepageTitle()
        {
            var title = driver.Title;
            Assert.That(title, Is.EqualTo("STORE"));
        }

        [Test, Order(2)]
        public void Test_WebsiteNavLinksText()
        {
            Assert.That(homeLink.Text, Is.EqualTo("Home\r\n(current)"));
            Assert.That(contactLink.Text, Is.EqualTo("Contact"));
            Assert.That(aboutUsLink.Text, Is.EqualTo("About us"));
            Assert.That(cartLink.Text, Is.EqualTo("Cart"));
            Assert.That(loginLink.Text, Is.EqualTo("Log in"));
            Assert.That(signUpLink.Text, Is.EqualTo("Sign up"));
        }

        [Test, Order(3)]
        public void Test_ShowSignInAndLogIn()
        {
            signUpLink.Click();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var signUpBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("signInModal")));
            var signUpBoxTitle = driver.FindElement(By.Id("signInModalLabel"));

            Assert.True(signUpBox.Displayed);
            Assert.That(signUpBoxTitle.Text, Is.EqualTo("Sign up"));

            var random = randomString.ToString() + "User";
            var username = random.Replace(" ", "");
            var signInUsernameField = driver.FindElement(By.Id("sign-username"));
            var signInPasswordField = driver.FindElement(By.Id("sign-password"));
            var singUpBtn = driver.FindElement(By.CssSelector("#signInModal .btn-primary"));

            signInUsernameField.SendKeys(username);
            signInPasswordField.SendKeys("pass123");
            singUpBtn.Click();

            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(alert.Text, Is.EqualTo("Sign up successful."));
            alert.Accept();

            loginLink.Click();
            
            var logInBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("logInModal")));
            var logInBoxTitle = driver.FindElement(By.Id("logInModalLabel"));
            Assert.True(logInBox.Displayed);
            Assert.That(logInBoxTitle.Text, Is.EqualTo("Log in"));

            var logInUsernameField = driver.FindElement(By.Id("loginusername"));
            var logInPasswordField = driver.FindElement(By.Id("loginpassword"));
            var logInBtn = driver.FindElement(By.CssSelector("#logInModal .btn-primary"));
            logInUsernameField.SendKeys(username);
            logInPasswordField.SendKeys("pass123");

            logInBtn.Click();
            
            var loggedInUserMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nameofuser")));
            Assert.That(loggedInUserMessage.Text, Is.EqualTo("Welcome " + username));
        }

        [TestCase("#tbodyid > div:nth-child(1) h4 a", "Samsung galaxy s6", "#tbodyid > div:nth-child(1) h5","$360"), Order(4)]
        [TestCase("#tbodyid > div:nth-child(2) h4 a", "Nokia lumia 1520", "#tbodyid > div:nth-child(2) h5", "$820")]
        [TestCase("#tbodyid > div:nth-child(3) h4 a", "Nexus 6", "#tbodyid > div:nth-child(3) h5", "$650")]
        [TestCase("#tbodyid > div:nth-child(4) h4 a", "Samsung galaxy s7", "#tbodyid > div:nth-child(4) h5", "$800")]
        [TestCase("#tbodyid > div:nth-child(5) h4 a", "Iphone 6 32gb", "#tbodyid > div:nth-child(5) h5", "$790")]
        [TestCase("#tbodyid > div:nth-child(6) h4 a", "Sony xperia z5", "#tbodyid > div:nth-child(6) h5", "$320")]
        [TestCase("#tbodyid > div:nth-child(7) h4 a", "HTC One M9", "#tbodyid > div:nth-child(7) h5", "$700")]
        [TestCase("#tbodyid > div:nth-child(8) h4 a", "Sony vaio i5", "#tbodyid > div:nth-child(8) h5", "$790")]
        [TestCase("#tbodyid > div:nth-child(9) h4 a", "Sony vaio i7", "#tbodyid > div:nth-child(9) h5", "$790")]
        public void Test_ProductsTitle(string deviceNamePath, string deviceName, string devicePricePath, string devicePrice)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var productTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(deviceNamePath)));
            var productPrice = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(devicePricePath)));
            Assert.That(productTitle.Text, Is.EqualTo(deviceName));
            Assert.That(productPrice.Text, Is.EqualTo(devicePrice));
        }

        [Test, Order(5)]
        public void Test_PickupProductAndOpenItsPage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var firstProductTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#tbodyid > div:nth-child(2) h4 a")));
            firstProductTitle.Click();
            var getPageLink = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("product-deatil")));
            string url = this.driver.Url;
            Assert.That(url, Is.EqualTo("https://www.demoblaze.com/prod.html?idp_=2"));

            var productName = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("name")));
            var productPrice = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("price-container")));
            var productDescription = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#myTabContent p")));
            var addToCart = driver.FindElement(By.CssSelector("#tbodyid .btn"));

            Assert.That(productName.Text, Is.EqualTo("Nokia lumia 1520"));
            Assert.That(productPrice.Text, Is.EqualTo("$820 *includes tax"));
            Assert.That(productDescription.Text, Is.EqualTo("The Nokia Lumia 1520 is powered by 2.2GHz quad-core Qualcomm Snapdragon 800 processor and it comes with 2GB of RAM."));
            
            addToCart.Click();
            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
            Assert.That(alert.Text, Is.EqualTo("Product added."));
            alert.Accept();
        }
    }
}