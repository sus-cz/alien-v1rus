using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D{
	private Tile selected_tile=null;
	private TileType selected_tile_type;
	private Dictionary<TileType, PackedScene> templates = new Dictionary<TileType, PackedScene>();
	
	private void change_camera_travel_distance(){
		PlayerCamera camera = (PlayerCamera) GetParent().GetNode<Camera2D>("PlayerCamera");
		camera.set_max_travel_distance(Config.maptiles_amount_x * Config.tile_size, Config.maptiles_amount_y * Config.tile_size);
	}
	
	private void load_templates(){
		templates[TileType.EMPTY] = GD.Load<PackedScene>("res://scenes/buildings/Tile.tscn");
		templates[TileType.PATH] = GD.Load<PackedScene>("res://scenes/buildings/Path.tscn");
		templates[TileType.TOWER] = GD.Load<PackedScene>("res://scenes/buildings/Tower.tscn");
		templates[TileType.ACCU] = GD.Load<PackedScene>("res://scenes/buildings/Accu.tscn");
		templates[TileType.HOME] = GD.Load<PackedScene>("res://scenes/buildings/Home.tscn");
		templates[TileType.NEST] = GD.Load<PackedScene>("res://scenes/buildings/Nest.tscn");
		templates[TileType.DESTROYED] = GD.Load<PackedScene>("res://scenes/buildings/Destroyed.tscn");
	}
	
	private void generate_map(){
		for(int y=0;y<Config.maptiles_amount_y;y++){
			for(int x=0;x<Config.maptiles_amount_x;x++){
				Tile rect = templates[TileType.EMPTY].Instantiate<Tile>();
				AddChild(rect);
				rect.set_size(new Vector2(Config.tile_size, Config.tile_size));
				rect.set_position(new Vector2(Config.tile_size * x, Config.tile_size * y));
			}
		}
	}
	
	public void set_selected_tile(Tile tile){
		selected_tile = tile;
		start_building();
	}
	
	public void set_selected_building(TileType tile){
		selected_tile_type = tile;
		start_building();
	}
	
	public void start_building(){
		if(selected_tile == null || selected_tile_type <= TileType.HOME)
			return;
		if(selected_tile.get_type() != TileType.EMPTY)
			return;
			
		Vector2 position = selected_tile.Position;
		Vector2 size = selected_tile.get_size();
		selected_tile.QueueFree();
		selected_tile = null;
		Tile tile = templates[selected_tile_type].Instantiate<Tile>();
		AddChild(tile);
		tile.set_position(position);
		tile.set_size(size);
	}
	
	public override void _Ready(){
		load_templates();
		generate_map();
		change_camera_travel_distance();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
	}
}
