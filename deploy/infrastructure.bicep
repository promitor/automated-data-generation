param location string = resourceGroup().location
param resourceNamePrefix string = 'promitor-automation-data-generation-${geo}'
param geo string = 'we'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: 'promitordatageneration'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
}


resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: '${resourceNamePrefix}-logs'
  location: location
  properties: {
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
    features: {
      immediatePurgeDataOn30Days: true
    }
  }
}

resource applicationInsights 'microsoft.insights/components@2020-02-02' = {
  name: '${resourceNamePrefix}-telemetry'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    IngestionMode: 'LogAnalytics'
    WorkspaceResourceId: logAnalyticsWorkspace.id
    DisableLocalAuth: false
  }
  dependsOn:[
    logAnalyticsWorkspace
  ]
}

resource classicApplicationInsights 'microsoft.insights/components@2020-02-02' = {
  name: '${resourceNamePrefix}-telemetry-classic'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 30
  }
}

resource serverlessAppPlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${resourceNamePrefix}-serverless-app-plan'
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
  }
  kind: 'functionapp'
  properties: {
    reserved: true
  }
}

resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${resourceNamePrefix}-serverless-functions'
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: serverlessAppPlan.id
    reserved: true
    keyVaultReferenceIdentity: 'SystemAssigned'
    
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~3'
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: 'azurefunctions'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_WORKSPACE_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CLASSIC_INSTRUMENTATIONKEY'
          value: classicApplicationInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: 'InstrumentationKey=${applicationInsights.properties.InstrumentationKey}'
        }
      ]
    }
  }
}

output functionAppName string = functionApp.name
