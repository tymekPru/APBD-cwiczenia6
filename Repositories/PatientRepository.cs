using APBD_cwiczenia6.DAK;
using APBD_cwiczenia6.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_cwiczenia6.Repositories
{
    public class PatientRepository(HospitalDbContext context) : IPatientRepository
    {
        private readonly HospitalDbContext _context = context;

        public Task<List<Patient>> GetPatientsAsync(string? search)
        {
            var query = _context.Patients
                .AsNoTracking()
                .Include(x => x.Admissions)
                    .ThenInclude(y => y.Ward)
                .Include(x => x.BedAssignments)
                    .ThenInclude(y => y.Bed)
                    .ThenInclude(y => y.BedType)
                .Include(x => x.BedAssignments)
                    .ThenInclude(y => y.Bed)
                    .ThenInclude(y => y.Room)
                        .ThenInclude(z => z.Ward)
                .OrderByDescending(x => x.Pesel)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(p =>
                    EF.Functions.Like(p.FirstName, $"%{search}%") ||
                    EF.Functions.Like(p.LastName, $"%{search}%")
                );
            }

            return query
                .OrderByDescending(p => p.Pesel)
                .ToListAsync();
        }

    }
}