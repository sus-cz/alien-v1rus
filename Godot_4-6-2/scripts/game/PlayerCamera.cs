using Godot;
using System;

public partial class PlayerCamera : Camera2D{
	private float speed = Config.tile_size * Config.camera_speed;
	private float speed_up_factor = 2f;
	private float max_distance_x = 0, max_distance_y = 0;
	
	
	public void camera_zoom(){
		if(Input.IsActionJustPressed("zoom_in") && Zoom.X > Config.camera_max_zoom_in){
			Zoom *= new Vector2(0.9f, 0.9f);
		}
		if(Input.IsActionJustPressed("zoom_out") && Zoom.X < Config.camera_max_zoom_out){
			Zoom *= new Vector2(1.1f, 1.1f);
		}
	}
	
	public void camera_movement(float delta){
		Vector2 direction = Vector2.Zero;
		if(Input.IsActionPressed("move_left") && Position.X > 0){
			direction.X = -1;
		}
		if(Input.IsActionPressed("move_right") && Position.X < max_distance_x){
			direction.X = 1;
		}
		if(Input.IsActionPressed("move_up") && Position.Y > 0){
			direction.Y = -1;
		}
		if(Input.IsActionPressed("move_down")  && Position.Y < max_distance_y){
			direction.Y = 1;
		}
		if(Input.IsActionPressed("speed_up")){
			direction *= speed_up_factor;
		}
		Position += direction * (float)delta * speed;
	}
	
	public void set_max_travel_distance(float distance_x, float distance_y){
		max_distance_x = distance_x;
		max_distance_y = distance_y;
	}
	
	public override void _Process(double delta){
		camera_movement((float)delta);
		camera_zoom();
	}
}
