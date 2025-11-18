using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.Constants;
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
            // Start with all blogs
            var query = context.Blogs.AsQueryable();

            // Apply category filter if specified
            if (blogParams.CategoryId.HasValue && blogParams.CategoryId > 0)
            {
                query = query.Where(b => b.BlogCategoryID == blogParams.CategoryId.Value);
            }

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(blogParams.searchTerm))
            {
                var searchTerm = blogParams.searchTerm.Trim().ToLower();
                query = query.Where(b => 
                    b.Title.ToLower().Contains(searchTerm) || 
                    b.Description.ToLower().Contains(searchTerm) || 
                    b.Authour.ToLower().Contains(searchTerm)
                );
            }

            // Get total count for pagination
            int totalCount = await query.CountAsync();

            // Apply pagination
            var pagedBlogs = await query
                .Include(b => b.BlogCategory)
                .OrderByDescending(b => b.CreateDate)
                .Skip((blogParams.PageIndex - 1) * blogParams.pageSize)
                .Take(blogParams.pageSize)
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    InnerHtml = b.InnerHtml,
                    ImageUrl = b.ImageUrl,
                    Authour = b.Authour,
                    AuthorName = b.Authour, // Using existing author field
                    PublishDate = b.CreateDate,
                    ReadTime = CalculateReadTime(b.InnerHtml),
                    Count = totalCount,
                    BlogCategoryName = b.BlogCategory.Name,
                    BlogCategoryId = b.BlogCategoryID
                })
                .ToListAsync();

            return pagedBlogs;
        }

        public async Task<List<BlogDto>> GetAllBlogsByCategory(BlogsByCategoryParams blogParams)
        {
            //2 . Appling Searching by Category
            var sortedcategoryNameBlogs = context.BlogCategory.Where(row => row.Name.Contains(blogParams.categoryName.Trim()));

            //2 . Appling Searching
            var sortedBlogs = context.Blogs.Where(row => row.BlogCategoryID== sortedcategoryNameBlogs.FirstOrDefault().Id);


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
                Count = totalCount,
                BlogCategoryName = c.BlogCategory.Name
            }).ToListAsync();

            return results;
        }

        public async Task<List<FaqDto>> GetAllFaqByCategoryAsync(int Category, string search = null)
        {
            var query = context.FAQs.Where(c => c.CategoryId == Category);
            
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(f => 
                    f.Title.ToLower().Contains(search) || 
                    f.Description.ToLower().Contains(search)
                );
            }
            
            var ListFaq = await query.ToListAsync();
            var ListFaqDto = mapper.Map<List<FaqDto>>(ListFaq);
            return ListFaqDto;
        }

        public async Task<List<FaqCategoryDto>> GetAllFaqCategoryAsync()
        {
            var ListFaqCategory = await context.FAQCategories.ToListAsync();
            var ListFaqCategoryDto = ListFaqCategory.Select(c => new FaqCategoryDto()
            {
                Id = c.Id,
                Name = c.CategoryTitle
            }).ToList();
            return ListFaqCategoryDto;
        }

        public async Task<BlogDto> GetBlogDetails(int blogId)
        {
            // Get the main blog post
            var blog = await context.Blogs
                .Include(b => b.BlogCategory)
                .FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null)
                return null;

            // Create the blog DTO with detailed information
            var blogDto = new BlogDto
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                InnerHtml = blog.InnerHtml,
                ImageUrl = blog.ImageUrl,
                Authour = blog.Authour,
                AuthorName = blog.Authour,
                PublishDate = blog.CreateDate,
                ReadTime = CalculateReadTime(blog.InnerHtml),
                BlogCategoryName = blog.BlogCategory.Name,
                BlogCategoryId = blog.BlogCategoryID
            };

            // Get related posts (posts in the same category, excluding current post)
            blogDto.RelatedPosts = await context.Blogs
                .Where(b => b.BlogCategoryID == blog.BlogCategoryID && b.Id != blogId)
                .OrderByDescending(b => b.CreateDate)
                .Take(3)
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    Authour = b.Authour,
                    AuthorName = b.Authour,
                    PublishDate = b.CreateDate,
                    ReadTime = CalculateReadTime(b.InnerHtml),
                    BlogCategoryName = blog.BlogCategory.Name
                })
                .ToListAsync();

            return blogDto;
        }

        // Helper method to calculate read time based on content length
        private static int CalculateReadTime(string content)
        {
            if (string.IsNullOrEmpty(content))
                return 1;
            
            // Average reading speed: 200 words per minute
            int wordCount = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int readTimeMinutes = (int)Math.Ceiling(wordCount / 200.0);
            return readTimeMinutes > 0 ? readTimeMinutes : 1; // Minimum 1 minute
        }

        // Method to get all blog categories
        public async Task<List<BlogCategoryDto>> GetAllBlogCategories()
        {
            return await context.BlogCategory
                .Select(c => new BlogCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();
        }

        public async Task<List<HomeCategoryDto>> GetAllCategories(CategoryParams categoryParams)
        {
            return await context.BusinessCategories.Select(c=> new HomeCategoryDto() { 
            Id = c.ID,
            Name = c.Name,
            Description = c.Description
            }).ToListAsync();
        }

        public Task<List<FaqDto>> GetAllFaqsCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TrendingBusinessDto>> GetTrendingBusinesses(TrendingBusinessParams trendingBusinessParams)
        {
            throw new NotImplementedException();
        }

        Task<List<CategoryDto>> IHomeService.GetAllCategories(CategoryParams categoryParams)
        {
            throw new NotImplementedException();
        }
    }
}
