extends Node2D

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_texture_button_pressed():
	$Control/VBoxContainer/Buttons.visible = false
	$Control/VBoxContainer/Quit.visible = false
	$Control/VBoxContainer/ShipSelect.visible = true

func LoadGame():
	get_tree().change_scene_to_file("res://Scenes/world.tscn")

func _on_cruiser_button_pressed():
	UserVariables.loadedShip = 0
	LoadGame()

func _on_star_ship_button_pressed():
	UserVariables.loadedShip = 1
	LoadGame()