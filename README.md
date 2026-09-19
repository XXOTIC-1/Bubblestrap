<div align="center">
  <h1>Tuffstrap</h1>
  <p>A barebones Roblox bootstrapper for FPS.</p>
</div>

> [!NOTE]
> **Tuffstrap only supports Windows 10 and later**.

**Tuffstrap** is a stripped-down Bubblestrap/Fishstrap fork. Five pages, English only, no Discord RPC, no mods, no themes. The settings window opens on **FPS**.

## Flag policy

Every flag Tuffstrap ships was checked against the compiled client flag table and a live `PCDesktopClient` dump. Flags that resolved in neither source were dropped rather than shipped, because a preset full of retired flags looks like it works and changes nothing.

Strict mode is on by default: flags outside Tuffstrap's allowlist stay in your list but are never written to Roblox. Pasting a large flag pack into the editor will not make it apply.

## FPS

- **Max FPS** and **Balanced** presets
- FPS cap slider (`DFIntTaskSchedulerTargetFps`), which turns off the 240 limiter for you above 240
- FPS counter, low textures, no grass, display-scaling fix
- MSAA, rendering mode, FRM quality, mesh detail

## Risky

Gated behind a warning you have to accept. These change what you can see, not how fast the game runs. They are undocumented, Roblox knows about them, and using them may get your account moderated or banned.

- Fullbright (gray sky + paused voxelizer)
- Wireframe rendering
- Skip mesh voxelizer

Turning the acknowledgement back off clears every risky flag.

## Launcher

- Fluent bootstrapper only
- Memory trimmer on, crash handler closed, no launch confirm
- Auto-update off
- Channel and deployment controls
- Fast Flag editor

## Building

Windows with the .NET 9 SDK:

```bash
dotnet publish Bloxstrap/Bloxstrap.csproj -c Release -r win-x64 --self-contained false -o publish
```

CI does this on every push and uploads `Tuffstrap.exe` as a workflow artifact.
