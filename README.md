## Unturned Map Merge Tool
Program for merging multiple unturned maps into one.  
Work in progress

### Example maps created using this tool
* [Triple Classic (Peyukton)](https://steamcommunity.com/sharedfiles/filedetails/?id=2867004062) by adLay

### Currently Mergeable Files
Landscape merge works by renaming and copying the files from the original map.  
All of the `.dat` files I'm reading into objects, shifting the positions inside the files, combining and then saving into the output map directory.

**Landscape**  
* Heightmaps/*
* Holes/*
* Splatmaps/*

**Spawns**
* Fauna.dat
* Items.dat
* Jars.dat
* Players.dat
* Vehicles.dat
* Zombies.dat

**Environment**
* Bounds.dat
* Flags.dat
* Flags_Data.dat
* Nodes.dat
* Paths.dat
* Roads.dat

**Terrain**
* Trees.dat

**Level**
* Buildables.dat
* Objects.dat

### Example board for the insane size map  
This is an example board used to get right coordinates for the map you want to be generated from another  
![](UnturnedMapMergeTool.jpg)

