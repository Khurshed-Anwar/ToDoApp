using Microsoft.Playwright;
using Xunit;

namespace ToDoApp.Tests;

public class TodoSmokeTests
{
    private const string BaseUrl = "https://todolist-hzgwbseffuh7gdg8.australiaeast-01.azurewebsites.net/";

    [Fact]
    public async Task TodoApp_HomePage_LoadsSuccessfully()
    {
        using var playwright = await Playwright.CreateAsync();

        await using var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = true
            });

        var page = await browser.NewPageAsync(
            new BrowserNewPageOptions
            {
                IgnoreHTTPSErrors = true
            });

        var response = await page.GotoAsync(BaseUrl);

        Assert.NotNull(response);
        Assert.True(response!.Ok);

        Assert.Contains("ToDoApp", await page.TitleAsync());
    }

    [Fact]
    public async Task CreateTodo_CreatesNewTaskSuccessfully()
    {
        using var playwright = await Playwright.CreateAsync();

        await using var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = true
            });

        var page = await browser.NewPageAsync(
            new BrowserNewPageOptions
            {
                IgnoreHTTPSErrors = true
            });

        // Open Create Task page
        await page.GotoAsync($"{BaseUrl}/Todo/Create");

        // Enter task
        await page.Locator("#Task").FillAsync("Automated Test Task");

        // Enter due date
        await page.Locator("#DueDate").FillAsync("2026-12-31");

        // Select Priority
        await page.Locator("#Priority").SelectOptionAsync(new[] { "1" });

        // Click Save
        await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

        // Verify we returned to the Todo list
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify the created task is displayed
        var task = page.GetByText("Automated Test Task");

        Assert.True(
            await task.CountAsync() > 0,
            "The newly created Todo was not displayed."
        );
    }
}