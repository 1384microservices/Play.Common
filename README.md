# Play Common
Play economy services common libraries.

## About
This project implements libraries used across all Play Economy services like:
* Configurations
* MongoDB storage connections
* MassTransit & RabbitMQ
* ASP.Net Core Identity

## Contribute
### Prerequisites
* Install [winget](https://learn.microsoft.com/en-us/windows/package-manager/winget/)
* Install git: `winget install --id Git.Git --source winget`
* Install dotnet 6 (or greater) SDK: `winget install --d Microsoft.DotNet.SDK.6`

### Clone
Create a project folder on your box. **D:\Projects\Play Economy** is a good idea but you can choose whatever fits your needs.

For Windows boxes you have to issue this command in a Powershell window: `New-Item -ItemType Directory -Path 'D:\Projects\Play Economy'`.

Clone this repository to your box: `git clone https://github.com/1384microservices/Play.Common.git`

### Create and publish package
Switch to project folder.
``` powershell
# Change with your package version.
$version="1.0.6"

# Change with your organisation name.
$owner="1384microservices"

# Change with your GitHub Personal Access Token
$gh_pat="[type your PAT here]"

# Change with your repository URL
$repositoryUrl="https://github.com/1384microservices/Play.Common"

# Build package
dotnet pack src\Play.Common\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=$repositoryUrl -o ..\packages\

dotnet nuget push ..\packages\Play.Common.$version.nupkg --api-key $gh_pat --source "github"
```