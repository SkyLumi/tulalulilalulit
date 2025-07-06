extends Node

func set_player_cant_move():
	var players = get_tree().get_nodes_in_group("player")
	if players.size() > 0:
		players[0].canMove = false

func set_player_can_move():
	var players = get_tree().get_nodes_in_group("player")
	if players.size() > 0:
		players[0].canMove = true
