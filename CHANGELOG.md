Alpha v3.0; April 28th, 2023:

- Added prefab for `ReactorRoom`
- Updated comments and indentation in: `EngineScript`, `RoomScript`, `ShieldScript` and `WeaponRoomScript`
- Added `assigned_power` and its respective methods (`getAssignedPower()` and `setAssignedPower()`) to `RoomScript`
- Refactored method used to obtain the floor of the `hp` property of `ReactorRoom`.
- Added `accumulative_power` and algorithm for indexing rooms to respective filtered lists (`missiles`, `lasers`, `hangars`, etc.)
- Updated all instances extending `WeaponRoomScript` (`LaserRoomScript`, `MissileRoomScript`, etc.) so that they use `assigned_power` instead of `hp` in their `reload_timer` relation.

Alpha v3.0; April 29th, 2023:


- Added initialization method for `CrewMemberScript`
- Added method for updating the crew member location (with visual movement)
- Added method to check whether or not a crewmate is in their assigned room
- Added prefab for `Scientist`
- Modified indentation and spacing for `RoomScript`
- Added `addMemberToShip` method in `ShipScript`
- Added methods for obtaining crew list and crewmate by ID in `ShipScript`
- Added methods for obtaining filtered rooms (`getMissilesList()`, `getLasersList()`, etc.) in `ShipScript`