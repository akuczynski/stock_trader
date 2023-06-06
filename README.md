# About

Simple Az Function to fetch data from the external REST API.

# Testing 

For local testing I have used [json server](https://www.npmjs.com/package/json-server)

Run Json server locally (workdir is resources folder): 
>json-server --watch reddit.json

## ToDO

- check timestamp (local/utc time zone information) 
- configure connection string 
- deploy to Azure 
- change cron expression for import Func 

### Interesting HOWTO arcticles

- [Az Functions Input validations](https://www.tomfaltesek.com/azure-functions-input-validation/)
- [How To set dynamically blob name](https://www.davidguida.net/how-to-dynamically-set-blob-name-in-an-azure-function/)
- [FakeItEasy tutorial](https://techmindfactory.com/Easy-mocking-in-C-code-with-FakeItEasy-library/) 
