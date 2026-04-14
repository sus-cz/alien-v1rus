using Godot;
using System;
using System.Collections.Generic;

public partial class MoneyManager : Node{
	private int money;
	private Label money_label;
	private Dictionary<TileType, int> prices = new Dictionary<TileType, int>();
	
	public override void _Ready(){
		money = Config.MONEY_BEGIN;
		money_label = (Label)GetNode($"../ShopMenu/Control/VBoxContainer/MoneyLabel");
		money_label.AddThemeFontSizeOverride("font_size", 28);
		load_prices();
		update_label();
	}
	
	private void load_prices(){
		prices[TileType.PATH] = Config.PRICE_PATH;
		prices[TileType.TOWER] = Config.PRICE_TOWER;
		prices[TileType.ACCU] = Config.PRICE_ACCU;
	}
	
	private void update_label(){
		money_label.Text = "$ " + money;
	}
	
	public int get_current_money(){
		return money;
	}
	
	public void add_value(int win){
		if(win < 0)
			win *= (-1);
		money += win;
		update_label();
	}
	
	public bool is_affordable(TileType tile){
		try{
			return money >= prices[tile];
		}catch(Exception){
			return false;
		}
	}
	
	public bool pay(TileType tile){
		if(!is_affordable(tile))
			return false;
			
		money -= prices[tile];
		update_label();
		return true;
	}
}
