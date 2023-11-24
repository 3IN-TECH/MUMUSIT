# Universal Metaverse Kit - MUMUSIT Project

Universal Metaverse Kit is a pioneering initiative aimed at advancing the development of multi-platform and multi-user solutions, contributing to the evolution of the Metaverse. Our project seeks to streamline the creation of immersive, interactive, and collaborative XR experiences.

## Table of Contents

- [Dependencies](#dependencies)
- [Template Usage](#template-usage)
- [Template Tools](#template-tools)
- [Avatar Selector Configuration](#avatar-selector-configuration)
- [Online Template Prefab](#online-template-prefab)
- [Creation of Avatar Prefabs](#creation-of-avatar-prefabs)
- [Selection of VR/Non-VR Modes](#selection-of-vrnon-vr-modes)
- [Folder Structure](#folder-structure)

## Dependencies

Before using the VRTemplate, ensure the following dependencies are installed and set up:

- Unity XR plugins
- OpenXR plugin
- XR Interaction Toolkit
- Photon: Multiplayer platform for Unity
  - Photon PUN
  - Photon Voice
- Firebase: Google’s web application development platform
  - Firebase Auth
  - Firebase Database
  - Firebase Storage
- Animation rigging
- TextMeshPro

## Template Usage

This template includes two scenes:

1. **LobbyRoom:**
   - Offline scene for users to select avatars, microphone settings, etc.
   
2. **OnlineRoom:**
   - Online room where players can meet.

## Template Tools

### LobbyTemplate Prefab

- **EventSystem:**
  - Unity's event system with XR UI Input module.
- **Firebase:**
  - Singleton with functions for Firebase calls.
- **AvatarSelector:**
  - UI element for player avatar selection.
- **LobbyCanvas:**
  - Logic to connect to an online room.

### OnlineTemplate Prefab

- **Pun Voice:**
  - Uses Photon to share audio between players.
- **Spawn system:**
  - Handles player spawn logic.

## Avatar Selector Configuration

The AvatarSelector GameObject requires configuration:

- **AvatarCanvas:**
  - Avatar Prefabs: List of avatars prefabs.
  - Buttons in canvas: List of buttons included in the canvas.
  - Buttons to change avatar: List of buttons for selecting a player avatar.
  - Offset for mirror: Display options for the avatar mirror in the lobby.

## Online Template Prefab

Handles player spawn logic and audio in the online room.

## Creation of Avatar Prefabs

1. Copy the template from `Assets\VRTemplate\Resources\Avatar\Avatar_X`.
2. Add the 3D model as a child of the prefab.
3. Use the AvatarPlayer component to set Head, Left Hand, and Right Hand constraints.
4. Optionally, hide parts for VR by adding them to the list of hidden parts.
5. Save the new prefab.

## Selection of VR/Non-VR Modes

To select VR/Non-VR mode:

1. Use the proper player prefab (VR or NotVR Player).
2. In Project settings, enable XR Plugin Management for VR build or disable for a non-VR build.
