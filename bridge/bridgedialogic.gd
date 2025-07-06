extends Node2D

func _isMutiaraDimakan(isMutiaraDimakan: bool):
	Dialogic.VAR["is_mutiara_dimakan"] = isMutiaraDimakan
	Dialogic.start('puzzle_1')

func _isCahayaPuzzle(isCahayaPuzzle: bool):
	Dialogic.VAR["is_cahaya_puzzle1"] = isCahayaPuzzle
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
