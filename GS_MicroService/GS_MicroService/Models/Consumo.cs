namespace MonitoramentoEnergia.Models
{
    public class Consumo
    {
        public string Id { get; set; }          // Identificador único do consumo
        public double EnergiaConsumida { get; set; }  // Quantidade de energia consumida (em kWh)
        public DateTime Data { get; set; }      // Data do consumo
    }
}
