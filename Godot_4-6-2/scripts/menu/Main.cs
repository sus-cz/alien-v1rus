using Godot;
using System;

public partial class Main : Node2D{
	public Godot.Collections.Dictionary get_default_data(){
		Resource settings_file = GD.Load<Resource>("res://scripts/global/settings.tres");
		Godot.Collections.Dictionary settings_data = (Godot.Collections.Dictionary) settings_file.Get("data");
		
		return settings_data;
	}
	
	public override void _Ready(){
		Godot.Collections.Dictionary settings_data = get_default_data();
		
		//GD.Print(settings_data["price_acc"]);
	}

}
