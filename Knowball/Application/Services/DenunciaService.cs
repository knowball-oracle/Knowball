using Knowball.Application.DTOs;
using Knowball.Application.Exceptions;
using Knowball.Domain;
using Knowball.Domain.Repositories;

namespace Knowball.Application.Services
{
    public class DenunciaService : IDenunciaService
    {
        private readonly IDenunciaRepository _repository;

        public DenunciaService(IDenunciaRepository repository)
        {
            _repository = repository;
        }

        public void CriarDenuncia(DenunciaDto dto)
        {
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

            if (!denuncia.StatusValido()) throw new BusinessException("Status inválido");
            if (!denuncia.ProtocoloValido()) throw new BusinessException("Protocolo inválido");
            if (!denuncia.RelatoValido()) throw new BusinessException("Relato inválido");

            _repository.Add(denuncia);
        }

        public IEnumerable<DenunciaDto> ListarDenuncias()
        {
            var denuncias = _repository.GetAll();
            return denuncias.Select(d => new DenunciaDto
            {
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
            var d = _repository.GetById(id);
            if (d == null) throw new BusinessException("Denúncia não encontrada");

            return new DenunciaDto
            {
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
            var d = _repository.GetById(id);
            if (d == null) throw new BusinessException("Denúncia não encontrada");

            d.IdPartida = dto.IdPartida;
            d.IdArbitro = dto.IdArbitro;
            d.Protocolo = dto.Protocolo;
            d.Relato = dto.Relato;
            d.DataDenuncia = dto.DataDenuncia;
            d.Status = dto.Status;
            d.ResultadoAnalise = dto.ResultadoAnalise;

            if (!d.StatusValido()) throw new BusinessException("Status inválido");
            if (!d.ProtocoloValido()) throw new BusinessException("Protocolo inválido");
            if (!d.RelatoValido()) throw new BusinessException("Relato inválido");

            _repository.Update(d);
        }

        public void RemoverDenuncia(int id)
        {
            _repository.Remove(id);
        }
    }
}
