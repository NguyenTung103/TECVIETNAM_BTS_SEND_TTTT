Function DeployServices($buildLocation, $webRunFolder, $appSettingJson, $serviceName, $applicationExe) {	
	Get-Date -Format "dd/MM/yyyy HH:mm:ss K"


    $service = Get-Service -Name $serviceName -ErrorAction SilentlyContinue
    $exclude = @('appsettings.json')
	
	Write-Host "Duong dan file publish copy..."
	Write-Host $webRunFolder
	
    # Tạo thư mục đích nếu chưa tồn tại
    if (-not (Test-Path -Path $webRunFolder)) {
        New-Item -ItemType Directory -Path $webRunFolder | Out-Null
    }

    if ($service) {
        Write-Host "Dich vu da duoc cai dat."

        if ($service.Status -eq 'Stopped') {           
			echo "Set Location $buildLocation"
			Set-Location $buildLocation
			echo "restore project"
			dotnet restore
			echo "publish project -c Release"
			dotnet publish -c Release -o $webRunFolder
			
			$sourceFileSettingJson = Join-Path $webRunFolder $appSettingJson
			$destFileSettingJson   = Join-Path $webRunFolder "appsettings.Production.json"	
			Write-Host "Duong dan config sourceFileSettingJson: $sourceFileSettingJson vao file: $destFileSettingJson ...."
			Copy-Item -Path $sourceFileSettingJson -Destination $destFileSettingJson -Force

            if (Test-Path -Path $webRunFolder) {

                Start-Service -Name $serviceName
                Write-Host "Dich vu da duoc khoi dong lai."
            } else {
                Write-Host "Sao chep khong thanh cong."
            }
        } elseif ($service.Status -eq 'Running') {            
            Write-Host "Dich vu dang chay, dung dich vu va cho..."            
            Stop-Service -Name $serviceName -Force
            Start-Sleep -Seconds 35            
            echo "Set Location $buildLocation"
			Set-Location $buildLocation
			echo "restore project"
			dotnet restore
			echo "publish project -c Release"
			dotnet publish -c Release -o $webRunFolder
						
			$sourceFileSettingJson = Join-Path $webRunFolder $appSettingJson
			$destFileSettingJson   = Join-Path $webRunFolder "appsettings.Production.json"	
			Write-Host "Duong dan config sourceFileSettingJson: $sourceFileSettingJson vao file: $destFileSettingJson ...."
			Copy-Item -Path $sourceFileSettingJson -Destination $destFileSettingJson -Force

            if (Test-Path -Path $webRunFolder) {

                Start-Service -Name $serviceName
                Write-Host "Dich vu da duoc khoi dong lai."
            } else {
                Write-Host "Sao chep khong thanh cong."
            }
        }
    } else {
        Write-Host "Dich vu chua dc cai dat..."
        echo "Set Location $buildLocation"
		Set-Location $buildLocation
		echo "restore project"
		dotnet restore
		echo "publish project -c Release"
		dotnet publish -c Release -o $webRunFolder
		
		$sourceFileSettingJson = Join-Path $webRunFolder $appSettingJson
		$destFileSettingJson   = Join-Path $webRunFolder "appsettings.Production.json"	
		Write-Host "Duong dan config sourceFileSettingJson: $sourceFileSettingJson vao file: $destFileSettingJson ...."
		Copy-Item -Path $sourceFileSettingJson -Destination $destFileSettingJson -Force
        if (Test-Path -Path $webRunFolder) {          
			$exePath = Join-Path $webRunFolder $applicationExe            
            # Tạo dịch vụ mới
            sc.exe create $serviceName binPath= "`"$exePath`"" start= auto

            Start-Service -Name $serviceName
            Write-Host "Dich vu da duoc tao va khoi dong."
        } else {
            Write-Host "Sao chep khong thanh cong."
        }
    }
}