# param (
    # [string]$ProjectDir,
    # [string]$Configuration
# )
$ProjectDir = $args[0]
$Configuration = $args[1]


if ($Configuration -eq "Release") {
    
	$cert = Get-ChildItem -Path Cert:\* -Recurse -CodeSigningCert
	Write-Output "Signing output in $ProjectDir\bin\Release\net8.0"
	
	cd $ProjectDir\bin\Release\net8.0
	
	$signed = Get-ChildItem -include ('*.dll', '*.exe') -Recurse | ForEach-object {Get-AuthenticodeSignature $_} | Where-Object {$_.status -ne "Valid"}
    $notSigned = Get-ChildItem -include ('*.dll', '*.exe') -Recurse | ForEach-object {Get-AuthenticodeSignature $_} | Where-Object {$_.status -eq "Valid"}
    $notSignedFileNames = Get-ChildItem -Path $notSigned.Path | Sort-Object | Select-Object -Unique -Property Name
	
	if($signed -ne $null)
    {
       Set-AuthenticodeSignature -FilePath $signed.path -Certificate $cert -TimestampServer "http://timestamp.comodoca.com/authenticode"
    }
	
} else {
    Write-Output "Debug: Nothing signed!"
}