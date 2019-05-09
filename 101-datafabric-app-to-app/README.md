# Introduction 

This sample gives a quick introduction in how to do app-to-app API calls in Veracity Data Fabric, refer to [dev.veracity.com](https://developer.veracity.com/doc/data-fabric-api#overview-0) for documentation on Data API.
In this sample we do the following:
- Authenticate as an application
- Get an access token
- Call /api/1/application to get our own application profile
- Call /api/1/resources to get list of containers shared with our application


# Requirements
Your service or application must have been onboarded in our identity provider and must be registered as an applicaiton in Veracity Data Fabric, this can be done through the onboarding process, if you require further info how to set this up, please contact [our support](https://services.veracity.com/form/SupportAnonymous), once onboarded the following should be available to you (and is required for app-to-app).

```
ClientId - Application ID
ClientSecret - Application secret
ResourceUrl - Full Data Fabric Resource url
AAD Tenant id - Azure AD Tenant Id
```

# Acquiring the access token
The access token can be acquired directly in any language that has support for HTTP methods. All you are required to do is make a POST request towards the identity providers token endpoint with the correct credentials, in powershell:
```
$clientid = '<Your client id>'
$clientSecret = '<Your client secret>'
$tenantId = '<Tenant Id>'
$resource = '<Resource url>'
$GrantType = "client_credentials"
$Uri = "https://login.microsoftonline.com/" + $tenantId + "/oauth2/token"
 
 
$Body = @{
    "grant_type" = $GrantType
    "client_id" = $clientid
    "resource" = $resource
    "client_secret" = $clientSecret
}
 
$body
$token = Invoke-RestMethod -Uri $Uri -Method Post -Body $Body -ContentType "application/x-www-form-urlencoded"
$token
```