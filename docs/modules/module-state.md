---
layout: default
title: Celin.State
parent: Modules
has_children: true
nav_order: 1
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
# Use the AllowPrerelease flag for latest Prerelease
install-Module celin.state -AllowPrerelease
```

### Example

An example to demonstrate some basic concepts.

```powershell
# Initialise a new state 'test' with members 'a', 'b' and 'c'
# The Force flag overrides an existing state
$var = new-celin.state test a,b,c -Force
# Set member 'a'
$var.a = "This is a Test for A"
# Display the state
$var
# Set member 'b'
$var.b = 200
# Display the state
$var
# Set member 'a' to a new value
$var.a = "New Test for A"
# Display the state
$var
# Instead of overriding 'a', a new state record is created
# preserving previous values
$var.GetStates
# To consolidate a state, stick a label for later reference
# The labelled state is returned
$var.SetLabel("my label")
# A label state record has been consolidated with the label
$var.GetStates
# Set member 'a'
$var.a = "New Label Test"
# Another state record is added
$var.GetStates
# Place another label and start with a clear state
$var.SetLabel("second label", $true)
# Values are now cleared
$var
# Set new values
$var.a = "More tests"
$var.b = $true
$var.c = 350
# Display values
$var
# Individual values can be used with the dot syntax
$var.c + 200
# Display labels array, the label is the '#' member
$var.GetLabels
# Retrieve label PowerShell style
$label = $var.GetLabels | where-Object {$_.'#' -eq "my label"}
$label
$label.a
# Write a Trace log with changes
$var.GetTrace
```
