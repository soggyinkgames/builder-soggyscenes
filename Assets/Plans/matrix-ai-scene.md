# Matrix-AI Scene — Implementation Plan

> A new VR ride-along scene reusing the Geirangerfjord terrain, where the AI-generated
> seaplane flies (autopilot + manual takeover) through the fjord water channels, framed by
> a Cinemachine chase rig, under a Matrix x synthwave green aesthetic.

---

## Decisions captured (from clarification)
| Topic | Decision |
|---|---|
| Viewing mode | **VR ride-along** — keep XR rig; sit in a "chase seat" behind the plane; head still tracks freely |
| Camera | **Install Cinemachine 3.x**; CinemachineCamera drives a chase anchor (not the HMD directly) |
| Flight | **Arcade physics**, **Autopilot + Manual takeover toggle** |
| Look (interpreted) | **Green digital-rain skybox** + **synthwave grid on terrain/water**. Green post-processing **DEFERRED**. |

> ⚠️ **Open contradiction to confirm on review:** your look picks included both "Green digital-rain
> skybox" and "Just the grid for now". This plan includes BOTH the rain skybox and the grid, and
> defers post-processing. Also note **grid-water requires a water surface** (none exists in the
> current scene), so a stylised flat grid-water plane is included as part of the grid deliverable.
> If you'd rather skip the skybox or the water plane, say so and I'll trim Steps 6 / 7.

---

# Project Overview
- **Game Title:** Matrix-AI (showcase / AI feature test scene)
- **High-Level Concept:** A neon "digital fjord" where an AI-generated seaplane skims the water channels of a Matrix-styled Geirangerfjord while the player rides along in VR. A sandbox to test the studio's new AI-generated assets.
- **Players:** Single player (VR, seated/standing ride-along).
- **Inspiration / Reference Games:** *The Matrix* (digital rain), synthwave/outrun visuals, *Pilotwings* / flight showcase reels. Reference image: `Assets/_YOURNAME/Art/Textures/ai.png`.
- **Tone / Art Direction:** Dark, neon-green Matrix code on black, crossed with synthwave glowing grids. High contrast, emissive, minimal real-world color.
- **Target Platform:** Android (Quest / Meta XR via OpenXR).
- **Screen Orientation / Resolution:** VR stereo (HMD-native).
- **Render Pipeline:** URP 17.5 (Unity 6000.5.0b7).

---

# Game Mechanics

## Core Gameplay Loop
The seaplane continuously flies a looping route through the lowest fjord channels (the "water sections", world Y ≈ 40–120). The player rides in a chase rig behind the plane. At any time the player can **toggle manual control** to steer the plane with the VR controllers, then release back to autopilot. The loop is: *observe the stylised world → optionally take control → fly the water channels → release → repeat.* This is primarily a **showcase/testing** loop for AI-generated assets and the new look.

## Controls and Input Methods (New Input System / XRI)
- **Toggle autopilot ↔ manual:** primary button (A/X) — `MatrixFlightController.OnToggleControl`.
- **Manual flight (when active):**
  - Right thumbstick Y → pitch; Right thumbstick X → roll/yaw turn.
  - Left thumbstick Y → throttle (speed).
- **Head look:** free HMD tracking at all times (unaffected by chase rig motion).
- Inputs are read via XRI Input Actions (the rig already has `InputActionManager`). New `InputActionReference`s are wired in the controller; fallback to direct `XRController` thumbstick reads if no asset is exposed.

---

# UI
Minimal diegetic HUD (deferred-friendly):
- **Mode indicator:** small world-space TMP label on the chase rig dashboard: `AUTOPILOT` / `MANUAL` (green, glowing).
- **Speed/altitude readout (optional):** world-space TMP, green monospace, bottom of view.
- No menus this pass. (Wireframe: a single green text panel anchored ~1.2 m in front, lower third of view, semi-transparent black backing.)

```
+--------------------------------------------------+
|                                                  |
|            (open fjord / plane ahead)            |
|                                                  |
|        [ MANUAL ]      ALT 82m   SPD 140         |  <- green mono, lower third
+--------------------------------------------------+
```

---

# Key Asset & Context

## Reused assets
- **Terrain:** `Assets/Prefabs/Geirangerfjord/Terrain_Geirangerfjord.prefab` (+ TerrainData `Terrain_Geirangerfjord.asset`). Size **18000 × 1611.9 × 18000**; place at world **(32.77, 9.43, 59.16)** (same as current scene).
- **Seaplane:** `Assets/_YOURNAME/Art/Models/Generated/plane.prefab` (model `plane_Assets/selected.glb`, material `Shader Graphs/glTF-pbrMetallicRoughness`). Renderer bounds only **~1.0 × 0.37 × 0.89 m** → must be scaled up **~150×** (≈150 m) to read against the fjord.
- **XR rig:** copy `XR Origin Hands (XR Rig)` setup from `PROTOTYPE-YOUR-NAME-HANDS.unity` (XROrigin, CharacterController, InputActionManager, hands, locomotion). Main Camera near 0.01 / far 100000 (already large enough).
- **URP asset to clone:** `Assets/Settings/VR.asset` + `Assets/Settings/VR_Renderer.asset` (only if we later enable post-processing — see Deferred).
- **Reference image:** `Assets/_YOURNAME/Art/Textures/ai.png` (target mood).

## Flight corridor (from terrain height analysis — low "water" cells, world Y ≈ 10–120)
A meandering NE-trending low channel suitable as the autopilot route (X, Z in world units):
```
(4533, 6359)  y≈12  ->  (6333, 8159) y≈40  ->  (8133, 9959) y≈140
   ->  (8133, 11759) y≈10  ->  (9933, 13559) y≈46  ->  (9933, 15359) y≈56  ->  (9933, 17159) y≈65
```
Autopilot waypoints will hover **~30–60 m above** these floor heights, so cruise Y ≈ 60–180.

## New assets to create
> **All NEW assets live under a new top-level folder `Assets/_MATRIXAI/`** (not `_YOURNAME`).
> Reused assets (terrain prefab, plane prefab, `ai.png`) stay in their existing locations.

| Asset | Path | Purpose |
|---|---|---|
| Scene | `Assets/_MATRIXAI/Scenes/matrix-ai.unity` | The new scene |
| `MatrixFlightController.cs` | `Assets/_MATRIXAI/Scripts/` | Arcade flight physics + autopilot waypoint follow + manual input + toggle |
| `MatrixChaseRig.cs` | `Assets/_MATRIXAI/Scripts/` | Syncs XR Origin root to the Cinemachine chase anchor each LateUpdate (VR-safe) |
| `MatrixRainSkydome` (Shader Graph) | `Assets/_MATRIXAI/Shaders/` | Inverted skydome unlit shader: scrolling green code on black |
| `M_MatrixRainSkydome.mat` | `Assets/_MATRIXAI/Materials/` | Skydome material |
| `MatrixGrid` (Shader Graph) | `Assets/_MATRIXAI/Shaders/` | World-space green grid + emission (used on terrain & water) |
| `M_MatrixGrid_Terrain.mat`, `M_MatrixGrid_Water.mat` | `Assets/_MATRIXAI/Materials/` | Grid materials |
| `M_MatrixPlane.mat` | `Assets/_MATRIXAI/Materials/` | Green emissive seaplane skin |
| `MatrixCode` texture (optional) | `Assets/_MATRIXAI/Textures/` | Glyph/katakana strip for rain (or procedural noise in-shader) |

## Cinemachine VR integration note (important)
In VR the **HMD drives the Main Camera** via `TrackedPoseDriver`; a CinemachineBrain must **not** drive the HMD camera. Approach:
1. Add a `CinemachineCamera` (chase) with **Follow = plane**, position behind/above, with damping (Position Composer / Rotation Composer or Third Person Follow).
2. The chase CinemachineCamera drives a lightweight **proxy/anchor transform** (a hidden non-rendering brain camera or direct read of the vcam's resolved state).
3. `MatrixChaseRig.cs` copies the anchor's world pose to the **XR Origin root** in `LateUpdate`. The HMD then adds head pose **on top** via Camera Offset — head tracking stays free.
This gives smooth Cinemachine damping/framing without fighting XR head tracking.

---

# Implementation Steps

### Step 1 — Install Cinemachine 3.x
- **Description:** Add `com.unity.cinemachine` (3.x, Unity-6 compatible) to `Packages/manifest.json`; let it resolve; verify no console errors.
- **Assigned role:** developer
- **Dependencies:** None
- **Parallelizable:** Yes (independent of art steps)

### Step 2 — Create the matrix-ai scene + terrain
- **Description:** Create the `Assets/_MATRIXAI/` folder structure (Scenes, Scripts, Shaders, Materials, Textures). Create `Assets/_MATRIXAI/Scenes/matrix-ai.unity`. Add an instance of `Terrain_Geirangerfjord.prefab` at world (32.77, 9.43, 59.16). Add a Directional Light (rot 50,330,0). Keep ambient dark. Add scene to Build Settings.
- **Assigned role:** developer
- **Dependencies:** None
- **Parallelizable:** Yes

### Step 3 — Bring in the VR rig
- **Description:** Add the XR Origin Hands rig (XR Interaction Manager, XR Origin + Camera Offset + Main Camera, hands, locomotion, InputActionManager) into matrix-ai, mirroring PROTOTYPE-YOUR-NAME-HANDS. Confirm a single MainCamera/AudioListener. Verify XR launches.
- **Assigned role:** developer
- **Dependencies:** Step 2
- **Parallelizable:** No

### Step 4 — Seaplane setup (scale + physics)
- **Description:** Instance `plane.prefab`, scale ≈150×, add Rigidbody (gravity off / scripted), colliders, and `M_MatrixPlane.mat` (created Step 7). Place at the route start (~X4533, Y80, Z6359). Tag for camera follow.
- **Assigned role:** developer
- **Dependencies:** Step 2
- **Parallelizable:** Yes (can run alongside Step 5/6/7)

### Step 5 — Flight system (`MatrixFlightController.cs` + autopilot + manual + toggle)
- **Description:** Implement arcade flight: forward cruise, banking turns, altitude hold. **Autopilot** follows the waypoint list (see corridor) hovering above terrain height (sample `Terrain.SampleHeight`). **Manual** reads XRI thumbsticks (pitch/roll/throttle). **Toggle** on primary button. Loop the route. Author the waypoint set as serialized Transforms or Vector3 array.
- **Assigned role:** developer
- **Dependencies:** Step 1 (input), Step 4 (plane)
- **Parallelizable:** No

### Step 6 — Cinemachine chase rig + VR sync (`MatrixChaseRig.cs`)
- **Description:** Add CinemachineCamera (Follow = plane, behind+above, damping). Implement `MatrixChaseRig.cs` to copy the resolved chase pose to the XR Origin root in LateUpdate so the player rides behind the plane while the HMD adds free head look. Tune offset/damping for comfort (avoid harsh accelerations → VR sickness).
- **Assigned role:** developer
- **Dependencies:** Step 1, Step 3, Step 5
- **Parallelizable:** No

### Step 7 — Matrix x synthwave shaders & materials
- **Description:**
  - **MatrixRainSkydome** Shader Graph (unlit, double-sided/inverted): scrolling green glyph/noise columns on black, emissive. Apply to a large inverted sphere skydome enclosing the scene (radius ~9000+). Keep RenderSettings ambient dark.
  - **MatrixGrid** Shader Graph: world-space grid lines (frac of world XZ), green emission, fade with distance. Create terrain & water variants.
  - Assign `M_MatrixGrid_Terrain.mat` to the terrain's Material slot (world-space grid works without splatmaps).
  - Add a large flat **grid-water plane** at sea level (Y ≈ 40) using `M_MatrixGrid_Water.mat` so the plane visibly flies "through water sections".
  - `M_MatrixPlane.mat`: dark with green emissive edges/wireframe.
- **Assigned role:** developer
- **Dependencies:** Step 2 (terrain present)
- **Parallelizable:** Yes (art track, parallel with Steps 4–6)

### Step 8 — Minimal HUD
- **Description:** World-space TMP `AUTOPILOT/MANUAL` label (+ optional ALT/SPD) on the chase rig, green monospace, driven by `MatrixFlightController` state.
- **Assigned role:** developer
- **Dependencies:** Step 5, Step 6
- **Parallelizable:** No

### Step 9 — Integration pass & comfort tuning
- **Description:** Play-test in VR: verify autopilot stays in channels (no terrain clipping), manual control feels responsive, chase rig is smooth and comfortable, look reads as Matrix/synthwave. Tune speeds, damping, grid density, rain speed, emission/bloom-readiness.
- **Assigned role:** developer
- **Dependencies:** Steps 3–8
- **Parallelizable:** No

### (Deferred) Step 10 — Green post-processing
- **Description:** Clone `VR.asset`/`VR_Renderer.asset` for matrix-ai (enable HDR + depth texture), add a Volume profile (Bloom + green Color Adjustments/Split Toning + Vignette, optional scanline/CRT fullscreen renderer feature), enable `renderPostProcessing` on Main Camera. **Deferred per "just the grid for now"** — flip on when desired.
- **Assigned role:** developer
- **Dependencies:** Step 9
- **Parallelizable:** Yes (when enabled)

---

# Verification & Testing
- **Scene loads:** matrix-ai opens with terrain at correct world position; no missing references; console clean.
- **Cinemachine present:** package resolves; CinemachineCamera exists; no errors.
- **VR launch:** HMD tracks; single AudioListener; hands/locomotion intact.
- **Autopilot:** plane follows the waypoint corridor without clipping terrain; loops cleanly; cruise altitude sampled above terrain height.
- **Manual toggle:** primary button switches modes; HUD label updates; thumbstick pitch/roll/throttle respond; releasing returns to autopilot.
- **Chase rig comfort:** player rides behind the plane, head look free, motion smooth (no nausea-inducing jerks); test damping values.
- **Look:** green digital rain visible on the sky; world-space green grid on terrain + water reads as synthwave/Matrix; plane emissive.
- **Performance (Quest/Android):** check frame timing; grid/rain shaders mobile-friendly (no excessive overdraw on the large skydome/water plane); consider MSAA vs bloom trade-offs before enabling post FX.
- **Edge cases:** plane reaching last waypoint loops to first; manual mode preventing leaving terrain bounds; terrain SampleHeight at route extremes; skydome large enough to never clip the 18 km terrain.
