# Define a hashtable mapping DbContext to its startup project
$contextStartupProjects = @{
    # "AuthDbContext" = "../AppApi.AuthService/AppApi.AuthService.csproj"
    "WebApiDbContext" = "../AppApi.WebApi/AppApi.WebApi.csproj" # Replace with actual startup project
}

foreach ($context in $contextStartupProjects.Keys) {
    $startupProject = $contextStartupProjects[$context]
    # Write-Host là lệnh để in thông báo ra
    Write-Host "Updating database for $context using startup project $startupProject..."
    dotnet ef database update --context $context --project ../AppApi.DataAccess/AppApi.DataAccess.csproj --startup-project $startupProject

    # Kiểm tra: $LASTEXITCODE lưu trữ mã thoát (exit code); -eq tương tự == trong C#
    # Nếu mã thoát là 0 (không có lỗi), lệnh thành công; nếu không, lệnh thất bại
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Successfully updated $context"
    } else {
        Write-Host "Failed to update $context"
        exit 1
    }
}
Write-Host "All contexts updated successfully!"