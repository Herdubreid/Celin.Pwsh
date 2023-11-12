---
layout: default
title: State Api
parent: Celin.State
grand_parent: Modules
nav_order: 2
---

# Api

Most of `Celin.State` functionality is provided as an `Api` on the state variable, returned by the `New-Celin.State` or `Use-Celin.State` Cmdlets.

#### [PSObject] this[string member]

 Member accessor.

#### [Hashtable] _SetLabel_ (string label, bool clear = false, bool force = false)

Sets the label name on the current state.

#### _Undo()_

Deletes that latest state, making current state the previous.

#### [string] _Name_

Returns the state's name.

#### [bool] _Tracing_

Returns `true` if state flagged with `Trace`. 

#### [PSCustomObject[]] Trace

Returns the state as an array of `PSCustomObject`.

#### [Hashtable[]] _Labels_

Returns an array of labels as `Hashtable`.

#### [Hashtable[]] _Values_

Returns the current states values as `Hashtable`.

#### [List<StateValue>] _States_

Returns a list of all `StateValues`.
