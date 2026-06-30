# Project Technical Overview: Soggy Scenes / Auslan Gesture Experience

## 1. Project Description
This project is an immersive VR experience designed for the Meta Quest, focusing on **Auslan (Australian Sign Language)** hand gesture recognition and interactive visual/audio feedback. It utilizes Unity 6 (6000.5.0b7) and the XR Hands package to track player gestures (like the alphabet) and trigger environmental effects. The core pillars are accessibility, tactile-like VR interaction without controllers, and visual storytelling through SDF (Signed Distance Field) driven VFX and custom shaders.

## 2. Gameplay Flow / User Loop
1.  **Boot/Initialization**: The application starts in a VR environment (e.g., `PROTOTYPE-YOUR-NAME-HANDS.unity`). The XR Interaction Toolkit and XR Hands subsystems initialize to track the user's hands.
2.  **Interaction Loop**: 
    *   **Gesture Recognition**: The user performs specific Auslan hand gestures.
    *   **Distance/Touch Checks**: System monitors if hands are touching or in specific relative positions while performing gestures.
    *   **Event Triggering**: Valid gestures or hand interactions fire `UnityEvents`.
3.  **Feedback**:
    *   **Visual**: Environmental shaders (like `ShowTheWay`) reveal paths or objects using cutoff height transitions. VFX (managed by `SDFManager`) update based on gesture input.
    *   **Audio**: The `AudioManager` plays sound clips associated with specific gestures or alphabet letters.
4.  **Progression**: Users move through "Soggy Scenes," triggering sequential events that reveal more of the environment or play educational audio content.

## 3. Architecture
The project follows an **Event-Driven Architecture** combined with a **Singleton Manager** pattern for global systems.

*   **Entry Points**: Major scenes like `PROTOTYPE-YOUR-NAME-HANDS` contain the `XR Origin` and manager objects.
*   **System Communication**: Uses static C# events (e.g., `AudioEvents`) to decouple triggers from audio playback, while using `UnityEvents` in the Inspector for scene-specific interactions (e.g., `HandsTouchingEvent` to `SDFManager`).
*   **Data Flow**: `AudioScriptableObject` defines sound data -> `AudioManager` listens for `AudioEvents` -> `AudioSource` components are dynamically generated or managed to play sounds.

`Location: Assets/_YOURNAME/Scripts`

## 4. Game Systems & Domain Concepts

### Gesture & Interaction System
Tracks hand movements and specific static gestures to trigger logic.
*   `HandsTouchingEvent`: Monitors the distance between two hands/transforms and requires specific `StaticHandGesture` performance to fire events.
*   `WristTracked`: Smoothly attaches UI or objects to the user's wrist using `SmoothDamp` and `Slerp`.
*   `TriggerUnityEvent`: A standard trigger-volume utility that filters for the "Player" tag and invokes `UnityEvents`.
`Location: Assets/_YOURNAME/Scripts`

### Audio System
A centralized management system for sound effects and alphabet voice-overs.
*   `AudioManager`: A persistent singleton that handles playback, pooling/lifecycle of `AudioSource` components, and scriptable object lookup.
*   `AudioScriptableObject`: Stores clip data, volume, pitch, and mixer groups.
*   `AudioEvents`: A static class providing global access to `OnPlaySound` and `OnStopSound` delegates.
`Location: Assets/_YOURNAME/Scripts`

### Environmental VFX System
Handles visual transitions and SDF-based effects.
*   `SDFManager`: Controls `VisualEffect` (VFX Graph) assets by switching `Texture3D` (SDFs) and sending events to the graph.
*   `ShowTheWay`: Animates a material's `_CutOffHeight` property to create "reveal" effects in the environment.
*   `RevealPath` & `DarkToLight`: Scripts that likely manage shader parameters for environmental progression.
`Location: Assets/_YOURNAME/Scripts`

## 5. Scene Overview
*   **Core Prototype**: `PROTOTYPE-YOUR-NAME-HANDS.unity` - The primary testing environment for hand interactions and Auslan gestures.
*   **Alphabet Scenes**: `HandGesturesAlphabet.unity` - Focused on educational sign language alphabet interactions.
*   **Demo Scenes**: `DemoVRKeyboardUS.unity` and `KeyboardDemo.unity` - Integration of Black Whale Studio's VR Keyboard for text input.
*   **Soggy Scenes (1-3)**: Level-based scenes (e.g., `SoggyScene1`) demonstrating environmental storytelling and shader transitions.

## 6. UI System
The project uses a mix of **World-Space UGUI** and **XR Composition Layers**.
*   **Wrist UI**: Managed by `WristTracked.cs`, allowing menus to follow the hand movement with configurable smoothing.
*   **Spatial Keyboard**: Uses the `VR Keyboard XRI` prefab for input, often attached to composition layers for better clarity in VR.
*   **Visual UI**: Particle-based and shader-based UI elements (e.g., `soggy-vfxui.unity`) provide feedback for gesture success.

## 7. Asset & Data Model
*   **ScriptableObjects**: Extensively used for audio (`AudioScriptableObject`) to allow designers to tweak sounds without touching code.
*   **Prefabs**:
    *   `SOGGY XRI Spatial Keyboard Variant`: Customized keyboard for VR input.
    *   `SoggyStand`: Environmental prop.
*   **Naming Conventions**: Scripts in `_YOURNAME/Scripts` use PascalCase (e.g., `SDFManager.cs`). Textures and Materials often use prefixes like `T_` and `M_`.
*   **External Assets**: Utilizes `Dreamteck Splines` for pathing (e.g., `ParticlePathTrigger`) and `Oculus/XR Hands` for tracking data.

## 8. Notes, Caveats & Gotchas
*   **Gesture Dependency**: `HandsTouchingEvent` relies on `UnityEngine.XR.Hands.Samples.GestureSample`. If upgrading XR Hands, ensure this namespace and the `StaticHandGesture` class haven't changed.
*   **Tagging**: Many triggers (`ShowTheWay`, `TriggerUnityEvent`) strictly check for the `"Player"` tag. Ensure the XR Origin/Camera Offset is correctly tagged.
*   **SDF Logic**: `SDFManager` requires a `VisualEffect` component with a specific texture property named `SDF_Texture`. Changing the property name in the VFX Graph will break the link.
*   **Audio Lifecycle**: Non-looping sounds in `AudioManager` are destroyed automatically based on clip length. Be careful with very short clips and high-frequency triggers to avoid overhead.