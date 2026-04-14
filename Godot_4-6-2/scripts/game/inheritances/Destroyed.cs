using Godot;
using System;

public partial class Destroyed : Tile{
	protected override void init(){
		tile_type = TileType.DESTROYED;
	}
}
