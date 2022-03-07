using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.Entities;

public class Message : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(8000)]
    public string Content { get; set; }

    public MessageType MessageType { get; set; }
        
    public Guid? MainMessageId { get; set; }
    [ForeignKey(nameof(MainMessageId))]
    public Message RepliedToMessage { get; set; }
    public virtual List<Message> Replies { get; set; }
}