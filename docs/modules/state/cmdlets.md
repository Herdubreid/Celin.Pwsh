---
layout: default
title: Cmdlets
parent: Celin.State
grand_parent: Modules
nav_order: 1
---

# Cmdlets Reference for Celin.State

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
