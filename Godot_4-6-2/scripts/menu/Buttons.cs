using Godot;
using System;

public partial class Buttons : CenterContainer{
	public void _on_play_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	}
	
	public void _on_exit_pressed(){
		GetTree().Quit();
	}
}
