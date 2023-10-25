# Celin.State

A PowerShell Module for script state management.

## Background

Like any scripting language, there is not much discipline around creating and using variable.  Variables are disposable objects requiring trivial effort.

The undisciplined, unstructured, easy come easy go design is great for task oriented scripts.

Proliferation of variables with different scope can however be a headache for  maintenance and troubleshooting of application-type scripts.  

## Objective

A simple way to manage variables as state objects.

### Example

An example to demonstrate the basic concepts.

```powershell
# Make sure to use the AllowPrerelease flag
install-Module celin.state -AllowPrerelease

# Initialise a new state 'test' with members 'a', 'b' and 'c'
# The Force flag overrides an existing state
$var = new-celin.state test a,b,c -Force

# Set member 'a'
Set-Celin.State a "This is a Test for A"
# Display the state
$var
# Set member 'b' using Alias 'cset' ('cs' is another Alias)
cset c 200
# Display the state
$var

# Set member 'a' to a new value
cset a "New Test for A"
# Display the state
$var
# Instead of overriding 'a', a new state record is created
# preserving previous values
$var.states
# To consolidate a state, stick a label for later reference
$var.setlabel("my label")
# A label state record has been consolidated with the label
$var.states

# Set member 'a' using the index method
$var["a"] = "New Label Test"
# Another state record is added
$var.states
# Place another label and start with a clear state
# The second parameter is a skipper:
# 0 - Label current state
# 1 - Label previous state
# n - Label nth previous state
$var.setlabel("second label", 0, $true)
# Values are now cleared
$var

# Set new values
cs a "More tests"
cs b $true
cs c 350
# Display value as Hashtable
$var.value
# Individual values can be accessed with the dot syntax
$var.value.c + 200
# Display labels array
$var.labels
# Retrieve individual label
get-celin.state "my label"
# Retrieve label PowerShell style
$label = $var.labels | where-Object {$_.'#' -eq "my label"}
$label
$label.a
```
