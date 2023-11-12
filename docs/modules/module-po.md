---
layout: default
title: Celin.Po
parent: Modules
nav_order: 1
---

# Celin.Po

A Module to convert/merge `PSCustomObject` and `Hashtable` variables.

## Getting Started

### Install Module

```powershell
# Use the AllowPrerelease flag for latest Prerelease
install-Module celin.po -AllowPrerelease
```

### Example

An example to demonstrate usage.

```powershell
# Construct couple of Hashtables
$ht1 = @{ a = "This is a in Hashtable" }
$ht2 = @{ b = "This is b in Hashtable" }
# Construct couple of PSCustomObjects
$po1 = [PSCustomObject]@{ b = "This b in PSCustomObject" }
$po2 = [PSCustomObject]@{ c = "This c in PSCustomObject" }
# Create a new PSCustomObject with cpo (Alias for Add-Celin.Po)
$o = cpo $po1, $ht1, $ht2, $po2
$o.gettype()
# Display the values.  Note that b is from $ht2, since it follows $po1
# Similar to JavaScript's { ...po1, ...ht1, ...ht2, ...po2 }
$o
```

## Cmdlets Reference

## New-Celin.Po

Create `PSCustomObject` with `null` values.

_Members_ [string[]]
: A list of object members.

Returns a `PSCustomObject` with the _Members_.

## Add-Celin.Po (Alias cpo)

Merge a list of one or more `PSCustomObject` and `Hashtable` objects.

_Objects_ [PSObject[]]
: A list of objects to merge.

_NameValuePair_ (Optional)
: Transforms `{ Name = <name>; Value = <value> }` object into `{ <name> = <value> }`.

Returns a `PSCustomObject` with merged members from _Objects_.
