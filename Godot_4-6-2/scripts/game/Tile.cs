using Godot;
using System;

public partial class Tile : Node2D{
	private Panel sprite;
	private CollisionShape2D collision;
	private Area2D area;
	
	public void set_size(Vector2 size){
		sprite.Size = size;
		RectangleShape2D shape = new RectangleShape2D();
		shape.Size = size;
		collision.Shape = shape;
	}
	
	public void set_position(Vector2 position){
		this.Position = position;
		collision.Position = new Vector2(Config.tile_size/2,Config.tile_size/2);
	}
	
	public override void _Ready(){
		sprite = GetNode<Panel>("Sprite");
		area = GetNode<Area2D>("Area2D");
		collision = area.GetNode<CollisionShape2D>("Collision2D");
		area.InputEvent += new Area2D.InputEventEventHandler(OnAreaInputEvent);
	}
	
	private void OnAreaInputEvent(Node viewport, InputEvent input_event, long shape_idx){
		if(!(input_event is InputEventMouseButton)){
			return;
		}
		InputEventMouseButton mouse_event = (InputEventMouseButton)input_event;
		if (mouse_event != null && 
			mouse_event.Pressed && 
			mouse_event.ButtonIndex == MouseButton.Left){
				GD.Print("Tile clicked");
		}
	}
}
