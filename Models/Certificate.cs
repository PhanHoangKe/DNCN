using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EduFlex.Models;

public partial class Certificate
{
    [Key]
    public int CertificateId { get; set; }

    public int EnrollmentId { get; set; }

    public string CertificateCode { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public string? PdfUrl { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;
}
