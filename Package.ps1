$dest = "Nuget\lib\net45"
$spec = "Nuget\AngleSharp.Scripting.JavaScript.nuspec"
New-Item $dest -type directory -Force
Copy-Item "AngleSharp.Scripting.JavaScript\bin\Release\AngleSharp.Scripting.JavaScript.dll" $dest
Copy-Item "AngleSharp.Scripting.JavaScript\bin\Release\AngleSharp.Scripting.JavaScript.xml" $dest
$file = $dest + "\AngleSharp.Scripting.JavaScript.dll"
$ver = (Get-Item $file).VersionInfo.FileVersion
$file = "Nuget\AngleSharp.Scripting.JavaScript." + $ver + ".nupkg"
$repl = '<version>' + $ver + '</version>'
(Get-Content $spec) | 
    Foreach-Object { $_ -replace "<version>([0-9\.]+)</version>", $repl } | 
    Set-Content $spec
nuget pack $spec -OutputDirectory "Nuget"
nuget push $file