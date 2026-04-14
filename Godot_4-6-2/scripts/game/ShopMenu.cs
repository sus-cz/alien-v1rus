using Godot;
using System;
using System.Collections.Generic;

public partial class ShopMenu : CanvasLayer{
	private Control control;
	private Map map;
	private Dictionary<TileType, Button> buttons = new Dictionary<TileType, Button>();
	
	private void get_buttons(){
		buttons[TileType.PATH] = GetNode<Button>("Control/HBoxContainer/Path");
		buttons[TileType.TOWER] = GetNode<Button>("Control/HBoxContainer/Tower");
		buttons[TileType.ACCU] = GetNode<Button>("Control/HBoxContainer/Accu");
		buttons[TileType.DESTROYED] = GetNode<Button>("Control/HBoxContainer/Destroy");
	}
	
	private void add_functions_to_buttons(){
		foreach(KeyValuePair<TileType, Button> pair in buttons){
			TileType key = pair.Key;
			Button button = pair.Value;
			button.Pressed += () => {map.set_selected_building(key);};
		}
	}
	
	private void disable_focus_on_buttons(){
		foreach (Button button in GetNode<HBoxContainer>("Control/HBoxContainer").GetChildren()){
		//if (node is Button button)
			button.FocusMode = Control.FocusModeEnum.None;
		}
	}
	
	public override void _Ready(){
		map = GetParent().GetNode<Map>("Map");
		control = GetNode<Control>("Control");
		control.MouseFilter = Control.MouseFilterEnum.Ignore;
		
		get_buttons();
		add_functions_to_buttons();
		disable_focus_on_buttons();
	//GetNode<Button>("Control/ButtonPlay").Pressed += OnPlayPressed;
	}
	
	

}
