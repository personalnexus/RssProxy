$ServiceName = "RssProxy3"

function install
{
  $LocalUrlPrefix = "http://localhost:8080/"
  New-Service -Name $ServiceName -DisplayName "RSS-via-HTTP Proxy" -Description "Proxy to download HTTPS resources via HTTP" -BinaryPathName ('"O:\OneDrive\Code\C# und .NET\RssProxy\bin\Debug\RssProxy.exe" ' + $LocalUrlPrefix) -StartupType Automatic
  Start-Service $ServiceName
}

function uninstall
{
  if (Get-Service $ServiceName -ErrorAction SilentlyContinue)
  {
    Stop-Service $ServiceName
    (Get-WmiObject Win32_Service -filter "name='$ServiceName'").Delete()
  }
}

uninstall