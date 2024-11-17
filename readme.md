# FacilityManagement

This plugin makes many things possible on your server.

## Configs
| **Name** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| IsEnabled | bool | Is the plugin enabled? |
| InfiniteAmmo | List<[ItemType](https://docs.exiled-team.net/articles/SCPSLRessources/NW_Documentation.html#itemtype)> | Items with InfiniteAmmo |
| EnergyMicroHid | float | Multiplier consumption of MicroHID (0 = No energy drain \ 2 = Double energy drain) |
| EnergyRadio | float | Multiplier consumption of EnergyRadio (Same as above) |
| GeneratorDuration | float | Time for the generator to be activated after being enabled (-1 disables the config) |
| LiftMoveDuration | Dictionary<[ElevatorType](https://docs.exiled-team.net/api/Exiled.API.Enums.ElevatorType.html), float> | Change the times of elevator Animation |
| StandardAmmoLimits | Dictionary<[AmmoType](https://docs.exiled-team.net/articles/SCPSLRessources/NW_Documentation.html#ammotype), ushort> | Sets the standard ammo limits for the player |
| StandardCategoryLimits | Dictionary<ItemCategory, sbyte> | Sets standard category limits |
| CustomText | Dictionary<[Intercom.State](https://docs.exiled-team.net/articles/SCPSLRessources/NW_Documentation.html#intercomstates), string> | Change the intercomText with specified State |
| WarheadCleanup | bool | Remove all pickups and ragdoll in the facility (lower than y 500) |
| CustomTesla | [TeslaBuild](https://github.com/louis1706/FacilityManagement#teslabuild) | (Null disables the change) |
| CustomScp914 | [Scp914Build](https://github.com/louis1706/FacilityManagement#scp914build) | (Null disables the change) |
| CustomWindows | Dictionary<[GlassType](https://docs.exiled-team.net/api/Exiled.API.Enums.GlassType.html), [GlassBuild](https://github.com/louis1706/FacilityManagement#glassbuild)> | Modify all the properties of Window as desired (null makes no change) |
| CustomDoors | Dictionary<[DoorType](https://docs.exiled-team.net/api/Exiled.API.Enums.DoorType.html), [DoorBuild](https://github.com/louis1706/FacilityManagement#doorbuild)> | Modify all the properties of Door as desired (null makes no change) |
| RoleTypeHumeShield | Dictionary<[RoleTypeId](https://github.com/Exiled-Team/EXILED/blob/dev/docs/docs/Resources/Intro.md#roletype-team-side-and-leadingteam), [AhpProccessBuild](https://github.com/louis1706/FacilityManagement#ahpproccessbuild)> | Create a custom AHP/HS for specified RoleType |
| CustomItem | Dictionary<[ItemType](https://github.com/Exiled-Team/EXILED/blob/dev/docs/docs/Resources/Intro.md#itemtype), [ItemBuild](https://github.com/louis1706/FacilityManagement#itembuild)> | Create a custom AHP/HS for specified RoleType |
| CustomRole | Dictionary<[RoleTypeId](https://github.com/Exiled-Team/EXILED/blob/dev/docs/docs/Resources/Intro.md#roletype-team-side-and-leadingteam), [RoleBuild](https://github.com/louis1706/FacilityManagement#rolebuild)>> | Create a custom AHP/HS for specified RoleType |

## TeslaBuild

| **PropertyName** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| IgnoredRoles | List<[RoleTypeId](https://docs.exiled-team.net/articles/SCPSLRessources/NW_Documentation.html#roletype-team-side-and-leadingteam)> | Tesla ignores players with this Role (Null disables the change) |
| TriggerRange | float? | Distance for tesla to be triggered (Null disables the change) |
| IdleRange | float? | Distance for tesla to start idling (Null disables the change) |
| ActivationTime | float? | Time for tesla to start (Null disables the change) |
| CooldownTime | float? | Time between each shock (Null disables the change) |

## Scp914Build

| **PropertyName** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| KnobChangeCooldown | float? | Cooldown between each interaction for when people modify the knob (Null disables the change) |
| DoorCloseTime | float? | Delay after the interaction then the Door will Close (Null disables the change) |
| ItemUpgradeTime | float? | Delay after the interaction then the content of Scp914 will be upgraded (Null disables the change) |
| DoorOpenTime | float? | Delay after the interaction then the Door will Open (Null disables the change) |
| TotalSequenceTime | float? | Cooldown for Scp914 to be reusable again (Null disables the change) |

## GlassBuild

| **PropertyName** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| Health | float? | Health of the window (Null disables the change) |
| DisableScpDamage | bool? | Disable the damage of Scp (Null disables the change) |

## DoorBuild

| **PropertyName** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| Health | float? | Health of the door (Null disables the change) |
| RequiredPermission | [KeycardPermissions](https://docs.exiled-team.net/api/Exiled.API.Enums.KeycardPermissions.html) | Change the RequiredPermission for interaction with the Door (Null disables the change) |
| RequireAllPermission | bool? | Require all the permissions of RequiredPermission (0 or None disables the change) |
| DamageTypeIgnored | DoorDamageType | Modify the DamageType ignored by the Door (0 disables the change) |

## AhpProccessBuild

| **PropertyName** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| amount | float | Max AHP/HS |
| regen | float | Amount of Regen per second (decay if negative) |
| efficacy | float | Amount of damage taken by AHP/HS (0 = Useless/ 1 = Perfect) |
| sustain | float | Required time before regen reactivates when player takes damage |

## ItemBuild

This ItemBuild permits the creation of a highly configurable default item value.

You can find all properties that you can modify in this [link](https://docs.exiled-team.net/api/Exiled.API.Features.Pickups.html).
You can only modify properties that have basic types like float, int, byte, Enum, Vector3. Some properties might not work; they need to be synced with Exiled.

| **PropertyName** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| Custom | List<string> | Each string needs to contain `PropertyName: Value` |

## RoleBuild

This RoleBuild permits the creation of a highly configurable default Role value.

You can find all properties that you can modify in this [link](https://docs.exiled-team.net/api/Exiled.API.Features.Roles.html).
You can only modify properties that have basic types like float, int, byte, Enum, Vector3.

| **PropertyName** | **TypeValue** | **Description** |
| :-------------: | :---------: | :--------- |
| Custom | List<string> | Each string needs to contain `PropertyName: Value` |

## CustomTextForIntercom

This plugin makes it possible to customize the IntercomText on your server; it's updated every tick.

This plugin uses [CommandInterpolation](https://en.scpslgame.com/index.php?title=Command_Interpolation) to allow you to create custom text that can change based on what is happening on the server.

We have added some CommandInterpolations for Intercom.

### CommandInterpolation
| **CommandInterpolation** | **ReturnValue** | **Description** |
| :-------------: | :---------: | :--------- |
| intercom_speech_remaining_time | int | Remaining speech time of the player to talk at intercom_bypass_speaking. |
| intercom_remaining_cooldown | int | Waiting time to talk again at intercom |
| intercom_is_in_use | bool | Intercom is being used |
| intercom_is_admin_speaking | bool | Is an admin who speaks at intercom |
| intercom_bypass_speaking | bool | Is a player speaking at intercom with bypass |
| intercom_mute_player_speak | bool | Is a player actually trying to talk and being muted |
| intercom_speaker_nickname | string | Nickname of the player at intercom |
| intercom_custom_text | string | The modified text by admin or plugin |

## Authors

For any help, please contact me:
- [@Yamato](https://github.com/louis1706) // Yamato#8987

![Alt](https://repobeats.axiom.co/api/embed/b7ce28f49b451ba6d949aaf509fedd4754c58ebd.svg "Repobeats analytics image")
