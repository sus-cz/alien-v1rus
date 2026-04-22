using Godot;
using System;

public partial class Tile : Node2D{
	private Panel sprite;
	private CollisionShape2D collision;
	private Area2D area;
	protected Map map;
	protected int hp = 0;
	protected bool is_tile_active=true;
	
	public virtual void activate(){
		is_tile_active=true;
		sprite.Modulate = new Color(1, 1, 1, 1f);
	}
	
	public virtual void deactivate(){
		is_tile_active=false;
		sprite.Modulate = new Color(1, 1, 1, 0.5f);
	}
	
	public virtual TileType get_type(){
		return TileType.EMPTY;
	}
	
	public Vector2 get_size(){
		return sprite.Size;
	}
	
	public void set_size(Vector2 size){
		sprite.Size = size;
		RectangleShape2D shape = new RectangleShape2D();
		shape.Size = size;
		collision.Shape = shape;
	}
	
	public void set_position(Vector2 position){
		this.Position = position;
		collision.Position = new Vector2(Config.TILE_SIZE/2,Config.TILE_SIZE/2);
	}
		
	protected virtual void init(){
		//..
	}
	
	public override void _Ready(){
		map = GetParent<Map>();
		sprite = GetNode<Panel>("Sprite");
		sprite.MouseFilter = Control.MouseFilterEnum.Ignore;
		area = GetNode<Area2D>("Area2D");
		collision = area.GetNode<CollisionShape2D>("Collision2D");
		area.InputEvent += new Area2D.InputEventEventHandler(OnAreaInputEvent);
		init();
	}
	
	protected void OnAreaInputEvent(Node viewport, InputEvent input_event, long shape_idx){
		if(!(input_event is InputEventMouseButton)){
			return;
		}
		InputEventMouseButton mouse_event = (InputEventMouseButton)input_event;
		if (mouse_event != null && 
			mouse_event.Pressed && 
			mouse_event.ButtonIndex == MouseButton.Left){
				map.set_selected_tile_on_map(this);
				handle_mouse();
		}
	}
	
	protected virtual void handle_mouse(){
		//...
	}
}
