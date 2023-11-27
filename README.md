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

You can find more information about the installation and configuration process here: [Install dependencies](#install-dependencies)

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

---

---

# Install dependencies

UMK has a few dependencies, some of them need to be installed and set up before using it:

-   Unity 2022.3.14f1

-     PC/MAC/iOS/Android Build Support

-   Unity XR plugins

-     OpenXR plugin

-     XR Interaction Toolkit

-   Photon: Multiplayer platform for Unity

-     Photon PUN

-     Photon Voice

-   Firebase: Google's web application development platform providing multiple tools.

-     Firebase Auth

-     Firebase Database

-     Firebase Storage

-   Animation rigging

-   TextMeshPro

-   Universal Metaverse Kit: <https://github.com/3IN-TECH/MUMUSIT/releases/tag/v0.1>

Setup XR Interaction Toolkit
----------------------------

1.  Go to Edit/Project Settings.../XRPluginManagement and install XR Plugin Management.

![](https://lh7-us.googleusercontent.com/ZmO193Jwdu0uACCYC6DAJr5i-nRwEsfUKKHsM5qXcg4oe7PLI9rFp5FY_jEGjYBE0C6zhTkHYhKfBES62yExy_4zjq1Xu9708O1VEaFdouWfIQCMIGClVr2aI_hRhKq89-Z-xtI_jNCwcnLV94pDg7Y)

2.  Once it is installed, check the OpenXR option for the required platforms. Be patient, as this will install more packages.

![](https://lh7-us.googleusercontent.com/uU7NlzG3QuzFpeMAA3UVZYJRq3qtZ6IJHYfFd-HMgnWXz2s0H4Q-CKrU2XKp32Kdv5Dzwxr1VJ-JC0Bjdl7IQ18YMpCaIyebBaJ9lb1Pwaxm9bt6y8TFmKLzt9vZfaDsgz8dRPbZjADf6rBwy-5oPgc)

3.  Click Yes if prompted to change the input system.

![](https://lh7-us.googleusercontent.com/FNQRnygw6saEQ86j5hmiAEUdB0kgljl50LZw0hSXKeeYxNNUG8hwHki8GS2gesuJwkgu9Q2lOPAgTBiUDiZWd0JEoyXrYjQMdS05Upc_gfwMYabmuvy61pzyKkWyDrXbv5V2PUgZZRr0xIgg1Ny4qJE)

4.  Open OpenXR settings under XR plugin management in the left side menu

![](https://lh7-us.googleusercontent.com/jLRUwkkfou8jSllwtiEIv_2709ZSL3vXneqMnseCIO8MKU-dfOUjER_UICIOgjGEth2nAXF3ARMWQhP8bes3zTZQeJfrh6D57--6bnR_ETGWpdFheljcs9qSFCKnkH2eY_sIwMz0A9N5LFIRI612Qt0)

5.  Add devices in "Enabled Interaction Profiles" section

![](https://lh7-us.googleusercontent.com/0AaD1hT0xzYtaYEMKoMSSUA0QtpEqR6AjURGi2gw5Nm9UiDqmucM0-_Jnd0FVDD0pVR6QF9pc4QVDT9LWKX_THiihudd1AN23LU-SWYBP7OrcX7UDox-i3B94iVNyQWmG_FUUeLHQz43pAYlGPDwDWI)

6.  In the package manager, install XR Interaction Toolkit.

![](https://lh7-us.googleusercontent.com/qyouVB6LgjeU_Cwuoe9TwZ3wQo_5FWUuteQMEG6CkkIHJvnXvAgfBfm_3TtGJBd9wrDcx19LFoyeUQG5VTECrnDZT9xnqz5Wum2gn_W8Rfv5L7wv_6jhR4LrI8gc0MLT-wt7R7vLWbcp5aVLkT__vsg)

![](https://lh7-us.googleusercontent.com/csG5OjNPWY8Elzj_dZIIMf0nY7ezWjE4ay0FSTnlKcF59UubJkyvg3vbXNPNVh1cgp_-PbHDKIdNXAu9WtFuIbvDDHe3WUBm86LxuLNk1qxyGK3HX4md5nbiBvafnYFUBeUMBNpeBHKICjyX5UEQOdY)

7.  If prompted, click "I Made a BackUp, Go Ahead!" to continue.

![](https://lh7-us.googleusercontent.com/AGjZgOQorLYnfJyRb0bNsHVJQs-JBfCkldUsq1s1lKONDaZuFaQB0hnllhs_rvLxQgpb7tWlrFQTzvehb9wJu6-nR6j7qLLTwfaxykVICLHiYQj8FqAvuD0-cdRlqyVuK-uEmKVaClV66Gjwd8SRS5E)

8.  Go to XRInteraction Toolkit-> Samples  and install Starter Assets.

![](https://lh7-us.googleusercontent.com/6qfXFU5qUh0VtnFFwZtW1T74LZWRQ2AbatJ1_lGdWQre8ljN_EdnAK3jUNti9CuuDmwLs-6H29dW7swf4DEwZztjAGcRayThPsYPI-Wipr2VReNfNw05wYo_krNZFlURry_OAVoOggwFTQCc9y2zzCg)

![](https://lh7-us.googleusercontent.com/dgDHNxbkjsm2lBY_R9ntCsHh4eEZzUFhHeNdmPhSjHJz65AKK4n-gR1vMPl4dgO19EW1D7yT4c66uI21P50mClFWGVmN6Fldxn-OT1ZAgaoP_Ak_jmZ3zKWionSAL-N9yL4KB9itnnR9yjolWEcoy8A)

Setup Photon
------------

1.  Download and import the assets: <https://assetstore.unity.com/packages/tools/network/pun-2-free-119922> and <https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518>

If the PUN Wizard window appears , don't close it:

![](https://lh7-us.googleusercontent.com/Nn0D8lFa56nbCUgv0Be95jXpieQ7abtUQosDBEzNwm1EUTCbaXPz_QMfywzfQzaTUe8i9n4pCGY7sEhcGQdWnmJyiXNAW4Gwyst3gWui4VABSOb1ZesCvRCGo8wBvQtz5qxi1FzliRXj5T00Y5qdnaU)

2.  Now we need two IDs for Photon PUN and Photon Voice.

3.  Enter in the web: <https://www.photonengine.com/>

4.  Login or Sign In in your photon account and go to your dashboard

5.  Create new app\

![](https://lh7-us.googleusercontent.com/57Uh3F9H_9BGzhYrHwVLeig5jPpUZvdQiUQkeHTqxkS3qFgFAq4fe68ZgiVTo5uzEI7CrC_Pt97K3lbaVQ0K0MO-zSToaQib73uKPwVBOKdPjoOtgUof5eVTuKCL-e5PUhjx3zl1UvDqKwU0TW_KSqc)

6.  Select "Pun" as the Photon SDK, and enter an application name.\

![](https://lh7-us.googleusercontent.com/l1FERbgi-3oQYArioPtrCEKY0uOM1jxxFZZDK_uO1DdPCuL98ExBMfgO0ip-ncWOxLTjTCp8mSkGpSdimfPatd_zL4G7TU2JwGS0ZrTvShgV3pDgI1iNNF7T_XDwJNcIWMH47fPtE5T8Ny8k4QY7rrE)

7.  Repeat steps 5 and 6 but now selecting "Voice" as the Photon SDK.

8.  You will need the App ID of each project:\

![](https://lh7-us.googleusercontent.com/GOJzLshACUJmYIyr2xWgxGoKSDf6NA4HA3Uq4wEyyjgvIBG3PTTm5hUUHkxl6c0rx0zB9XrUJbAM04hFnJaDVShs2KxYEJR3-3i_9ZpDhVSoTcQtuwjeTIu_brmOX4C8dGX3Z4JAm-T7rddS1VF1HLc)![](https://lh7-us.googleusercontent.com/gJRQF_oIkVs6oacDu7rrlhRdnID0GkL2HiZd8MiZQXxMXQsSuyXLalJrdjjWW2m0rKo56ESxhCHI4l2opexp4wJ5qCSmVIWygcubB37pHZpf7XwkYDwACA0H2mJupI5fka-W3ETi2ONUKGmDG0Vjgns)

9.  Add the App ID of the Pun project in the PUN Wizard window in Unity\

![](https://lh7-us.googleusercontent.com/NJaJYePFYyuZCGW973Hu5GgODSpSd4Rkx5uDoOQFnMlgoOtIvXD67OmZd5hy9SJuAQJYgThXy8ANHI9AsXVFt5vleBzLKwmvab-UQdAZJvpLY6V4X_qUvhLSe6WrXbtXcQ-ebWanUPDDr5RUEb_CUHg)

10.  In Unity, click on Window/Photon Unity Networking/Highlight Server Settings.

![](https://lh7-us.googleusercontent.com/YWIYkUjo9ZN4iRz2Xcc8BWUSa1hO61TH78vWabyTC9SvUm3mvS3Kr41sOlx8m7A7jLdHLdUG13pr7_AyVrsmq6TeEuFKZ2CqS_VOrHaDIZEsLpsNaKIusAzpsZtGDBi_FRI4DjfXkuF8RiDzxAlxluA)

11. Add the App IDs of both Photon Projects (PUN and Voice)

![](https://lh7-us.googleusercontent.com/OrIDWPEOgy44ZKWvTyNYWINpslOao4JHo6DgAf_f4OH2O4qpPQbzt0cOTlgFnkzP6BJvxkXyDl09hGqgAENAPOKinPiGVgUTi2u7EbqvdO0tdQLeTNr78ekd9VOtHDNNZQe63YdVVfmXhjpamf8dK1c)

Setup Firebase
--------------

1.  Enter in Firebase: <https://firebase.google.com/>

2.  Create a Project

3.  Add a Unity app and follow the steps that will appear after that.

4.  Make sure to initialize the services you want to use and after that, re-download the google-service.json from the project configuration / your apps / sdk configuration.

5.  Now install the services you are going to use from the firebase_unity_sdk you downloaded creating the app, do not forget that this project uses FirebaseDatabase, FirebaseAuth and FirebaseStorage, although you can always comment the code that gives you an error if you do not want to use any of the services, although it is recommended not to do it with FirebaseAuth for the security of your data.

![](https://lh7-us.googleusercontent.com/-aKysgyw21Es7RWQL3mCUi1YfLQ0-8toL1rCD558qwdlbnkNQOdHSzfFm74HT_4Cfh88zDQABe1f3dZw3xzUyJQFayXVCkmYp2h4NJJfbEr_swltTyCBuHVjI-CbQYY_EF_94PzFmZz16-M1ZZO0btU)
=================================================================================================================================================================================================

Setup VRTemplate
----------------

1.  Import unity package from Package Manager "Animation Rigging"

2.  Import unity package from Package Manager TextMeshpro Essentials (If it is not installed)

3.  Import Universal Metaverse Kit from <https://github.com/3IN-TECH/MUMUSIT/releases/tag/v0.1>

4.  Build scenes

![](https://lh7-us.googleusercontent.com/3iGDgZ3Sb4w2SJFhs56PLLD_ZRuFwpiD5_tmh4r3-8edRbBgL_nSNxSWBzdDN2gbKrlvQcLujG4wNnwDwQAHm8XU_V9RIhZ9o4mBsJi8IY-7cPAb6vG19fmBoJHmSz3biNrxF1B6GlAwoN_m75JgWa4)

5.  Go to VRTemplate/Resources/Player and inside the prefab "VR Player" you will find a child GameObject called Settings with a component called VRSettingsPlayer. In the parameter "Settings Menu Action"  add the action "XRI Left Hand Interaction".

6.  Fix warnings in Project Settings/XR Plugin-in Management/Project Validation

![](https://lh7-us.googleusercontent.com/tX5FQ11MLRW1fTL534y4p9_qS2xdfmatA3IpQanqfsIi4AQB2yDWWAWFIoV2nmozpgSjGFjOCMXKVTw2Dex4f09W9r5h5XElDN2gsqgD1_U7KKuYEh6rBWoH20_fJUOZPk4U8kOP0bb7ffo0pYdrUDk)
