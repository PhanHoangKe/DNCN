# 🎓 EduFlex - Hệ Thống Quản Lý Học Tập Trực Tuyến

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-blue?style=for-the-badge&logo=aspnet)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-9.0-green?style=for-the-badge&logo=entity-framework)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red?style=for-the-badge&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-purple?style=for-the-badge&logo=bootstrap)

**Một nền tảng học tập trực tuyến hiện đại, được xây dựng với ASP.NET Core MVC**

[🚀 Tính năng](#-tính-năng) • [🛠️ Công nghệ](#️-công-nghệ) • [📦 Cài đặt](#-cài-đặt) • [🎯 Sử dụng](#-sử-dụng) • [📊 Cấu trúc](#-cấu-trúc) • [🤝 Đóng góp](#-đóng-góp)

</div>

---

## 🌟 Tổng quan

**EduFlex** là một hệ thống quản lý học tập trực tuyến toàn diện, được thiết kế để mang lại trải nghiệm học tập tối ưu cho cả học viên và giảng viên. Với giao diện thân thiện và các tính năng mạnh mẽ, EduFlex giúp việc học trực tuyến trở nên hiệu quả và thú vị hơn bao giờ hết.

### 🎯 Tại sao chọn EduFlex?

- ✨ **Giao diện hiện đại**: Thiết kế responsive với Bootstrap 5.3 và NiceAdmin template
- 🔒 **Bảo mật cao**: Mã hóa mật khẩu với BCrypt, xác thực đa lớp
- 📱 **Đa nền tảng**: Hoạt động mượt mà trên mọi thiết bị
- 🚀 **Hiệu suất cao**: Tối ưu hóa với Entity Framework Core và SQL Server
- 🎨 **Dễ tùy chỉnh**: Kiến trúc MVC rõ ràng, dễ mở rộng

---

## 🚀 Tính năng

### 👥 Quản lý người dùng
- **Đa vai trò**: Admin, Instructor, Student, Moderator, Support
- **Hồ sơ cá nhân**: Avatar, thông tin liên hệ, tiểu sử
- **Xác thực an toàn**: Mã hóa mật khẩu, reset password
- **Theo dõi hoạt động**: Log chi tiết các hành động người dùng

### 📚 Quản lý khóa học
- **Danh mục phân cấp**: Hệ thống danh mục cha-con linh hoạt
- **Nhiều cấp độ**: Beginner, Intermediate, Advanced, Expert, Professional
- **Nội dung đa dạng**: Video, Text, Quiz, Assignment, Live Session
- **Đánh giá & Phản hồi**: Hệ thống rating, review, Q&A

### 🎓 Học tập tương tác
- **Tiến độ học tập**: Theo dõi % hoàn thành khóa học
- **Quiz & Kiểm tra**: Hệ thống câu hỏi trắc nghiệm với nhiều lần thử
- **Chứng chỉ**: Tự động cấp chứng chỉ khi hoàn thành khóa học
- **Thảo luận**: Bình luận, hỏi đáp trong từng bài học

### 💰 Thương mại điện tử
- **Giỏ hàng**: Thêm/bỏ khóa học khỏi giỏ hàng
- **Thanh toán**: Hỗ trợ nhiều phương thức thanh toán
- **Mã giảm giá**: Hệ thống coupon linh hoạt
- **Báo cáo doanh thu**: Thống kê chi tiết về doanh số

### 🛠️ Quản trị hệ thống
- **Dashboard**: Tổng quan thống kê với biểu đồ trực quan
- **Quản lý nội dung**: Duyệt, phê duyệt khóa học
- **Thông báo**: Hệ thống thông báo real-time
- **Báo cáo**: Báo cáo chi tiết về người dùng, khóa học, doanh thu

---

## 🛠️ Công nghệ

### Backend
- **ASP.NET Core 9.0** - Framework web hiện đại
- **Entity Framework Core 9.0** - ORM mạnh mẽ
- **SQL Server** - Cơ sở dữ liệu quan hệ
- **BCrypt.Net** - Mã hóa mật khẩu an toàn

### Frontend
- **Bootstrap 5.3** - Framework CSS responsive
- **NiceAdmin Template** - Giao diện admin chuyên nghiệp
- **jQuery** - Thư viện JavaScript
- **ApexCharts** - Biểu đồ tương tác
- **Bootstrap Icons** - Icon set đẹp mắt

### Tools & Libraries
- **Visual Studio 2022** - IDE phát triển
- **SQL Server Management Studio** - Quản lý database
- **Git** - Version control
- **jQuery Validation** - Validation phía client

---

## 📦 Cài đặt

### Yêu cầu hệ thống
- .NET 9.0 SDK
- SQL Server 2019 trở lên
- Visual Studio 2022 (khuyến nghị)
- Git

### Các bước cài đặt

1. **Clone repository**
```bash
git clone https://github.com/yourusername/EduFlex.git
cd EduFlex
```

2. **Cài đặt dependencies**
```bash
dotnet restore
```

3. **Cấu hình database**
   - Tạo database `EduFlex` trong SQL Server
   - Cập nhật connection string trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EduFlex;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

4. **Chạy migration**
```bash
dotnet ef database update
```

5. **Import dữ liệu mẫu**
```bash
# Chạy script SQL trong file data.sql
sqlcmd -S localhost -d EduFlex -i data.sql
```

6. **Chạy ứng dụng**
```bash
dotnet run
```

7. **Truy cập ứng dụng**
   - Website: `https://localhost:5001`
   - Admin Panel: `https://localhost:5001/Admin`

---

## 🎯 Sử dụng

### Tài khoản mặc định
- **Admin**: `admin@example.com` / `password`
- **Instructor**: `instructor1@example.com` / `password`
- **Student**: `student1@example.com` / `password`

### Hướng dẫn nhanh

1. **Đăng nhập** với tài khoản admin
2. **Truy cập Admin Panel** để quản lý hệ thống
3. **Tạo danh mục** khóa học mới
4. **Thêm khóa học** với nội dung phong phú
5. **Quản lý người dùng** và phân quyền
6. **Theo dõi báo cáo** và thống kê

---

## 📊 Cấu trúc

```
EduFlex/
├── Areas/
│   └── Admin/                 # Khu vực quản trị
│       ├── Controllers/       # Controllers cho admin
│       ├── Views/            # Views cho admin
│       └── Models/           # Models riêng cho admin
├── Controllers/              # Controllers chính
├── Models/                   # Data models
│   ├── Category.cs          # Model danh mục
│   ├── Course.cs            # Model khóa học
│   ├── User.cs              # Model người dùng
│   └── ...                  # Các models khác
├── Views/                    # Views chính
├── wwwroot/                  # Static files
│   ├── admin/               # Assets cho admin
│   ├── css/                 # Custom CSS
│   └── uploads/             # Uploaded files
├── data.sql                 # Dữ liệu mẫu
├── database.sql            # Schema database
└── Program.cs              # Entry point
```

### 🗄️ Database Schema

Hệ thống sử dụng **33 bảng** chính với các mối quan hệ phức tạp:

- **Users & Roles**: Quản lý người dùng và phân quyền
- **Courses & Categories**: Cấu trúc khóa học phân cấp
- **Lessons & Sections**: Nội dung học tập chi tiết
- **Enrollments & Progress**: Theo dõi tiến độ học tập
- **Orders & Payments**: Hệ thống thương mại điện tử
- **Quizzes & Questions**: Đánh giá và kiểm tra
- **Notifications & Activity**: Thông báo và log hoạt động

---

## 🎨 Giao diện

### Admin Dashboard
- **Tổng quan thống kê** với biểu đồ trực quan
- **Quản lý người dùng** với giao diện table hiện đại
- **Quản lý khóa học** với preview và edit inline
- **Báo cáo doanh thu** với charts tương tác

### Responsive Design
- **Mobile-first** approach
- **Bootstrap 5.3** grid system
- **NiceAdmin** professional template
- **Custom CSS** cho branding

---

## 🔧 Phát triển

### Thêm tính năng mới

1. **Tạo Model** trong thư mục `Models/`
2. **Thêm DbSet** vào `EduFlexContext`
3. **Tạo Migration**:
```bash
dotnet ef migrations add FeatureName
dotnet ef database update
```
4. **Tạo Controller** với CRUD operations
5. **Tạo Views** tương ứng
6. **Cập nhật Admin Menu** nếu cần

### Coding Standards

- **C#**: Tuân thủ Microsoft coding conventions
- **HTML**: Semantic HTML5, accessibility
- **CSS**: BEM methodology, responsive design
- **JavaScript**: ES6+, jQuery best practices

---

## 🚀 Triển khai

### Production Setup

1. **Cấu hình Production**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=EduFlex;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

2. **Publish ứng dụng**
```bash
dotnet publish -c Release -o ./publish
```

3. **Deploy lên IIS/Azure**
   - Cấu hình IIS với .NET Core Hosting Bundle
   - Hoặc deploy lên Azure App Service

---

## 🤝 Đóng góp

Chúng tôi hoan nghênh mọi đóng góp! Hãy:

1. **Fork** repository
2. **Tạo branch** mới: `git checkout -b feature/AmazingFeature`
3. **Commit** thay đổi: `git commit -m 'Add some AmazingFeature'`
4. **Push** lên branch: `git push origin feature/AmazingFeature`
5. **Tạo Pull Request**

### Báo lỗi
Sử dụng [GitHub Issues](https://github.com/yourusername/EduFlex/issues) để báo cáo lỗi hoặc đề xuất tính năng mới.

---

## 📄 License

Dự án này được phân phối dưới [MIT License](LICENSE). Xem file `LICENSE` để biết thêm thông tin.

---

## 👥 Tác giả

**EduFlex Team**
- **Lead Developer**: [Tên của bạn]
- **UI/UX Designer**: [Tên designer]
- **Database Architect**: [Tên DBA]

---

## 🙏 Lời cảm ơn

- [BootstrapMade](https://bootstrapmade.com/) - NiceAdmin template
- [Microsoft](https://microsoft.com/) - ASP.NET Core framework
- [Entity Framework Team](https://docs.microsoft.com/ef/) - ORM framework
- [Bootstrap Team](https://getbootstrap.com/) - CSS framework

---

<div align="center">

**⭐ Nếu dự án này hữu ích, hãy cho chúng tôi một star! ⭐**

[⬆ Về đầu trang](#-eduflex---hệ-thống-quản-lý-học-tập-trực-tuyến)

</div>