using Microsoft.Playwright;
using System.Globalization;


public class MainPage
{
    private readonly IPage _page;
    private readonly ILocator _searchField;
    private readonly ILocator _cityheader;

    public MainPage(IPage page)
    {
        _page = page;
        _searchField = page.Locator("#searchField");
        _searchField = page.Locator("#searchField");
        _cityheader = page.Locator("h1.currentCity");
    }

    public async Task Open()
    {
        await _page.GotoAsync("https://sinoptik.bg");
    }

    public async Task SearchCity(string text)
    {
        await _searchField.FillAsync(text);
        // Needed to trigger the search
        await _searchField.DispatchEventAsync("keyup");

        await _page.Locator("div.autocomplete > a").First.ClickAsync();
    }

    public async Task VerifyCityHeader(string cityName)
    {
        await Assertions.Expect(_cityheader).ToHaveTextAsync(cityName);
    }

    public async Task ChooseForecast(string forecastName)
    {
        await _page.GetByText(forecastName).ClickAsync();
    }

    public ILocator GetElementsLocator(string locator)
    {
        return _page.Locator(locator);
    }

    public async Task AssertLocatorCount(ILocator dayElementsLocator, ILocator dateElementsLocator)
    {
        await Assertions.Expect(dayElementsLocator).ToHaveCountAsync(14);
        await Assertions.Expect(dateElementsLocator).ToHaveCountAsync(14);
    }

    public async Task AssertDaysOfWeekAndDates(ILocator dayElementsLocator, ILocator dateElementsLocator)
    {
        var dayElements = await dayElementsLocator.AllAsync();
        var dateElements = await dateElementsLocator.AllAsync();

        // Verify each date and day
        for (int i = 0; i < 14; i++)
        {
            var actualDate = dateElements[i];
            var actualDay = dayElements[i];

            var date = DateTime.Today.AddDays(i);
            var expectedDate = date.ToString("d MMMM", new CultureInfo("bg-BG")); //(e.g., "23.03")

            var dayName = date.ToString("ddd", new CultureInfo("bg-BG"));
            var expectedDayName = char.ToUpper(dayName[0]) + dayName.Substring(1) + ".";

            await Assertions.Expect(actualDate).ToHaveTextAsync(expectedDate);
            await Assertions.Expect(actualDay).ToHaveTextAsync(expectedDayName);
        }
    }
}