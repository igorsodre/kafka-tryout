using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Index(nameof(SerialNumber), nameof(EncryptedSharedSecret))]
public class Device : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MinLength(24), MaxLength(32)]
    public string SerialNumber { get; set; }

    [MaxLength(64)]
    public string EncryptedSharedSecret { get; set; }

    [MaxLength(25)]
    public string FirmwareVersion { get; set; }
        
}