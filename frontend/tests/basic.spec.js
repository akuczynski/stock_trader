// @ts-check
const { test, expect } = require('@playwright/test');

const appUrl = 'https://zealous-hill-05a313d03.3.azurestaticapps.net/';

test('has title', async ({ page }) => {
  await page.goto(appUrl);

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Trader App/);
});
 
test(' data range', async ({ page }) => {
  await page.goto(appUrl);

  const date = new Date("2023-06-12T08:00:00"); 

  let fromDate = date.getDate().toString();
  let toDate = (date.getDate() + 1).toString(); 

  // Pick the date range 
  await page.getByRole('combobox', { name: 'Date range' }).click();
  
  await page.getByText(fromDate, { exact: true }).click();
  await page.getByText(toDate, { exact: true }).click();
  await page.getByRole('button', { name: 'Done' }).click();
  await page.getByRole('button', { name: 'Apply' }).click();
  
  // order by Success column 
  await page.getByText('Success').click();
  await page.getByText('Success1').click();

  await page.getByRole('gridcell', { name: '06/12/2023 08:00:01' }).click();

  // check that AI ticker has been loaded to the 2nd table 
  const locator = page.getByRole('gridcell', { name: 'AI' });
  await expect(locator).toBeAttached();
});
 