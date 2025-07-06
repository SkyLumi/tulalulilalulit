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

func _map03Dial01():
	Dialogic.start('map03-01')

func _map03Dial02():
	Dialogic.start('map03-02')

func _map03Dial03():
	Dialogic.start('map03-03')

func _map03Dial04():
	Dialogic.start('map03-04')

func _map03Dial05():
	Dialogic.start('map03-05')
	Dialogic.connect("timeline_ended", Callable(self, "_map03Dial06"), CONNECT_ONE_SHOT)

func _map03Dial06():
	Dialogic.start('map03-06')

func _isOpening():
	Dialogic.start('puzzle_1-opening')
