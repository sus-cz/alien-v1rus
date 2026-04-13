using Godot;
using System;

public partial class Map : Node2D{
	private void generate_map(){
		PackedScene tile_template = GD.Load<PackedScene>("res://scenes/Tile.tscn");
		
		for(int y=0;y<Config.maptiles_amount_y;y++){
			for(int x=0;x<Config.maptiles_amount_x;x++){
				Tile rect = tile_template.Instantiate<Tile>();
				AddChild(rect);
				rect.set_size(new Vector2(Config.tile_size, Config.tile_size));
				rect.set_position(new Vector2(Config.tile_size * x, Config.tile_size * y));
			}
		}
	}
	
	public override void _Ready(){
		generate_map();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
