# Navega at� o diret�rio especificado
Set-Location -Path '.\DesafioVsoft.Api'

# Define as vari�veis de ambiente
$env:ASPNETCORE_ENVIRONMENT = 'Development'
# Exibe as vari�veis para confirma��o
Write-Host "ASPNETCORE_ENVIRONMENT: $env:ASPNETCORE_ENVIRONMENT"

# Solicita o nome da migra��o
$nomeDaMigration = Read-Host -Prompt 'Digite o nome da migra��o'

# Executa o comando de adicionar a migra��o
dotnet ef migrations add $nomeDaMigration -p ..\DesafioVsoft.Migrations\DesafioVsoft.Migrations.csproj

# Pergunta se deve rodar o comando de update
$rodarUpdate = Read-Host -Prompt 'Deseja rodar o update? (Y/N)'

# Verifica a resposta do usu�rio e, se for 'Y', roda o update
if ($rodarUpdate -eq 'Y') {
   
    # Gera o script SQL no caminho especificado
    dotnet ef migrations script -p ..\DesafioVsoft.Migrations\DesafioVsoft.Migrations.csproj 

} else {
    # Pergunta se deseja remover a migra��o
    $removerMigration = Read-Host -Prompt 'Deseja remover a migra��o? (Y/N)'
    
    if ($removerMigration -eq 'Y') {
        dotnet ef migrations remove -f -p ..\DesafioVsoft.Migrations\DesafioVsoft.Migrations.csproj
        Write-Host "Migra��o removida com sucesso."
    } else {
        Write-Host "Migra��o n�o foi removida."
    }
}

# Navega at� o diret�rio especificado
Set-Location -Path '..\'

