# Navega até o diretório especificado
Set-Location -Path '.\DesafioVsoft.Api'

# Define as variáveis de ambiente
$env:ASPNETCORE_ENVIRONMENT = 'Development'
# Exibe as variáveis para confirmação
Write-Host "ASPNETCORE_ENVIRONMENT: $env:ASPNETCORE_ENVIRONMENT"

# Solicita o nome da migração
$nomeDaMigration = Read-Host -Prompt 'Digite o nome da migração'

# Executa o comando de adicionar a migração
dotnet ef migrations add $nomeDaMigration -p ..\DesafioVsoft.Migrations\DesafioVsoft.Migrations.csproj

# Pergunta se deve rodar o comando de update
$rodarUpdate = Read-Host -Prompt 'Deseja rodar o update? (Y/N)'

# Verifica a resposta do usuário e, se for 'Y', roda o update
if ($rodarUpdate -eq 'Y') {
   
    # Gera o script SQL no caminho especificado
    dotnet ef migrations script -p ..\DesafioVsoft.Migrations\DesafioVsoft.Migrations.csproj 

} else {
    # Pergunta se deseja remover a migração
    $removerMigration = Read-Host -Prompt 'Deseja remover a migração? (Y/N)'
    
    if ($removerMigration -eq 'Y') {
        dotnet ef migrations remove -f -p ..\DesafioVsoft.Migrations\DesafioVsoft.Migrations.csproj
        Write-Host "Migração removida com sucesso."
    } else {
        Write-Host "Migração não foi removida."
    }
}

# Navega até o diretório especificado
Set-Location -Path '..\'

