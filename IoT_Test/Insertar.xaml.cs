namespace IoT_Test;

public partial class Insertar : ContentPage
{
	public Insertar()
	{
		InitializeComponent();
	}

	influxDB db = new influxDB();

    private async void btnEnviar_Clicked(object sender, EventArgs e)
    {
		
		await db.ObtenerRegistros();
    }

    private void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        db.InsertarDato(dpFechaSalida.Date, Convert.ToInt16(tbValor.Text));
    }
}