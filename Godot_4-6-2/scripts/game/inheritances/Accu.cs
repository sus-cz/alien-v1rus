using Godot;
using System;

public partial class Accu : Tile{
	protected override void init(){
		hp = Config.DEF_HP_ACCU;
	}
	public override TileType get_type(){
		return TileType.ACCU;
	}
}
