using MongoDB.Driver;
using MonitoramentoEnergia.Models;
using Microsoft.Extensions.Options;

namespace MonitoramentoEnergia.Services
{
    public class ConsumoService
    {
        private readonly IMongoCollection<Consumo> _consumos;

        public ConsumoService(IOptions<MongoSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _consumos = database.GetCollection<Consumo>("consumos");
        }

        // Método para inserir um novo consumo
        public async Task<Consumo> InserirConsumoAsync(Consumo consumo)
        {
            await _consumos.InsertOneAsync(consumo);
            return consumo;
        }

        // Método para recuperar todos os consumos
        public async Task<List<Consumo>> RecuperarConsumosAsync()
        {
            return await _consumos.Find(consumo => true).ToListAsync();
        }

        // Método para recuperar um consumo por ID
        public async Task<Consumo> RecuperarConsumoPorIdAsync(string id)
        {
            return await _consumos.Find(consumo => consumo.Id == id).FirstOrDefaultAsync();
        }
    }

    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
