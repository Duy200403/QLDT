$contextConfig = @{
    # "AuthDbContext" = @{
    #     StartupProject = "../AppApi.AuthService/AppApi.AuthService.csproj"
    #     OutputDir      = "Migrations/Auth"
    #     MigrationName  = "AddParentIdRoleOrdernumber"
    # }
    "WebApiDbContext" = @{
        StartupProject = "../AppApi.WebApi/AppApi.WebApi.csproj"
        OutputDir      = "Migrations/WebApi"
        MigrationName  = "qldt"
    }
}

foreach ($context in $contextConfig.Keys) {
    $config         = $contextConfig[$context]
    $startupProject = $config["StartupProject"]
    $outputDir      = $config["OutputDir"]
    $migrationName  = $config["MigrationName"]
    # Write-Host là lệnh để in thông báo ra
    Write-Host "Generating migrations for $context using startup project $startupProject and output directory $outputDir..."

    dotnet ef migrations add $migrationName `
        --context $context `
        --project "../AppApi.DataAccess/AppApi.DataAccess.csproj" `
        --startup-project $startupProject `
        --output-dir $outputDir
    # Kiểm tra: $LASTEXITCODE lưu trữ mã thoát (exit code); -eq tương tự == trong C#
    # Nếu mã thoát là 0 (không có lỗi), lệnh thành công; nếu không, lệnh thất bại
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Successfully generated migrations for $context"
    }
    else {
        Write-Host "Failed to generate migrations for $context"
        exit 1
    }
}

Write-Host "All migrations generated successfully!"