$logPath = "C:\Users\galih\.gemini\antigravity-ide\brain\499194de-5efb-48e8-924a-f3522ee0287b\.system_generated\logs\transcript.jsonl"
if (Test-Path $logPath) {
    $lines = Get-Content $logPath
    for ($i = $lines.Length - 1; $i -ge 0; $i--) {
        $line = $lines[$i]
        if ($line -like "*cb_tl*" -and $line -like "*borderCol*" -and $line -like "*write_to_file*") {
            try {
                $json = ConvertFrom-Json $line
                $targetFile = $json.tool_calls[0].args.TargetFile
                $code = $json.tool_calls[0].args.CodeContent
                
                # Check if it was targeting generate_autotiles.ps1 and is NOT a recovery script
                if ($targetFile -like "*generate_autotiles.ps1" -and $code -notlike "*recover*") {
                    $code | Set-Content "Assets\generate_autotiles.ps1"
                    Write-Output "Successfully recovered the original generate_autotiles.ps1!"
                    break
                }
            } catch {
                # ignore malformed lines
            }
        }
    }
} else {
    Write-Output "Log path not found!"
}
