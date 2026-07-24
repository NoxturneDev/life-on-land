
$word = New-Object -ComObject Word.Application
$word.Visible = $false
$files = @(
    "c:\Users\galih\Documents\Projects\Game\My project\docs\submissions\TUGAS_10_PROJECT_BASED_2.docx",
    "c:\Users\galih\Documents\Projects\Game\My project\docs\submissions\TUGAS_11_PROJECT_BASED_3.docx",
    "c:\Users\galih\Documents\Projects\Game\My project\docs\submissions\TUGAS_11_PROJECT_BASED_3_REVISED.docx"
)

foreach ($file in $files) {
    if (Test-Path $file) {
        $pdfPath = [System.IO.Path]::ChangeExtension($file, ".pdf")
        Write-Host "Converting $file -> $pdfPath"
        $doc = $word.Documents.Open($file)
        $doc.SaveAs([ref]$pdfPath, [ref]17) # 17 = wdFormatPDF
        $doc.Close()
    }
}
$word.Quit()
Write-Host "All conversions completed!"
