using Autovelox.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autovelox.Application.Dtos;
using Autovelox.Application.Extensions;

namespace Autovelox.Application.Services
{
    public class ComuniService
    {
        private readonly AutoveloxContext _context;
        public ComuniService(AutoveloxContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<GetComuniDto>> GetAllAsync()
        {
            return await _context.Comuni.Include(c => c.IdProvinciaNavigation).Select( c => 
                new GetComuniDto() { 
                    Denominazione = c.Denominazione,
                    Provincia = c.IdProvinciaNavigation.Sigla
                }).ToListAsync();
        }

        public async Task<IEnumerable<DettagliComuneDto>> GetById(int idComune)
        {
            return await _context.Comuni.Include(c => c.IdProvinciaNavigation).ThenInclude(p => p.IdRegioneNavigation)
                .ThenInclude(r => r.IdRipartizioneGeograficaNavigation).Where(c => c.IdComune == idComune).Select(c =>
                new DettagliComuneDto()
                {
                    IdComune = c.IdComune,
                    Nome = c.Denominazione,
                    Provincia = c.IdProvinciaNavigation.Denominazione,
                    Regione = c.IdProvinciaNavigation.IdRegioneNavigation.Denominazione,
                    RipartizioneGeografica = c.IdProvinciaNavigation.IdRegioneNavigation.IdRipartizioneGeograficaNavigation.Denominazione
                }).ToListAsync();
        }

        public async Task<IEnumerable<GetComuniDto>> GetAllByProvincia(int idProvincia)
        {
            return await _context.Comuni.Include(c => c.IdProvinciaNavigation).Where(c => c.IdProvincia == idProvincia).Select(c =>
                new GetComuniDto()
                {
                    Denominazione = c.Denominazione,
                    Provincia = c.IdProvinciaNavigation.Sigla
                }).ToListAsync();
        }

        
        // ricerca generica di una stringa dentro i campi denominazione di ogni entità
        public async Task<IEnumerable<DettagliComuneDto>> GetByTextFilter(string textFilter)
        {
            var comuni = await _context.Comuni.Include(c => c.IdProvinciaNavigation)
                .ThenInclude(p => p.IdRegioneNavigation).ThenInclude(r => r.IdRipartizioneGeograficaNavigation)
                .WhereIf(!string.IsNullOrEmpty(textFilter), c => c.Denominazione.Contains(textFilter) || c.IdProvinciaNavigation.Denominazione.Contains(textFilter)
                    || c.IdProvinciaNavigation.IdRegioneNavigation.Denominazione.Contains(textFilter)
                    || c.IdProvinciaNavigation.IdRegioneNavigation.IdRipartizioneGeograficaNavigation.Denominazione.Contains(textFilter)
                ).Select(c => new DettagliComuneDto()
                {
                    IdComune = c.IdComune,
                    Nome = c.Denominazione,
                    Provincia = c.IdProvinciaNavigation.Denominazione,
                    Regione = c.IdProvinciaNavigation.IdRegioneNavigation.Denominazione,
                    RipartizioneGeografica = c.IdProvinciaNavigation.IdRegioneNavigation.IdRipartizioneGeograficaNavigation.Denominazione
                }).ToListAsync();

            if (comuni == null) 
            {
                throw new Exception();
            }

            return comuni;
        }
    }
}
