---
layout: default
title: Celin.State
nav_order: 1
parent: Modules
---

# Celin.State

Like any scripting language, there is not much discipline around creating and using variable.  Variables are disposable objects requiring trivial effort.

The undisciplined, unstructured, easy come easy go design is great for task oriented scripts.

Proliferation of variables with different scope can however be a headache for  maintenance and troubleshooting of application-type scripts.  

## Objective

A simple way to manage variables as state objects.

## Getting Started

### Install Module

```powershell
# Make sure to use the AllowPrerelease flag
install-Module celin.state -AllowPrerelease
```

### Example

An example to demonstrate some basic concepts.

```powershell
# Initialise a new state 'test' with members 'a', 'b' and 'c'
# The Force flag overrides an existing state
$var = new-celin.state test a,b,c -Force

# Set member 'a'
Set-Celin.State a "This is a Test for A"
# Display the state
$var
# Set member 'b' using Alias 'cstate'
cstate c 200
# Display the state
$var

# Set member 'a' to a new value
cstate a "New Test for A"
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
cstate a "More tests"
cstate b $true
cstate c 350
# Display values
$var
# Individual values can be accessed with the dot syntax
$var.c + 200
# Display labels array
$var.labels
# Retrieve individual label
get-celin.state "my label"
# Retrieve label PowerShell style
$label = $var.labels | where-Object {$_.'#' -eq "my label"}
$label
$label.a
# Write a Trace log with changes
$var.trace
```

## Reference

### New-Celin.State

Create a state.

_Name_ [string]
: A unique name for the state.

_Members_ [string[]]
: A list of the state members.  Members can't be added or removed after state creation.

_Force_ (Optional)
: If state name already exist, then override it.  Otherwise an exception is thrown for duplicate state names.

_UseIfExist_ (Optional)
: If state name already exist, then use it.  The _Force_ option is ignored as well as the _Members_ parameter.

_Trace_ (Optional)
: If set, then state history not purged when Labelled.  Useful for debugging.

Returns the state variable.

### Use-Celin.State

Use an existing state.

_Name_ [string]
: The state name.  An exception is thrown if it doesn't exist.

_Trace_
: See explanation above.

Retruns the state variable.

### Set-Celin.State (Alias cstate)

Set a state member.

_Member_ [string]
: Member name (case sensitive).  Throws an exception for invalid name. 

_Value_ [psobject]
: The value to set the member.

_FalseIfNull_ (Optional)
: `null` value is treated like no-value, which means it will be overriden by any cascading value (previously set).  Instead to force no-value, it can be set to false (works the same when testing in an `if` statement).  The _FalseIfNull_ flag will replace `null` with `false` for this purpose.

### Get-Celin.State

Get state with set Label.

_Label_ [string]
: The states label.

_Name_ [string] (Optional)
: State name.  Uses current state if not set.

_FalseIfNone_ (Optional)
: Returns false instead of throwing exception if label not found.

Returns the Labelled state.
