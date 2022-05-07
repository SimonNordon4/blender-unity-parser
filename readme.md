# Blender to Unity

The goal is to create a Blender & Unity plugin that makes importing content from Blender to Unity seamless & Easy.

## Stratergies

There's Several ways we could approach this:

Modify the Unity-BlenderToFBX script in order to have a finer control over how blender is exported to an FBX. We can then use AssetPostproccesor Class to do further modifications after Unity has imported that FBX.

**Pros:** Uses Unity Default Import Methods.

**Cons:** Uses Unity Default Import Methods.

Or

Extend DCC Plugins to be more feature rich. Drawback is this is another Unity Reliance.

Or

Create our own Blender - Unity pipeline that bypasses as much of Unity's default importers as possible.

___

https://docs.unity3d.com/Manual/ScriptedImporters.html

This would allow us to create our own custom Importer for a blend model, or probably more likely a ".unityblender file (*.ublend) that prints the current blender file in a unity friendly way.

The .ublend file would be specifically for unity so that "export to .ublend" would just feed Unity exactly what it needs.

### To Do
- [ ] Import Meshes
- [ ] Import Textures
- [ ] Import Materials