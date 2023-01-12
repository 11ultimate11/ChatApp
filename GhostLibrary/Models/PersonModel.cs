using GhostLibrary.Models.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GhostLibrary.Helpers.Enums;

namespace GhostLibrary.Models;

public class PersonModel : AbstractSqlModel
{
    [Required]
    public string? Nachname { get; set; }
    [Required]
    public string? Vorname { get; set; }
    [ForeignKey(nameof(Credentials))]
    public int CredentialsId { get; set; }
    [Required]
    public CredentialsModel? Credentials { get; set; }
    public Genders Gender { get; set; }
    [ForeignKey(nameof(Media))]
    public int MediaId { get; set; }
    [Required]
    public MediaModel? Media { get; set; }
}
