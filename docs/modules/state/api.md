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

##### [PSObject] this[string member]

Read-only member accessor.

```powershell
# Get the value of member 'a'
$var.a
```

##### [Hashtable] _SetLabel_ (string label, bool clear = false, bool force = false)

Sets the label name on the current state.

```powershell
# Label the state, set all members to null and
# force override any existing label of same name.
# The 'done' variable holds the labelled state value.
$done = $var.setLabel("done", $true, $true)
```

##### _Undo()_

Deletes that latest state, making current state the previous.

```powershell
$var.undo()
```

##### [string] _Name_

Returns the state's name.

```powershell
# Display the state's name
"The State's name is $($var.name)"
```

##### [bool] _Tracing_

Returns `true` if state flagged with `Trace`. 

```powershell
# Write if the state is tracing
"The $($var.name) state is$($var.tracing ? $null : ' NOT') tracing!"
```

##### [PSCustomObject[]] Trace

Returns the state as an array of `PSCustomObject` with all the member values as strings (using `ToString()`).

```powershell
# Display the trace
$var.trace
```

##### [Hashtable[]] _Labels_

Returns an array of labels as `Hashtable`.

```powershell
# Display Labels
$var.labels
```

##### [Hashtable[]] _Values_

Returns the current states values as `Hashtable`.

```powershell
# Display Values
$var.values
```

##### [List<StateValue>] _States_

Returns the state as a list of `StateValue` type, this is native type.

```powershell
# Display state
$var.states
```
