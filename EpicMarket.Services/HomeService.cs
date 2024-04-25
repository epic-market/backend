using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public HomeService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<BlogDto>> GetAllBlogs(BlogParams blogParams)
        {


            //2 . Appling Searching
            var sortedBlogs = context.Blogs.Where(row => row.Title.Contains(blogParams.searchTerm));


            //getting the total count
            int totalCount = sortedBlogs.Count();


            // 4. Apply pagination (skip and take)
            var pagedBlogs = sortedBlogs
                .Skip((blogParams.PageIndex - 1) * blogParams.pageSize) // Skip items for previous pages
                .Take(blogParams.pageSize); // Take items for the current page

            // 5. Select data and add SRNO
            var results = await pagedBlogs.Select(c => new BlogDto()
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Authour = c.Authour,
                Count = totalCount
            }).ToListAsync();

            return results;
        }

        public async Task<List<FaqDto>> GetAllFaqByCategoryAsync(int Category)
        {
            var ListFaq = await context.FAQs.Where(c => c.CategoryId == Category).ToListAsync();
            var ListFaqDto = mapper.Map<List<FaqDto>>(ListFaq);
            return ListFaqDto;
        }

        public async Task<List<FaqCategoryDto>> GetAllFaqCategoryAsync(string typeOfFAQ)
        {
            var ListFaqCategory = await context.FAQCategories.Where(c => c.TypeOfFAQ == typeOfFAQ).ToListAsync();
            var ListFaqCategoryDto = mapper.Map<List<FaqCategoryDto>>(ListFaqCategory);
            return ListFaqCategoryDto;
        }

        public async Task<BlogDto> GetBlogDetails(int blogId)
        {
            return await context.Blogs.Where(row => row.Id == blogId).Select(c => new BlogDto()
            {
                Id = c.Id,
                Title = c.Title,
                InnerHtml = c.InnerHtml,
                ImageUrl = c.ImageUrl,
                Authour = c.Authour
            }).FirstOrDefaultAsync(); ;
        }
    }
}
