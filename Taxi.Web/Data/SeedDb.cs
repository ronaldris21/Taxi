using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Enums;
using Taxi.Web.Data.Entities;
using Taxi.Web.Helpers;

namespace Taxi.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _datacontext;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext dataContext,
            IUserHelper userHelper)
        {
            this._datacontext = dataContext;
            this._userHelper = userHelper;
        }



        public async Task SeedAsync()
        {
            await _datacontext.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            var admin = await CheckUserAsync("1010", "Ronald", "Ris", "rrris402@gmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Admin);
            var driver = await CheckUserAsync("2020", "Juan", "Zuluaga", "jzuluaga55@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Driver);
            var user1 = await CheckUserAsync("3030", "Juan", "Zuluaga", "carlos.zuluaga@globant.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            var user2 = await CheckUserAsync("4040", "Juan", "Zuluaga", "juanzuluaga2480@correo.itm.edu.co", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            await CheckTaxisAsync(driver, user1, user2);
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Driver.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<UserEntity> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckTaxisAsync(
            UserEntity driver,
            UserEntity user1,
            UserEntity user2)
        {
            if (!_datacontext.Taxis.Any())
            {
                _datacontext.Taxis.Add(new TaxiEntity
                {
                    User = driver,
                    Plaque = "TPQ123",
                    Trips = new List<TripEntity>
                    {
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.5f,
                            Source = "ITM Fraternidad",
                            Target = "ITM Robledo",
                            Remarks = "Muy buen servicio",
                            User = user1
                        },
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.8f,
                            Source = "ITM Robledo",
                            Target = "ITM Fraternidad",
                            Remarks = "Conductor muy amable",
                            User = user1
                        }
                    }
                });

                _datacontext.Taxis.Add(new TaxiEntity
                {
                    Plaque = "THW321",
                    User = driver,
                    Trips = new List<TripEntity>
                    {
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.5f,
                            Source = "ITM Fraternidad",
                            Target = "ITM Robledo",
                            Remarks = "Muy buen servicio",
                            User = user2
                        },
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.8f,
                            Source = "ITM Robledo",
                            Target = "ITM Fraternidad",
                            Remarks = "Conductor muy amable",
                            User = user2
                        }
                    }
                });

                await _datacontext.SaveChangesAsync();
            }
        }







        private async Task CheckTaxiAsyncBOGUS()
        {
            if (_datacontext.Taxis.Any())
            {
                return;
            }

            //TODO: Poblate with autogenerated Data - Testing Data
            Faker<TripEntity> tripFaker = new Faker<TripEntity>()
                .RuleFor(x => x.SourceLongitude, f => f.Address.Longitude(-90, -89))
                .RuleFor(x => x.SourceLatitude, f => f.Address.Latitude(13, 14))
                .RuleFor(x => x.Source, f => f.Address.StreetAddress())
                .RuleFor(x => x.StartDate, f => f.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5)))
                .RuleFor(x => x.EndDate, f => f.Date.Between(DateTime.UtcNow.AddMinutes(5), DateTime.UtcNow.AddMinutes(10)))
                .RuleFor(x => x.Qualification, f => f.Random.Float() * 10)
                .RuleFor(x => x.Remarks, f => f.Lorem.Slug());

            Faker<TaxiEntity> taxiFaker = new Faker<TaxiEntity>()
                .RuleFor(x => x.Plaque, f => f.Random.AlphaNumeric(6).ToUpper());

            TaxiEntity taxi;
            for (int i = 0; i < 5; i++)
            {
                do //till it gets a unique Plaque, in case it already exist on DB
                {
                    taxi = taxiFaker.Generate();
                } while (_datacontext.Taxis.FirstOrDefault(t => t.Plaque == taxi.Plaque) != default(TaxiEntity));

                taxi.Trips = new List<TripEntity>();
                for (int j = 0; j < 5; j++)
                {
                    taxi.Trips.Add(tripFaker.Generate());
                }
                _datacontext.Taxis.Add(taxi);
            }
            await _datacontext.SaveChangesAsync();
        }
    }
}
