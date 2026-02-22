namespace RemoteKeycard;

#if EXILED
using Exiled.API.Features;
using Exiled.API.Features.Items;
#else
using LabApi.Features.Wrappers;
#endif
using CustomPlayerEffects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Keycards;

/// <inheritdoc />
public static class Extensions
{
    /// <summary>
    ///     Checks whether the player has a keycard of a specific permission.
    /// </summary>
    /// <param name="player"><see cref="Player" /> trying to interact.</param>
    /// <param name="requester">The permission that's gonna be searched for.</param>
    /// <returns>Whether the player has the required keycard.</returns>
    public static bool HasKeycardPermission(this Player player, IDoorPermissionRequester requester)
    {
        if (Plugin.Instance.Config.AmnesiaMatters
#if EXILED
            && player.IsEffectActive<AmnesiaVision>())
#else
            && player.HasEffect<AmnesiaVision>())
#endif 
            return false;

        foreach (Item item in player.Items)
        {
            if (item.Base is not IDoorPermissionProvider provider)
                continue;

            if (!requester.CheckPermissions(provider, out PermissionUsed callback))
                continue;

            // Callback is null if the door/provider doesn't have any permission/none flag.
            if (callback != null && item.Base is SingleUseKeycardItem singleUseKeycard)
                singleUseKeycard._destroyed = true;

            return true;
        }

        return false;
    }
}