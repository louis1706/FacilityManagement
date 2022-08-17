# FacilityManagement

This plugin make possible many things on your server.

## Configs
|     Name    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| IsEnabled | Bool | Is plugin Enable |
| InfiniteAmmo | List<ItemType> | Item who having InfiniteAmmo |
| EnergyMicroHid | float | Multiplier conssumption of MicroHID (0 = No energy drain \ 2 = Double energy drain) |
| EnergyRadio | float | Multiplier conssumption of EnergyRadio (Same than above) |
| GeneratorDuration | float | Time for generator to be Activated after being enable (-1 disable the config) |
| LiftMoveDuration | Dictionary<ElevatorType, float> | Change the times of Elevator to teleport |
| CustomText | Dictionary<Intercom.State, string> | Change the intercomText with specified there specified State  |
| WarheadCleanup | bool | Remove all Pickup and Ragdoll in the facility (lower than y 500) |
| Scp106LureAmount | int | How many sacrifices it takes to lure 106. Values below 1 set the recontainer to always active |
| Scp106ChanceOfSuccess | float | Probability of succes for sacrifice work before enough player was enter in the recontainer |
| Scp106LureReload | int | Amount of time before another sacrifice can be made |
| Scp106LureTeam | List<Team> |  Teams that can enter the femur breaker |
| CustomWindows | Dictionary<DoorType, GlassBuild> | Modify all the property of Window like you want (null make no change) |
| CustomDoors | Dictionary<DoorType, DoorBuild> | Modify all the property of Door like you want (null make no change) |
| RoleTypeHumeShield | Dictionary<RoleType, AhpProccessBuild> | Create a custoom AHP/HS for specified RoleType |

## GlassType

|    PropertyName    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| Health | float? | Health of the window (Null disable the change) |
| DisableScpDamage | bool? | Disable the damage of Scp (Null disable the change) |

## DoorBuild

|    PropertyName    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| Health | float? | Health of the door (Null disable the change) |
| RequiredPermission | KeycardPermissions? | Change the RequiredPermission for interact with the Door (Null disable the change) |
| RequireAllPermission | bool? | Required all the permission of RequiredPermission (Null disable the change) |
| DamageTypeIgnored | DoorDamageType? | Modified the DamageType ignored by the Door  (Null disable the change) |

## AhpProccessBuild

|    PropertyName    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| amount | float | Max AHP/HS |
| regen | float | The ammount of Regen per secound (decay if negative) |
| efficacy | float | Ammount of damage take by AHP/HS (0 = Useless/ 1 = Perfect) |
| sustain | float | Required time before regen reactive when player take damage |

## CustomTextForIntercom

This plugin make possible to customise the IntercomText on your server it's will be actualise every tick

This plugin use [CommandInterpolation](https://en.scpslgame.com/index.php?title=Command_Interpolation)
it's permit you to make custom text who can change with with what is happening on the server

whe have add just some CommandInterpolation for Intercom

### CommandInterpolation
|     CommandInterpolation    | ReturnValue | Description |
| :-------------: | :---------: | :--------- |
| intercom_speech_remaining_time | Int | The remaining speech time of the player to talk at intercom_bypass_speaking. |
| intercom_remaining_cooldown | Int | The waiting time to talk again at intercom |
| intercom_speaker_nickname | String | The nickname of the player at intercom |
| intercom_is_in_use | Bool | Is the intercom being used |
| intercom_is_admin_speaking | Bool | Is an admin who speak at intercom |
| intercom_bypass_speaking | Bool | The player who speak at intercom have bypass |
| intercom_mute_player_speak | Bool | The player who actually trying to talk is muted |


## Authors

- [@Yamato](https://github.com/louis1706) // Yamato#8987
