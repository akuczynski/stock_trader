# About

This project contains implementation of the three simple Az Functions. 
This was a technical task of the one company to which I had applied.  

Short summary of the Az Funcs: 
- <b>ImportExternalData</b> - this is Time Triggered function, it is responsible for fetching data from the external REST API 
- <b>GetAllLogs</b> - GET API call to list all logs for the specific time period (from/to) 
- <b>GetLog</b> - GET API call to fetch a payload from blob for the specific log entry


## Local testing 

### Backend

For local testing I have used [json server](https://www.npmjs.com/package/json-server)
This allowed me to be indepented of accessibility of the external REST API. 

Run Json server locally (working directory must be resources folder): 
>json-server --watch reddit.json

Important remark: Some of the tests (integration tests) 
require to run AzFunction project first. Those tests works well with the data hosted by the json server.

### Frontend 

Install [Live Server extension](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) to VS Code. 
Open 'frontend' folder and run the project ('Go Live' option). 

In Chrome add [Allow CORS: Access-Control-Allow-Origin extension](https://chrome.google.com/webstore/detail/allow-cors-access-control/lhobafahddgcelffkeicbaginigeejlf?utm_source=googleads&utm_source=googleads&utm_campaign=19594216058&utm_medium=cpc&gclid=CjwKCAjwhJukBhBPEiwAniIcNS-ZaIZESXN_DWZsWgKcM9Uze5u85mQ5gHG-WFPaeltb1W8KwU8ZVhoCakQQAvD_BwE) to enable CORS on your localhost. 
This is required as frontend and backend runs on different ports. 



### Interesting HOW TO arcticles

- [Az Functions Input validations](https://www.tomfaltesek.com/azure-functions-input-validation/)
- [How To set dynamically blob name](https://www.davidguida.net/how-to-dynamically-set-blob-name-in-an-azure-function/)
- [FakeItEasy tutorial](https://techmindfactory.com/Easy-mocking-in-C-code-with-FakeItEasy-library/) 
- [About DateTime conversions in C#](https://itecnote.com/tecnote/c-parse-a-date-string-into-a-certain-timezone-supporting-daylight-saving-time/)