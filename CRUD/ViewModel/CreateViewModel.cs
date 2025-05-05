using CRUD.Models;
using System.ComponentModel.DataAnnotations;

namespace CRUD.ViewModel
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Country { get; set; }


       

       
    }
    public static class Helper
    {
        public static City ConvertToCity(CreateViewModel cityDTO)
        {
            return new City
            {
                Name = cityDTO.Name,
                Code = cityDTO.Code,
                Country = cityDTO.Country,
            };
        }

        public static void UpdateCity(City newC,City old)
        {
           
           newC.Name = old.Name;
           newC.Code = old.Code;
         newC.Country= old.Country;
           
        }
    }

}
