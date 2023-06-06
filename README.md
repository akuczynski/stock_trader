# About

This project contains implementation of the three simple Az Functions. 
This was a technical task of the one company to which I had applied.  

Shory summary of the Az Funcs: 
- <b>ImportExternalData</b> - this is Time Triggered function, it is responsible for fetching data from the external REST API 
- <b>GetAllLogs</b> - GET API call to list all logs for the specific time period (from/to) 
- <b>GetLog</b> - GET API call to fetch a payload from blob for the specific log entry


## Local testing 

For local testing I have used [json server](https://www.npmjs.com/package/json-server)

Run Json server locally (working directory must be resources folder): 
>json-server --watch reddit.json

Important remark: Some of the unit tests (it is event better to call them integration tests) 
require to run AzFunction project first. Those tests works well with the data hosted by the json server on the 
local PC.


### Interesting HOW TO arcticles

- [Az Functions Input validations](https://www.tomfaltesek.com/azure-functions-input-validation/)
- [How To set dynamically blob name](https://www.davidguida.net/how-to-dynamically-set-blob-name-in-an-azure-function/)
- [FakeItEasy tutorial](https://techmindfactory.com/Easy-mocking-in-C-code-with-FakeItEasy-library/) 
- [About DateTime conversions in C#](https://itecnote.com/tecnote/c-parse-a-date-string-into-a-certain-timezone-supporting-daylight-saving-time/)