using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace RestAPIWithXML.Models
{
   // [XmlRoot("Blog")]
    [Table("Blog")]
    public class BlogModel
    {
        //  [XmlElement("Id")]
        //  [Column("Id")]
        [Key]
        public int Id { get; set; }

      //  [XmlElement("Title")]
      //  [Column("Title")]
        public string Title { get; set; } = string.Empty;

      //  [XmlElement("Content")]
      //  [Column("Content")]
        public string Content { get; set; } = string.Empty;
    }
}
