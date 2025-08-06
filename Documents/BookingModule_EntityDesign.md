# Thiết kế Chi tiết các Thực thể (Entities) - Module Booking

## 🏗️ Kiến trúc chung

### Phân cấp kế thừa
```
BaseEntity<T> (Generic base)
├── BaseEntity (Guid-based)
└── AuditableEntity<T>
    └── AuditableEntity (Guid-based)
        ├── Booking (IAggregateRoot)
        ├── BookingPayment (IAggregateRoot)
        └── CalendarSlotMatrix (IAggregateRoot)
```

### Mối quan hệ chính
```
Booking (1) ←→ (0..1) GuestInfo
Booking (1) ←→ (0..1) IndividualBookingDetail
Booking (1) ←→ (0..1) RecurringBookingDetail
Booking (1) ←→ (0..*) BookingPayment
Booking (1) ←→ (0..*) BookingStatusHistory

IndividualBookingDetail (1) ←→ (0..*) BookingSlot
RecurringBookingDetail (1) ←→ (0..*) RecurringBookingSchedule
RecurringBookingDetail (1) ←→ (0..*) RecurringBookingSession

BookingPayment (1) ←→ (0..*) PaymentTransaction
```

---

## 📝 Booking - Đơn đặt sân (Aggregate Root)

**Mục đích**: Entity chính đại diện cho một đơn đặt sân, có thể là đặt riêng lẻ hoặc đặt định kỳ


### Các trường dữ liệu chi tiết:

#### Thông tin cơ bản
- **`Type`** (BookingType enum): Loại đặt sân
  - *Individual*: Đặt riêng lẻ (slots rời rạc)
  - *Recurring*: Đặt định kỳ (lặp lại theo chu kỳ)
  - *Sử dụng*: Phân biệt workflow xử lý khác nhau

- **`Origin`** (BookingOrigin enum): Nguồn gốc đặt sân
  - *CustomerDirect*: Khách hàng đặt trực tiếp
  - *AdminBooking*: Admin đặt hộ
  - *GuestBooking*: Khách vãng lai (không đăng nhập)
  - *Sử dụng*: Phân biệt quyền hạn và workflow

- **`Status`** (BookingStatus enum): Trạng thái đơn đặt
  - *Pending*: Chờ xử lý
  - *Confirmed*: Đã xác nhận
  - *Cancelled*: Đã hủy
  - *InProgress*: Đang diễn ra
  - *Completed*: Hoàn thành
  - *NoShow*: Khách không đến
  - *Blocked*: Bị chặn (nghi vấn)

- **`FacilityId`** (Guid): ID cơ sở thể thao - **🆕 Đã chuyển thành Guid**
  - *Sử dụng*: Liên kết với FieldService.Facility
  - *Lưu ý*: Không lưu FieldId vì có thể đặt nhiều sân

#### Thông tin người đặt
- **`UserId`** (Guid?): ID người dùng đã đăng nhập - **🆕 Đã chuyển thành Guid**
  - *null*: Cho guest booking
  - *Sử dụng*: Liên kết với IdentityService.User

- **`CreatedByAdminId`** (Guid?): ID admin đặt hộ - **🆕 Đã chuyển thành Guid**
  - *Sử dụng*: Tracking khi admin đặt hộ khách hàng

#### Thông tin tài chính
- **`BaseAmount`** (decimal): Số tiền cơ bản
  - *Sử dụng*: Tổng tiền trước khi áp dụng giảm giá

- **`DiscountAmount`** (decimal): Số tiền giảm giá
  - *Mặc định*: 0
  - *Sử dụng*: Khuyến mãi, voucher, giảm giá cho recurring

- **`TotalAmount`** (decimal): Tổng tiền cuối cùng
  - *Công thức*: BaseAmount - DiscountAmount

#### Thông tin thời gian
- **`BookingDate`** (DateTime): Ngày đặt sân
  - *Sử dụng*: Cho Individual booking (ngày cụ thể)
  - *Recurring booking*: Sử dụng StartDate trong RecurringBookingDetail

- **`ConfirmedDate`** (DateTime?): Thời gian xác nhận
- **`CancelledDate`** (DateTime?): Thời gian hủy
- **`CancellationReason`** (string?): Lý do hủy

#### Ghi chú
- **`Notes`** (string?): Ghi chú của khách hàng
- **`AdminNotes`** (string?): Ghi chú nội bộ của admin

### Navigation Properties
- **`GuestInfo`**: Thông tin khách vãng lai (0..1)
- **`IndividualDetail`**: Chi tiết đặt riêng lẻ (0..1)
- **`RecurringDetail`**: Chi tiết đặt định kỳ (0..1)
- **`Payments`**: Danh sách thanh toán (0..*)
- **`StatusHistory`**: Lịch sử thay đổi trạng thái (0..*)

### Factory Methods
- **`CreateIndividualBooking()`**: Tạo đặt sân riêng lẻ
- **`CreateRecurringBooking()`**: Tạo đặt sân định kỳ

### Business Methods
- **`Confirm()`**: Xác nhận đặt sân
- **`Cancel()`**: Hủy đặt sân
- **`UpdateTotalAmount()`**: Cập nhật tổng tiền

---

## 👤 GuestInfo - Thông tin khách vãng lai

**Mục đích**: Lưu thông tin khách vãng lai cho Individual booking (không cần đăng nhập)

### Các trường dữ liệu chi tiết:

- **`BookingId`** (Guid): ID đơn đặt sân
  - *Quan hệ*: One-to-One với Booking

- **`FullName`** (required string): Họ tên đầy đủ
  - *Ví dụ*: "Nguyễn Văn A"
  - *Sử dụng*: Hiển thị trong calendar, liên hệ

- **`PhoneNumber`** (required string): Số điện thoại
  - *Ví dụ*: "0901234567"
  - *Sử dụng*: Liên hệ xác nhận, thông báo

- **`Email`** (string?): Email liên hệ
  - *Tùy chọn*: Có thể bỏ trống
  - *Sử dụng*: Gửi xác nhận booking

- **`Notes`** (string?): Ghi chú bổ sung
- **`UpdatedDate`** (DateTime?): Thời gian cập nhật thông tin

### Business Methods
- **`UpdateInfo()`**: Cập nhật thông tin khách

---

## 📋 IndividualBookingDetail - Chi tiết đặt riêng lẻ

**Mục đích**: Chi tiết cho Individual booking - cho phép chọn các slots rời rạc trên nhiều sân

### Các trường dữ liệu chi tiết:

- **`BookingId`** (Guid): ID đơn đặt sân
  - *Quan hệ*: One-to-One với Booking

### Navigation Properties
- **`Booking`**: Đơn đặt sân chính
- **`BookingSlots`**: Danh sách slots được đặt (1..*)

### Business Methods
- **`AddSlot(fieldId, timeSlotId, price)`**: Thêm slot vào booking
- **`RemoveSlot(fieldId, timeSlotId)`**: Xóa slot khỏi booking
- **`RemoveSlotById(slotId)`**: Xóa slot theo ID
- **`GetTotalAmount()`**: Tính tổng tiền các slot
- **`GetActiveTimeSlotIds()`**: Lấy danh sách TimeSlot IDs
- **`GetActiveFieldIds()`**: Lấy danh sách Field IDs
- **`GetActiveSlots()`**: Lấy danh sách (FieldId, TimeSlotId) pairs

---

## 🎯 BookingSlot - Slot được đặt

**Mục đích**: Đại diện cho intersection của (Field, TimeSlot, Date) trong Individual booking

### **🚨 Thay đổi quan trọng:**
- **`FieldId`** (Guid): Đã thay đổi từ string thành **Guid**
- **`TimeSlotId`** (string): Giữ nguyên string để phù hợp với FieldService.TimeSlot

### Các trường dữ liệu chi tiết:

#### Liên kết
- **`BookingId`** (Guid): ID đơn đặt sân
- **`FieldId`** (Guid): ID sân được đặt - **🆕 Đã chuyển thành Guid**
  - *Sử dụng*: Reference đến FieldService.Field
- **`TimeSlotId`** (required string): ID khung giờ
  - *Sử dụng*: Reference đến FieldService.TimeSlot

#### Thông tin giá và trạng thái
- **`Price`** (decimal): Giá của slot này
  - *Sử dụng*: Lưu giá tại thời điểm đặt (snapshot pricing)
- **`IsActive`** (bool): Slot có hiệu lực
  - *Mặc định*: true
  - *Sử dụng*: Vô hiệu hóa slot mà không xóa
- **`Notes`** (string?): Ghi chú cho slot

### Business Methods
- **`UpdatePrice()`**: Cập nhật giá slot
- **`Deactivate()`**: Vô hiệu hóa slot
- **`Activate()`**: Kích hoạt lại slot

---

## 🔄 RecurringBookingDetail - Chi tiết đặt định kỳ

**Mục đích**: Chi tiết cho Recurring booking - sử dụng TimeSlots liên tục, lặp lại theo chu kỳ

### **🚨 Thay đổi quan trọng:**
- **`FieldId`** (Guid): Đã thay đổi từ string thành **Guid**

### Các trường dữ liệu chi tiết:

#### Fields và TimeSlots
- **`BookingId`** (Guid): ID đơn đặt sân
- **`FieldId`** (Guid): ID sân duy nhất - **🆕 Đã chuyển thành Guid**
  - *Lưu ý*: Recurring booking chỉ cho 1 sân
- **`TimeSlotIds`** (string): Danh sách TimeSlot IDs (JSON)
  - *Ví dụ*: ["slot1","slot2","slot3"]
  - *Yêu cầu*: Các slots phải liên tục

#### Chu kỳ lặp lại
- **`StartDate`** (DateTime): Ngày bắt đầu
- **`EndDate`** (DateTime?): Ngày kết thúc
  - *null*: Có thể bỏ trống, mặc định 1 tháng
- **`DaysOfWeek`** (string): Ngày trong tuần (JSON)
  - *Ví dụ*: [1,3,5] = thứ 2,4,6

#### Trạng thái và quản lý
- **`RecurringStatus`** (RecurringBookingStatus enum): Trạng thái riêng
  - *Active*: Đang hoạt động
  - *Suspended*: Tạm dừng
  - *Cancelled*: Đã hủy
  - *Expired*: Hết hạn
  - *Extended*: Đã gia hạn

- **`SuspendedDate`** (DateTime?): Ngày tạm dừng
- **`ResumedDate`** (DateTime?): Ngày tiếp tục
- **`SuspensionReason`** (string?): Lý do tạm dừng

#### Pricing
- **`BasePrice`** (decimal): Giá cơ bản
- **`DiscountPercentage`** (decimal): Phần trăm giảm giá
  - *Mặc định*: 0
  - *Sử dụng*: Ưu đãi cho đặt định kỳ
- **`MonthlyAmount`** (decimal): Số tiền hàng tháng

### Navigation Properties
- **`Booking`**: Đơn đặt sân chính
- **`Schedules`**: Lịch sinh schedule theo tháng (0..*)
- **`Sessions`**: Các buổi cụ thể (0..*)

### Business Methods
- **`Suspend(reason)`**: Tạm dừng booking
- **`Resume()`**: Tiếp tục booking
- **`ExtendEndDate(newEndDate)`**: Gia hạn
- **`GetDaysOfWeek()`**: Parse ngày trong tuần từ JSON
- **`GetTimeSlotIds()`**: Parse TimeSlot IDs từ JSON
- **`UpdateTimeSlots(timeSlotIds)`**: Cập nhật danh sách slots
- **`IsActiveOnDate(date)`**: Kiểm tra hoạt động vào ngày cụ thể
- **`GetTimeSlotDisplay()`**: Hiển thị danh sách slots

---

## 📅 RecurringBookingSchedule - Lịch sinh schedule

**Mục đích**: Quản lý việc sinh schedule tự động cho Recurring booking theo tháng

### Các trường dữ liệu chi tiết:

- **`RecurringBookingDetailId`** (Guid): ID recurring booking detail
- **`ScheduleMonth`** (DateTime): Tháng được sinh schedule
  - *Ví dụ*: 2024-01-01 (đại diện tháng 1/2024)
- **`IsGenerated`** (bool): Đã sinh schedule chưa
  - *Mặc định*: false
- **`GeneratedDate`** (DateTime?): Thời gian sinh schedule
- **`TotalSessions`** (int): Số buổi trong tháng
- **`MonthlyAmount`** (decimal): Số tiền tháng này
- **`Notes`** (string?): Ghi chú

### Business Methods
- **`MarkAsGenerated(totalSessions, monthlyAmount)`**: Đánh dấu đã sinh
- **`IsCurrentMonth()`**: Kiểm tra có phải tháng hiện tại
- **`IsFutureMonth()`**: Kiểm tra có phải tháng tương lai

---

## 🎪 RecurringBookingSession - Buổi cụ thể

**Mục đích**: Từng buổi cụ thể trong Recurring booking - admin có thể đánh dấu từng buổi

### **🚨 Thay đổi quan trọng:**
- **`MarkedByAdminId`** (Guid?): Đã thay đổi từ string thành **Guid**

### Các trường dữ liệu chi tiết:

#### Thông tin cơ bản
- **`RecurringBookingDetailId`** (Guid): ID recurring booking detail
- **`SessionDate`** (DateTime): Ngày của buổi này
- **`Status`** (BookingStatus): Trạng thái buổi
  - *Mặc định*: Confirmed

#### Admin tracking
- **`IsMarkedByAdmin`** (bool): Admin đã đánh dấu chưa
  - *Mặc định*: false
  - *Yêu cầu*: Admin đánh dấu từng buổi (requirement #37)
- **`MarkedDate`** (DateTime?): Thời gian admin đánh dấu
- **`MarkedByAdminId`** (Guid?): ID admin đánh dấu - **🆕 Đã chuyển thành Guid**
- **`AdminNotes`** (string?): Ghi chú của admin

#### Session specific
- **`IsSkipped`** (bool): Bỏ qua buổi này
  - *Mặc định*: false
- **`SkipReason`** (string?): Lý do bỏ qua
- **`SessionAmount`** (decimal): Số tiền buổi này
- **`IsNoShow`** (bool): Khách không đến
  - *Mặc định*: false

### Business Methods
- **`MarkByAdmin(adminId, notes)`**: Admin đánh dấu buổi
- **`Skip(reason, adminId)`**: Bỏ qua buổi
- **`MarkAsNoShow(adminId)`**: Đánh dấu khách không đến
- **`Complete(adminId)`**: Hoàn thành buổi
- **`IsToday()`**: Kiểm tra có phải hôm nay
- **`IsUpcoming()`**: Kiểm tra có phải buổi sắp tới
- **`IsPast()`**: Kiểm tra có phải buổi đã qua
- **`GetTimeSlotDisplay()`**: Hiển thị thông tin slots

---

## 📜 BookingStatusHistory - Lịch sử trạng thái

**Mục đích**: Lịch sử thay đổi trạng thái của booking (audit trail)

### **🚨 Thay đổi quan trọng:**
- **`ChangedByUserId`** (Guid?): Đã thay đổi từ string thành **Guid**

### Các trường dữ liệu chi tiết:

- **`BookingId`** (Guid): ID đơn đặt sân
- **`FromStatus`** (BookingStatus): Trạng thái trước
- **`ToStatus`** (BookingStatus): Trạng thái sau
- **`ChangeReason`** (string?): Lý do thay đổi
- **`ChangedByUserId`** (Guid?): ID người thực hiện thay đổi - **🆕 Đã chuyển thành Guid**
  - *Có thể là*: User hoặc Admin
- **`ChangedDate`** (DateTime): Thời gian thay đổi

### Business Methods
- **`GetStatusChangeDescription()`**: Mô tả thay đổi
- **`IsStatusUpgrade()`**: Kiểm tra có phải nâng cấp trạng thái
- **`IsStatusDowngrade()`**: Kiểm tra có phải hạ cấp trạng thái

---

## 💳 BookingPayment - Thanh toán (Aggregate Root)

**Mục đích**: Quản lý thanh toán cho booking với workflow phức tạp

### **🚨 Thay đổi quan trọng:**
- **`ApprovedByAdminId`** (Guid?): Đã thay đổi từ string thành **Guid**

### Các trường dữ liệu chi tiết:

#### Thông tin cơ bản
- **`BookingId`** (Guid): ID đơn đặt sân
- **`Amount`** (decimal): Số tiền thanh toán
- **`Method`** (PaymentMethod enum): Phương thức thanh toán
  - *Cash*: Tiền mặt (admin booking)
  - *BankTransfer*: Chuyển khoản (cần ảnh minh chứng)
  - *MoMo*: MoMo e-wallet
  - *VnPay*: VnPay gateway
  - *PayLater*: Trả sau (cần admin approve)

- **`Status`** (PaymentStatus enum): Trạng thái thanh toán
  - *Pending*: Chờ thanh toán
  - *Processing*: Đang xử lý
  - *WaitingProof*: Chờ minh chứng
  - *WaitingApproval*: Chờ admin duyệt
  - *Completed*: Đã thanh toán thành công
  - *Failed*: Thất bại
  - *Cancelled*: Đã hủy
  - *Expired*: Hết hạn timeout
  - *Refunded*: Đã hoàn tiền

#### Timeout management
- **`DueDate`** (DateTime?): Hạn thanh toán
- **`TimeoutDuration`** (TimeSpan?): Thời gian timeout
  - *15 phút*: Cho MoMo/VnPay/BankTransfer

#### Payment proof (for bank transfer)
- **`ProofImageUrl`** (string?): URL ảnh minh chứng
- **`ProofUploadedDate`** (DateTime?): Thời gian upload ảnh

#### Approval workflow
- **`RequiresApproval`** (bool): Có cần admin duyệt
  - *true*: PayLater, BankTransfer
- **`ApprovedByAdminId`** (Guid?): ID admin duyệt - **🆕 Đã chuyển thành Guid**
- **`ApprovedDate`** (DateTime?): Thời gian duyệt
- **`ApprovalNotes`** (string?): Ghi chú khi duyệt

#### Transaction info
- **`TransactionId`** (string?): ID giao dịch từ gateway
- **`GatewayResponse`** (string?): Response từ gateway
- **`GatewayTransactionId`** (string?): ID giao dịch từ gateway

#### Refund info
- **`RefundedAmount`** (decimal?): Số tiền đã hoàn
- **`RefundedDate`** (DateTime?): Thời gian hoàn tiền
- **`RefundReason`** (string?): Lý do hoàn tiền
- **`RefundTransactionId`** (string?): ID giao dịch hoàn tiền

### Navigation Properties
- **`Booking`**: Đơn đặt sân
- **`Transactions`**: Chi tiết giao dịch (0..*)

### Factory Methods
- **`CreateGatewayPayment()`**: Tạo thanh toán qua gateway
- **`CreateBankTransferPayment()`**: Tạo thanh toán chuyển khoản
- **`CreatePayLaterPayment()`**: Tạo thanh toán trả sau
- **`CreateAdminPayment()`**: Tạo thanh toán thủ công (admin)

### Business Methods
- **`SubmitProof(proofImageUrl)`**: Nộp ảnh minh chứng
- **`ApprovePayment(adminId, notes)`**: Admin duyệt thanh toán
- **`RejectPayment(adminId, reason)`**: Admin từ chối
- **`CompleteGatewayPayment()`**: Hoàn thành từ gateway
- **`FailGatewayPayment()`**: Thất bại từ gateway
- **`ExpirePayment()`**: Hết hạn timeout
- **`ProcessRefund()`**: Xử lý hoàn tiền

### Query Methods
- **`IsExpired()`**: Kiểm tra đã hết hạn
- **`GetRemainingTime()`**: Thời gian còn lại
- **`CanBeRefunded()`**: Có thể hoàn tiền không
- **`RequiresProof()`**: Cần ảnh minh chứng không
- **`RequiresAdminAction()`**: Cần admin xử lý không
- **`GetRefundableAmount()`**: Số tiền có thể hoàn

---

## 🔄 PaymentTransaction - Chi tiết giao dịch

**Mục đích**: Chi tiết các giao dịch thanh toán (có thể có nhiều attempts)

### Các trường dữ liệu chi tiết:

#### Thông tin cơ bản
- **`BookingPaymentId`** (Guid): ID booking payment
- **`Amount`** (decimal): Số tiền giao dịch
- **`Method`** (PaymentMethod): Phương thức
- **`Status`** (PaymentStatus): Trạng thái

#### Transaction details
- **`ExternalTransactionId`** (string?): ID giao dịch từ bên ngoài
- **`GatewayReference`** (string?): Reference từ gateway
- **`GatewayResponse`** (string?): Response từ gateway
- **`ProcessedDate`** (DateTime?): Thời gian xử lý

#### Retry/attempt info
- **`AttemptNumber`** (int): Số lần thử
  - *Mặc định*: 1
- **`IsRetry`** (bool): Có phải retry không
- **`FailureReason`** (string?): Lý do thất bại

### Business Methods
- **`MarkAsCompleted()`**: Đánh dấu thành công
- **`MarkAsFailed()`**: Đánh dấu thất bại
- **`IsSuccessful()`**: Kiểm tra thành công
- **`CanRetry()`**: Có thể retry không (max 3 lần)

---

## 📊 CalendarSlotMatrix - Ma trận lịch (Aggregate Root)

**Mục đích**: Đại diện cho ma trận bảng lịch: intersection của (Field, TimeSlot, Date)

### **🚨 Thay đổi quan trọng:**
- **`FacilityId`** (Guid): Đã thay đổi từ string thành **Guid**
- **`FieldId`** (Guid): Đã thay đổi từ string thành **Guid**
- **`BookedByUserId`** (Guid?): Đã thay đổi từ string thành **Guid**

### Các trường dữ liệu chi tiết:

#### Composite key
- **`FacilityId`** (Guid): ID cơ sở - **🆕 Đã chuyển thành Guid**
- **`FieldId`** (Guid): ID sân - **🆕 Đã chuyển thành Guid**
- **`TimeSlotId`** (required string): ID khung giờ
- **`Date`** (DateTime): Ngày cụ thể

#### Trạng thái slot
- **`State`** (SlotState enum): Trạng thái hiện tại
  - *Available*: Còn trống
  - *Unavailable*: Không khả dụng
  - *TempLocked*: Tạm khóa
  - *Selected*: Đang chọn
  - *Booked*: Đã đặt

#### Booking information (if booked)
- **`BookingId`** (Guid?): ID booking (nếu đã đặt)
- **`BookedByUserId`** (Guid?): ID người đặt - **🆕 Đã chuyển thành Guid**
- **`BookedByName`** (string?): Tên người đặt (guest)
- **`BookingType`** (BookingType?): Loại booking
- **`BookingStatus`** (BookingStatus?): Trạng thái booking
- **`PaymentStatus`** (PaymentStatus?): Trạng thái thanh toán

#### Admin information
- **`AdminNotes`** (string?): Ghi chú admin
- **`IsBlockedByAdmin`** (bool): Admin có chặn không
- **`BlockReason`** (string?): Lý do chặn

#### Lock information
- **`LockedBySessionId`** (string?): Session đang khóa
- **`LockedUntil`** (DateTime?): Khóa đến khi nào

#### Pricing
- **`Price`** (decimal?): Giá slot này

### Business Methods
- **`Book()`**: Đặt slot
- **`Lock(sessionId, duration)`**: Khóa tạm thời
- **`Unlock()`**: Mở khóa
- **`Select(sessionId)`**: Chọn slot
- **`CancelBooking()`**: Hủy đặt
- **`BlockByAdmin(reason)`**: Admin chặn
- **`UnblockByAdmin()`**: Admin mở chặn
- **`UpdateBookingStatus()`**: Cập nhật trạng thái booking
- **`UpdatePaymentStatus()`**: Cập nhật trạng thái thanh toán

### Query Methods
- **`IsExpiredLock()`**: Khóa đã hết hạn chưa
- **`IsLockedBySession(sessionId)`**: Session có đang khóa không
- **`IsAvailableForBooking()`**: Có thể đặt không
- **`BelongsToUser(userId)`**: Thuộc về user không
- **`IsUserVisible(userId, isAdmin)`**: User có thấy được không
- **`GetDisplayInfo(isAdmin)`**: Thông tin hiển thị
- **`GetSlotKey()`**: Composite key duy nhất

---

## 🔒 SlotLock - Khóa slot tạm thời

**Mục đích**: Quản lý khóa tạm thời slot trong quá trình booking (real-time conflict prevention)

### **🚨 Thay đổi quan trọng:**
- **`FieldId`** (Guid): Đã thay đổi từ string thành **Guid**
- **`UserId`** (Guid?): Đã thay đổi từ string thành **Guid**

### Các trường dữ liệu chi tiết:

#### Slot information
- **`FieldId`** (Guid): ID sân - **🆕 Đã chuyển thành Guid**
- **`TimeSlotId`** (required string): ID khung giờ
- **`BookingDate`** (DateTime): Ngày đặt

#### Lock information
- **`SessionId`** (string?): Browser session ID
- **`UserId`** (Guid?): User ID (nếu đã đăng nhập) - **🆕 Đã chuyển thành Guid**
- **`LockedAt`** (DateTime): Thời gian khóa
- **`ExpiresAt`** (DateTime): Thời gian hết hạn
- **`LockReason`** (SlotState): Lý do khóa
  - *Mặc định*: TempLocked
- **`IsActive`** (bool): Khóa có hiệu lực
  - *Mặc định*: true

### Business Methods
- **`ExtendLock(additionalTime)`**: Gia hạn khóa
- **`ReleaseLock()`**: Mở khóa
- **`IsExpired()`**: Kiểm tra hết hạn
- **`GetRemainingTime()`**: Thời gian còn lại
- **`BelongsToSession(sessionId)`**: Thuộc về session không
- **`BelongsToUser(userId)`**: Thuộc về user không

---

## 🎯 Enums liên quan

### BookingType
```csharp
public enum BookingType
{
    Individual = 1,     // Đặt riêng lẻ (slots rời rạc)
    Recurring = 2       // Đặt định kỳ (khung giờ liên tục)
}
```

### BookingOrigin
```csharp
public enum BookingOrigin
{
    CustomerDirect = 1,     // Khách hàng đặt trực tiếp
    AdminBooking = 2,       // Admin đặt hộ
    GuestBooking = 3        // Khách vãng lai
}
```

### RecurringBookingStatus
```csharp
public enum RecurringBookingStatus
{
    Active = 1,         // Đang hoạt động
    Suspended = 2,      // Tạm dừng
    Cancelled = 3,      // Đã hủy
    Expired = 4,        // Hết hạn
    Extended = 5        // Đã gia hạn
}
```

### SlotState
```csharp
public enum SlotState
{
    Available = 1,      // Còn trống
    Unavailable = 2,    // Không khả dụng
    TempLocked = 3,     // Tạm khóa
    Selected = 4,       // Đang chọn
    Booked = 5          // Đã đặt
}
```

### PaymentMethod
```csharp
public enum PaymentMethod
{
    Cash = 1,           // Tiền mặt (admin booking)
    BankTransfer = 2,   // Chuyển khoản (cần ảnh minh chứng)
    MoMo = 3,          // MoMo e-wallet
    VnPay = 4,         // VnPay gateway
    PayLater = 5        // Trả sau (cần admin approve)
}
```

### PaymentStatus
```csharp
public enum PaymentStatus
{
    Pending = 1,            // Chờ thanh toán
    Processing = 2,         // Đang xử lý
    WaitingProof = 3,       // Chờ minh chứng
    WaitingApproval = 4,    // Chờ admin duyệt
    Completed = 5,          // Đã thanh toán thành công
    Failed = 6,             // Thất bại
    Cancelled = 7,          // Đã hủy
    Expired = 8,            // Hết hạn timeout
    Refunded = 9,           // Đã hoàn tiền
    PartiallyRefunded = 10  // Hoàn một phần
}
```

---

## 🎯 Domain Events - Format mới

### **🚨 Cần cập nhật sang primary constructor record:**
Hiện tại các events vẫn sử dụng constructor pattern cũ, cần chuyển sang **primary constructor record syntax** như FieldService:

```csharp
// Hiện tại (cũ)
public record BookingCreatedEvent : BaseDomainEvent
{
    public Guid BookingId { get; }
    public BookingType BookingType { get; }
    // ... constructor
}

// Cần chuyển thành (mới)
public record BookingCreatedEvent(
    Guid BookingId,
    BookingType BookingType,
    BookingOrigin Origin,
    Guid FacilityId,
    List<Guid> FieldIds,
    Guid? UserId,
    DateTime BookingDate,
    decimal TotalAmount
) : BaseDomainEvent;
```

### Các Domain Events hiện có:
- **`BookingCreatedEvent`**: Khi tạo booking mới
- **`BookingConfirmedEvent`**: Khi xác nhận booking  
- **`BookingCancelledEvent`**: Khi hủy booking
- **`BookingConflictDetectedEvent`**: Khi xảy ra xung đột giữa booking đã confirm với thao tác sân như bảo trì, chặn lịch phía admin
- **`PaymentCompletedEvent`**: Thanh toán thành công
- **`PaymentRequiresApprovalEvent`**: Cần admin duyệt thanh toán
- **`SlotStateChangedEvent`**: Thay đổi trạng thái slot
- **`RecurringBookingScheduleGeneratedEvent`**: Sinh schedule mới

---

## 🔗 Mối quan hệ và tương tác giữa các thực thể

### Luồng nghiệp vụ chính:

#### Individual Booking Flow:
1. **Tạo booking**: User chọn facility → Hiển thị CalendarSlotMatrix → Chọn slots → Tạo Booking + IndividualBookingDetail + BookingSlots
2. **Lock slots**: Tạo SlotLock cho các slots được chọn (15 phút)
3. **Payment**: Tạo BookingPayment → Xử lý theo method → Update CalendarSlotMatrix state
4. **Confirmation**: Payment complete → Booking.Confirm() → Update calendar → Clear locks

#### Recurring Booking Flow:
1. **Tạo booking**: User chọn field + time slots + schedule → Tạo Booking + RecurringBookingDetail
2. **Generate schedule**: Tạo RecurringBookingSchedule cho từng tháng
3. **Generate sessions**: Tạo RecurringBookingSession cho từng buổi cụ thể
4. **Monthly billing**: Tạo BookingPayment cho từng tháng
5. **Admin tracking**: Admin đánh dấu từng session

#### Payment Workflows:
1. **Gateway (MoMo/VnPay)**: Create payment → Redirect → Webhook → Update status
2. **Bank Transfer**: Create payment → User upload proof → Admin approve → Update status
3. **Pay Later**: Create payment → Admin approve → Update status
4. **Cash (Admin)**: Create payment → Auto complete

### Real-time Calendar Management:
1. **Slot selection**: CalendarSlotMatrix.Select() → SlotLock.Create()
2. **Conflict prevention**: Check SlotLock + CalendarSlotMatrix state
3. **State sync**: Update CalendarSlotMatrix when booking confirmed/cancelled
4. **Lock expiry**: Background job clear expired SlotLocks

### Ràng buộc nghiệp vụ:

#### Individual Booking:
- Phải có ít nhất 1 BookingSlot
- Mỗi BookingSlot phải unique (FieldId, TimeSlotId, Date)
- Guest booking bắt buộc có GuestInfo
- TotalAmount = Sum(BookingSlot.Price) - DiscountAmount

#### Recurring Booking:
- Chỉ được chọn 1 field
- TimeSlots phải liên tục (validation trong business logic)
- Tối thiểu 1 tháng (StartDate → EndDate >= 1 month)
- Phải có ít nhất 1 ngày trong tuần
- MonthlyAmount được tính tự động

#### Payment:
- Individual: Thanh toán 100% ngay
- Recurring: Thanh toán 1 tháng đầu, các tháng sau có thông báo
- Timeout: 15 phút cho gateway/transfer
- Approval required: BankTransfer + PayLater
- Max 3 retry attempts cho failed transactions

#### Calendar Matrix:
- Composite key: (FacilityId, FieldId, TimeSlotId, Date) must be unique
- State transitions: Available → TempLocked → Selected → Booked
- Admin can block any slot except Booked slots
- Lock expiry auto-cleanup

#### Conflict Resolution:
- SlotLock prevents double booking
- CalendarSlotMatrix provides real-time state
- Optimistic locking for concurrent updates
- Event sourcing for audit trail

### Migration Notes:
Khi update từ phiên bản cũ:
1. **ID Fields**: Convert string IDs thành Guid cho tất cả user/admin references
2. **Foreign Keys**: Update all entity relationships 
3. **Domain Events**: Chuyển sang primary constructor record syntax
4. **AggregateRoot.DeletedDate**: Thêm field mới từ Common module
5. **Data consistency**: Đảm bảo TimeSlotId vẫn là string để consistent với FieldService

Thiết kế này đảm bảo hệ thống có thể xử lý tất cả các yêu cầu phức tạp từ đặt sân đơn giản đến recurring booking, payment workflows, real-time calendar management và admin operations một cách hiệu quả và maintainable với data types nhất quán.