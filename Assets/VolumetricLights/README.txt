********************************
*       VOLUMETRIC LIGHTS      *
*      Created by Kronnect     *   
*          README FILE         *
********************************

Requirements & Setup
--------------------
1) Volumetric Lights works only with Universal Rendering Pipeline (v7.1.8 or later)
2) Make sure you have Universal RP package imported in the project before using Volumetric Lights.
3) Make sure you have a Universal Rendering Pipeline asset assigned to Project Settings / Graphics. There's a URP sample asset in Demo/URP Pipeline Settings folder.


Demo Scene
----------
There's a demo scene which lets you quickly check if Volumetric Lights is working correctly in your project.


Documentation
-------------
Please read the documentation (PDF) for additional instructions and options.


Support
-------
* Support: contact@kronnect.com
* Website-Forum: https://kronnect.com/support
* Twitter: @Kronnect


Future updates
--------------
All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of Volumetric Lights will be eventually available on the Asset Store.


Version history
---------------

v4.7
- Added new demo scene (temple)

v4.6.2
- Added Xbox compatibility

v4.6.1
- Added dithering option to Volumetric Lights Render Feature
- Volumetric Lights Render Feature optimizations
- Shadow occlusion render texture optimization

v4.6
- Added downscaling option to Volumetric Lights Renderer feature

v4.5.1
- Improved edge preservation when blur option is used

v4.5
- Added custom depth prepass for improved support of transparent and semi-transparent objects

v4.4.1
- Target camera property is now exposed for all light types (used for autoToggle options)
- [Fix] Fixed mesh being generated by inspector when volumetric light is disabled
- [Fix] Fixed PS4 compilation issue

v4.4
- New option Shadow Orientation for point lights
- Optimization related to baked shadows for point lights in OnStart mode

v4.2
- Added auto-toggle "Time Check Interval" parameter.
- Removed macro console warning in Unity 2020.1
- When "Sync With Profile" is enabled, properties in inspector are now readonly
- Using cookie texture no longer requires occlusion cam improving performance

v4.1
- Simplification. It's no longer required to create a volumetric light profile to use the effect.
- Timeline support is now simpler. The VolumetricLightAnimation is no longer used. Instead you can modify directly the VolumetricLight fields.

v4.0
- Added global blur option (check documentation)
- Removed top limit for shadow intensity
- Prefab instances no longer forces unpack
- [Fix] Fixed an issue when creating a volumetric point light in prefab mode

v3.6.5
- Improved compatibility with Unity 2020.3
- Removed a harmeless Editor warning message related to 3D textures

v3.6.4
- Profile must now created specifically when using volumetric lights in prefabs

v3.6.3
- Improved dust particle rendering

v3.6
- Shadow pass optimization when dust particles are also enabled
- Added support for Timeline/Animation (check documentation to enable this option)
- [Fix] Fixed an issue with occlusion camera being destroyed during a physic event resulting in a console error
- [Fix] Fixed/changed the way spotlights render when angle is >90

v3.5
- Added support for "Colored Cookie Texture" (new option in profile)

v3.4
- Added ability to dim and/or deactivate volumetric effect based on distance. Added "Autotoggle", "Distance Start Dimming" and "Distance Deactivation" properties
- [Fix] Fixed an issue that cause the effect no rendering properly when changing light range at runtime

v3.3
- When duplicating an existing volumetric light, settings are no longer shared unless they use an previously created profile asset
- When creating a Volumetric Fog Profile, the resulting asset is now placed in the current selected folder
- Volumetric area lights can now be linked to directional lights to keep their rotation and color synced
- Material handling optimizations

v3.2
- Added Raymarch Presets which provides reference quality/performance settings
- Improved default settings when adding new volumetric lights to the scene
- [Fix] Fixed dithering issue that could add an halo artifact around certain lights

v2.5.3
- [Fix] Fixed editor issue when dragging a volumetric light into a new prefab

v2.5.2
- Minor shader optimizations

v2.5.1
- Improved behaviour when instantiated from a prefab

v2.5
- Ensures light transform scale is normalized to prevent scaling issues when parent transform is changed
- Reduced shader variants by 1/4

v2.4 11-Sep-2020
- API: added "settings" property to allow modifications of individual lights without affecting a shared profile
- [Fix] Fixed particles not appearing immediately when disabling/reenabling light

v2.3 8-Sep-2020
- Added Shadow Auto Toggle and Shadow Visible Distance to optimize shadow rendering based on light distance to camera
- Added further optimizations for dust particles and shadows when not visible in frustum


v2.2 22-Aug-2020
- Added support for orthographic camera
- Added "Raymarching Min Step" parameter which can improve performance
- [Fix] Fixed VR issue due to URP not setting inverted VP matrices correctly
- [Fix] Fixed rare clipping issue on Android due to lack of floating point precision 

v2.1.4 28-Jun-2020
- Improved fit for rect light shadow map

v2.1.3 23-Jun-2020
- Particle system user modifications are now preserved
- Improved bluenoise sampling
- Added "Attenuation Mode" option (Simple and Quadratic modes supported with ability to specify quadratic coeficients)
- Added "Blend Mode" : PreMultiply
- Dust Particles: added "Distance Attenuation" and "Auto Toggle" options based on distance
- Improved profile editor UI
- [Fix] Fixed clipping issue on some platforms

v1.4 14-JUN-2020
- Enhanced blue noise jitter operator

v1.3 9-MAY-2020
- Added blue noise option
- Changed default render queue to 3101 for improved compatibility with Volumetric Fog & Mist 2

v1.2 April 2020
- Shadow occlusion optimizations
- [Fix] Fixed VR issues

v1.1 April 2020
- Added warning to inspector if Depth Texture option is not enabled in URP settings override in Project Settings / Quality
- API: added ScheduleShadowUpdate() method to issue a manual shadow update when shadow bake at start is enabled

v1.0 March / 2020
First release