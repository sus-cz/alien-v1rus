using Godot;
using System;

public partial class Tile : Node2D{
	private Panel sprite;
	
	public void set_size(Vector2 size){
		sprite.Size = size;
	}
	
	public void set_position(Vector2 position){
		this.Position = position;
	}
	
	public override void _Ready(){
		sprite = GetNode<Panel>("Sprite");
	}
}
