extends Node2D

func _isMutiaraDimakan(isMutiaraDimakan: bool):
	Dialogic.VAR["is_mutiara_dimakan"] = isMutiaraDimakan
	Dialogic.start('puzzle_1')

func _isCahayaPuzzle(isCahayaPuzzle: bool):
	Dialogic.VAR["is_cahaya_puzzle1"] = isCahayaPuzzle
	Dialogic.start('puzzle_1-cahaya')
