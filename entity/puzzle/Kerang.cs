using Godot;

public partial class Kerang : Node2D
{
	// Drag & drop reference ke Sprite2D di Inspector, atau pakai GetNode di _Ready
	[Export] public Sprite2D buka;
	[Export] public Sprite2D tutup;
	[Export] public Area2D triggerArea;

	private bool mutiaraSudahMasuk = false;

	public override void _Ready()
	{
		// Inisialisasi: buka terlihat, tutup tidak
		if (buka != null) buka.Visible = true;
		if (tutup != null) tutup.Visible = false;

		// Pastikan Area2D sudah di-assign
		if (triggerArea == null)
			triggerArea = GetNode<Area2D>("Area2D");

		// Connect signal
		triggerArea.BodyEntered += OnBodyEntered;
		triggerArea.BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node body)
	{
		// Cek apakah yang masuk adalah Mutiara
		if (body.Name == "Mutiara") // atau body is Mutiara
		{
			mutiaraSudahMasuk = true;
			if (buka != null) buka.Visible = false;
			if (tutup != null) tutup.Visible = true;
		}
	}

	private void OnBodyExited(Node body)
	{
		// Kalau mutiara keluar lagi
		if (body.Name == "Mutiara")
		{
			mutiaraSudahMasuk = false;
			if (buka != null) buka.Visible = true;
			if (tutup != null) tutup.Visible = false;
		}
	}
}
