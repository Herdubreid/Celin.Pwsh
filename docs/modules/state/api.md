---
layout: default
title: State Api
parent: Celin.State
grand_parent: Modules
nav_order: 2
---

# Api

Most of `Celin.State` functionality is provided as an `Api` on the state variable, returned by the `New-Celin.State` or `Use-Celin.State` Cmdlets.

```powershell
# Create a new state and return state variable
$var = New-Celin.State test a, b, c
# Get a variable to an existing state
$test = Use-Celin.State test
# The state value is an Hashtable with current state value
$test
```

##### [Hashtable] _SetLabel_ (string label, bool clear = false, bool force = false)

Sets the label name on the current state.

```powershell
# Label the state, set all members to null and
# force override any existing label of same name.
# The 'done' variable holds the labelled state value.
$done = $var.setLabel("done", $true, $true)
```

##### _UndoLast()_

Deletes that latest state, making current state the previous.

```powershell
$var.UndoLast()
```

##### [string] _GetName_

Returns the state's name.

```powershell
# Display the state's name
"The State's name is $($var.GetName)"
```

##### [bool] _GetTracing_

Returns `true` if state flagged with `Trace`. 

```powershell
# Write if the state is tracing
"The $($var.GetName) state is$($var.GetTracing ? $null : ' NOT') tracing!"
```

##### [object[]] _GetTrace_

Returns the state as an `object` array with all the member values as strings (using `ToString()`).

```powershell
# Display the trace
$var.GetTrace
```

##### [Hashtable[]] _GetLabels_

Returns an array of labels as `Hashtable` in __Reverse__ order.  The label uses '#' as membe name (uses # inside quotation marks when referenced).

```powershell
# Display Labels
$var.GetLabels
# Display the last Label set
$var.GetLabels[0]
# Display the first Label set
$var.GetLabels[$var.GetLabels.length - 1]
```

##### [Hashtable[]] _GetValues_

Returns the current states values as `Hashtable` in __Reverse__ order.

```powershell
# Display Values
$var.GetValues
# Display Current Values (same as $var)
$var.GetValues[0]
# Display Previous Values
$var.GetValues[1]
```

##### [List<StateValue>] _GetStates_

Returns the state as a list of `StateValue` type, which is the native type.

```powershell
# Display state
$var.GetStates
```
