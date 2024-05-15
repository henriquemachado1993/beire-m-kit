param (
    [Parameter(Mandatory=$true)]
    [string]$settingsJsonFile
)

function Get-settingsFromJson($jsonFilePath) {
    try {
        $jsonData = Get-Content -Raw -Path $jsonFilePath | ConvertFrom-Json
        return $jsonData
    } catch {
        Write-Host "Error reading API Settings from JSON file: $_"
        return $null
    }
}

function PublishProject($project)
{
    try {
        
        Write-Host "Project name: $($project.name)"

        $xml = [Xml] (Get-Content $project.pathSolution)
        $currentVersion = [Version] $xml.Project.PropertyGroup.Version
        Write-Host "Current version: $currentVersion"
        
        $newVersion = $currentVersion.Major, $currentVersion.Minor, ($currentVersion.Build + 1) -join '.'
        $csprojContent = Get-Content $project.pathSolution
        $csprojContent -replace "<Version>$currentVersion</Version>", "<Version>$newVersion</Version>" | Set-Content $project.pathSolution

        $xml = [Xml] (Get-Content $project.pathSolution)
        $newVersion2 = [Version] $xml.Project.PropertyGroup.Version
        Write-Host "New version: $newVersion2"

        Write-Host "Compiling project..."
        $buildResult = dotnet build --configuration Release $project.pathSolution
        
        Write-Host "Packing project..."
        $packResult = dotnet pack --configuration Release $project.pathSolution
        
        $pathToBin = (Get-Item $project.pathSolution).Directory.Name
        $packageName = [System.IO.Path]::GetFileNameWithoutExtension($project.pathSolution)
        $packagePath = "$($project.basePath)\$pathToBin\bin\Release\$packageName.$newVersion.nupkg"

        Write-Host "Pushing package to NuGet..."
        $nugetResult = dotnet nuget push $packagePath --api-key $settings.apiKey --source https://api.nuget.org/v3/index.json
        Write-Host ""
        Write-Host "$($nugetResult)"

        Write-Host ""
    } catch {
        Write-Host "An error occurred: $_"
        return
    }
}

$settings = Get-settingsFromJson -jsonFilePath $settingsJsonFile
if ($settings.apiKey -eq $null) {
    Write-Host "Failed to retrieve API key from JSON file."
    return
}

if ($settings.projects -eq $null) {
    Write-Host "Failed to retrieve projects from JSON file."
    return
}

foreach ($project in $settings.projects) {
    $publishThisProject = Read-Host "Do you want to publish $($project.name)? (Y/N)"
    if ($publishThisProject -ne "Y") {
        continue
    }
    PublishProject $project
}