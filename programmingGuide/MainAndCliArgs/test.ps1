if ($LastExitCode -eq 0) {
	Write-Host "Execution succeeded"
} else
{
	Write-Host "Execution Failed"
}
Write-Host "Return value = " $LastExitCode