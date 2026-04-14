using Godot;
using System;

public partial class Game : Node2D{
	public override void _Ready(){
		if(Config.fullscreen)
			GetWindow().Mode = Window.ModeEnum.Fullscreen;
		else
			GetWindow().Mode = Window.ModeEnum.Windowed;
	}
}
