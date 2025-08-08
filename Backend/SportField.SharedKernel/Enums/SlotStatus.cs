namespace SportField.SharedKernel.Enums;

public enum SlotStatus
{
    Available = 1,      // Còn trống
    Unavailable = 2,    // Không khả dụng (maintenance, blocked)
    TempLocked = 3,     // Tạm khóa (đang trong quá trình đặt)
    Selected = 4,       // Đang chọn (trong session)
    Booked = 5          // Đã đặt
}