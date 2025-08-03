param staticSite_Name string
param location string
param tags object

resource staticSites_household_ae_test_banking_name_resource 'Microsoft.Web/staticSites@2024-11-01' = {
  name: staticSite_Name
  location: location
  tags: tags
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
  properties: {
    repositoryUrl: 'https://github.com/ChrisonSimtian/Household'
    branch: 'main'
    stagingEnvironmentPolicy: 'Enabled'
    allowConfigFileUpdates: true
    provider: 'GitHub'
    enterpriseGradeCdnStatus: 'Disabled'
  }
}

resource staticSites_household_ae_test_banking_name_default 'Microsoft.Web/staticSites/basicAuth@2024-11-01' = {
  parent: staticSites_household_ae_test_banking_name_resource
  name: 'default'
  properties: {
    applicableEnvironmentsMode: 'SpecifiedEnvironments'
  }
}
