<p align="center">
    <h1 align="center">Status Page</h1>
    <h3 align="center">Simple yet powerful status page that allows you to show and configure 
    different services all in one beautiful and material page</h3>
    <br>
    <img src="https://cdn.argonaut.pw/file/5fc23499-025f-4626-a00e-128182c6636c.png" />
    <img src="https://cdn.argonaut.pw/file/e42eac8a-1013-40f2-a53d-7613df58fc28.png" />
</p>

# Simple from the ground up
Status Page was built from the ground up to be extremly easy to use but also to host. It does not require a database or any additional services. It runs off a single entities.json config file for the statuses and the appsettings.json file for general service settings. 

# Configure Services to work with Status Page
All you need is to add a health endpoint to whatever service you wish to integrate with the Status Page.
That endpoit must respond with a json object with the following properties: 
```json
"identifier": "My Service",
"status": 0
```
Identifier should be unique because otherwise you cannot distinguish between services.

Status is an enum with these values: 
```
0 - Healthy
1 - Outage
2 - Partial Outage
```

The json object can have also have these informations optionally: 
```json
"description": "Description of the service if you don't want to set it in the frontend or json file",

"error": "Here you could pass an error that should be displayed on the frontend. Keep in mind that this error is visible by all users thus you shoulnd't add stacktraces here" 
```

# Configurable from the web
If you don't want to you'll never have to touch or create an entities.json file. You can configure all the services from the beautiful dashboard right within the website.

## Define a special notice
If you want to let your customers know that you're working on a particular outage you can set a special notice that will be displayed on the frontpage.

![Special notice](https://cdn.argonaut.pw/file/8ca7582a-6d56-4e56-9082-34f608c1f434.png)
![Special notice shown](https://cdn.argonaut.pw/file/33976342-447a-42be-af69-c724f7a336e7.png)

## Manage existing service configurations
You can easily manage your current service configurations in the same dashboard. Give them a fitting description. You can also define them as categories like the "Sora" service in the screenshots above.

![Special notice shown](https://cdn.argonaut.pw/file/8edd846c-792d-4766-a979-b67b1506ba39.png)

## Create new service configurations
You can also add new service configurations from the dashboard that will then be checked for their status.

![Special notice shown](https://cdn.argonaut.pw/file/8e166406-2be7-42ea-ae75-8d488fa0fc53.png)