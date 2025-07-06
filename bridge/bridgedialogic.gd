extends Node2D

func _isMutiaraDimakan(isMutiaraDimakan: bool):
	Dialogic.VAR["is_mutiara_dimakan"] = isMutiaraDimakan
	Dialogic.start('puzzle_1')
