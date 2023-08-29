# Upgrading
The safest way to upgrade to a new version is to download the project and place it in a different directory. If you cannot do this because you have many assets and don't want to manually copy and paste everything, or maybe you are using the pull command, or for any other reason, you need to delete the library folder each time.
## Important:
Every time you upgrade to a new version of the TValle.SMA.Modding project, delete the TValle.SMA.Modding/Library folder.
## Important:
Every time you upgrade reimport all clothing meshes by right-clicking and selecting Reimport.

# Modding clothes

Download this Unity project to your Documents folder.

You need:

Blender 2.93 (optional): https://www.blender.org/download/releases/2-93/

"SMA Clothing Avatar" Blender Project: https://drive.google.com/drive/folders/1Ns57uRbClJEcZo0K3eE5zS1WH9RpUcdF?usp=sharing

Unity: https://download.unity3d.com/download_unity/fb119bb0b476/Windows64EditorInstaller/UnitySetup64-2022.3.0f1.exe

You can also download Unity from Unity Hub; select LTS version 2022.3.0f1.




# Blender (optional; you can use any tool)

First of all, import the clothing meshes into some Blender 3D project that contains the armature and the "Female Clothing Avatar" mesh published in the "SMA Clothing Avatar.blend" project. The "Female Clothes Shape Keys" mesh is optional and could be used or not; it will be explained below.


## Sculpting

Mold the clothing mesh as closely as possible to the mesh of the female avatar; depending on its elevation from the skin surface, the procedures to follow will change a bit.


## Topology

The closer the clothing is to the skin, the more similar the clothing topology should be to reduce the likelihood of clipping. The game does not have any mechanism to hide vertices; for this reason, the closer the clothing is to the skin, the more similar the topology should be.
If the clothes have exactly the same topology as the female avatar, it is not necessary for the surface of the clothes to be above the skin surface of the female avatar; they can be in the same position in 3D space, and then when creating the materials For this clothing within the game engine, you can set the height of the vertices (explained later), and in this way, the clothing mesh will appear above the skin mesh.


## Skinning

(Remember to have the "Armature" modifier turned off.)
Again, it all depends on how close the clothing mesh is to the skin mesh of the female avatar; if it is very close and they have the same topology, simply transfer the vertex groups using the "Nearest face interpolated" option, which gives you better results.

After transferring the vertex groups, it is a good idea to clean them using the "LimpiarGruposVacios" addon.


![vertex group cleaner](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/4746ef88-f686-49e0-ad32-2cf2df08a5ca)


If the clothing mesh and the female avatar have very different topologies, you will have to take additional steps. A recommendation is to use the smooth, selecting the bones with influence on the zone of the mesh with skinning problems (most bones that have a vertex group assigned end in ".DEF") as well as in the picture.


![Skinning Smoth](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/1f3a3db8-9513-4960-9e5b-616125c166e9)


Then you will have to manually paint until you get a good result using the body proportions that your female character has in the game.
That is, you can produce clothes thinking of all the possible physical forms of the female characters or only having one type in mind (e.g., big breasts), and then within the game you create this character with these characteristics and save it as a portrait.


## Blend Shapes or Shape Keys

(Remember to have the "Armature" modifier turned off.)
The addon with which I have the most experience is "LichtwerkMeshTransfer", which can deliver good results depending on the mesh topology.
Also, it is good to use the additional addon "CleanAndRemoveUnusedShapeKeys".
First of all, make sure that the transforms of the clothing meshes and the female avatar have the same properties.


![Transforms de ropa y piel iguales](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/161e87f6-b42b-470e-8e57-1f60a34791eb)


If, when changing the properties of the clothing transform, it rotates, moves, enlarges, or shrinks, that is, if it is out of sync with the female avatar, what you should do is:
1: Press shift + s and select "Cursor to World Origin" .
2: In the "Transform Pivot Point" select the option "3D Cursor".
3: Select your clothing mesh and go to edit mode. Select all the vertices and rotate, translate, or scale them so they are back in sync with the female avatar.
Now your clothing mesh and the female avatar should be positioned correctly.


![Before mapping](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/05f3ca1d-2e1e-47ac-9c08-a6735ca3d949)


Set the "Source Object" as "Female Clothing Avatar" or "Female Clothes Shape Keys", Keep in mind that by selecting the latter, the result can be much better, but the time it will take will be considerably longer (several minutes or even hours, depending on the number of vertices your clothing mesh has).
"Avarage verts": if the topology of the clothing mesh and the female avatar are the same, you can leave this value at one; the more different the topologies are, the more you should increase this value; Values greater than 8 do not show much improvement.
The "transform" and "modifiers" should be disabled.
Press "Create Vertex Mapping" and wait for the process (blender will just freeze; that's normal) to finish. Depending on the "Source Object" and the number of vertices that the clothing mesh has, this process can take several seconds to several minutes.


![After mapping](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/03a59e0f-135a-4268-b15f-fbceb71cb5b9)

Configure the interface as in the image and press "Transfer Shapekeys", Once again, depending on the number of vertices involved, the process will take from several seconds to several minutes.

If the result of the shapes is not as desired, you can repeat the process with a different configuration.
If the result is not perfect but not too bad, you can go from shape to shape, making corrections. Remember that if you are designing clothes for a specific type of female figure, it is not necessary to correct all the shapes.

Once you are happy with the Blend Shapes or Shape Keys, you can use the "CleanAndRemoveUnusedShapeKeys" addon, which will reduce the size of the FBX when exported and make it easier to import into the game engine.


![Shape keys cleaner](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/d7608d3a-1920-46c8-99cf-3e775189a33d)


## Vertex Colors

The game only uses the green and red channels; green is used for skin proximity displacement, for example, when the protagonist's hand touches the model's skin and this skin deforms.
and the red channel, which is used to negate the recalculation of the breast normals when they are small.
I recommend always transferring the red channel and the green channel only if the clothing mesh is very close to or at the same height as the skin.
If the clothing mesh is slightly higher than the skin, you can transfer only a percentage of the green channel, say 50%, and see the result in-game.

![Current Vertex Colors](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/56082720-0c70-4fcb-b3ca-c8da53b754e5)

## Interactions with clothing

Make a copy of the original clothing mesh, then sculpt it in such a way that it exposes some part of the body, e.g., unbuttoned pants. Then repeat all the steps above without deleting any vertices. In the original clothing mesh, select the "Join as Shapes" option. In this way, you will have a shape that morphs your original clothing mesh into the one that exposes some part of the body.

![ejemplo de interacciones de ropa](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/2cfdac84-9da8-4feb-84bf-3a5f6933e6ae)

In this way, you will have the original mesh, a shape that transforms your original mesh into the mesh exposing some part of the body, and the mesh that exposes the part of the body. You will need them later in Unity to configure your clothing asset with the interaction.


## Export

Activate the "Armature" modifier of the clothing mesh, select the armature plus the clothing mesh, select the export fbx option, and set the options like this:


![Blender Export settings](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/b10d5ae9-68e9-4b18-9a2d-573078e06dac)


After configuring these settings, you can save this preset, so you only need to select it every time you have to export a mesh.



# Unity

Open the project and wait for it to download all the necessary packages.
Please do not modify the project configuration; all the necessary configuration is already done.
Already within the project, there are two examples of clothing mods that you can export and then try in the game:
To create a new mod:
Create a new folder with your artistic name in "Assets/Modding/Clothing", as shown in the image.


![ubicacion para mods de ropa](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/5ac05e6e-bdb4-444a-a10c-61e35e62733c)



## Mesh import

Inside this folder (your artistic name), create one with the name of your mod, and import your clothing meshes by dragging them to this folder. Important: The import configuration is correct by default if your clothing mesh was imported into a subfolder in "Assets/Modding/Clothing" If not, you can use a preset already created in the project.


![usar mesh import preset](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/0dd1cd3d-729d-4b64-8785-f81dd770ba9e)


Simply follow the image intrusions and double-click the preset to apply it.
### IMPORTANT:
You need to manually set the labels for your imported clothing mesh since the preset is not able to do this function.
To do it, select your imported mesh, and in the lower right corner, you will see the name of your imported mesh. If you do not see the 3D figure, you must drag the bar up so you will see the mesh in its 3D form, and you will also see the label options.
Choose these two labels for ALL your clothes mesh imports: "CorrectBlendShapeNormals" and "CorrectFemaleClothesBindPoses", If they don't exist, copy these labels, paste them in the search bar, and press enter. It is very important that your mods implement these two labels.



![mesh labels](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/4ec198e7-9803-46be-824b-268765db7d83)


## Materials

Within this same folder of your mod, right-click and navigate to the option to create material, as well as in the image.


![crear material ](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/759bd902-cb26-4405-b69d-52bebedb24fe)


Select this new material and apply the preset already in the project, so you don't have to deal with so many options.


![default material preset](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/f88e8435-74db-4896-a3ac-1f22ae98badb)


This preset uses tessellation, but it is not required; it depends on the topology of the clothing mesh already explained above.
Import the textures to this same folder; there is no preset for this import. I just want to highlight that there is an option to declare your texture as a "normal map" and it must be selected if any of your textures is a "normal map" Also, if your texture is not colored, that is, if it is a smooth or "abient occlusion" map, etc., it's a good idea to disable the "sRGB (Color Texture)" option and see if the results are better.
There are many tutorials on creating materials in Unity. Remember that the game uses HDRP, i.e., it does not use the default materials of the game engine. Everything is already configured in the project; you don't have to worry.
The game could work with materials created from shaders created in the "Shader Graph," but it has never been tested. You can try it if you wish.
Now that you have your materials created for your clothing, you can see the results. To do so, select the imported clothing mesh and the "Materials" tab, assign or drag your materials to their corresponding boxes, and click Apply.


![reeemplazar materiales](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/d11ef057-355e-445d-be42-8d2df06c06b5)


Now that you have your clothing mesh, you can test it. Look for the "GameObject" "AddClothingPiecesToAvatar" and drag your "SkinnedMeshRenderer" of the imported clothing to the "Clothing Assets" field. You can add more items to the list if you need them.


![test ropa bialando](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/342defe1-88e4-4987-8d4a-43a00b19fae3)


Click Play to see your clothes animated.


## Maps

Of course, now you need to tell the game how you want your clothing items to be used.
For this, you must create a "MaterialMap" for each material and a "ClothingItemMap" for each clothing mesh. It is better that you create them in the same folder where your materials and mesh are. To create them, right-click inside the folder and select the "MaterialMap" or "ClothingItemMap" option. Start with the materials first.


![creando mapas](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/429c484b-0212-4d21-bc7f-1fa5a66626f9)


Each field of the maps is explained with tooltips; just remember that in the case of "ClothingItemMap" the "address" can be all the imported clothing mesh or it can be just the "SkinnedMeshRenderer", For most cases, it is better to use only the "SkinnedMeshRenderer".
Also important: the fields Organization," Category," "Full Name," and "In Game Names" are mandatory; do not leave them empty.


## Bundle configuration

With the mesh imported, the materials created, and the maps configured, you must create a new group with the same name as your mod in the "addressables" interface.


![Crear grupo](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/0c2c4d3f-f089-442d-994b-0e2a80c5c1af)


Double-click on this new group and rename it to the same name as your mod.
As you had already configured the "MaterialMap" and the "ClothingItemMap" maps, your materials and the imported mesh should already be declared in the "Default Local Group", Drag them out to the group you just created. You should also drag the "MaterialMap" and the " ClothingItemMap" you created. The textures are not needed; they will be automatically added to the bundle when exporting.


![arrastar items a grupo](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/b21e91b3-5e7c-4244-94b4-f50fd65fd553)




## Export Clothing Mods

Now you just have to export your mod. To do it, go to "TValle/Modding/Windows/Clothing Mods Export Windows" in the toolbar.



![abrir ventana de export](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/a0b9e42a-6a3c-422f-aaf4-475dbcd9af4a)


Select only the mods you want to export and click "Export", The first time you export, it will take longer since the shaders have to be compiled. After that, it will be much faster.

![export](https://github.com/TValleGames/TValle.SMA.Modding/assets/139646206/13321f4f-884a-4a18-9f37-5424e805b982)

Your mods are already exported to a location where the game can read them; the next thing you need to do is view them in-game.
If you want to publish your mods, remember to do it in the form of a folder, the one that opens automatically when your mod is exported, without changing the names of the files or this folder.
