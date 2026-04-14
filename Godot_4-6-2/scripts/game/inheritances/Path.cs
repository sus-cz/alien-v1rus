using Godot;
using System;

public partial class Path : Tile{
	protected override void init(){
		//..
	}
	public override TileType get_type(){
		return TileType.PATH;
	}
}
