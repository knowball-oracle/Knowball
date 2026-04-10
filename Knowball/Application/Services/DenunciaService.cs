using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Models;

namespace Fiap.Knowball.Application.Services
{
    public class DenunciaService : IDenunciaService
    {
        private readonly IDenunciaRepository _repository;
        private readonly ILogger<DenunciaService> _logger;

        public DenunciaService(IDenunciaRepository repository, ILogger<DenunciaService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public DenunciaDto CriarDenuncia(DenunciaDto dto)
        {
            _logger.LogInformation("Criando denúncia: Protocolo={Protocolo}, PartidaId={IdPartida}, ArbitroId={IdArbitro}",
                dto.Protocolo, dto.IdPartida, dto.IdArbitro);

            var denuncia = new Denuncia
            {
                IdPartida = dto.IdPartida,
                IdArbitro = dto.IdArbitro,
                Protocolo = dto.Protocolo,
                Relato = dto.Relato,
                DataDenuncia = dto.DataDenuncia,
                Status = dto.Status,
                ResultadoAnalise = dto.ResultadoAnalise
            };

            if (!denuncia.StatusValido())
            {
                _logger.LogWarning("Status inválido ao criar denúncia: {Status}", dto.Status);
                throw new BusinessException("Status inválido");
            }
            if (!denuncia.ProtocoloValido())
            {
                _logger.LogWarning("Protocolo inválido ao criar denúncia: {Protocolo}", dto.Protocolo);
                throw new BusinessException("Protocolo inválido");
            }
            if (!denuncia.RelatoValido())
            {
                _logger.LogWarning("Relato inválido ao criar denúncia com Protocolo={Protocolo}", dto.Protocolo);
                throw new BusinessException("Relato inválido");
            }

            _repository.Add(denuncia);
            _logger.LogInformation("Denúncia criada com sucesso: IdDenuncia={IdDenuncia}, Protocolo={Protocolo}",
                denuncia.IdDenuncia, denuncia.Protocolo);

            return new DenunciaDto
            {
                IdDenuncia = denuncia.IdDenuncia,
                IdPartida = denuncia.IdPartida,
                IdArbitro = denuncia.IdArbitro,
                Protocolo = denuncia.Protocolo,
                Relato = denuncia.Relato,
                DataDenuncia = denuncia.DataDenuncia,
                Status = denuncia.Status,
                ResultadoAnalise = denuncia.ResultadoAnalise
            };
        }

        public IEnumerable<DenunciaDto> ListarDenuncias()
        {
            _logger.LogInformation("Listando todas as denúncias");
            var denuncias = _repository.GetAll();
            return denuncias.Select(d => new DenunciaDto
            {
                IdDenuncia = d.IdDenuncia,
                IdPartida = d.IdPartida,
                IdArbitro = d.IdArbitro,
                Protocolo = d.Protocolo,
                Relato = d.Relato,
                DataDenuncia = d.DataDenuncia,
                Status = d.Status,
                ResultadoAnalise = d.ResultadoAnalise
            });
        }

        public DenunciaDto ObterPorId(int id)
        {
            _logger.LogInformation("Buscando denúncia: IdDenuncia={IdDenuncia}", id);

            var d = _repository.GetById(id);
            if (d == null)
            {
                _logger.LogWarning("Denúncia não encontrada: IdDenuncia={IdDenuncia}", id);
                return null;
            }

            return new DenunciaDto
            {
                IdDenuncia = d.IdDenuncia,
                IdPartida = d.IdPartida,
                IdArbitro = d.IdArbitro,
                Protocolo = d.Protocolo,
                Relato = d.Relato,
                DataDenuncia = d.DataDenuncia,
                Status = d.Status,
                ResultadoAnalise = d.ResultadoAnalise
            };
        }

        public void AtualizarDenuncia(int id, DenunciaDto dto)
        {
            _logger.LogInformation("Atualizando denúncia: IdDenuncia={IdDenuncia}", id);

            var d = _repository.GetById(id);
            if (d == null)
            {
                _logger.LogWarning("Denúncia não encontrada para atualização: IdDenuncia={IdDenuncia}", id);
                throw new BusinessException("Denúncia não encontrada");
            }

            d.IdPartida = dto.IdPartida;
            d.IdArbitro = dto.IdArbitro;
            d.Protocolo = dto.Protocolo;
            d.Relato = dto.Relato;
            d.DataDenuncia = dto.DataDenuncia;
            d.Status = dto.Status;
            d.ResultadoAnalise = dto.ResultadoAnalise;

            if (!d.StatusValido())
            {
                _logger.LogWarning("Status inválido ao atualizar denúncia IdDenuncia={IdDenuncia}: {Status}", id, dto.Status);
                throw new BusinessException("Status inválido");
            }
            if (!d.ProtocoloValido())
            {
                _logger.LogWarning("Protocolo inválido ao atualizar denúncia IdDenuncia={IdDenuncia}: {Protocolo}", id, dto.Protocolo);
                throw new BusinessException("Protocolo inválido");
            }
            if (!d.RelatoValido())
            {
                _logger.LogWarning("Relato inválido ao atualizar denúncia IdDenuncia={IdDenuncia}", id);
                throw new BusinessException("Relato inválido");
            }

            _repository.Update(d);
            _logger.LogInformation("Denúncia atualizada com sucesso: IdDenuncia={IdDenuncia}", id);
        }

        public void RemoverDenuncia(int id)
        {
            _logger.LogInformation("Removendo denúncia: IdDenuncia={IdDenuncia}", id);
            _repository.Remove(id);
            _logger.LogInformation("Denúncia removida com sucesso: IdDenuncia={IdDenuncia}", id);
        }
    }
}