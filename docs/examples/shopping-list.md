---
layout: default
title: Shopping List
parent: Modules
nav_order: 1
---

# Shopping List

This example parses a shopping list with RegEx.

Accepts the list as first parameter and optionally with the `-Trace` if second parameter is `$true`.

```powershell
function parser {
    param (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$list,
        [bool]$trace
    )
    # Results
    $result = @{
        failed  = ""
        success = @()
    }
    # Initialise the State
    $var = New-Celin.State shop match, caps, date -Force -Trace:$trace
    # RegEx Pattern
    $pat = "^(?:(?'items'\d*)\s+)?(?'volume'\d+g|\d+ml)\s+(?'text'(?:(?!^\d)\w+\s)+)(?'amount'\d+.\d+)$|^Date\s+(?'date'\d{1,2}\/\d{1,2})$"
    foreach ($line in ($list -split "`r`n|`n")) {
        try {
            $var.match = ($line | Select-String -Pattern $pat)
            if ($var.match) {
                # Create an array of successfully captured values
                $var.caps = ($var.match.matches.groups | `
                        Select-Object success, name, value -Skip 1 | `
                        Where-Object success -eq $true)
                # Convert the array to PSCustomObject using the NameValuePair options
                $var.caps = (cpo $var.caps -NameValuePair)
                if ($var.caps.date) {
                    # This is a record with date, split the day and month
                    $day, $month = $var.caps.date -split "/"
                    # Set the date variable
                    cstate date (Get-Date -Year (Get-Date).Year -Month $month -Day $day -Hour 0 -Minute 0 -Second 0)
                }
                else {
                    # Append the date to captured values
                    $var.caps = (cpo $var.caps, (@{date = $var.date }))
                    # Add to success
                    $result.success += , $var.caps
                }
            }
            else {
                # Add to failed match
                $result.failed += "$line`n"
            }
            # Label done (don't clear and force)
            $var.setLabel("done", $false, $true) | Out-Null
        }
        catch {
            write-host $_ -ForegroundColor Red
            break           
        }
    }
    return $result
}
```

### Example Shopping List

This will throw an exception on "Date 4/15".

```text
Date 1/10
300g bread 4.20
150g butter 6.50
400g yogurt 7.20
120g ham 4.60
400g coffee 8.00
6 500ml beer 16.00
Date 4/15
100g This will not parse
300g bread 4.20
150g ham 5.10
6 480g eggs 4.80
200ml Date 5/10
Nor will this
6 500ml beer 16.00
250g baked beans 3.00
Date 6/10
300g bread 4.20
120g ham 4.70
```
