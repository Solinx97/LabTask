using LabTask.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Server.Models;

public record CommentModel(
    [Required] Guid Id, 
    [Required][StringLength(Comment.CONTENT_MAX_LENGTH)] string Content, 
    [Required] Guid DocumentId,
    [Required] Guid UserId
    );