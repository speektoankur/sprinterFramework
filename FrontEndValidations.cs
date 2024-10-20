using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests;

[TestClass]
public class FETest : PageTest
{  
    [TestInitialize]
    public async Task SetUpTracing()
        {
            // Start tracing before running the test
            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Title = $"{TestContext.FullyQualifiedTestClassName}.{TestContext.TestName}",
                Screenshots = true,    
                Snapshots = true,      
                Sources = true         
            });
        }


    [TestMethod]
    [TestCategory("UI")]
    [DataRow("IFRS 17", "Results for the year ended 31 December 2023")]
    [DataRow("IFRS 17", "Interim results for the six months ended 30 June 2022")]
    [DataRow("IFRS 17", "Interim Report 2023")]
    [DataRow("IFRS 17", "John King")]
    public async Task ValidateSearchResults(String searchInput, String expectedResultLabel)
    {
        await Page.GotoAsync("https://www.britinsurance.com/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await Page.Locator("//button[@aria-label='Search button']").ClickAsync();
        await Page.GetByPlaceholder("Search for people, services").FillAsync(searchInput);
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Expect(Page.GetByText("Search results matching")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = expectedResultLabel })).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("UI")]
    [DataRow("IFRS 17", 8)]
    public async Task ValidateNumberOfSearchResults(String searchInput, int expectedCountOfResults)
    {
        await Page.GotoAsync("https://www.britinsurance.com/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await Page.Locator("//button[@aria-label='Search button']").ClickAsync();
        await Page.GetByPlaceholder("Search for people, services").FillAsync(searchInput);
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Expect(Page.GetByText("Search results matching")).ToBeVisibleAsync();
        var elements = Page.Locator(".s-results__tag");
        int noOfResults = await elements.CountAsync();
        Assert.AreEqual(expectedCountOfResults, noOfResults);
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await Context.Tracing.StopAsync(new()
        {
            Path = Path.Combine(
                Environment.CurrentDirectory,
                "playwright-traces",
                $"{TestContext.FullyQualifiedTestClassName}.{TestContext.TestName}.zip"
            )
        });
    }
}