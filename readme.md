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

### To Do
- [ ] Import Meshes
- [ ] Import Textures
- [ ] Import Materials