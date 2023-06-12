// @ts-check
const { test, expect } = require('@playwright/test');

const appUrl = 'https://zealous-hill-05a313d03.3.azurestaticapps.net/';

test('has title', async ({ page }) => {
  await page.goto(appUrl);

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Trader App/);
});

 
test('get data range', async ({ page }) => {
  await page.goto(appUrl);

  const date = new Date(); 

  let today = date.getDate().toString();
  let tomorrow = (date.getDate() + 1).toString(); 

  // Click the get started link.
  await page.getByRole('combobox', { name: 'Date range' }).click();
  
  await page.getByText(today, { exact: true }).click();
  await page.getByText(tomorrow, { exact: true }).click();

  await page.getByRole('button', { name: 'Done' }).click();
  // Expects the URL to contain intro.

//  var table = $$("maintable");

  // await expect(page).toHaveURL(/.*intro/);
});
 