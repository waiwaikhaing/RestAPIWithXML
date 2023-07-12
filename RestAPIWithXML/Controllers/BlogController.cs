using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIWithXML.Data;
using RestAPIWithXML.Models;
using System.Reflection.Metadata;
using System.Xml.Serialization;
using static System.Reflection.Metadata.BlobBuilder;

namespace RestAPIWithXML.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public BlogController(BlogDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogModel>>> GetBlogs()
        {
            var blogs = await _context.Blogs.ToListAsync();
            // Serialize the data to XML
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<BlogModel>));
            var xmlString = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, blogs);
                xmlString = stringWriter.ToString();
            }

            // Return the XML data
            return Content(xmlString, "application/xml");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogModel>> GetBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }
            // Serialize the data to XML
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(BlogModel));
            var xmlString = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, blog);
                xmlString = stringWriter.ToString();
            }

            // Return the XML data
            return Content(xmlString, "application/xml");
        }

        //[HttpPost]
        //[Consumes("application/xml")]
        //public async Task<ActionResult<BlogModel>> CreateBlog(BlogModel blog)
        //{
        //    _context.Blogs.Add(blog);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
        //}

        [HttpPost]
        [Consumes("application/xml")]
        public async Task<ActionResult<BlogModel>> CreateBlog()
        {
            using var reader = new StreamReader(Request.Body);
            var xmlString = await reader.ReadToEndAsync();
            var blog = DeserializeXml<BlogModel>(xmlString);
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            // Serialize the data to XML
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(BlogModel));
            var xmlString1 = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, blog);
                xmlString1 = stringWriter.ToString();
            }
            // Return the XML data
            return Content(xmlString1, "application/xml");
        }

        private static T DeserializeXml<T>(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stringReader = new StringReader(xmlString);
            return (T)serializer.Deserialize(stringReader);
        }

        [HttpPut("{id}")]
        [Consumes("application/xml")]
        public async Task<ActionResult<BlogModel>> UpdateBlog(int id)
        {
            using var reader = new StreamReader(Request.Body);
            var xmlString = await reader.ReadToEndAsync();
            var blog = DeserializeXml<BlogModel>(xmlString);
            if (blog.Id != id )
            {
                return BadRequest();
            }
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // Serialize the data to XML
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(BlogModel));
            var xmlString1 = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, blog);
                xmlString1 = stringWriter.ToString();
            }

            // Return the XML data
            return Content(xmlString1, "application/xml");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

