using Microsoft.AspNetCore.Mvc;
using MonitoramentoEnergia.Models;
using MonitoramentoEnergia.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoramentoEnergia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumoController : ControllerBase
    {
        private readonly ConsumoService _consumoService;

        public ConsumoController(ConsumoService consumoService)
        {
            _consumoService = consumoService;
        }

        // Rota para verificar o status da API
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Serviço funcionando normalmente" });
        }

        // Rota para registrar um consumo de energia
        [HttpPost("consumo")]
        public async Task<IActionResult> RegistrarConsumo([FromBody] Consumo consumo)
        {
            if (consumo == null || consumo.EnergiaConsumida <= 0)
            {
                return BadRequest("Dados inválidos.");
            }

            consumo.Id = Guid.NewGuid().ToString();  // Gera um ID único para o consumo
            consumo.Data = DateTime.Now;

            try
            {
                var consumoSalvo = await _consumoService.InserirConsumoAsync(consumo);
                return CreatedAtAction(nameof(ConsultarConsumo), new { id = consumoSalvo.Id }, consumoSalvo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar o consumo: {ex.Message}");
            }
        }

        // Rota para consultar todos os consumos registrados
        [HttpGet("consumo")]
        public async Task<IActionResult> ConsultarConsumos()
        {
            try
            {
                var consumos = await _consumoService.RecuperarConsumosAsync();
                if (consumos.Count == 0)
                {
                    return NotFound("Nenhum consumo registrado.");
                }
                return Ok(consumos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao recuperar os consumos: {ex.Message}");
            }
        }

        // Rota para consultar um consumo específico pelo ID
        [HttpGet("consumo/{id}")]
        public async Task<IActionResult> ConsultarConsumo(string id)
        {
            try
            {
                var consumo = await _consumoService.RecuperarConsumoPorIdAsync(id);
                if (consumo == null)
                {
                    return NotFound($"Consumo com ID {id} não encontrado.");
                }
                return Ok(consumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao recuperar o consumo: {ex.Message}");
            }
        }
    }
}
