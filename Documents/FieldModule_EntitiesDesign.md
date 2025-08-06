# Thiết kế Chi tiết các Thực thể (Entities) - Hệ thống Quản lý Sân Thể thao

## 🏗️ Kiến trúc chung

### Phân cấp kế thừa
```
BaseEntity<T> (Generic base)
├── BaseEntity (Guid-based)
└── AuditableEntity<T> (Generic aggregate)
    └── AuditableEntity (Guid-based)
        ├── Field (IAggregateRoot)
        ├── Facility (IAggregateRoot)
        └── TimeSlot (IAggregateRoot)
```

### BaseEntity - Lớp cơ sở cho tất cả entities
**Mục đích**: Cung cấp các tính năng chung cho mọi thực thể trong hệ thống

**Các trường dữ liệu:**
- `Id`: Khóa chính duy nhất (Guid.CreateVersion7() - UUID v7 có thứ tự thời gian)
- `DomainEvents`: Danh sách các sự kiện miền để xử lý nghiệp vụ bất đồng bộ

### AuditableEntity
**Mục đích**: Quản lý audit trail và đảm bảo tính nhất quán dữ liệu

**Các trường dữ liệu:**
- `CreatedDate`: Thời gian tạo bản ghi (UTC)
- `CreatedBy`: ID người tạo bản ghi 
- `ModifiedDate`: Thời gian cập nhật cuối cùng (UTC)
- `ModifiedBy`: ID người cập nhật cuối cùng
- `DeletedDate`: Thời gian xóa mềm (soft delete)

---

## 🏢 Facility - Cơ sở thể thao

**Mục đích**: Đại diện cho một cơ sở thể thao (trung tâm, sân bóng, câu lạc bộ) chứa nhiều sân

### Các trường dữ liệu chi tiết:

#### Thông tin cơ bản
- **`Name`** (required string): Tên cơ sở
  - *Ví dụ*: "Sân bóng Thăng Long", "Trung tâm thể thao Mỹ Đình"
  - *Sử dụng*: Hiển thị trong danh sách, tìm kiếm cơ sở

- **`Address`** (required string): Địa chỉ đầy đủ
  - *Ví dụ*: "123 Đường ABC, Phường XYZ, Quận 1, TP.HCM"
  - *Sử dụng*: Hiển thị cho khách hàng, tích hợp bản đồ, giao hàng

#### Thông tin liên hệ
- **`PhoneNumber`** (string?): Số điện thoại liên hệ
  - *Ví dụ*: "0901234567", "+84-28-12345678"
  - *Sử dụng*: Khách hàng gọi đặt sân, xác nhận booking

- **`Email`** (string?): Email liên hệ
  - *Ví dụ*: "info@sanbanh.com"
  - *Sử dụng*: Gửi thông báo, xác nhận booking qua email

- **`Description`** (string?): Mô tả chi tiết cơ sở
  - *Ví dụ*: "Cơ sở hiện đại với 5 sân bóng đá mini, có chỗ để xe miễn phí"
  - *Sử dụng*: Marketing, hiển thị thông tin cho khách hàng

#### Thời gian hoạt động
- **`OpenTime`** (TimeOnly): Giờ mở cửa chung của cơ sở
  - *Mặc định*: 6:00 AM
  - *Sử dụng*: Ràng buộc giờ hoạt động của các sân con

- **`CloseTime`** (TimeOnly): Giờ đóng cửa chung của cơ sở  
  - *Mặc định*: 10:00 PM
  - *Sử dụng*: Ràng buộc giờ hoạt động của các sân con

#### Quản lý
- **`IsActive`** (bool): Trạng thái hoạt động
  - *Mặc định*: true
  - *Sử dụng*: Tạm ngưng hoạt động toàn bộ cơ sở (bảo trì, đóng cửa)

- **`ManagerId`** (string?): ID của người quản lý cơ sở
  - *Sử dụng*: Phân quyền quản lý, liên hệ khi có vấn đề

#### Navigation Properties
- **`Fields`**: Danh sách các sân thuộc cơ sở này
  - *Quan hệ*: One-to-Many với Field

---

## ⚽ Field - Sân thể thao

**Mục đích**: Đại diện cho một sân cụ thể trong cơ sở (sân bóng đá, sân cầu lông, sân tennis...)

### Các trường dữ liệu chi tiết:

#### Thông tin cơ bản
- **`Name`** (required string): Tên sân
  - *Ví dụ*: "Sân A", "Sân bóng đá 1", "Sân cầu lông VIP"
  - *Sử dụng*: Phân biệt các sân, hiển thị trong booking

- **`FacilityId`** (required string): ID cơ sở chứa sân này
  - *Sử dụng*: Liên kết với Facility, group theo cơ sở

- **`Type`** (FieldType enum): Loại sân
  - *Các giá trị*: Football5v5, Football7v7, Football11v11, Badminton, Tennis, Basketball...
  - *Sử dụng*: Lọc sân theo môn thể thao, tính giá khác nhau

- **`BasePrice`** (decimal): Giá cơ bản mỗi giờ
  - *Ví dụ*: 200000 (200k VNĐ/giờ)
  - *Sử dụng*: Giá nền trước khi áp dụng pricing rules phức tạp

#### Mô tả và cấu hình
- **`Description`** (string?): Mô tả chi tiết sân
  - *Ví dụ*: "Sân cỏ nhân tạo 5v5, có mái che, đèn chiếu sáng"
  - *Sử dụng*: Hiển thị thông tin cho khách hàng

- **`Capacity`** (int): Số người tối đa
  - *Mặc định*: 10 người
  - *Sử dụng*: Kiểm soát số lượng người, tính phí theo nhóm

- **`IsActive`** (bool): Trạng thái hoạt động
  - *Mặc định*: true
  - *Sử dụng*: Tạm ngưng sân (bảo trì, hỏng hóc) mà không xóa dữ liệu

#### Navigation Properties
- **`Facility`**: Cơ sở chứa sân này
- **`TimeSlots`**: Các khung giờ có thể booking
- **`FieldPricings`**: Bảng giá theo khung giờ/ngày
- **`OperatingHours`**: Giờ hoạt động theo từng ngày trong tuần
- **`MaintenanceSchedules`**: Lịch bảo trì
- **`BlockedAvailabilities`**: Các khoảng thời gian bị chặn

---

## ⏰ TimeSlot - Khung thời gian

**Mục đích**: Định nghĩa các khoảng thời gian cố định có thể đặt sân

### **🚨 Thay đổi quan trọng:**
- **`Id`** (string): Sử dụng **string** thay vì Guid để dễ dàng tạo ID có ý nghĩa
  - *Ví dụ*: "06:00-07:00", "14:30-16:00", "evening-slot-1"
  - *Lợi ích*: Dễ debug, dễ hiểu, có thể tạo ID có pattern

### Các trường dữ liệu chi tiết:

- **`StartTime`** (TimeOnly): Thời gian bắt đầu
  - *Ví dụ*: 08:00, 14:30, 20:00
  - *Sử dụng*: Xác định thời điểm bắt đầu slot

- **`EndTime`** (TimeOnly): Thời gian kết thúc  
  - *Ví dụ*: 09:00, 15:30, 21:00
  - *Sử dụng*: Xác định thời điểm kết thúc slot

- **`DurationInMinutes`** (int): Thời lượng tính bằng phút
  - *Tự động tính*: EndTime - StartTime
  - *Ví dụ*: 60, 90, 120 phút
  - *Sử dụng*: Tính toán giá tiền, hiển thị thông tin

- **`IsActive`** (bool): Trạng thái kích hoạt
  - *Mặc định*: true
  - *Sử dụng*: Vô hiệu hóa slot không còn sử dụng

### Factory Methods:
- **`Create(startTime, endTime)`**: Tạo với ID tự động (Guid)
- **`CreateWithId(id, startTime, endTime)`**: Tạo với ID tùy chỉnh (string)

### Phương thức hỗ trợ:
- **`GetDisplayName()`**: Hiển thị dạng "08:00 - 09:00"
- **`IsOverlapping()`**: Kiểm tra trùng lặp với slot khác

---

## 💰 FieldPricing - Bảng giá sân

**Mục đích**: Quản lý giá sân theo khung giờ, ngày trong tuần và thời gian có hiệu lực

### **🚨 Thay đổi quan trọng:**
- **`TimeSlotId`** (string): Đã thay đổi từ Guid thành **string** để phù hợp với TimeSlot.Id

### Các trường dữ liệu chi tiết:

#### Liên kết
- **`FieldId`** (Guid): ID sân áp dụng giá này
- **`TimeSlotId`** (required string): ID khung giờ áp dụng - **🆕 Đã chuyển thành string**
  - *Sử dụng*: Một slot có thể có giá khác nhau theo ngày, thời gian

#### Giá và thời gian áp dụng
- **`Price`** (decimal): Giá tiền cho slot này
  - *Ví dụ*: 250000 (250k VNĐ)
  - *Sử dụng*: Ghi đè BasePrice của Field

- **`DayOfWeek`** (DayOfWeek?): Ngày trong tuần áp dụng
  - *null*: Áp dụng cho tất cả các ngày
  - *Ví dụ*: Monday, Saturday, Sunday
  - *Sử dụng*: Giá cuối tuần khác giá ngày thường

#### Thời gian hiệu lực
- **`EffectiveFrom`** (DateTime?): Ngày bắt đầu có hiệu lực
  - *null*: Có hiệu lực ngay lập tức
  - *Ví dụ*: 2024-01-01 (bắt đầu từ năm mới)
  - *Sử dụng*: Lên lịch tăng giá, khuyến mãi

- **`EffectiveTo`** (DateTime?): Ngày hết hiệu lực
  - *null*: Có hiệu lực vô thời hạn
  - *Ví dụ*: 2024-01-31 (chỉ áp dụng trong tháng 1)
  - *Sử dụng*: Khuyến mãi có thời hạn

- **`IsActive`** (bool): Trạng thái kích hoạt
  - *Mặc định*: true
  - *Sử dụng*: Vô hiệu hóa pricing rule mà không xóa

### Navigation Properties
- **`Field`**: Sân áp dụng bảng giá
- **`TimeSlot`**: Khung giờ áp dụng bảng giá

### Phương thức hỗ trợ:
- **`IsValidForDate(DateTime date)`**: Kiểm tra bảng giá có áp dụng cho ngày cụ thể

---

## 🕐 FieldOperatingHours - Giờ hoạt động sân

**Mục đích**: Quản lý giờ mở/đóng cửa của sân theo từng ngày trong tuần

### Các trường dữ liệu chi tiết:

#### Liên kết và thời gian
- **`FieldId`** (required Guid): ID sân áp dụng
- **`DayOfWeek`** (DayOfWeek): Ngày trong tuần
  - *Ví dụ*: Monday, Tuesday, Sunday
  - *Sử dụng*: Mỗi ngày có thể có giờ hoạt động khác nhau

- **`OpenTime`** (TimeOnly): Giờ mở cửa
  - *Ví dụ*: 06:00, 07:00
  - *Sử dụng*: Giới hạn booking từ giờ này

- **`CloseTime`** (TimeOnly): Giờ đóng cửa
  - *Ví dụ*: 22:00, 23:00
  - *Sử dụng*: Giới hạn booking đến giờ này

#### Trạng thái hoạt động
- **`IsActive`** (bool): Kích hoạt giờ hoạt động
  - *Mặc định*: true
  - *Sử dụng*: Vô hiệu hóa tạm thời

- **`IsClosed`** (bool): Đóng cửa hoàn toàn trong ngày
  - *Mặc định*: false
  - *Sử dụng*: Đóng cửa đột xuất (bảo trì, sự cố)

- **`Notes`** (string?): Ghi chú
  - *Ví dụ*: "Đóng cửa do mưa lớn", "Bảo trì định kỳ"
  - *Sử dụng*: Thông tin cho khách hàng

### Navigation Properties
- **`Field`**: Sân áp dụng giờ hoạt động

### Phương thức hỗ trợ:
- **`IsWithinOperatingHours(TimeOnly time)`**: Kiểm tra thời gian có trong giờ hoạt động
- **`IsOperatingOnDate(DateTime date)`**: Kiểm tra có hoạt động vào ngày cụ thể
- **`GetOperatingDuration()`**: Tính tổng thời gian hoạt động trong ngày

---

## 🔧 FieldMaintenance - Bảo trì sân

**Mục đích**: Lập lịch và theo dõi các hoạt động bảo trì, sửa chữa sân

### Các trường dữ liệu chi tiết:

#### Thông tin cơ bản
- **`FieldId`** (Guid): ID sân cần bảo trì
- **`Title`** (string): Tiêu đề công việc bảo trì
  - *Ví dụ*: "Thay cỏ nhân tạo", "Sửa hệ thống đèn"
  - *Sử dụng*: Mô tả ngắn gọn công việc

- **`Description`** (string?): Mô tả chi tiết
  - *Ví dụ*: "Thay toàn bộ cỏ nhân tạo khu vực khung thành do bị mòn"
  - *Sử dụng*: Hướng dẫn chi tiết cho kỹ thuật viên

#### Thời gian thực hiện
- **`StartTime`** (DateTime): Thời gian bắt đầu bảo trì
  - *Sử dụng*: Chặn booking từ thời điểm này

- **`EndTime`** (DateTime): Thời gian kết thúc dự kiến
  - *Sử dụng*: Mở lại booking sau thời điểm này

#### Trạng thái và phân loại
- **`Status`** (MaintenanceStatus enum): Trạng thái hiện tại
  - *Các giá trị*: Scheduled, InProgress, Completed, Cancelled
  - *Sử dụng*: Workflow quản lý công việc

- **`Type`** (MaintenanceType enum): Loại bảo trì
  - *Các giá trị*: Preventive, Corrective, Emergency, Upgrade
  - *Sử dụng*: Phân loại để thống kê, báo cáo

#### Phân công và chi phí
- **`AssignedTo`** (string?): ID người được giao việc
  - *Sử dụng*: Theo dõi trách nhiệm, liên hệ

- **`EstimatedCost`** (decimal?): Chi phí ước tính
  - *Sử dụng*: Lập kế hoạch ngân sách

- **`ActualCost`** (decimal?): Chi phí thực tế
  - *Sử dụng*: Theo dõi chi phí, báo cáo

- **`Notes`** (string?): Ghi chú bổ sung
  - *Sử dụng*: Lưu thông tin phát sinh, kết quả thực hiện

#### Bảo trì định kỳ
- **`IsRecurring`** (bool): Có phải bảo trì định kỳ
  - *Sử dụng*: Tự động tạo lịch bảo trì tiếp theo

- **`RecurrencePattern`** (string?): Mẫu lặp lại (JSON)
  - *Ví dụ*: {"interval": "monthly", "day": 15} (ngày 15 hàng tháng)
  - *Sử dụng*: Cấu hình chu kỳ bảo trì

### Navigation Properties
- **`Field`**: Sân được bảo trì

### Phương thức hỗ trợ:
- **`IsActiveOnDate(DateTime date)`**: Kiểm tra có bảo trì vào ngày cụ thể
- **`ConflictsWith(DateTime start, DateTime end)`**: Kiểm tra xung đột thời gian
- **`GetDuration()`**: Tính thời gian bảo trì

---

## 🚫 FieldAvailability - Chặn lịch sân

**Mục đích**: Chặn các khung thời gian không cho phép booking (sự kiện đặc biệt, reserved...)

### **🚨 Thay đổi quan trọng:**
- **`TimeSlotId`** (string): Đã thay đổi từ Guid thành **string** để phù hợp với TimeSlot.Id

### Các trường dữ liệu chi tiết:

#### Liên kết
- **`FieldId`** (Guid): ID sân bị chặn
- **`TimeSlotId`** (required string): ID khung giờ bị chặn - **🆕 Đã chuyển thành string**
  - *Sử dụng*: Chặn chính xác khung giờ cụ thể

#### Thời gian chặn
- **`FromDate`** (DateTime): Ngày bắt đầu chặn
  - *Sử dụng*: Chặn từ ngày này

- **`ToDate`** (DateTime?): Ngày kết thúc chặn
  - *null*: Chặn vô thời hạn
  - *Sử dụng*: Chặn đến ngày này, sau đó tự động mở

#### Trạng thái và lý do
- **`IsBlocked`** (bool): Có đang bị chặn không
  - *Mặc định*: true
  - *Sử dụng*: Có thể mở lại mà không xóa record

- **`Reason`** (string?): Lý do chặn
  - *Ví dụ*: "Tổ chức giải bóng đá", "Reserved cho VIP"
  - *Sử dụng*: Thông tin cho staff, khách hàng

### Navigation Properties
- **`Field`**: Sân bị chặn
- **`TimeSlot`**: Khung giờ bị chặn

### Phương thức hỗ trợ:
- **`IsBlockedOnDate(DateTime date)`**: Kiểm tra có bị chặn vào ngày cụ thể

---

## 🎯 Domain Events - Format mới

### **🚨 Thay đổi quan trọng:**
Tất cả Domain Events đã được chuyển sang **primary constructor record syntax** cho clean code:

```csharp
// Cũ (class với constructor)
public class FieldCreatedEvent : BaseDomainEvent
{
    public Guid FieldId { get; }
    public string Name { get; }
    
    public FieldCreatedEvent(Guid fieldId, string name)
    {
        FieldId = fieldId;
        Name = name;
    }
}

// Mới (record với primary constructor)
public record FieldCreatedEvent(
    Guid FieldId,
    string Name,
    Guid FacilityId,
    FieldType Type
) : BaseDomainEvent;
```

### Các Domain Events hiện có:
- **`FieldCreatedEvent`**: Khi tạo sân mới
- **`FieldAvailableEvent`**: Khi sân khả dụng trở lại
- **`FieldUnavailableEvent`**: Khi sân không khả dụng
- **`FieldMaintenanceScheduledEvent`**: Lên lịch bảo trì
- **`FieldMaintenanceStartedEvent`**: Bắt đầu bảo trì
- **`FieldMaintenanceCompletedEvent`**: Hoàn thành bảo trì
- **`FieldMaintenanceCancelledEvent`**: Hủy bảo trì
- **`TimeSlotPriceUpdatedEvent`**: Cập nhật giá slot
- **`FieldOperatingHoursUpdatedEvent`**: Cập nhật giờ hoạt động
- **`FieldClosedForDayEvent`**: Đóng cửa trong ngày
- **`FieldOpenedForDayEvent`**: Mở cửa trở lại

---

## 🔗 Mối quan hệ và tương tác giữa các thực thể

### Luồng nghiệp vụ chính:

1. **Tạo booking**: Check Field.IsActive → FieldOperatingHours → FieldAvailability → FieldMaintenance
2. **Tính giá**: Field.BasePrice → FieldPricing (theo TimeSlot, DayOfWeek, thời gian)
3. **Bảo trì**: Tạo FieldMaintenance → Block Field tự động
4. **Quản lý cơ sở**: Facility quản lý nhiều Field, mỗi Field có các thông tin riêng

### Ràng buộc nghiệp vụ:
- Không được book sân khi: IsActive = false, trong thời gian bảo trì, ngoài giờ hoạt động, bị block
- Giá sân được ưu tiên: FieldPricing (specific) > Field.BasePrice (general)
- Thời gian bảo trì không được trùng lặp
- Giờ hoạt động sân phải nằm trong giờ hoạt động của cơ sở
- **TimeSlot.Id phải unique** và có format nhất quán trong hệ thống

### Migration Notes:
Khi update từ phiên bản cũ:
1. **TimeSlot.Id**: Convert từ Guid sang string (có thể giữ nguyên Guid string hoặc tạo format mới)
2. **FieldPricing.TimeSlotId**: Update foreign key references
3. **FieldAvailability.TimeSlotId**: Update foreign key references
4. **Domain Events**: Chuyển sang record syntax để code cleaner
5. **AggregateRoot.DeletedDate**: Thêm field mới cho soft delete tracking
