using System.Security.Cryptography;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace first_test;

public class Tests
{

    public IWebDriver driver;
    public WebDriverWait wait;


        [SetUp]
    public void Setup() 
    {
       driver = new ChromeDriver();
    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3); 
    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
    }


        [TearDown]
    public void TearDown()
    {
       driver.Quit();
        driver.Dispose();
    }   

        
        private void Authorization()
    {
        
    driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/"); 

    var login = driver.FindElement(By.Id("Username"));
    login.SendKeys("diana192746@gmail.com");

    var password = driver.FindElement(By.Id("Password"));
    password.SendKeys("Busobe40!!!");

    var enter = driver.FindElement(By.Name("button"));
    enter.Click();

    wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid= 'Title']")));
    
    }


    [Test]
    public void AuthorizationTest()
    {  
    Authorization();

    Assert.That(driver.Title, Does.Contain("Новости"), "Не удалось перейти на страницу Новости");
    }


    [Test]
public void NavigateMenuTest()
{

Authorization();

var SidebarMenuBotton = driver.FindElement(By.CssSelector("[data-tid= 'SidebarMenuButton']"));
SidebarMenuBotton.Click();

wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid= 'SidePage__root']")));

var community = driver.FindElements(By.CssSelector("[data-tid= 'Community']"))
    .First(element => element.Displayed); //фильтрация
community.Click();

wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/communities"));
var titlePageElement = driver.FindElement(By.CssSelector("[data-tid= 'Title']"));

Assert.That(titlePageElement.Text, Does.Contain("Сообщества"), "При переходе на вкладку Сообщество не смогли найти заголовок Сообщество");
}


    [Test]
    public void SearchTest()
    {
    Authorization();

    var search = driver.FindElement(By.CssSelector("[data-tid = 'SearchBar']")); //поиск строки поиска
    search.Click();

    var searchInput = driver.FindElement(By.CssSelector("[placeholder = 'Поиск сотрудника, подразделения, сообщества, мероприятия']"));
    searchInput.SendKeys("Медведева Диана Владимировна");

    Assert.That(searchInput.GetAttribute("value"), Does.Contain("Медведева Диана Владимировна"), "Поле поиска должно содержать введенное значение");
    }



    [Test]
    public void СreateComment()
    {
Authorization();

    var clickComm = driver.FindElement(By.CssSelector("[placeholder='Комментировать...']"));
    clickComm.Click();

    var writeComment = driver.FindElement(By.CssSelector("[placeholder='Комментировать...']"));
     writeComment.SendKeys("comm");

    var sendComment = driver.FindElement(By.CssSelector("[data-tid = 'SendComment']"));
    sendComment.Click();

    wait.Until(driver => driver.PageSource.Contains("comm"));

     Assert.That(driver.PageSource.Contains("comm"), "Комментарий 'comm' не отображается на странице после отправки");

    }



    [Test]
    public void CreateCommunity()
    {

    Authorization();

    driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");

    var createButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'СОЗДАТЬ')]")));
    createButton.Click();

    var writeName = driver.FindElement(By.CssSelector("[data-tid='Name']"));
     writeName.SendKeys("Сообщество");

    var createCommunity = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
     createCommunity.Click();

     wait.Until(driver => driver.PageSource.Contains("Сообщество"));

     Assert.That(driver.PageSource.Contains("Сообщество"), "Сообщество не отображается на странице после создаия");
    }

    }