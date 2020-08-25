﻿using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Web.Data.Entities;

namespace Taxi.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _datacontext;

        public SeedDb(DataContext dataContext)
        {
            this._datacontext = dataContext;
        }

        public async Task SeedAsync()
        {
            await _datacontext.Database.EnsureCreatedAsync();
            await CheckTaxiAsync();

        }

        private async Task CheckTaxiAsync()
        {
            if (_datacontext.Taxis.Any())
            {
                return;
            }

            //TODO: Poblate with autogenerated Data - Testing
            Faker<TripEntity> tripFaker = new Faker<TripEntity>()
                .RuleFor(x => x.SourceLongitude, f => f.Address.Longitude())
                .RuleFor(x => x.SourceLatitude, f => f.Address.Latitude())
                .RuleFor(x => x.Source, f => f.Address.StreetAddress())
                .RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5)))
                .RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.UtcNow.AddMinutes(5), DateTime.UtcNow.AddMinutes(10)))
                .RuleFor(x => x.Qualification, f => f.Random.Float() * 10)
                .RuleFor(x => x.Remarks, f => f.Lorem.Slug());

            Faker<TaxiEntity> taxiFaker = new Faker<TaxiEntity>()
                .RuleFor(x => x.Plaque, f => f.Random.AlphaNumeric(6).ToUpper());

            for (int i = 0; i < 5; i++)
            {
                TaxiEntity taxi = taxiFaker.Generate();
                taxi.Trips = new List<TripEntity>();
                for (int j = 0; j < 3; j++)
                {
                    taxi.Trips.Add(tripFaker.Generate());
                }
                _datacontext.Taxis.Add(taxi);
            }
            await _datacontext.SaveChangesAsync();
        }
    }
}
