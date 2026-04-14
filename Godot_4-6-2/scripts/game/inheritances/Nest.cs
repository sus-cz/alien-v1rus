using Godot;
using System;

public partial class Nest : Tile{
	protected override void init(){
		hp = Config.DEF_HP_NEST;
	}
	public override TileType get_type(){
		return TileType.NEST;
	}
}
