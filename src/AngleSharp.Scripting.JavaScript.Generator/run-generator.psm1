# Run in Package Manager Console:
#
#   cd .\AngleSharp.Scripting.JavaScript.Generator
#   Import-Module .\run-generator.psm1 -DisableNameChecking
#   Run-Generator
#
# Should do everything to create Jint Bindings

function Run-Generator() {
	Write-Host "Adding generator library ..."

	$lib = "bin\Release\AngleSharp.Scripting.JavaScript.Generator.dll"
	$result = (Add-Type -Path $lib)
	$project = (Get-Project "AngleSharp.Scripting.JavaScript")
	
	Write-Host "Checking destination folder ..."

    $projectPath = [System.IO.Path]::GetDirectoryName($project.FullName)
	$destination = (Join-Path $projectpath "Dom")
	$result = (New-Item -ItemType Directory -Force -Path $destination)

	Write-Host "Generating JavaScript binders ..."

	$config = [AngleSharp.Scripting.JavaScript.Generator.Options]::ForJint()
    $files = [AngleSharp.Scripting.JavaScript.Generator.Files]::Generate($config)
	
	Write-Host "Saving generated files ..."

	ForEach ($file in $files) {
		$fileName = (Join-Path $destination $file.FileName)
		$source = $file.Content
		$result = (New-Item $fileName -ItemType "file" -Force -Value $source)
		$result = $project.ProjectItems.AddFromFile($fileName)

		Write-Host ">> Generated binding for $($file.FileName)"
	}

	Write-Host "Done."
}

Export-ModuleMember "Run-Generator"