using Autovelox.Data.Models;
using Microsoft.EntityFrameworkCore;
using Autovelox.Application.Dtos;
using AutoMapper;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;

namespace Autovelox.WebAPI.Services
{
    public class MappeService
    {
        private readonly AutoveloxContext _context;
        private readonly IMapper _mapper;

        public MappeService(AutoveloxContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // per ritornare i dati dentro un Dto ho fatto qualche esempio di mapping manuale e qualche esempio di utilizzo di AutoMapper

        public async Task<IEnumerable<MappaDto>> GetAllAsync()
        {
            return await _context.Mappe.ProjectTo<MappaDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<MappaDto> GetById(int id)
        {
            return await _context.Mappe.Select(m => new MappaDto()
            {
                Id = m.Id,
                Nome = m.Nome,
                Latitudine = m.Latitudine,
                Longitudine = m.Longitudine
            }).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MappaDto>> GetByComune(int idComune)
        {
            return await _context.Mappe.Where(m => m.IdComune == idComune).ProjectTo<MappaDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<MappaDto>> GetByProvincia(int idProvincia)
        {
            return await _context.Comuni.Include(p => p.Mappe).Where(c => c.IdProvincia == idProvincia)
                .SelectMany(c => c.Mappe).ProjectTo<MappaDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<MappaDto>> GetByRegione(int idRegione)
        {
            return await _context.Province.Include(p => p.Comuni).ThenInclude(c => c.Mappe).Where(p => p.IdRegione == idRegione)
                .SelectMany(p => p.Comuni.SelectMany(c => c.Mappe)).ProjectTo<MappaDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<DettagliMappaDto> GetDettagliMappa(int id)
        {
            // var mappa = await _context.RipartizioneGeografiche.Include(g => g.Regiones).ThenInclude(r => r.Province).ThenInclude(p => p.Comuni).ThenInclude(c => c.Mappe).Where(m => m.Id == id).FirstAsync();
            var mappa = await _context.Mappe.Include(m => m.IdComuneNavigation).ThenInclude(c => c.IdProvinciaNavigation)
                .ThenInclude(p => p.IdRegioneNavigation).ThenInclude(r => r.IdRipartizioneGeograficaNavigation).FirstOrDefaultAsync(m => m.Id == id);

            if (mappa == null)
            {
                throw new Exception();
            }

            return new DettagliMappaDto()
            {
                Id = id,
                Nome = mappa.Nome,
                Comune = mappa.IdComuneNavigation.Denominazione,
                SiglaProvincia = mappa.IdComuneNavigation.IdProvinciaNavigation.Sigla,
                Regione = mappa.IdComuneNavigation.IdProvinciaNavigation.IdRegioneNavigation.Denominazione,
                RipartizioneGeografica = mappa.IdComuneNavigation.IdProvinciaNavigation.IdRegioneNavigation.IdRipartizioneGeograficaNavigation.Denominazione,
                AnnoInserimento = mappa.AnnoInserimento,
                DataOraInserimento = mappa.DataOraInserimento,
                IdentificatoreOpenStreetMap = mappa.IdentificatoreOpenStreetMap,
                Latitudine = mappa.Latitudine,
                Longitudine = mappa.Longitudine
            };
        }
    }
}
