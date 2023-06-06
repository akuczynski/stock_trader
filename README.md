# About

Simple Az Function to fetch data from the external REST API.

# Testing 

For local testing I have used [json server](https://www.npmjs.com/package/json-server)

Run Json server locally (workdir is resources folder): 
>json-server --watch reddit.json

# ToDO

- check timestamp (local/utc time zone information) 
- configure connection string 
- deploy to Azure 
- change cron expression for import Func 