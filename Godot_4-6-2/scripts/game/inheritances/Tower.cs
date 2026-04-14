using Godot;
using System;

public partial class Tower : Tile{
	protected override void init(){
		tile_type = TileType.TOWER;
	}
}
