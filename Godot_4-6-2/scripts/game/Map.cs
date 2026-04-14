using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D{
	private Tile selected_tile=null;
	private string error_mesage_for_player = null;
	private TileType selected_tile_type;
	private Dictionary<TileType, PackedScene> templates = new Dictionary<TileType, PackedScene>();
	private TileType[,] map_of_tile_types;
	private MoneyManager money_manager;
	
	private void change_camera_travel_distance(){
		PlayerCamera camera = (PlayerCamera) GetParent().GetNode<Camera2D>("PlayerCamera");
		camera.set_max_travel_distance(Config.maptiles_amount_x * Config.TILE_SIZE, Config.maptiles_amount_y * Config.TILE_SIZE);
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
		map_of_tile_types = new TileType[Config.maptiles_amount_x, Config.maptiles_amount_y];
		for(int y=0;y<Config.maptiles_amount_y;y++){
			for(int x=0;x<Config.maptiles_amount_x;x++){
				Tile rect;
				if(x==0 && y==0){
					rect = templates[TileType.HOME].Instantiate<Tile>();
					map_of_tile_types[x,y] = TileType.HOME;
				}else if(x==Config.maptiles_amount_x-1 && y==Config.maptiles_amount_y-1){
					rect = templates[TileType.NEST].Instantiate<Tile>();
					map_of_tile_types[x,y] = TileType.NEST;
				}else{
					rect = templates[TileType.EMPTY].Instantiate<Tile>();
					map_of_tile_types[x,y] = TileType.EMPTY;
				}
				AddChild(rect);
				rect.set_size(new Vector2(Config.TILE_SIZE, Config.TILE_SIZE));
				rect.set_position(new Vector2(Config.TILE_SIZE * x, Config.TILE_SIZE * y));
			}
		}
	}
	
	private void change_tile(Tile old_tile, TileType new_tile_type){
		int idx_x = (int)old_tile.Position.X / Config.TILE_SIZE;
		int idx_y = (int)old_tile.Position.Y / Config.TILE_SIZE;
		if(idx_x < 0 || idx_y < 0 || idx_x >= Config.maptiles_amount_x || idx_y >= Config.maptiles_amount_y)
			throw new Exception("MAP: Changing tile with index out of bound (" + idx_x + "," +idx_y +")");
		
		Tile new_tile = templates[new_tile_type].Instantiate<Tile>();
		AddChild(new_tile);
		new_tile.set_size(old_tile.get_size());
		new_tile.set_position(old_tile.Position);
		old_tile.QueueFree();
		selected_tile = null;
		map_of_tile_types[idx_x, idx_y] = new_tile_type;
	}
	
	public void set_selected_tile_on_map(Tile tile){
		selected_tile = tile;
		start_building();
	}
	
	public void set_tile_template(TileType tile){//MAP selected
		selected_tile_type = tile;
	}
	
	private List<TileType> get_neighbors(Vector2 position){
		List<TileType> result = new List<TileType>();
		int idx_x = (int)position.X / Config.TILE_SIZE;
		int idx_y = (int)position.Y / Config.TILE_SIZE;
		
		if(idx_x > 0){
			result.Add(map_of_tile_types[idx_x-1, idx_y]);
		}
		if(idx_x < Config.maptiles_amount_x -1){
			result.Add(map_of_tile_types[idx_x+1, idx_y]);
		}
		if(idx_y > 0){
			result.Add(map_of_tile_types[idx_x, idx_y-1]);
		}
		if(idx_y < Config.maptiles_amount_y -1){
			result.Add(map_of_tile_types[idx_x, idx_y+1]);
		}
		return result;
	}
	
	private bool check_conditions_for_building(Tile tile, TileType wished_type){
		List<TileType> neighbors = get_neighbors(tile.Position);
		bool has_valid_neighbor=false, has_enough_money;
		
		if(wished_type == TileType.PATH){
			has_valid_neighbor = (neighbors.Contains(TileType.HOME) || neighbors.Contains(TileType.PATH));
		}else{
			has_valid_neighbor = neighbors.Contains(TileType.PATH);
		}
		
		has_enough_money = money_manager.is_affordable(wished_type);
		if(!has_enough_money)
			error_mesage_for_player = "Not enough money";
		else
			error_mesage_for_player = "Invalid placement";
		return has_valid_neighbor && has_enough_money;
	}
	
	public void start_building(){
		if(selected_tile == null || selected_tile_type <= TileType.HOME)
			return;
		if(selected_tile.get_type() != TileType.EMPTY)
			return;
		
		if(check_conditions_for_building(selected_tile, selected_tile_type)){
			change_tile(selected_tile, selected_tile_type);
			money_manager.pay(selected_tile_type);
		}else{
			GD.Print(error_mesage_for_player);
			error_mesage_for_player = null;
		}
	}
	
	public override void _Ready(){
		money_manager = (MoneyManager)GetNode($"../Money");
		load_templates();
		generate_map();
		change_camera_travel_distance();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
	}
}
