# FizzyFacepunch
FizzyFacepunch is an alternative version of **[FizzySteamworks](https://github.com/Chykary/FizzySteamworks)** that uses [Facepunch.Steamworks](https://github.com/Facepunch/Facepunch.Steamworks) instead of Steamworks.NET.

It brings together **[Steam](https://store.steampowered.com)** and **[Mirror](https://github.com/vis2k/Mirror)**. It supports both the old SteamNetworking API as well as the new SteamNetworkingSockets API.

Mirror **[docs](https://mirror-networking.gitbook.io/docs/transports/fizzyfacepunch-transport)** and the official community **[Discord](https://discord.gg/N9QVxbM)**.

Facepunch.Steamworks **[docs](https://wiki.facepunch.com/steamworks/)** and **[GitHub](https://github.com/Facepunch/Facepunch.Steamworks)**.
## Installation
### Dependencies
Both of these projects need to be installed and working before you can use this transport.
1. **[Facepunch.Steamworks](https://wiki.facepunch.com/steamworks/)** - FizzyFacepunch relies on Facepunch.Steamworks to communicate with the **[Steamworks API](https://partner.steamgames.com/doc/sdk)**. It implements Valve's clunky, raw C++ SDK into a clean, modern C# library.
2. **[Mirror](https://mirror-networking.com/)** - FizzyFacepunch is also obviously dependent on Mirror which is a streamlined, bug fixed, maintained version of UNET for Unity.
> [!NOTE]
> Older Unity versions may require the API Compatibility Level set to .NET Framework 4.x.

### Setting Up
1. Install the [dependencies](#dependencies) (Facepunch.Steamworks & Mirror).
2. Install the FizzyFacepunch **[unitypackage](https://github.com/Chykary/FizzyFacepunch/releases)** from the release section.
3. In your **"NetworkManager"** object, set **FizzyFacepunch** as your Transport.
4. Enter your Steam App ID in the **"FizzyFacepunch"** script. For Development purposes, you can also use the 480. 

> [!NOTE]
> If the latest FizzyFacepunch unitypackage doesn't work, then download the source code of this repo, and copy it to the `Assets/Mirror/Runtime/Transports/FizzyFacepunch` folder in your Unity project. Somebody may have already fixed it in the latest source code.

> [!WARNING]
> **The  default 480 (Spacewar) appid is a legal grey area. Technically, it's not allowed to use it, but Valve doesn't really do anything about it. Though, as soon as you get your own appid from [Steamworks](https://partner.steamgames.com/doc/gettingstarted/onboarding), you should change it. 480 should never be used in production.
> If you know a better way around this please open an [issue](https://github.com/Chykary/FizzyFacepunch/issues)**.

## Usage
### Host
1. Start Steam.
2. Launch your game.
3. Start the server through Mirror's NetworkManager.
4. Share your **SteamID64** with your buddy.

### Client
1. Send the game to your buddy.
2. Share the **SteamID64** of the host with your buddy.
3. Place the **SteamID64** into **"localhost"** then click **"Client"**
4. Then they will be connected to you.

### Testing your game locally
Testing your game locally requires two separate Steam accounts. Since running multiple Steam clients on the same device isn't supported, you can either use **another device with a different Steam account** or switch the Transport to **KCP/Telepathy**.
