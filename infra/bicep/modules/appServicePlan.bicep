@description('Name of the App Service Plan')
param appServicePlan_Name string

@description('Location for the App Service Plan')
param location string

param tags object

resource appServicePlan_Resource 'Microsoft.Web/serverfarms@2024-11-01' = {
  name: appServicePlan_Name
  location: location
  tags: tags
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
  kind: 'functionapp'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: false
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
    asyncScalingEnabled: false
  }
}

output Id string = appServicePlan_Resource.id
