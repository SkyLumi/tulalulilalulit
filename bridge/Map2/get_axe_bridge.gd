extends Node2D

signal dialog_finished(result: String)

func ShowDialog():
	if not Dialogic.signal_event.is_connected(_on_dialogic_signal):
			Dialogic.signal_event.connect(_on_dialogic_signal)
	Dialogic.start("GetAxe")
	

func _on_dialogic_signal(argument: String):
	SendSignal(argument)
		
func SendSignal(result: String):
	emit_signal("dialog_finished", result)
