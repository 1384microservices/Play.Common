# Play Common
Play economy services microservices common libraries.

## About
This project implements libraries used across all Play Economy services like:
* Configurations
* MongoDB storage connections

## Contribute
### Prerequisites
* Install [winget](https://learn.microsoft.com/en-us/windows/package-manager/winget/)
* Install git: `winget install --id Git.Git --source winget`
* Install dotnet 6 (or greater) SDK: `winget install --d Microsoft.DotNet.SDK.6`

### Clone
Create a project folder on your box. **D:\Projects\Play Economy** is a good idea but you can choose whatever fits your needs.

For Windows boxes you have to issue this command in a Powershell window: `New-Item -ItemType Directory -Path 'D:\Projects\Play Economy'`.

Clone this repository to your box: `git clone https://github.com/1384microservices/Play.Common.git`

## Publish package
Switch to project folder.

Run publish command: `dotnet pack -o ../../packages/ `
