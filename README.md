# FizzyFacepunch

This is an alternative version of **[FizzySteamworks](https://github.com/Chykary/FizzySteamworks)** that uses Facepunch instead of Steamworks.NET.

Mirror **[docs](https://mirror-networking.gitbook.io/docs/transports/fizzyfacepunch-transport)** and the official community **[Discord](https://discord.gg/N9QVxbM)**.

FizzyFacepunch brings together **[Steam](https://store.steampowered.com)** and **[Mirror](https://github.com/vis2k/Mirror)** . It supports both the old SteamNetworking as well as the new SteamSockets.

## Dependencies
Both of these projects need to be installed and working before you can use this transport.
1. **[Facepunch](https://github.com/Facepunch/Facepunch.Steamworks)** FizzyFacepunch relies on Facepunch to communicate with the **[Steamworks API](https://partner.steamgames.com/doc/sdk)**. **Requires .Net 4.x**  
2. **[Mirror](https://github.com/vis2k/Mirror)** FizzyFacepunch is also obviously dependant on Mirror which is a streamline, bug fixed, maintained version of UNET for Unity.

## Setting Up

1. Install Mirror **(Requires Mirror 53.0+)** from the official repo **[Download Mirror](https://github.com/vis2k/Mirror/releases)**.
2. Install FizzyFacepunch **[unitypackage](https://github.com/Chykary/FizzyFacepunch/releases)** from the release section.
3. In your **"NetworkManager"** object replace **"Telepathy/KCP"** script with **"FizzyFacepunch"** script.
4. Enter your Steam App ID in the **"FizzyFacepunch"** script.

If the latest FizzyFacepunch unitypackage doesn't work then download the source of this repo,
and copy it to the `Assets/Mirror/Runtime/Transports/FizzyFacepunch` folder in your Unity project.
(because maybe someone already fixed it)

**Note: The  default 480(Spacewar) appid is a very grey area, technically, it's not allowed but they don't really do anything about it. When you have your own appid from steam then replace the 480 with your own game appid.
If you know a better way around this please make a [Issue ticket.](https://github.com/Chykary/FizzyFacepunch/issues)**

## Host
To be able to have your game working you need to make sure you have Steam running in the background.

## Client
1. Send the game to your buddy.
2. Your buddy needs your **steamID64** to be able to connect. The transport shows your Steam User ID after you have started a server.
3. Place the **steamID64** into **"localhost"** then click **"Client"**
5. Then they will be connected to you.

## Testing your game locally
You cant connect to yourself locally while using **FizzyFacepunch** since it's using steams P2P. If you want to test your game locally you'll have to use **"Telepathy/KCP Transport"** instead of **FizzyFacepunch**.
