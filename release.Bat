@echo off

echo [1/3] Limpando pastas antigas...
if exist "publish" rd /s /q "publish"
if exist "Releases" rd /s /q "Releases"

echo.
echo [2/3] Publicando o projeto via .NET CLI...
dotnet publish -c Release -o ./publish
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERRO] Falha ao compilar ou publicar o projeto.
    goto end
)

echo.
echo [3/3] Criando o instalador com Velopack...
vpk pack --packId "InventorySystem" --packTitle "Inventory System" --packVersion "1.1.0" --outputDir "./Releases" --packAuthors "Inocencio" --mainExe "Gestqo-Inventario.exe" --packDir "./publish"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERRO] O Velopack falhou ao gerar o pacote.
    goto end
)

echo.
echo [SUCESSO] Processo concluido! Verifique a pasta Releases.

:end
echo.
pause