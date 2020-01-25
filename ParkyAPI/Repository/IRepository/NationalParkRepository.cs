using ParkyAPI.Data;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;
        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateNationalPark(NationalPark nationalpark)
        {
            _db.NationalParks.Add(nationalpark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalpark)
        {
            _db.NationalParks.Remove(nationalpark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalparkid)
        {
            return _db.NationalParks.FirstOrDefault(n => n.Id == nationalparkid);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(n => n.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _db.NationalParks.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalParks.Any(n => n.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalpark)
        {
            _db.NationalParks.Update(nationalpark);
            return Save();
        }
    }
}
