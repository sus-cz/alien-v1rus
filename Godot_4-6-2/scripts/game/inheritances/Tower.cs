using Godot;
using System;

public partial class Tower : Tile{
	protected override void init(){
		hp = Config.DEF_HP_TOWER;
	}
	
	public override TileType get_type(){
		return TileType.TOWER;
	}
}
