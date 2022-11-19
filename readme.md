

# FacilityManagement

This plugin make possible many things on your server.

## Configs
|     **Name**    | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| IsEnabled | bool | Is plugin Enable. |
| InfiniteAmmo | List<[ItemType](https://exiled-team.github.io/Web/docs/Resources/Intro#itemtype)> | Item who having InfiniteAmmo |
| EnergyMicroHid | float | Multiplier conssumption of MicroHID (0 = No energy drain \ 2 = Double energy drain) |
| EnergyRadio | float | Multiplier conssumption of EnergyRadio (Same than above) |
| GeneratorDuration | float | Time for generator to be Activated after being enable (-1 disable the config) |
| LiftMoveDuration | Dictionary<[ElevatorType](https://exiled-team.github.io/Web/docs/Resources/Intro#elevatortype), float> | Change the times of Elevator to teleport |
| CustomText | Dictionary<[Intercom.State](https://exiled-team.github.io/Web/docs/Resources/Intro#intercomstates), string> | Change the intercomText with specified there specified State  |
| WarheadCleanup | bool | Remove all Pickup and Ragdoll in the facility (lower than y 500) |
| Scp106LureAmount | int | How many sacrifices it takes to lure 106. Values below 1 set the recontainer to always active |
| Scp106ChanceOfSuccess | float | Probability of succes for sacrifice work before enough player was enter in the recontainer |
| Scp106LureReload | int | Amount of time before another sacrifice can be made |
| Scp106LureTeam | List<[Team](https://exiled-team.github.io/Web/docs/Resources/Intro#roletype-team-side-and-faction)> |  Teams that can enter the femur breaker |
| CustomTesla | [TeslaBuild](https://github.com/louis1706/FacilityManagement#teslabuild) | |
| CustomWindows | Dictionary<[GlassType](https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.GlassType.html), [GlassBuild](https://github.com/louis1706/FacilityManagement#glassbuild)> | Modify all the property of Window like you want (null make no change) |
| CustomDoors | Dictionary<[DoorType](https://exiled-team.github.io/Web/docs/Resources/Intro#doortype), [DoorBuild](https://github.com/louis1706/FacilityManagement#doorbuild)> | Modify all the property of Door like you want (null make no change) |
| RoleTypeHumeShield | Dictionary<[RoleType](https://exiled-team.github.io/Web/docs/Resources/Intro#roletype-team-side-and-faction), [AhpProccessBuild](https://github.com/louis1706/FacilityManagement#ahpproccessbuild)> | Create a custoom AHP/HS for specified RoleType |



## TeslaBuild

|    PropertyName    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| IgnoredRoles | List<[RoleType](https://exiled-team.github.io/Web/docs/Resources/Intro#roletype-team-side-and-faction)> | Tesla going to ignored player with this Role (Null disable the change) |
| TriggerRange | float? | Distance for tesla to be trigger (Null disable the change) |
| IdleRange | float? | Distance for tesla to start idling (Null disable the change) |
| ActivationTime | float? | Time for tesla to start (Null disable the change) |
| CooldownTime | float? | Time between each Shock (Null disable the change) |

## GlassBuild

|    PropertyName    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| Health | float? | Health of the window (Null disable the change) |
| DisableScpDamage | bool? | Disable the damage of Scp (Null disable the change) |

## DoorBuild

|    PropertyName    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| Health | float? | Health of the door (Null disable the change) |
| RequiredPermission | [KeycardPermissions](https://exiled-team.github.io/Web/docs/Resources/Intro#keycardpermissions) | Change the RequiredPermission for interact with the Door (Null disable the change) |
| RequireAllPermission | bool? | Required all the permission of RequiredPermission (0 or None disable the change) |
| DamageTypeIgnored | DoorDamageType | Modified the DamageType ignored by the Door  (0 disable the change) |

## AhpProccessBuild

|    PropertyName    | TypeValue | Description |
| :-------------: | :---------: | :--------- |
| amount | float | Max AHP/HS |
| regen | float | Ammount of Regen per secound (decay if negative) |
| efficacy | float | Ammount of damage take by AHP/HS (0 = Useless/ 1 = Perfect) |
| sustain | float | Required time before regen reactive when player take damage |

## CustomTextForIntercom

This plugin make possible to customise the IntercomText on your server it's will be actualise every tick

This plugin use [CommandInterpolation](https://en.scpslgame.com/index.php?title=Command_Interpolation
) it's permit you to make custom text who can change with with what is happening on the server

whe have add just some CommandInterpolation for Intercom

### CommandInterpolation
|     CommandInterpolation    | ReturnValue | Description |
| :-------------: | :---------: | :--------- |
| intercom_speech_remaining_time | int | Remaining speech time of the player to talk at intercom_bypass_speaking. |
| intercom_remaining_cooldown | int | Waiting time to talk again at intercom |
| intercom_is_in_use | bool | Intercom is being used |
| intercom_is_admin_speaking | bool | Is an admin who speak at intercom |
| intercom_bypass_speaking | bool | Is player speak at intercom have bypass |
| intercom_mute_player_speak | bool | Is player actually trying to talk and being muted |
| intercom_speaker_nickname | string | Nickname of the player at intercom |
| intercom_custom_text | string | The Modified text by admin or plugin |

## Authors

- [@Yamato](https://github.com/louis1706) // Yamato#8987

