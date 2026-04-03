namespace RemoteKeycard;

#if EXILED
using PlayerEvents = Exiled.Events.Handlers.Player;
using PlayerUnlockingGeneratorEventArgs = Exiled.Events.EventArgs.Player.UnlockingGeneratorEventArgs;
using PlayerInteractingLockerEventArgs = Exiled.Events.EventArgs.Player.InteractingLockerEventArgs;
using PlayerUnlockingWarheadButtonEventArgs = Exiled.Events.EventArgs.Player.ActivatingWarheadPanelEventArgs;
using PlayerInteractingDoorEventArgs = Exiled.Events.EventArgs.Player.InteractingDoorEventArgs;
#else
using LabApi.Events.Handlers;
using LabApi.Events.Arguments.PlayerEvents;
#endif
using System;
using MapGeneration.Distributors;
using Log = LabApi.Features.Console.Logger;

/// <summary>
/// Provides methods to register and unregister event handlers for all keycards interactions, enabling custom behavior based on configuration settings.
/// </summary>
public class EventHandlers
{
    private static Config Config => Plugin.Instance.Config;

    /// <summary>
    /// Registers all events used.
    /// </summary>
    public EventHandlers()
    {
        PlayerEvents.InteractingDoor += OnDoorInteract;
        PlayerEvents.UnlockingGenerator += OnGeneratorUnlock;
        PlayerEvents.InteractingLocker += OnLockerInteract;
#if EXILED
        PlayerEvents.ActivatingWarheadPanel += OnWarheadUnlock;
#else
        PlayerEvents.UnlockingWarheadButton += OnWarheadUnlock;
#endif
    }

    /// <summary>
    /// Unregisters all events used.
    /// </summary>
    ~EventHandlers()
    {
        PlayerEvents.InteractingDoor -= OnDoorInteract;
        PlayerEvents.UnlockingGenerator -= OnGeneratorUnlock;
        PlayerEvents.InteractingLocker -= OnLockerInteract;
#if EXILED
        PlayerEvents.ActivatingWarheadPanel -= OnWarheadUnlock;
#else
        PlayerEvents.UnlockingWarheadButton -= OnWarheadUnlock;
#endif
    }

    private void OnDoorInteract(PlayerInteractingDoorEventArgs ev)
    {
        Log.Debug("Door Interact Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!Config.AffectDoors || ev.Player == null)
                return;

#if EXILED
            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Door?.Base) && !ev.Door.IsLocked)
                ev.IsAllowed = true;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Door?.Base)}, Current Item: ${ev.Player.CurrentItem}", Plugin.Instance.Config.Debug);
#else
            if (!ev.CanOpen && ev.Player.HasKeycardPermission(ev.Door?.Base) && !ev.Door.IsLocked)
                ev.CanOpen = true;

            Log.Debug($"Allowed: {ev.CanOpen}, Permission?: {ev.Player.HasKeycardPermission(ev.Door?.Base)}, Current Item: ${ev.Player.CurrentItem}", Plugin.Instance.Config.Debug);
#endif
        }
        catch (Exception e)
        {
            if (Config.ShowExceptions)
                Log.Warn($"{nameof(OnDoorInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnGeneratorUnlock(PlayerUnlockingGeneratorEventArgs ev)
    {
        Log.Debug("Generator Unlock Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!Config.AffectGenerators || ev.Player == null)
                return;
            
#if EXILED
            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Generator?.Base))
                ev.IsAllowed = true;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator?.Base)}", Plugin.Instance.Config.Debug);
#else
            if (!ev.CanOpen && ev.Player.HasKeycardPermission(ev.Generator?.Base))
                ev.CanOpen = true;

            Log.Debug($"Allowed: {ev.CanOpen}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator?.Base)}", Plugin.Instance.Config.Debug);
#endif
        }
        catch (Exception e)
        {
            if (Config.ShowExceptions)
                Log.Warn($"{nameof(OnGeneratorUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnLockerInteract(PlayerInteractingLockerEventArgs ev)
    {
        Log.Debug("Locker Interact Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!Config.AffectScpLockers || ev.Player == null)
                return;
#if EXILED
            LockerChamber locker = ev.InteractingChamber?.Base;

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(locker))
                ev.IsAllowed = true;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(locker)}", Plugin.Instance.Config.Debug);
#else
            LockerChamber locker = ev.Chamber?.Base;

            if (!ev.CanOpen && ev.Player.HasKeycardPermission(locker))
                ev.CanOpen = true;

            Log.Debug($"Allowed: {ev.CanOpen}, Permission?: {ev.Player.HasKeycardPermission(locker)}", Plugin.Instance.Config.Debug);
#endif

        }
        catch (Exception e)
        {
            if (Config.ShowExceptions)
                Log.Warn($"{nameof(OnLockerInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnWarheadUnlock(PlayerUnlockingWarheadButtonEventArgs ev)
    {
        Log.Debug("Warhead Unlock Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!Config.AffectWarheadPanel || ev.Player == null)
                return;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(AlphaWarheadActivationPanel.Instance)}", Plugin.Instance.Config.Debug);

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(AlphaWarheadActivationPanel.Instance))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (Config.ShowExceptions)
                Log.Warn($"{nameof(OnWarheadUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }
}