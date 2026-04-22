using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D{
	private Tile selected_tile=null;
	private string error_mesage_for_player = null;
	private TileType selected_tile_type;
	private Dictionary<TileType, PackedScene> templates = new Dictionary<TileType, PackedScene>();
	//private TileType[,] map_of_tile_types;
	private Tile[,] map_of_tiles;
	private MoneyManager money_manager;
	private TileConnectionSearcher tcs = new TileConnectionSearcher();
	
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
		map_of_tiles = new Tile[Config.maptiles_amount_x, Config.maptiles_amount_y];
		for(int y=0;y<Config.maptiles_amount_y;y++){
			for(int x=0;x<Config.maptiles_amount_x;x++){
				Tile rect;
				if(x==Config.home_position_x && y==Config.home_position_y){
					rect = templates[TileType.HOME].Instantiate<Tile>();
				}else if(x==Config.maptiles_amount_x-1 && y==Config.maptiles_amount_y-1){
					rect = templates[TileType.NEST].Instantiate<Tile>();
				}else{
					rect = templates[TileType.EMPTY].Instantiate<Tile>();
				}
				map_of_tiles[x,y] = rect;
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
		map_of_tiles[idx_x, idx_y] = new_tile;
	}
	
	public void set_selected_tile_on_map(Tile tile){
		selected_tile = tile;
		start_building();
	}
	
	public void set_tile_template(TileType tile){
		selected_tile_type = tile;
	}
	
	private bool check_conditions_for_building(Tile tile, TileType wished_type){
		List<TileType> neighbors = tcs.get_neighbors_types(map_of_tiles,tile.Position);
		bool has_valid_neighbor=false, has_enough_money;
		
		if(wished_type == TileType.PATH){
			has_valid_neighbor = (neighbors.Contains(TileType.HOME) || neighbors.Contains(TileType.PATH));
		}else{
			has_valid_neighbor = neighbors.Contains(TileType.PATH);
		}
		
		has_enough_money = money_manager.is_affordable(wished_type);
		if(!has_enough_money)
			error_mesage_for_player = "Not enough money";
		else if(!has_valid_neighbor)
			error_mesage_for_player = "Invalid placement";
		return has_valid_neighbor && has_enough_money;
	}
	
	public void start_building(){
		if(selected_tile == null || selected_tile_type <= TileType.HOME)
			return;
		//destroying building by player
		if(selected_tile.get_type() > TileType.DESTROYED && selected_tile_type == TileType.DESTROYED){ 
			if(money_manager.is_affordable(TileType.DESTROYED)){
				change_tile(selected_tile, TileType.EMPTY);
				money_manager.pay(TileType.DESTROYED);
				tcs.update_connections_on_map(map_of_tiles, new Vector2(Config.home_position_x,Config.home_position_y));
			}else{
				error_mesage_for_player = "Not enough money";
			}
		}else if(selected_tile.get_type() != TileType.EMPTY){
			return;
		}else{
			//normal building
			if(check_conditions_for_building(selected_tile, selected_tile_type)){
				change_tile(selected_tile, selected_tile_type);
				money_manager.pay(selected_tile_type);
				tcs.update_connections_on_map(map_of_tiles, new Vector2(Config.home_position_x,Config.home_position_y));
			}
		}
		
		if(error_mesage_for_player != null){
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

}
