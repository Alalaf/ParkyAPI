using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail Trail)
        {
            _db.Trails.Add(Trail);
            return Save();
        }

        public bool DeleteTrail(Trail Trail)
        {
            _db.Trails.Remove(Trail);
            return Save();
        }

        public Trail GetTrail(int Trailid)
        {
            return _db.Trails.Include(n => n.NationalPark).FirstOrDefault(n => n.Id == Trailid);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(n => n.NationalPark).OrderBy(n => n.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(n => n.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail Trail)
        {
            _db.Trails.Update(Trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npid)
        {
            return _db.Trails.Include(n => n.NationalPark).Where(t => t.NationalParkId == npid).ToList();
        }
    }
}
