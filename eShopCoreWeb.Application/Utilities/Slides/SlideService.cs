using eShopCoreWeb.Data.EF;
using eShopCoreWeb.Data.Entities;
using eShopCoreWeb.ViewModels.Utilities.Slides;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopCoreWeb.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly EShopDbContext _context;

        public SlideService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
                .Select(x => new SlideViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Url = x.Url,
                    Image = x.Image
                }).ToListAsync();

            return slides;
        }
    }
}
