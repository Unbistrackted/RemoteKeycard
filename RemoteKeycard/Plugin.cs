#nullable enable
namespace RemoteKeycard;

#if EXILED
using Exiled.API.Features;
# else
using LabApi.Loader.Features.Plugins;
#endif
using System;

/// <inheritdoc />
public class Plugin : Plugin<Config>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Plugin" /> class.
    ///     Instance initializer.
    /// </summary>
    public Plugin()
    {
        Instance = this;
    }

    /// <summary>
    ///     Gets a static instance of this class.
    /// </summary>
    public static Plugin? Instance { get; private set; }

    /// <inheritdoc />
    public override string Name => "RemoteKeycard";

    /// <inheritdoc />
    public override string Author => "Beryl && Parkeymon (Maintained by Unbistrackted)";

#if EXILED
    /// <inheritdoc />
    public override Version RequiredExiledVersion => new(9, 13, 1);

#else
        /// <inheritdoc />
    public override Version RequiredApiVersion => new(1, 1, 5);

        /// <inheritdoc />
    public override string Description => "Plugin that allows you to use your keycards without the need of having them on your hand all the time";
#endif

    /// <inheritdoc />
    public override Version Version => new(3, 4, 1);

    /// <inheritdoc cref="EventHandlers" />
    private EventHandlers? Handler { get; set; }

    /// <inheritdoc />
#if EXILED
    public override void OnEnabled()
#else
    public override void Enable()
#endif
    {
        Handler = new EventHandlers(Config);
        Handler.Start();

#if EXILED
        base.OnEnabled();
#endif
    }

    /// <inheritdoc />
#if EXILED
    public override void OnDisabled()
#else
    public override void Disable()
#endif
    {
        Handler?.Stop();
        Handler = null;

#if EXILED
        base.OnDisabled();
#endif
    }
}
