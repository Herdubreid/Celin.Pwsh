---
layout: default
title: Celin.State Cmdlets
nav_order: 1
parent: Celin.State
---

# Celin.State Cmdlets Reference

## New-Celin.State

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

## Use-Celin.State

Use an existing state.

_Name_ [string]
: The state name.  An exception is thrown if it doesn't exist.

_Trace_
: See explanation above.

Retruns the state variable.

## Set-Celin.State (Alias cstate)

Set a state member.

_Member_ [string]
: Member name (case sensitive).  Throws an exception for invalid name. 

_Value_ [psobject]
: The value to set the member.

_FalseIfNull_ (Optional)
: `null` value is treated like no-value, which means it will be overriden by any cascading value (previously set).  Instead to force no-value, it can be set to false (works the same when testing in an `if` statement).  The _FalseIfNull_ flag will replace `null` with `false` for this purpose.

## Get-Celin.State

Get state with set Label.

_Label_ [string]
: The states label.

_Name_ [string] (Optional)
: State name.  Uses current state if not set.

_FalseIfNone_ (Optional)
: Returns false instead of throwing exception if label not found.

Returns the Labelled state.
