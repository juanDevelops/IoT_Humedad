using System.Collections.ObjectModel;

namespace IoT_Test;

public partial class Inicio : ContentPage
{
	public Inicio()
	{
		InitializeComponent();
        //CargarDatos();
	}

    public ObservableCollection<DatoHumedad> listaRegistros { get; set; }

    private void btnActualizar_Clicked(object sender, EventArgs e)
    {
        CargarDatos();
    }

	public async void CargarDatos()
	{
        var tabbedPage = Application.Current.MainPage as Menu;
        Menu menu = tabbedPage as Menu;
        influxDB db = new influxDB();

        try
        {
            var autos = await db.ObtenerRegistros();
            listaRegistros = new ObservableCollection<DatoHumedad>();
            ListaCarros.ItemsSource = listaRegistros;
            foreach (var auto in autos)
            {
                listaRegistros.Add(auto);
            }

            
        }
        catch (Exception)
        {

        }
    }
}