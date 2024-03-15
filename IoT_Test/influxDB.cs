using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;

namespace IoT_Test
{
    class influxDB
    {
        public influxDB()
        {

        }

        const string bucket = "Test";
        const string org = "TEC Guasave";
        

        public bool InsertarDato(DateTime fecha, int dato)
        {
            string errorMessage = "";
            var token = "fKrOwRjmCeXQlBwf-6RbRLiujdEUbpwF_faBSr3FiRSwcALQVh1jEkSfBPU056M3PDX5RSv69Tf1p2ooLNzQcA==";
            using var client = new InfluxDBClient("http://localhost:8086", token);

            var currentPSTTime = DateTime.Now; // Convert to PST
            var point = PointData
              .Measurement("test")
              .Tag("host", "host1")
              .Field("percent_hum", dato)
              .Timestamp(fecha, WritePrecision.Ms);

            try
            {
                using (var writeApi = client.GetWriteApi())
                {
                    writeApi.WritePoint(point, bucket, org);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            return true;
        }

        public async Task<List<DatoHumedad>> ObtenerRegistros()
        {
            List<DatoHumedad> recordsList = new List<DatoHumedad>();
            string errorMessage = "";
            var token = "fKrOwRjmCeXQlBwf-6RbRLiujdEUbpwF_faBSr3FiRSwcALQVh1jEkSfBPU056M3PDX5RSv69Tf1p2ooLNzQcA==";
            using var client = new InfluxDBClient("http://localhost:8086", token);

            try
            {
                var query = "from(bucket: \"Test\") |> range(start: -3h)";
                var tables = await client.GetQueryApi().QueryAsync(query, org);

                tables.ForEach(table =>
                {
                    var fluxRecords = table.Records;

                    fluxRecords.ForEach(fluxRecord =>
                    {
                        //recordsList.Add($"{fluxRecord.GetTime()}", $"{fluxRecord.GetValue()}");

                        var datoTemporal = new DatoHumedad()
                        {
                            Time = fluxRecord.GetTime().ToString(),
                            Value = fluxRecord.GetValue().ToString()
                        };

                        recordsList.Add(datoTemporal);
                    });
                });

                //foreach (var record in tables.SelectMany(table => table.Records))
                //{
                //    recordsList.Add(record));
                //}
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return recordsList;
        }

       



    }
}
