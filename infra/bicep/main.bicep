param userAssignedIdentities_household_ae_tes_id_bca4_name string = 'household-ae-tes-id-bca4'
param userAssignedIdentities_household_ae_test_banking_uami_name string = 'household-ae-test-banking-uami'

@description('Prefix for all resources')
param resourcePrefix string = 'household-ae-test'

@description('Location for the resources')
param location string = resourceGroup().location

@description('Environment')
@allowed(['Development', 'Test', 'Production'])
param environment string = 'Test'

var tags = {
  Environment: environment
}

var locationMap = {
  'East US': 'eastus'
  'West Europe': 'westeurope'
  'Australia East': 'australiaeast'
  'North Europe': 'northeurope'
  'West US 2': 'westus2'
  'Central US': 'centralus'
  'South Central US': 'southcentralus'
  'Southeast Asia': 'southeastasia'
  'Japan East': 'japaneast'
  'UK South': 'uksouth'
  'Canada Central': 'canadacentral'
  'Korea Central': 'koreacentral'
  'France Central': 'francecentral'
  'Germany West Central': 'germanywestcentral'
  'Switzerland North': 'switzerlandnorth'
  'UAE North': 'uaenorth'
  'Brazil South': 'brazilsouth'
  'South Africa North': 'southafricanorth'
  'India Central': 'centralindia'
  'Australia Southeast': 'australiasoutheast'
  'Norway East': 'norwayeast'
  'Sweden Central': 'swedencentral'
  'Poland Central': 'polandcentral'
}

var canonicalLocation = locationMap[location]


@description('Resource prefix without dashes')
var resourcePrefixSimple = replace(resourcePrefix, '-', '')

module appServicePlan './modules/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: {
    appServicePlan_Name: '${resourcePrefix}-hostingplan'
    location: location
    tags: tags
  }
}

module storageAccount './modules/storageAccount.bicep' = {
  name: 'storageAccount'
  params: {
    storageAccount_Name: '${resourcePrefixSimple}storage'
    canonicalLocation: canonicalLocation
    tags: tags
  }
}

module functionApp './modules/functionApp.bicep' = {
  name: 'functionApp'
  params: {
    storageAccount_Name: storageAccount.outputs.storageAccountName
    location: location
    environment: environment
    functionApp_Name: '${resourcePrefix}-banking-api'
    appServicePlanId: appServicePlan.outputs.Id
  }
}

module logAnalyticsWorkspace './modules/logAnalyticsWorkspace.bicep' = {
  name: 'logAnalyticsWorkspace'
  params: {
    workspace_Name: '${resourcePrefix}-law'
    conanonicalLocation: canonicalLocation
    tags: tags
  }
}

module applicationInsights './modules/applicationInsights.bicep' = {
  name: 'applicationInsights'
  params: {
    applicationInsights_Name: '${resourcePrefix}-ai'
    canonicalLocation: canonicalLocation
    tags: tags
    logAnalyticsWorkspaceId: logAnalyticsWorkspace.outputs.Id
  }
}

module staticWebApp './modules/staticWebApp.bicep' = {
  name: 'staticWebApp'
  params: {
    staticSite_Name: '${resourcePrefix}-banking-web'
    location: location
    tags: tags
  }
}

resource userAssignedIdentities_household_ae_tes_id_bca4_name_resource 'Microsoft.ManagedIdentity/userAssignedIdentities@2025-01-31-preview' = {
  name: userAssignedIdentities_household_ae_tes_id_bca4_name
  location: canonicalLocation
}

resource userAssignedIdentities_household_ae_test_banking_uami_name_resource 'Microsoft.ManagedIdentity/userAssignedIdentities@2025-01-31-preview' = {
  name: userAssignedIdentities_household_ae_test_banking_uami_name
  location: canonicalLocation
  tags: tags
}

resource userAssignedIdentities_household_ae_tes_id_bca4_name_6hvhwwitmevuc 'Microsoft.ManagedIdentity/userAssignedIdentities/federatedIdentityCredentials@2025-01-31-preview' = {
  parent: userAssignedIdentities_household_ae_tes_id_bca4_name_resource
  name: '6hvhwwitmevuc'
  properties: {
    issuer: 'https://token.actions.githubusercontent.com'
    subject: 'repo:ChrisonSimtian/Household:ref:refs/heads/main'
    audiences: [
      'api://AzureADTokenExchange'
    ]
  }
}
