using Godot;
using System.Collections.Generic;

public partial class TileConnectionSearcher{
	
	// returns buildings connected
	private List<Tile> sort_neighbors(List<Tile> neighbors, List<Tile> stack_neighbors){
		Tile current_tile;
		List<Tile> buildings = new List<Tile>();
		for(int i=neighbors.Count-1; i>=0;i--){
				current_tile = neighbors[i];
				if(current_tile.get_type() == TileType.PATH){
					if(!stack_neighbors.Contains(current_tile)){
						stack_neighbors.Add(current_tile);
					}
				}else if(current_tile.get_type() == TileType.TOWER || current_tile.get_type() == TileType.ACCU){
					buildings.Add(current_tile);
				}
		}
		return buildings;
	}
	
	private void deactivate_all_buildings_except(List<Tile> active_buildings, Tile[,] map_of_tiles){
		for(int y=0;y<Config.maptiles_amount_y;y++){
			for(int x=0;x<Config.maptiles_amount_x;x++){
				if(active_buildings.Contains(map_of_tiles[x,y])){
					map_of_tiles[x,y].activate();
				}else if(map_of_tiles[x,y].get_type() == TileType.TOWER || map_of_tiles[x,y].get_type() == TileType.ACCU){
					map_of_tiles[x,y].deactivate();
				}
			}
		}
	}
	
	public void update_connections_on_map(Tile[,] map_of_tiles, Vector2 home_position){
		List<Tile> stack_neighbors = new List<Tile>{map_of_tiles[(int)home_position.X, (int)home_position.Y]};
		List<Tile> active_buildings = new List<Tile>();
		int active_index=0;
		List<Tile> current_neighbors;
		Vector2 current_position;
		while(active_index < stack_neighbors.Count){
			current_position = new Vector2(stack_neighbors[active_index].Position.X, stack_neighbors[active_index].Position.Y);
			current_neighbors=get_neighbors(map_of_tiles, current_position);
			active_buildings.AddRange(sort_neighbors(current_neighbors, stack_neighbors));
			active_index++;
		}
		deactivate_all_buildings_except(active_buildings, map_of_tiles);
	}
	
	public List<TileType> get_neighbors_types(Tile[,] map_of_tiles, Vector2 position){
		List<TileType> result = new List<TileType>();
		List<Tile> neighbors = get_neighbors(map_of_tiles, position);
		
		foreach(Tile neighbor in neighbors){
			result.Add(neighbor.get_type());
		}
		return result;
	}
	
	public List<Tile> get_neighbors(Tile[,] map_of_tiles, Vector2 position){
		List<Tile> result = new List<Tile>();
		int idx_x = (int)position.X / Config.TILE_SIZE;
		int idx_y = (int)position.Y / Config.TILE_SIZE;
		if(idx_x > 0){
			result.Add(map_of_tiles[idx_x-1, idx_y]);
		}
		if(idx_x < Config.maptiles_amount_x -1){
			result.Add(map_of_tiles[idx_x+1, idx_y]);
		}
		if(idx_y > 0){
			result.Add(map_of_tiles[idx_x, idx_y-1]);
		}
		if(idx_y < Config.maptiles_amount_y -1){
			result.Add(map_of_tiles[idx_x, idx_y+1]);
		}
		return result;
	}
}
