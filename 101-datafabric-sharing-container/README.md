# Introduction 
This sample gives a quick introduction in how to do sharing of a container to multiple users. For full documentation of the API calls to Veracity Data Fabric, refer to [dev.veracity.com](https://developer.veracity.com/doc/data-fabric-api#overview-0) for documentation on Data API.

In this sample we do the following:
- GET /api/1/resources to get a list of containers shared with you
- GET /api/1/users/me to get our own profile
- GET /api/1/keyteampltes to get a list of available key templates
- POST /api/1/resources/{resourceId}/accesses to share a key share for a container



# Notes
There is currently no way for you as a user to get your access token programatically outside of [onboarded services on Veracity](https://developer.veracity.com/doc/onboarding-a-service). Once you are onboarded you can refer to [this documentation](https://developer.veracity.com/doc/create-veracity-app) on how to create an application for veracity.

For short scripts or small tasks that require automation of our API on your behalf, you can still acquire an access token manually, and then call our APIs programatically. See below under requirements on how to do this.


# Requirements
- Access to our api-portal
- A B2C access-token
- An API Management subscription key
- UserIds, which is a list of GUIDs in the format `ec7db5a7-a22a-42bf-84c6-e0765465d81f`

## API-Portal
You need access to our [api-portal](https://api-portal.veracity.com/). For help on setting this up, please contact us [here](https://services.veracity.com/form/SupportAnonymous).

## Access Token & Subscription key
Once logged into our API Portal, navigate to any data fabric endpoint and try them out, this will prompt a login with our identity provider, and post-login you will be able to see your access token and subscription key (press the little 'eye' icon near the subscription key and access token text-fields).


# How to run this sample
The relevant code is in DataFabricApp.cs. 

Once you have acquired your access token and subscription key, paste their values in `configuration.json`, also fill out the list of usersIds,  build and run, you should then in the console see some output indicating that sharing was sucessful.