using Godot;
using System;
using System.Collections.Generic;

public partial class ShopMenu : CanvasLayer{
	private enum MenuType{
		NONE,
		SHOP,
		UPGRADE_TOWER,
		UPGRADE_ACCU,
	}
	
	private MenuType current_active_menu;
	private Control shop_menu;
	private Control upgrade_tower_menu;
	private Control upgrade_accu_menu;
	private Control status_screen;
	private Map map;
	private Dictionary<TileType, Button> buttons_shop = new Dictionary<TileType, Button>();
	
	private void get_all_buttons(){
		buttons_shop[TileType.EMPTY] = shop_menu.GetNode<Button>("HBoxContainer/Movement");
		buttons_shop[TileType.PATH] = shop_menu.GetNode<Button>("HBoxContainer/Path");
		buttons_shop[TileType.TOWER] = shop_menu.GetNode<Button>("HBoxContainer/Tower");
		buttons_shop[TileType.ACCU] = shop_menu.GetNode<Button>("HBoxContainer/Accu");
		buttons_shop[TileType.DESTROYED] = shop_menu.GetNode<Button>("HBoxContainer/Destroy");
	}
	
	private void add_functions_to_buttons(){
		foreach(KeyValuePair<TileType, Button> pair in buttons_shop){
			TileType key = pair.Key;
			Button button = pair.Value;
			button.Pressed += () => {map.set_tile_template(key);};
		}
	}
	
	private void disable_focus_on_buttons(){
		foreach (Button button in shop_menu.GetNode<HBoxContainer>("HBoxContainer").GetChildren()){
			button.FocusMode = Control.FocusModeEnum.None;
		}
	}
	
	public void show_shop_menu(){
		current_active_menu = MenuType.SHOP;
		shop_menu.Show();
		upgrade_tower_menu.Hide();
		upgrade_accu_menu.Hide();
	}
	
	public void show_upgrade_tower_menu(Tower selected_tower){
		current_active_menu = MenuType.UPGRADE_TOWER;
		upgrade_tower_menu.Show();
		shop_menu.Hide();
		upgrade_accu_menu.Hide();
	}
	
	public void show_upgrade_accu_menu(Accu selected_accu){
		current_active_menu = MenuType.UPGRADE_ACCU;
		upgrade_accu_menu.Show();
		shop_menu.Hide();
		upgrade_tower_menu.Hide();
	}
	
	public override void _Ready(){
		map = GetParent().GetNode<Map>("Map");
		shop_menu = GetNode<Control>("Items/Shop");
		upgrade_tower_menu = GetNode<Control>("Items/UpgradeTower");
		upgrade_accu_menu = GetNode<Control>("Items/UpgradeAccu");
		shop_menu.MouseFilter = Control.MouseFilterEnum.Ignore;
		upgrade_tower_menu.MouseFilter = Control.MouseFilterEnum.Ignore;
		upgrade_accu_menu.MouseFilter = Control.MouseFilterEnum.Ignore;
		
		get_all_buttons();
		add_functions_to_buttons();
		disable_focus_on_buttons();
		show_shop_menu();
	}
	
}
