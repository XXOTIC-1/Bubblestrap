#!/usr/bin/env bash
# Cloud Agent environment bootstrap for Bubblestrap.
#
# Bubblestrap is a Windows-only WPF app (TargetFramework net9.0-windows...).
# It cannot run on the Linux Cloud Agent VM, but it can be restored and
# compiled here for verification/CI using EnableWindowsTargeting.
set -euo pipefail

DOTNET_DIR="$HOME/.dotnet"
DOTNET_CHANNEL="9.0"

# Install the .NET SDK if it is not already present (a saved snapshot may
# already contain it, in which case this is skipped).
if [ ! -x "$DOTNET_DIR/dotnet" ]; then
  echo "==> Installing .NET SDK ${DOTNET_CHANNEL}"
  curl -sSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
  chmod +x /tmp/dotnet-install.sh
  /tmp/dotnet-install.sh --channel "$DOTNET_CHANNEL" --install-dir "$DOTNET_DIR"
fi

# Expose dotnet on PATH for the agent's interactive terminals. /usr/local/bin
# is on the login PATH; fall back to ~/.local/bin if sudo is unavailable.
if [ ! -e /usr/local/bin/dotnet ]; then
  if sudo -n true 2>/dev/null; then
    sudo ln -sf "$DOTNET_DIR/dotnet" /usr/local/bin/dotnet
  else
    mkdir -p "$HOME/.local/bin"
    ln -sf "$DOTNET_DIR/dotnet" "$HOME/.local/bin/dotnet"
  fi
fi

export DOTNET_ROOT="$DOTNET_DIR"
export PATH="$DOTNET_DIR:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# Ensure the Wpf.Ui submodule is checked out.
echo "==> Syncing git submodules"
git submodule update --init --recursive

# Restore and build so a fresh agent starts from a verified, compiled state.
echo "==> Restoring NuGet packages"
dotnet restore Bloxstrap/Bloxstrap.csproj -p:EnableWindowsTargeting=true

echo "==> Building Bubblestrap (Release)"
dotnet build Bloxstrap/Bloxstrap.csproj -c Release -p:EnableWindowsTargeting=true --no-restore

echo "==> Environment ready. Use: dotnet build Bloxstrap/Bloxstrap.csproj -p:EnableWindowsTargeting=true"
