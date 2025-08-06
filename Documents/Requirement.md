# yêu cầu cụ thể:
## yêu cầu đặt sân:
1. hình thức đặt sân:
    - đặt riêng lẻ: người dùng chọn ngày đặt, sau đó chọn các slot theo khung giờ và sân trong bảng lịch.
    - đặt định kỳ: chọn sân, ngày bắt đầu và ngày kết thúc (có thể bỏ trống ngày kết thúc), chọn ngày trong tuần, có thể gia hạn hoặc kết thúc sớm, không cho phép chọn khung giờ rời rạc. ví dụ: chọn khung giờ từ 6h00 - 10h00, ngày bắt đầu 1/7/2025 - 1/8/2025, chọn thứ: 3,5,7. như vậy các ngày thứ 3/5/7 vào khung giờ từ 6-10h trong khoảng ngày đã chọn sẽ được đặt, các ngày khác trong tuần không tính.
2. đổi sân: cho phép bên quản trị có thể rời lịch và đổi sân thủ công.
3. ràng buộc:
    - đặt riêng lẻ: cho phép khách vãng lai có thể đặt cần để lại tên và số điện thoại khi đặt
    - đặt định kỳ: cần đăng nhập để thực hiện, tối thiểu 1 tháng.

## yêu cầu giao diện bảng lịch:
- các trạng thái: còn trống, không khả dụng, tạm khóa, đang chọn, đã đặt.
- các loại bảng lịch: bảng lịch tổng quan của 1 chi nhánh cơ sở trong ngày (hiển thị các sân với khung giờ chung, mỗi sân 1 row), bảng lịch của 1 sân cụ thể từ ngày a đến ngày b, mỗi ngày nằm trên 1 row.
- thông tin mỗi ô trong bảng lịch của admin: người đặt, trạng thái xử lý và thanh toán.
- thông tin trong bảng lịch phía người dùng: trạng thái xử lý và thông tin đặt sân của riêng user đó, có option hiển thị đơn đặt bị hủy.
- admin có thể đặt hộ khách hàng bằng cách tương tác với bảng lịch khi bấm nút tạo đặt sân và hủy để về mode bình thường.

## yêu cầu nhất quán: cập nhật trạng thái real-time bảng lịch, không để tình trạng đặt trùng lịch.
## yêu cầu thanh toán: 
- user chọn hình thức thanh toán:
    - khi chọn thanh toán qua các cổng như MoMo/VnPay sẽ có thời gian đợi là 15 phút, hoàn thành sẽ tự động xác nhận đơn, hết thời gian sẽ bị hủy.
    - khi chọn thanh toán chuyển khoản sẽ có thời gian chờ là 15 phút, yêu cầu cung cấp ảnh minh chứng, khi hoàn thành sẽ ở trạng thái chờ, hết thời gian sẽ bị hủy. bên quản trị sẽ nhận được thông báo và xác nhận thủ công.
    - khi chọn thanh toán trả sau sẽ ngay lập tức đưa đơn đặt ở trạng thái chờ, bên quản trị sẽ nhận được thông báo và xác nhận thủ công.
- đối với hình thức đặt sân:
    - đặt riêng lẻ: thanh toán 100%
    - đặt định kỳ: yêu cầu thanh toán ngay 1 tháng đầu tiên (có giảm tiền), các tháng sau sẽ có thông báo yêu cầu thanh toán vào cuối tháng. tùy vào chính sách doanh nghiệp sẽ quyết định tính tiền số buổi hủy.
- bên quản trị/cskh đặt hộ: tự xác nhận thanh toán thủ công.
- có số tiền tạm tính hiển thị trong quá trình đặt sân.
- đảm bảo tính đúng giá khi áp dụng chương trình khuyến mãi/mã giảm giá.
- thông tin tất cả các loại giao dịch đều được lưu trữ chi tiết.
- admin/manager có thể xem lịch sử chi tiết các giao dịch.

## yêu cầu người dùng:
- role:
    - quản trị: có các role admin, manager, receptionis/cashier, accountant
    - user: customer, guest, organization, teamlead, teammember, coach, student
- yêu cầu quản trị:
    - đặt sân định kỳ: theo dõi đánh dấu từng buổi, một lần đanh dấu có hiệu lực cho tất cả các khung giờ đã chọn trong đơn đặt sân.
    - phân quyền: admin được phân quyền các user và role khác.
- hình thức đăng nhập: qua số điện thoại/email kèm mật khẩu, OAuth2 qua Google/Facebook

## yêu cầu quản lý sân:
- có thể quản lý giờ hoạt động của cơ sở
- có thể quản lý khung giờ hoạt động của từng sân
- có thể quản lý lịch bảo trì sân
- có thể khóa lịch sân để tổ chức sự kiện


## yêu cầu mở rộng:
- có khả năng mở rộng các chức năng mới như: dịch vụ trong sân và bán hàng, quản lý hóa đơn thanh toán, đào tạo học viên, đăng ký lịch sân cho huấn luyện viên.