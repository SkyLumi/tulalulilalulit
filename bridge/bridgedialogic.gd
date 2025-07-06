extends Node2D

signal ngirim_dialog(result: String)

func SendSignal(result: String):
	emit_signal("ngirim_dialog", result)

func _on_dialogic_signal(argument: String):
	if argument == "bunga":
		SendSignal("bunga")
	elif argument == "selesai":
		SendSignal("selesai")

func _isMutiaraDimakan(isMutiaraDimakan: bool):
	Dialogic.VAR["is_mutiara_dimakan"] = isMutiaraDimakan
	if not Dialogic.signal_event.is_connected(_on_dialogic_signal):
		Dialogic.signal_event.connect(_on_dialogic_signal)
	Dialogic.start('puzzle_1')

func _isCahayaPuzzle():
	Dialogic.start('puzzle_1-cahaya')

func _isOpening():
	Dialogic.start('puzzle_1-opening')
