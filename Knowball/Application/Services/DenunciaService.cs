using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using Fiap.Knowball.Models.Repositories;

namespace Fiap.Knowball.Application.Services
{
    public class DenunciaService : IDenunciaService
    {
        private readonly IDenunciaRepository _repository;
        private readonly IDenunciaLogRepository _logRepository;
        private readonly ILogger<DenunciaService> _logger;

        public DenunciaService(
            IDenunciaRepository repository,
            IDenunciaLogRepository logRepository,
            ILogger<DenunciaService> logger)
        {
            _repository = repository;
            _logRepository = logRepository;
            _logger = logger;
        }

        public DenunciaDto CriarDenuncia(DenunciaDto dto)
        {
            _logger.LogInformation(
                "Criando denúncia: Protocolo={Protocolo}, PartidaId={IdPartida}, ArbitroId={IdArbitro}",
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

            _ = _logRepository.RegistrarAsync(new DenunciaLog
            {
                DenunciaId = denuncia.IdDenuncia,
                Acao = "Criada",
                Detalhes = $"Protocolo: {denuncia.Protocolo} | Status: {denuncia.Status}"
            });

            _logger.LogInformation(
                "Denúncia criada com sucesso: IdDenuncia={IdDenuncia}, Protocolo={Protocolo}",
                denuncia.IdDenuncia, denuncia.Protocolo);

            return ToDto(denuncia);
        }

        public IEnumerable<DenunciaDto> ListarDenuncias()
        {
            _logger.LogInformation("Listando todas as denúncias");
            return _repository.GetAll().Select(ToDto);
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

            return ToDto(d);
        }

        public void AtualizarDenuncia(int id, DenunciaDto dto)
        {
            _logger.LogInformation("Atualizando denúncia: IdDenuncia={IdDenuncia}", id);

            var d = _repository.GetById(id);
            if (d == null)
            {
                _logger.LogWarning(
                    "Denúncia não encontrada para atualização: IdDenuncia={IdDenuncia}", id);
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
                _logger.LogWarning(
                    "Status inválido ao atualizar denúncia IdDenuncia={IdDenuncia}: {Status}",
                    id, dto.Status);
                throw new BusinessException("Status inválido");
            }
            if (!d.ProtocoloValido())
            {
                _logger.LogWarning(
                    "Protocolo inválido ao atualizar denúncia IdDenuncia={IdDenuncia}: {Protocolo}",
                    id, dto.Protocolo);
                throw new BusinessException("Protocolo inválido");
            }
            if (!d.RelatoValido())
            {
                _logger.LogWarning(
                    "Relato inválido ao atualizar denúncia IdDenuncia={IdDenuncia}", id);
                throw new BusinessException("Relato inválido");
            }

            _repository.Update(d);

            _ = _logRepository.RegistrarAsync(new DenunciaLog
            {
                DenunciaId = d.IdDenuncia,
                Acao = "Atualizada",
                Detalhes = $"Protocolo: {d.Protocolo} | Novo Status: {d.Status}"
            });

            _logger.LogInformation(
                "Denúncia atualizada com sucesso: IdDenuncia={IdDenuncia}", id);
        }

        public void RemoverDenuncia(int id)
        {
            _logger.LogInformation("Removendo denúncia: IdDenuncia={IdDenuncia}", id);

            _ = _logRepository.RegistrarAsync(new DenunciaLog
            {
                DenunciaId = id,
                Acao = "Removida",
                Detalhes = $"Denúncia IdDenuncia={id} excluída do sistema"
            });

            _repository.Remove(id);

            _logger.LogInformation(
                "Denúncia removida com sucesso: IdDenuncia={IdDenuncia}", id);
        }

        private static DenunciaDto ToDto(Denuncia d) => new()
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
}