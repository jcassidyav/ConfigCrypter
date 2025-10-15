# param (
    # [string]$ProjectDir,
    # [string]$Configuration
# )
$ProjectDir = $args[0]
$Configuration = $args[1]
$SolutionDir = $args[2]

Write-Output "Solution Dir: $SolutionDir"

$signToolPath = "$env:USERPROFILE\.dotnet\tools\azuresigntool.exe"

function Sign-Files {
    param (
        [string]$DirectoryPath
    )

    # Change to the specified directory
    cd $DirectoryPath

    # Get files that are not signed
    $signed = Get-ChildItem -include ('*.dll', '*.exe') -Recurse | ForEach-Object { Get-AuthenticodeSignature $_ } | Where-Object { $_.Status -ne "Valid" }
    $files = $signed | Select-Object -ExpandProperty Path
    # Sign the files if there are any unsigned files
    if ($null -ne $signed) {
        & $signToolPath sign -kvu $env:KEY_VAULT_URI -kvc $env:KEY_VAULT_CERTIFICATE_NAME -kvi $env:KEY_VAULT_APPLICATION_CLIENT_ID -kvs $env:KEY_VAULT_CLIENT_SECRET --azure-key-vault-tenant-id $env:KEY_VAULT_TENANT_ID -tr http://timestamp.globalsign.com/tsa/advanced -td sha256 --max-degree-of-parallelism 1  @files
    }
}


if ($Configuration -eq "Release") {
    
	
    $signLoc = "$ProjectDir\bin\Release\net8.0"
	Write-Output "*****Signing output in $signLoc"
	Sign-Files -DirectoryPath $signLoc


	# Delete any nugets now as unsigned, build step nuget pack, will create now so avoid conflicts
    cd $SolutionDir
    Get-ChildItem -Recurse -Filter *.nupkg | Remove-Item -Force

	
	
} else {
    Write-Output "Debug: Nothing signed!"
}

