# SportField

## Tổng quan
SportField là dự án quản lý đặt sân thể thao được phát triển trên nền tảng .NET Core 9, áp dụng kiến trúc Modular Monolith. Kiến trúc này cho phép ứng dụng được tổ chức thành các module độc lập, mỗi module được triển khai như một service riêng biệt, có thể scale thành microservice dễ dàng khi cần thiết.

## Tech stack:
- Backend: .NET 9, ASP.NET WebAPI, EF Core, PostgresDb, Docker, MediatR, AutoMapper
- Frontend: NextJs (dự kiến)

## Kiến trúc Modular Monolith
Ứng dụng được tổ chức theo kiến trúc Modular Monolith với các đặc điểm:
- Các module được phân tách rõ ràng về mặt logic
- Mỗi module có cơ sở dữ liệu riêng (Database per Service)
- Giao tiếp giữa các module thông qua API nội bộ
- Triển khai dưới dạng một ứng dụng đơn nhất (single deployment unit)

## Các Module chính
### 1. BookingService
Module quản lý quá trình đặt sân, bao gồm tạo, sửa, hủy và theo dõi các đơn đặt sân.
### 2. FieldService
Module quản lý thông tin sân thể thao, bao gồm các loại sân, giá cả, tình trạng và lịch sử sử dụng.
### 3. IdentityService
Module quản lý người dùng, phân quyền và xác thực.
### 4. FileService
Module quản lý việc lưu trữ và truy xuất tập tin (ảnh, tài liệu).
### 5. NotificationService
Module quản lý gửi thông báo đến người dùng (email, SMS, push notification).
### 6. Contract
Module chứa các DTO (Data Transfer Objects), định nghĩa event và request (sử dụng MediatR) được dùng để giao tiếp giữa các module.

## Common Library
Thư viện Common chứa các thành phần dùng chung giữa các module:
- **Abstractions**: Các interface và lớp cơ sở
- **Enums**: Các enum dùng chung
- **Exceptions**: Các exception dùng chung
- **PagedResult**: Đối tượng phân trang kết quả
- **OrderByRequest**: Đối tượng yêu cầu sắp xếp item

## Tương tác giữa các Module
Các module tương tác với nhau thông qua:
1. **API nội bộ**: Sử dụng Mediator pattern với CQRS (thư viện MediatR)
2. **Event-driven**: Sử dụng domain events và integration events
3. **Shared Contracts**: Sử dụng các DTO được định nghĩa trong module Contract

## Triển khai
Mặc dù được tổ chức thành các module riêng biệt, toàn bộ ứng dụng được triển khai dưới dạng một đơn vị duy nhất thông qua API Gateway (SportField.WebAPI).

## Clean Architecture
Một số module domain (Identity, Field, Booking) trong dự án được áp dụng nguyên tắc Clean Architecture với cấu trúc:
1. **Domain Layer**: Chứa các entities, messages, exceptions, interfaces, và logic nghiệp vụ
2. **Application Layer**: Chứa logic ứng dụng, commands/queries, validators, và mappings
3. **Infrastructure Layer**: Chứa cài đặt cho cơ sở dữ liệu, identity, file storage, và các dịch vụ bên ngoài
