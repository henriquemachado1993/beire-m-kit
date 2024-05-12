param (
    [Parameter(Mandatory=$true)]
    [string]$apiKeyJsonFile
)

function Get-ApiKeyFromJson($jsonFilePath) {
    try {
        $jsonData = Get-Content -Raw -Path $jsonFilePath | ConvertFrom-Json
        return $jsonData.api_key
    } catch {
        Write-Host "Error reading API key from JSON file: $_"
        return $null
    }
}

function UpdateAndPublishProject($projectPath, $projectFile, $apiKey)
{
    try {
        
        Write-Host "Project: $($projectFile)"

        $xml = [Xml] (Get-Content $projectFile)
        $currentVersion = [Version] $xml.Project.PropertyGroup.Version
        Write-Host "Current version: $currentVersion"
        
        $newVersion = $currentVersion.Major, $currentVersion.Minor, ($currentVersion.Build + 1) -join '.'
        $csprojContent = Get-Content $projectFile
        $csprojContent -replace "<Version>$currentVersion</Version>", "<Version>$newVersion</Version>" | Set-Content $projectFile

        $xml = [Xml] (Get-Content $projectFile)
        $newVersion2 = [Version] $xml.Project.PropertyGroup.Version
        Write-Host "New version: $newVersion2"

        Write-Host "Compiling project..."
        $buildResult = dotnet build --configuration Release $projectFile
        
        Write-Host "Packing project..."
        $packResult = dotnet pack --configuration Release $projectFile
        
        $pathToBin = (Get-Item $projectFile).Directory.Name
        $packageName = [System.IO.Path]::GetFileNameWithoutExtension($projectFile)
        $packagePath = "$projectPath\$pathToBin\bin\Release\$packageName.$newVersion.nupkg"

        Write-Host "Pushing package to NuGet..."
        $nugetResult = dotnet nuget push $packagePath --api-key $apiKey --source https://api.nuget.org/v3/index.json
        Write-Host ""
        Write-Host "$($nugetResult)"

        Write-Host ""
    } catch {
        Write-Host "An error occurred: $_"
        return
    }
}

$apiKey = Get-ApiKeyFromJson -jsonFilePath $apiKeyJsonFile
if ($apiKey -eq $null) {
    Write-Host "Failed to retrieve API key from JSON file."
    return
}

$projectsPath = "Code"
$projectFiles = @(
    "$projectsPath\BeireMKit.Data\BeireMKit.Data.csproj",
    "$projectsPath\BeireMKit.Domain\BeireMKit.Domain.csproj",
    "$projectsPath\BeireMKit.Notification\BeireMKit.Notification.csproj"
)

foreach ($projectFile in $projectFiles) {
    $publishThisProject = Read-Host "Do you want to publish $($projectFile)? (Y/N)"
    if ($publishThisProject -ne "Y") {
        continue
    }
    UpdateAndPublishProject $projectsPath $projectFile $apiKey
}