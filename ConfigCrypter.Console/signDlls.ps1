# param (
    # [string]$ProjectDir,
    # [string]$Configuration
# )
$ProjectDir = $args[0]
$Configuration = $args[1]
$SolutionDir = $args[2]

Write-Output "Solution Dir: $SolutionDir"

function Sign-Files {
    param (
        [string]$DirectoryPath,
        [System.Security.Cryptography.X509Certificates.X509Certificate2]$Certificate,
        [string]$TimestampServer = "http://timestamp.comodoca.com/authenticode"
    )

    # Change to the specified directory
    cd $DirectoryPath

    # Get files that are not signed
    $signed = Get-ChildItem -include ('*.dll', '*.exe') -Recurse | ForEach-Object { Get-AuthenticodeSignature $_ } | Where-Object { $_.Status -ne "Valid" }
    $notSigned = Get-ChildItem -include ('*.dll', '*.exe') -Recurse | ForEach-Object { Get-AuthenticodeSignature $_ } | Where-Object { $_.Status -eq "Valid" }
    $notSignedFileNames = Get-ChildItem -Path $notSigned.Path | Sort-Object | Select-Object -Unique -Property Name

    # Sign the files if there are any unsigned files
    if ($signed -ne $null) {
        Set-AuthenticodeSignature -FilePath $signed.Path -Certificate $Certificate -TimestampServer $TimestampServer
    }
}


if ($Configuration -eq "Release") {
    
	$cert = Get-ChildItem -Path Cert:\* -Recurse -CodeSigningCert
    # $signLoc = "$ProjectDir\obj\Release\net8.0"
	# Write-Output "*****Signing output in $signLoc"
	# Sign-Files -DirectoryPath $signLoc -Certificate $cert

    $signLoc = "$ProjectDir\bin\Release\net8.0"
	Write-Output "*****Signing output in $signLoc"
	Sign-Files -DirectoryPath $signLoc -Certificate $cert

    # $signLoc = "$SolutionDir\ConfigCrypter\bin\Release\net8.0"
	# Write-Output "*****Signing output in $signLoc"
	# Sign-Files -DirectoryPath $signLoc -Certificate $cert

	
	
	
} else {
    Write-Output "Debug: Nothing signed!"
}

