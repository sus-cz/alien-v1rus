using Godot;
using System;

public partial class Path : Tile{
	protected override void handle_mouse(){
		GD.Print("Path clicked");
	}
	
	protected override void init(){
		tile_type = TileType.PATH;
	}
}
