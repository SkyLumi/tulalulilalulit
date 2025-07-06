extends Node2D

signal dialog_finished(result: String)

func ShowDialog(begginingText: String, choiceText: String, isAxeTaken: bool, isCarveTaken: bool, isSawTaken: bool):
	Dialogic.VAR["beggining_text"] = begginingText
	Dialogic.VAR["choice_text"] = choiceText
	Dialogic.VAR["is_axe_taken"] = isAxeTaken
	Dialogic.VAR["is_carve_taken"] = isCarveTaken
	Dialogic.VAR["is_saw_taken"] = isSawTaken
	if not Dialogic.signal_event.is_connected(_on_dialogic_signal):
		Dialogic.signal_event.connect(_on_dialogic_signal)
	Dialogic.start("bucket1")

func _on_dialogic_signal(argument: String):
	if argument == "tidak ada yang ditaruh":
		SendSignal("tidak ada")
	elif argument == "taruh kapak":
		SendSignal("kapak")
	elif argument == "taruh pisau ukir":
		SendSignal("pisau ukir")
	elif argument == "taruh gergaji":
		SendSignal("gergaji")
		
func SendSignal(result: String):
	emit_signal("dialog_finished", result)
