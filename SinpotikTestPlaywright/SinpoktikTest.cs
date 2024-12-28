using Microsoft.Playwright;

namespace SinpotikTestPlaywright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class SinpoktikTest : PageTest
    {
        private MainPage _mainPage;

        public const string CityName = "Варна";

        public const string Fourteendays = "14-дневна";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Environment.SetEnvironmentVariable("HEADED", "1");
        }

        [SetUp]
        public async Task SetUp()
        {
            var cookie = new Cookie
            {
                Name = Constants.CookieConsentName,
                Value = Constants.CookieConsentValue,
                Domain = Constants.CookieConsentDomain,
                Path = "/",
                HttpOnly = false,
                Secure = true
            };

            await Context.AddCookiesAsync([cookie]);

            _mainPage = new MainPage(Page);
        }

        [Test]
        public async Task CorrectDaysOfWeek_When_EnterCityInDropdown()
        {
            await _mainPage.Open();

            await _mainPage.SearchCity(CityName);

            await _mainPage.VerifyCityHeader(CityName);

            await _mainPage.ChooseForecast(Fourteendays);

            var dayElementsLocator = _mainPage.GetElementsLocator("span.wf10dayRightDay");
            var dateElementsLocator = _mainPage.GetElementsLocator("span.wf10dayRightDate");

            await _mainPage.AssertLocatorCount(dayElementsLocator, dateElementsLocator);

            await _mainPage.AssertDaysOfWeekAndDates(dayElementsLocator, dateElementsLocator);
        }
    }
}
