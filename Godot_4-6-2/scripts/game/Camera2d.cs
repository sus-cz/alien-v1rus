using Godot;
using System;

public partial class Camera2d : Camera2D{
	private float speed = Config.tile_size * Config.camera_speed;
	
	public void camera_zoom(){
		if(Input.IsActionJustPressed("zoom_in")){
			Zoom *= new Vector2(0.9f, 0.9f);
		}
		if(Input.IsActionJustPressed("zoom_out")){
			Zoom *= new Vector2(1.1f, 1.1f);
		}
	}
	
	public void camera_movement(float delta){
		Vector2 direction = Vector2.Zero;
		if(Input.IsActionPressed("move_left")){
			direction.X = -1;
		}
		if(Input.IsActionPressed("move_right")){
			direction.X = 1;
		}
		if(Input.IsActionPressed("move_up")){
			direction.Y = -1;
		}
		if(Input.IsActionPressed("move_down")){
			direction.Y = 1;
		}
		Position += direction * (float)delta * speed;
	}
	
	public override void _Process(double delta){
		camera_movement((float)delta);
		camera_zoom();
	}
}
