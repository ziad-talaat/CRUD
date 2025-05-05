using CRUD.Data;
using CRUD.Models;
using CRUD.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CRUD.Controllers
{
    public class CityController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CityController(ApplicationDbContext context)
        {
            _context = context; 
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? searchProperty,string? searchValue,string? sort,bool isAssending=true)
        {
            var searcList = new List<SelectListItem>
            {
                new SelectListItem{Value=nameof(City.Name),Text="City Name"},
                new SelectListItem{Value=nameof(City.Code),Text="City Code"},
                new SelectListItem{Value=nameof(City.Country),Text="City Country"},

            };
            ViewBag.SearcList = searcList;
            ViewBag.SearchProperty = searchProperty;
            ViewBag.SearchValue = searchValue;
           
            ViewBag.IsAssending = isAssending;


            IQueryable<City> query = _context.Cities.AsNoTracking().AsQueryable();

            if(!string.IsNullOrEmpty(searchValue)&& !string.IsNullOrEmpty(searchProperty))
            {
               

                var parameter = Expression.Parameter(typeof(City), "x");
                var property = Expression.Property(parameter, searchProperty);

                var constant=Expression.Constant(searchValue);
                var method = typeof(string).GetMethod("Contains", new[] {typeof(string)} );
                var result = Expression.Call(property, method, constant);
                var lambda = Expression.Lambda<Func<City, bool>>(result, parameter);
                query=query.Where(lambda);  
            }


            if (!string.IsNullOrEmpty(sort))
            {
                var parameter = Expression.Parameter(typeof(City), "x");
                var property = Expression.Property(parameter, sort);
                var propertyType = property.Type;

                var method = isAssending==true?"OrderBy": "OrderByDescending";
                var lambda = Expression.Lambda(property, parameter);

                var result=Expression.Call(
                    typeof(Queryable),
                    method,
                    new Type[] {typeof(City),propertyType},
                     query.Expression ,
                     Expression.Quote(lambda));
                query = query.Provider.CreateQuery<City>(result);
            }

                List<City>cities= await query.ToListAsync();



            return View(cities);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel request)
        {
            if (ModelState.IsValid)
            {
                City city = Helper.ConvertToCity(request);
                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CityController.Index));
            }
            return View(request);
        }




        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            City? city=await _context.Cities.FindAsync(id);
            if (city == null)
            {
                ViewBag.NotFound = "No such City";
                return View("NotFound");
            }
               
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(City oldCity)
        {
            if (ModelState.IsValid)
            {
                City ?newcity = _context.Cities.Find(oldCity.Id);

                Helper.UpdateCity(newcity,oldCity);

                try
                {
                    _context.Cities.Update(newcity);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                    return View(oldCity);
                }
                return RedirectToAction(nameof(CityController.Index));
            }
            return View(oldCity);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            City? city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                ViewBag.NotFound = "No such City";
                return View("NotFound");
            }

            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletee(int id)
        {
          
               City? city=_context.Cities.Find(id);
                if(city == null)
                    return View("NotFound");
                try
                {
                    _context.Cities.Remove(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                    return View(city);
                }
                return RedirectToAction(nameof(CityController.Index));
            
            
        }
    }
}
