using Godot;
using System;

public partial class Home : Tile{
	protected override void init(){
		hp = Config.DEF_HP_HOME;
	}
	public override TileType get_type(){
		return TileType.HOME;
	}
}
